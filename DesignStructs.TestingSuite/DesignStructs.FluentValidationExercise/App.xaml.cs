using System.Collections.Generic;
using System.Linq;
using System.Windows;

using DesignStructs.FluentValidationExercise.ViewModels;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Extensions;

using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.FluentValidationExercise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static (int[] IdArray, int[][] IncidenceArray) Transform(IEnumerable<Beam> source)
        {
            List<Beam> beams = source.ToList();

            int[] idArray = beams.Select(b => b.Id).ToArray();
            //int[,] incidenceArray = new int[beams.Count, 2];

            //for (int i = 0; i < beams.Count; i++)
            //{
            //    incidenceArray[i, 0] = beams[i].StartNode.Id;
            //    incidenceArray[i, 1] = beams[i].EndNode.Id;
            //}

            var incidenceArray = beams.Select(b => new int[] { b.StartNode.Id, b.EndNode.Id }).ToArray();

            return (idArray, incidenceArray);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            StaadModel model = new StaadModel();
            OpenStaadWrapper wrapper = model.OpenStaadWrapper;
            OSGeometryUI geometry = wrapper.Geometry as OSGeometryUI;
            OSPropertyUI property = wrapper.Property as OSPropertyUI;

            var propId = 5;
            var plateIds = new List<int>() { 23497, 23498, 23499, 23500, 23501 };

            plateIds.ForEach(id => property.AssignPlateThickness(id, propId));

            property.AssignPlateThickness(plateIds.ToArray(), propId);


            List<Node> nodes = new List<Node>()
            {
                new Node(1, 0.0,0.0,0.0),
                new Node(2, 1.0,0.5,0.25),
                new Node(3, 3.0,1.0,0.50),
                new Node(4, 4.0,1.5,0.75),
            };

            List<Beam> beams = new List<Beam>()
            {
                new Beam(1, nodes[0], nodes[1]),
                new Beam(2, nodes[1], nodes[2]),
                new Beam(3, nodes[2], nodes[3])
            };

            //geometry.CreateMultipleNodes(nodes.ToHashSet());

            var (IdArray, IncidenceArray) = Transform(beams);

            object idArray = IdArray;
            object incidenceArray = IncidenceArray;


            geometry.CreateMultipleBeams(idArray, incidenceArray);

            //geometry.CreateMultipleNodes();


            MainViewModel viewModel = new MainViewModel();

            base.OnStartup(e);
            MainWindow = new MainWindow() { DataContext = viewModel };

            MainWindow.Show();
        }
    }
}
