using System;
using System.Collections.Generic;
using System.Windows.Input;

using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Services;
using OpenSTAADUI;
using System.Linq;
using System.Windows;
using ReInvented.UniformMesher.Enums;

namespace ReInvented.UniformMesher.ViewModels
{
    public class HomeViewModel : ValidatablePropertyStore
    {
        #region Default Constructor

        public HomeViewModel()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public OpenStaadWrapper Wrapper { get => Get<OpenStaadWrapper>(); private set => Set(value); }

        public HashSet<Node> AllSelectedNodes { get => Get<HashSet<Node>>(); private set => Set(value); }

        public HashSet<Node> FirstNodeSet { get => Get<HashSet<Node>>(); private set => Set(value); }

        public HashSet<Node> SecondNodeSet { get => Get<HashSet<Node>>(); private set => Set(value); }

        public HashSet<Plate> Plates { get => Get<HashSet<Plate>>(); private set => Set(value); }

        public Node Origin { get => Get<Node>(); private set => Set(value); }

        public bool NodesAreSelected { get => Get<bool>(); private set => Set(value); }

        public bool PlatesFormationCompleted { get => Get<bool>(); private set => Set(value); }

        public bool ValidModelIsSelected { get => Get<bool>(); private set => Set(value); }

        public Direction ProgressionDirection
        {
            get => Get<Direction>();
            set
            {
                Set(value);
                FormationDirectionValues = Enum.GetValues(typeof(Direction)).Cast<Direction>()
                                               .Select(d => d.ToString())
                                               .Where(d => d != ProgressionDirection.ToString());
                SelectedFormationDirection = Direction.Vertical.ToString(); ///FormationDirectionValues.FirstOrDefault();

                if (value != Direction.Circumferential)
                {
                    ClosedMesh = false;
                }
            }
        }

        public Direction FormationDirection { get => Get<Direction>(); set => Set(value); }

        public IEnumerable<string> FormationDirectionValues { get => Get<IEnumerable<string>>(); private set => Set(value); }

        public string SelectedFormationDirection
        {
            get => Get<string>();
            set
            {
                Set(value);
                if (value != null)
                {
                    FormationDirection = (Direction)Enum.Parse(typeof(Direction), value);
                }
            }
        }

        public double Tolerance { get => Get<double>(); set => Set(value); }

        public string StaadModelPath { get => Get<string>(); private set => Set(value); }

        public bool ClosedMesh { get => Get<bool>(); set => Set(value); }

        #endregion

        #region Commands

        public ICommand BrowseSourceStaadFileCommand { get; private set; }

        public ICommand GetFirstNodeSetCommand { get; private set; }

        public ICommand GetSecondNodeSetCommand { get; private set; }

        public ICommand GetAllSelectedNodesCommand { get; private set; }

        public ICommand FormPlateMeshCommand { get; private set; }

        public ICommand CreatePlatesInStaadCommand { get; private set; }

        #endregion

        #region Command Handlers

        private void OnBrowseSourceStaadFile()
        {
            StaadModelPath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad model files", "std"));
            Wrapper = OpenStaadWrapperProvider.Get(StaadModelPath);

            ValidModelIsSelected = !string.IsNullOrEmpty(StaadModelPath) && Wrapper != null;
        }

        private void OnGetSelectedNodes()
        {
            AllSelectedNodes = GetSelectedNodesExt(Wrapper.Geometry);
            NodesAreSelected = AllSelectedNodes != null && AllSelectedNodes.Count > 0;

            _ = AllSelectedNodes == null || AllSelectedNodes.Count <= 0
                ? MessageBox.Show("No nodes are selected", "Retrieve selected nodes", MessageBoxButton.OK)
                : MessageBox.Show("Selected nodes are successfully retrieved", "Retrieve selected nodes", MessageBoxButton.OK);
        }

        private void OnGetFirsNodeSet()
        {
            FirstNodeSet = GetSelectedNodesExt(Wrapper.Geometry).OrderBy(n => Node.PlanAngleIn360DegreesOf(n, Origin)).ToHashSet();
            _ = FirstNodeSet == null || FirstNodeSet.Count <= 0
                ? MessageBox.Show("No nodes are selected", "Retrieve selected nodes", MessageBoxButton.OK)
                : MessageBox.Show("Selected nodes are successfully retrieved", "Retrieve selected nodes", MessageBoxButton.OK);

            NodesAreSelected = FirstNodeSet != null && FirstNodeSet.Count > 0 && SecondNodeSet != null && FirstNodeSet != SecondNodeSet;
        }

        private void OnGetSecondNodeSet()
        {
            SecondNodeSet = GetSelectedNodesExt(Wrapper.Geometry).OrderBy(n => Node.PlanAngleIn360DegreesOf(n, Origin)).ToHashSet();
            _ = SecondNodeSet == null || SecondNodeSet.Count <= 0
                ? MessageBox.Show("No nodes are selected", "Retrieve selected nodes", MessageBoxButton.OK)
                : MessageBox.Show("Selected nodes are successfully retrieved", "Retrieve selected nodes", MessageBoxButton.OK);

            NodesAreSelected = SecondNodeSet != null && SecondNodeSet.Count > 0 && FirstNodeSet != null && FirstNodeSet != SecondNodeSet;
        }

