using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.Shared;

namespace SPro2023ConsoleApp
{
    public class VerticalStiffeners
    {
        //public (NodeCollection[], int[], long) GenerateVerticalAnnulusStiffenerMesh(NodeCollection[] nodesCR, NodeCollection[] nodesVA, NodeCollection[] nodesUF, long seqNodeNum, long seqPlateNum)
        //{
        //    double Lres, iBDiv;
        //    long nDivRes;
        //    double[] D;
        //    double MaxDim = 0.2; /// Original: double MaxDim;
        //    long inDivRes;

        //    Node P1, P2;
        //    NodeCollection[] nodesGr = new NodeCollection[nodesVA.Length];

        //    for (int i = 0; i < nodesVA.Length; i++)
        //    {
        //        if (i == 0)
        //        {
        //            nodesGr[i] = new NodeCollection { Nodes = nodesCR.SelectMany(nc => nc.Nodes.Where((node, index) => index % GD.nRB == 0)).ToArray() };
        //        }
        //        else
        //        {
        //            P1 = nodesUF[i].Nodes[0];
        //            P2 = nodesVA[i].Nodes[0];

        //            double yDiffTotal = P2.y - P1.y;
        //            Lres = Math.Sqrt((P2.x - P1.x).Squared() + (P2.y - P1.y).Squared() + (P2.z - P1.z).Squared());
        //            yDiffTotal = P2.y - P1.y;

        //            double yDiff = 0;

        //            for (int j = 0; j < (nodesGr[i - 1].Nodes.Length / GD.nRB) - 2; j++)
        //            {
        //                P1 = nodesGr[i - 1].Nodes[j * GD.nRB];
        //                P2 = nodesGr[i - 1].Nodes[(j + 1) * GD.nRB];
        //                MaxDim = Math.Sqrt((P2.x - P1.x).Squared() + (P2.y - P1.y).Squared() + (P2.z - P1.z).Squared());
        //            }

        //            iBDiv = Math.Min(MaxDim * GD.AspectRatio, GD.MaxPlateDim);
        //            nDivRes = (long)Math.Ceiling(Lres / iBDiv);
        //            iBDiv = Lres / nDivRes;

        //            nodesGr[i] = new NodeCollection { Nodes = new Node[0] };
        //            D = new double[] { GD.dCR };
        //            yDiff = yDiffTotal / nDivRes;

        //            for (inDivRes = 0; inDivRes <= nDivRes; inDivRes++)
        //            {
        //                if (inDivRes == 0)
        //                {
        //                    nodesGr[i].Nodes = nodesVA[i].Nodes.Where((node, index) => index % GD.nRB == 0).ToArray();
        //                }
        //                else if (inDivRes == nDivRes)
        //                {
        //                    nodesGr[i].Nodes = nodesUF[i].Nodes.Where((node, index) => index % GD.nRB == 0).ToArray();
        //                }
        //                else
        //                {
        //                    GenerateVASNodes(iBDiv, i, D, nodesGr, seqNodeNum, Component.CRVAS, yDiff);
        //                }
        //            }
        //        }
        //    }

        //    int[] pGr = new int[nodesGr.Length]; // Assuming appropriate initialization
        //    GenerateVASPlates(nodesGr, ref pGr, ref seqPlateNum, Component.CRVAS);

        //    return (nodesGr, pGr, seqPlateNum);
        //}


        //private (double[], NodeCollection[]) GenerateVASNodes(double iBDiv, int iDiv, double[] D, NodeCollection[] nodesGr, long seqNodeNum, Component com, double yDiff = double.NaN)
        //{
        //    int counter = D.Length;
        //    double[] resizedD = new double[counter + 1];
        //    Array.Copy(D, resizedD, counter);
        //    resizedD[counter] = D[counter - 1] - (2 * Math.Sqrt(iBDiv * iBDiv - yDiff * yDiff));
        //    double iR = resizedD[counter] / 2;

        //    Node ieNode = new Node
        //    {
        //        y = nodesGr[iDiv].Nodes[nodesGr[iDiv].Nodes.Length - 1].y - yDiff
        //    };

        //    for (int I = 0; I < GD.nRB; I++)
        //    {
        //        double iDelta = GD.sDelta + (I * GD.Omega);
        //        ieNode.x = iR * Math.Cos(iDelta);
        //        ieNode.z = iR * Math.Sin(iDelta);
        //        CreateNewNode(ref seqNodeNum, ref nodesGr, iDiv, ieNode.x, ieNode.y, ieNode.z);
        //    }

        //    return (resizedD, nodesGr);
        //}

        private void CreateNewNode(ref long SeqNodeNum, ref NodeCollection[] NodesGr, int iDiv, double x, double y, double z)
        {
            // Implementation of CreateNewNode
            // Not provided in the original VB.NET code
        }


        public void GenerateVASPlates(ref NodeCollection[] NodesGr, ref int[] pGr, ref long SeqPlateNum, Component com)
        {
            // Implementation of GenerateVASPlates
            // Not provided in the original VB.NET code
        }

        public class NodeCollection
        {
            public Node[] Nodes { get; set; }
        }

        public class Node
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
        }

        public enum Component
        {
            CRVAS
        }

        public static class GD
        {
            public static int nRB = 2; // Sample value, adjust as needed
            public static double AspectRatio = 1.5; // Sample value, adjust as needed
            public static double MaxPlateDim = 10.0; // Sample value, adjust as needed
            public static double dCR = 0.1; // Sample value, adjust as needed
        }

    }
}
