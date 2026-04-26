using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RakeMechanism.Geometry.Domain;
using RakeMechanism.Geometry.Export;

namespace RakeMechanism.UI.ViewModels
{
    public partial class CentralStructureViewModel : ObservableObject
    {
        public ObservableCollection<IStructureSegmentViewModel> Segments { get; } = new ObservableCollection<IStructureSegmentViewModel>();

        [ObservableProperty]
        private IStructureSegmentViewModel? _selectedSegment;

        [ObservableProperty]
        private double _baseElevation = 0.0;

        public CentralStructureViewModel()
        {
            Segments.CollectionChanged += (s, e) => UpdateElevations();
            
            // Add a default cage segment
            var defaultCage = new CageSegmentViewModel();
            RegisterSegment(defaultCage);
            Segments.Add(defaultCage);
            SelectedSegment = defaultCage;
        }

        partial void OnBaseElevationChanged(double value)
        {
            UpdateElevations();
        }

        private void Segment_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IStructureSegmentViewModel.Height))
            {
                UpdateElevations();
            }

            if (e.PropertyName == nameof(IStructureSegmentViewModel.HostsRakeArms) &&
                sender is IStructureSegmentViewModel selectedCarrier &&
                selectedCarrier.HostsRakeArms)
            {
                foreach (var segment in Segments.Where(segment => !ReferenceEquals(segment, selectedCarrier)))
                {
                    segment.HostsRakeArms = false;
                }
            }
        }

        private void UpdateElevations()
        {
            double currentElevation = BaseElevation;
            foreach (var segment in Segments)
            {
                segment.ComputedStartElevation = currentElevation;
                currentElevation += segment.Height;
            }
        }

        public void AddSegment(IStructureSegmentViewModel segment)
        {
            RegisterSegment(segment);
            Segments.Add(segment);
            SelectedSegment = segment;
        }

        [RelayCommand]
        private void RemoveSelected()
        {
            if (SelectedSegment != null)
            {
                SelectedSegment.PropertyChanged -= Segment_PropertyChanged;
                Segments.Remove(SelectedSegment);
                SelectedSegment = null;

                if (Segments.Count > 0)
                {
                    SelectedSegment = Segments[0];
                }
            }
        }

        [RelayCommand]
        private void GenerateAndExport()
        {
            try
            {
                var generator = new CentralStructureGenerator();
                foreach (var segmentVm in Segments)
                {
                    generator.Segments.Add(segmentVm.ToDomainModel());
                }

                // 1. Generate Domain Geometry
                generator.Generate();

                // 2. Export to STAAD .std file
                // We'll export to a fixed path for testing (or you could use SaveFileDialog)
                string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filePath = Path.Combine(docsPath, "RakeCentralStructure.std");
                
                StaadFileExporter.ExportToStd(generator, filePath);

                var message = new StringBuilder()
                    .AppendLine($"Generated {generator.AllNodes.Count} nodes, {generator.AllBeams.Count} beams, and {generator.AllPlates.Count} plates.")
                    .AppendLine()
                    .AppendLine($"STAAD file saved to:")
                    .AppendLine(filePath);

                if (generator.RakeArmConnectionSegment != null)
                {
                    message.AppendLine()
                        .AppendLine("Reserved for future rake arms:")
                        .AppendLine(generator.RakeArmConnectionSegment.SegmentLabel);
                }

                if (generator.ValidationWarnings.Count > 0)
                {
                    message.AppendLine()
                        .AppendLine("Validation notes:");

                    foreach (string warning in generator.ValidationWarnings)
                    {
                        message.AppendLine($"- {warning}");
                    }
                }

                MessageBox.Show(message.ToString(),
                    "Generation Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating structure: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterSegment(IStructureSegmentViewModel segment)
        {
            segment.PropertyChanged += Segment_PropertyChanged;
        }
    }
}
