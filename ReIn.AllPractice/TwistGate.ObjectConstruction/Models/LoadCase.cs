using System.Collections.Generic;
using System.Linq;

using TwistGate.ObjectConstruction.Enums;
using TwistGate.ObjectConstruction.Interfaces;

namespace TwistGate.ObjectConstruction.Models
{
    public class LoadCase : ILoadCase
    {
        private static HashSet<LoadCase> _loadCases = new HashSet<LoadCase>();

        public LoadCase(int id)
        {

        }

        public int Id { get; private set; }

        #region Public Properties

        public string Title { get; set; }

        public LoadCaseType Type { get; set; }


        #endregion

        #region Static Methods

        public static IReadOnlyCollection<LoadCase> CreatedLoadCases => _loadCases.ToList().AsReadOnly();

        public static LoadCase RetrieveOrCreate()
        {
            int currentId = _loadCases.Count + 1;

            /// Check if it exists

            return new LoadCase(1);
        }

        #endregion

        #region Equality

        public override bool Equals(object obj)
        {
            return !(obj is null) && obj is LoadCase loadCase && Id == loadCase.Id;
        }

        public bool Equals(LoadCase loadCase)
        {
            return !(loadCase is null) && Id == loadCase.Id;
        }

        public static bool operator ==(LoadCase a, LoadCase b)
        {
            return ReferenceEquals(a, b) || !(a is null) && !(b is null) && a.Id == b.Id;
        }

        public static bool operator !=(LoadCase a, LoadCase b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

    }
}
