using System.Collections.Generic;
using System.Linq;

namespace SRi.EntitiesSelector.Models
{
    public class StaadModel
    {
        public StaadModel()
        {

        }

        public HashSet<Node> Nodes { get; set; }

        public HashSet<Plate> Plates { get; set; }

        public void GetEntities()
        {
            OpenStaadProvider.Instantiate();

            if (OpenStaadProvider.OpenStaad != null)
            {
                GetAllNodes();

                GetAllPlates();
            }
        }


        private void GetAllNodes()
        {
            dynamic nodeCount = OpenStaadProvider.OSGeometry.GetNodeCount();

            if (nodeCount > 0)
            {
                object nodes = new int[nodeCount];

                OpenStaadProvider.OSGeometry.GetNodeList(ref nodes);

                Nodes = new HashSet<Node>();

                foreach (int n in (int[])nodes)
                {
                    int nodeNumber = n;

                    object xCoordinate = 0.0;
                    object yCoordinate = 0.0;
                    object zCoordinate = 0.0;

                    OpenStaadProvider.OSGeometry.GetNodeIncidence(nodeNumber, ref xCoordinate, ref yCoordinate, ref zCoordinate);

                    _ = Nodes.Add(new Node(nodeNumber, (double)xCoordinate, (double)yCoordinate, (double)zCoordinate));
                }
            }
        }


        private void GetAllPlates()
        {
            dynamic plateCount = OpenStaadProvider.OSGeometry.GetPlateCount();

            if (Nodes != null && plateCount > 0)
            {
                object plates = new int[plateCount];

                OpenStaadProvider.OSGeometry.GetPlateList(ref plates);

                Plates = new HashSet<Plate>();

                foreach (int p in (int[])plates)
                {
                    int plateNumber = p;

                    object nodeA = 0;
                    object nodeB = 0;
                    object nodeC = 0;
                    object nodeD = 0;

                    OpenStaadProvider.OSGeometry.GetPlateIncidence(p, ref nodeA, ref nodeB, ref nodeC, ref nodeD);

                    _ = Plates.Add(new Plate(p, Nodes.FirstOrDefault(n => n.Id == (int)nodeA), Nodes.FirstOrDefault(n => n.Id == (int)nodeB), Nodes.FirstOrDefault(n => n.Id == (int)nodeC), Nodes.FirstOrDefault(n => n.Id == (int)nodeD)));
                }

            }
        }
    }
}
