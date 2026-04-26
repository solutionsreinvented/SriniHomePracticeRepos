using RakeMechanism.Geometry.Domain;

using System.ComponentModel;

namespace RakeMechanism.UI.ViewModels
{
    public interface IStructureSegmentViewModel : INotifyPropertyChanged
    {
        string SegmentType { get; }

        bool HostsRakeArms { get; set; }
        
        /// <summary>
        /// Height of this specific segment.
        /// </summary>
        double Height { get; set; }
        
        /// <summary>
        /// The computed starting elevation of this segment (driven by the parent list).
        /// </summary>
        double ComputedStartElevation { get; set; }
        
        /// <summary>
        /// Converts the ViewModel to the corresponding Domain model.
        /// </summary>
        IStructureSegment ToDomainModel();
    }
}