        private void OnFormPlateMesh()
        {
            //ParseTopAndBottomNodes();

            if (Wrapper == null || FirstNodeSet == null || SecondNodeSet == null)
            {
                _ = MessageBox.Show("An issue is encountered. No plate mesh is formed.", "Form plate mesh.", MessageBoxButton.OK);
            }
            else if (FirstNodeSet == SecondNodeSet)
            {
                _ = MessageBox.Show("The number of nodes in the top nodes and bottom nodes set is different. No plate mesh is formed.", "Form plate mesh.", MessageBoxButton.OK);
            }
            else
            {
                if (FirstNodeSet.Count == SecondNodeSet.Count)
                {
                    List<Node> topNodesList = FirstNodeSet.ToList();
                    List<Node> botNodesList = SecondNodeSet.ToList();
                    int lastElemId = Math.Max(Wrapper.Geometry.GetLastBeamNo(), Wrapper.Geometry.GetLastPlateNo());

                    Plates = new HashSet<Plate>();

                    for (int i = 0; i < topNodesList.Count - 1; i++)
                    {
                        lastElemId += 1;
                        Plate plate = new Plate(lastElemId, topNodesList[i], topNodesList[i + 1], botNodesList[i + 1], botNodesList[i]);
                        _ = Plates.Add(plate);
                    }

                    if (ClosedMesh)
                    {
                        lastElemId += 1;
                        Plate plate = new Plate(lastElemId, topNodesList.Last(), topNodesList.First(), botNodesList.First(), botNodesList.Last());
                        _ = Plates.Add(plate);
                    }
                    _ = MessageBox.Show("Plates formation is completed.", "Form plate mesh", MessageBoxButton.OK);
                }
                else
                {
                    _ = MessageBox.Show("Number of top nodes and bottom nodes are different!. No plate mesh is formed.", "Form plate mesh.", MessageBoxButton.OK);
                }
            }

            PlatesFormationCompleted = Plates != null && Plates.Count > 0;
        }

        private void OnCreatePlatesInStaad()
        {
            if (Wrapper != null && Plates != null && Plates.Count() > 0)
            {
                foreach (Plate p in Plates)
                {
                    Wrapper.Geometry.CreatePlate(p.Id, p.A.Id, p.B.Id, p.C.Id, p.D.Id);
                }
                _ = MessageBox.Show("All plates are created in Staad.", "Create Plates.", MessageBoxButton.OK);
            }
            else
            {
                _ = MessageBox.Show("An issue is encountered. No plates created.", "Create Plates.", MessageBoxButton.OK);
            }

            NodesAreSelected = false;
            PlatesFormationCompleted = false;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Origin = new Node(0.0, 0.0, 0.0);
            PlatesFormationCompleted = false;
            ValidModelIsSelected = false;
            NodesAreSelected = false;
            ClosedMesh = true;
            Tolerance = 0.005;

            BrowseSourceStaadFileCommand = new RelayCommand(OnBrowseSourceStaadFile, true);

            GetFirstNodeSetCommand = new RelayCommand(OnGetFirsNodeSet, true);
            GetSecondNodeSetCommand = new RelayCommand(OnGetSecondNodeSet, true);

            GetAllSelectedNodesCommand = new RelayCommand(OnGetSelectedNodes, true);
            FormPlateMeshCommand = new RelayCommand(OnFormPlateMesh, true);
            CreatePlatesInStaadCommand = new RelayCommand(OnCreatePlatesInStaad, true);
            ProgressionDirection = Direction.Circumferential;
        }

        private void ParseTopAndBottomNodes()
        {
            List<IGrouping<double, Node>> groupedNodes;

            if (ProgressionDirection == Direction.Circumferential)
            {
                groupedNodes = FormationDirection == Direction.Radial
                    ? AllSelectedNodes.GroupBy(n => Math.Round(Node.Radius(n, Origin) / Tolerance) * Tolerance).ToList()
                    : AllSelectedNodes.GroupBy(n => n.Y).ToList();
            }
            else if (ProgressionDirection == Direction.Radial)
            {
                groupedNodes = FormationDirection == Direction.Circumferential
                    ? AllSelectedNodes.GroupBy(n => Math.Round(Node.PlanAngleIn360DegreesOf(n, Origin), 3)).ToList()
                    : AllSelectedNodes.GroupBy(n => n.Y).ToList();
            }
            else
            {
                groupedNodes = FormationDirection == Direction.Circumferential
                    ? AllSelectedNodes.GroupBy(n => Math.Round(Node.PlanAngleIn360DegreesOf(n, Origin), 3)).ToList()
                    : AllSelectedNodes.GroupBy(n => Math.Round(Node.Radius(n, Origin), 3)).ToList();
            }

            FirstNodeSet = groupedNodes[1].Select(n => n).ToHashSet();
            SecondNodeSet = groupedNodes[0].Select(n => n).ToHashSet();
        }

        private HashSet<Node> GetSelectedNodesExt(OSGeometryUI geometry)
        {
            dynamic count = geometry.GetNoOfSelectedNodes();

            object nodeNumbers = new int[count];
            geometry.GetSelectedNodes(ref nodeNumbers, 0);

            int[] nodeNumbersList = (int[])nodeNumbers;

            HashSet<Node> nodes = new HashSet<Node>();

            foreach (int n in nodeNumbersList)
            {
                _ = nodes.Add(geometry.GetNode(n));
            }

            return nodes;
        }

        #endregion
    }
}
