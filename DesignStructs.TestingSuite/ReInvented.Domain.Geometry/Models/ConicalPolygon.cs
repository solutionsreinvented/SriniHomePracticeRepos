using System.Collections.Generic;

using ReInvented.Domain.Geometry.Base;
using ReInvented.Domain.Geometry.Interfaces;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Geometry.Models
{
    public sealed class ConicalPolygon : Polygon, IPolygon
    {
        #region Parameterized Constructor

        public ConicalPolygon(List<Node> points, Node center, double startRadius, double endRadius, double startYCoordinate, double endYCoordiante) : 
            this(points, center, center, startRadius, endRadius, startYCoordinate, endYCoordiante)
        {

        }

        public ConicalPolygon(List<Node> points, Node startCenter, Node endCenter, double startRadius, double endRadius, double startYCoordinate, double endYCoordiante) : base(points)
        {
            StartRadius = startRadius;
            EndRadius = endRadius;
            StartYCoordinate = startYCoordinate;
            EndYCoordinate = endYCoordiante;
            StartCenter = startCenter;
            EndCenter = endCenter;
        }

        #endregion

        #region Public Properties

        public double StartRadius { get; private set; }

        public double EndRadius { get; private set; }

        public double StartYCoordinate { get; private set; }

        public double EndYCoordinate { get; private set; }

        public Node StartCenter { get; private set; }

        public Node EndCenter { get; private set; }

        #endregion

    }

}
