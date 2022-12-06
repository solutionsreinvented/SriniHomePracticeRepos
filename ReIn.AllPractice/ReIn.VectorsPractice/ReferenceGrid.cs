using System.Windows.Media.Media3D;

using ReIn.VectorsPractice.Interfaces;

using ReInvented.Shared.Stores;
using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice
{
    /// <summary>
    /// Grid with four nodes which forms the basis for generating the cross frames, longitudinal frames and other members
    /// of the bridge.
    /// </summary>
    public class ReferenceGrid : PropertyStore, IReferenceGrid, ILongitudinalFrameVectors, ICrossFrameVectors
    {
        #region Default Constructor

        public ReferenceGrid()
        {

        }

        #endregion

        #region Parameterized Constructor

        public ReferenceGrid(int id)
        {
            Id = id;
        }

        #endregion

        #region Public Properties

        public double Spacing { get => Get<double>(); set => Set(value); }

        public double Distance { get => Get<double>(); set => Set(value); }

        public Node A { get => Get<Node>(); set => Set(value); }

        public Node B { get => Get<Node>(); set => Set(value); }

        public Node C { get => Get<Node>(); set => Set(value); }

        public Node D { get => Get<Node>(); set => Set(value); }

        #endregion

        #region Read-only Properties - Cross Frame Vectors

        public int Id { get => Get<int>(); private set => Set(value); }

        public Vector3D VectorAB => new Vector3D(B.X - A.X, B.Y - A.Y, B.Z - A.Z);

        public Vector3D VectorCD => new Vector3D(D.X - C.X, D.Y - C.Y, D.Z - C.Z);

        public Vector3D VectorAC => new Vector3D(C.X - A.X, C.Y - A.Y, C.Z - A.Z);

        public Vector3D VectorBD => new Vector3D(D.X - B.X, D.Y - B.Y, D.Z - B.Z);

        public Vector3D VectorBA => Vector3D.Multiply(-1, VectorAB);

        public Vector3D VectorDC => Vector3D.Multiply(-1, VectorCD);

        public Vector3D VectorCA => Vector3D.Multiply(-1, VectorAC);

        public Vector3D VectorDB => Vector3D.Multiply(-1, VectorBD);

        #endregion

        #region Read-only Properties - Longitudinal Frame Vectors

        public Vector3D VectorA => NormalizedCrossProductVectorFrom(VectorAB, VectorAC);

        public Vector3D VectorB => NormalizedCrossProductVectorFrom(VectorBD, VectorBA);

        public Vector3D VectorC => NormalizedCrossProductVectorFrom(VectorCA, VectorCD);

        public Vector3D VectorD => NormalizedCrossProductVectorFrom(VectorDC, VectorDB);

        #endregion

        #region Private Helpers

        private Vector3D NormalizedCrossProductVectorFrom(Vector3D vectorA, Vector3D vectorB)
        {
            Vector3D crossProductVector = Vector3D.CrossProduct(vectorA, vectorB);
            crossProductVector.Normalize();

            return crossProductVector;
        }

        #endregion

    }
}
