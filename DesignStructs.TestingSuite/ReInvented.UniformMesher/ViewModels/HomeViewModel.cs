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

        public HashSet<Node> TopNodes { get => Get<HashSet<Node>>(); private set => Set(value); }

        public HashSet<Node> BottomNodes { get => Get<HashSet<Node>>(); private set => Set(value); }

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
                SelectedFormationDirection = FormationDirectionValues.FirstOrDefault();

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

        public ICommand GetTopNodesCommand { get; private set; }

        public ICommand GetBottomNodesCommand { get; private set; }

        public ICommand GetAllSelectedNodesCommand { get; private set; }

        public ICommand FormPlateMeshCommand { get; private set; }

        public ICommand CreatePlatesInStaadCommand { get; private set; }

        #endregion

        #region Command Handlers

        private void OnBrowseSourceStaadFile()
        {
            StaadModelPath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad model files", "std"));
            Wrapper = OpenStaadWrapperProvider.Get(StaadModelPath);

            ValidModelIsSelected = string.IsNullOrEmpty(StaadModelPath) && Wrapper != null;
        }

        private void OnGetSelectedNodes()
        {
            AllSelectedNodes = GetSelectedNodesExt(Wrapper.Geometry);
            NodesAreSelected = AllSelectedNodes != null && AllSelectedNodes.Count > 0;
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

        private void OnFormPlateMesh()
        {
            ParseTopAndBottomNodes();

            if (Wrapper == null || TopNodes == null || BottomNodes == null)
            {
                _ = MessageBox.Show("An issue is encountered. No plate mesh is formed.", "Form plate mesh.", MessageBoxButton.OK);
            }
            else
            {
                if (TopNodes.Count == BottomNodes.Count)
                {
                    List<Node> topNodesList = TopNodes.ToList();
                    List<Node> botNodesList = BottomNodes.ToList();
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

            TopNodes = groupedNodes[1].Select(n => n).ToHashSet();
            BottomNodes = groupedNodes[0].Select(n => n).ToHashSet();
        }

        //private void OnGetBottomNodes()
        //{
        //    BottomNodes = GetSelectedNodesExt(Wrapper.Geometry).OrderBy(n => Node.PlanAngleIn360DegreesOf(n, Origin)).ToHashSet();
        //    _ = MessageBox.Show("Bottom nodes are retrieval is completed.", "Get Bottom Nodes", MessageBoxButton.OK);
        //}

        //private void OnGetTopNodes()
        //{
        //    TopNodes = GetSelectedNodesExt(Wrapper.Geometry).OrderBy(n => Node.PlanAngleIn360DegreesOf(n, Origin)).ToHashSet();
        //    _ = MessageBox.Show("Top nodes are retrieval is completed.", "Get Top Nodes", MessageBoxButton.OK);
        //}

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
            GetAllSelectedNodesCommand = new RelayCommand(OnGetSelectedNodes, true);
            FormPlateMeshCommand = new RelayCommand(OnFormPlateMesh, true);
            CreatePlatesInStaadCommand = new RelayCommand(OnCreatePlatesInStaad, true);
            ProgressionDirection = Direction.Circumferential;
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
