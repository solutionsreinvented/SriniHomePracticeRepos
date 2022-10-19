using System.Collections.ObjectModel;

using ReInvented.Shared.Stores;

namespace ReIn.NavPractice.Models
{
    public class SupportStructure : PropertyStore
    {
        public SupportStructure()
        {
            GridRows = GenerateGridRows(5);
        }

        public ObservableCollection<GridRow> GridRows { get => Get<ObservableCollection<GridRow>>(); set => Set(value); }


        #region Helper Functions

        private Section GenerateColumn(int counter)
        {
            Section section = new Section() { Height = 200.0 + (counter * 25), Width = 150.0 + (counter * 10.0) };
            section.Designation = $"ISMB {section.Height}x{section.Width}";

            return section;
        }

        private Section GenerateRadialBracing(int counter)
        {
            Section section = new Section() { Height = 90.0 + counter * 10, Width = 90.0 + counter * 10.0 };
            section.Designation = $"SHS {section.Height}x{section.Width}";

            return section;
        }

        private Section GenerateCrossBracing(int counter)
        {
            Section section = new Section() { Designation = $"Cross Brace {counter}", Height = 60.0 + counter * 15, Width = 60.0 + counter * 15.0 };
            section.Designation = $"SHS {section.Height}x{section.Width}";

            return section;
        }

        private ObservableCollection<GridRow> GenerateGridRows(int numberOfRows = 1)
        {
            ObservableCollection<GridRow> rows = new ObservableCollection<GridRow>();

            for (int i = 0; i < numberOfRows; i++)
            {
                rows.Add(new GridRow()
                {
                    Description = $"PCD{i + 1}",
                    PCD = 10 + (2 * (i * 4.5)),
                    HasCrossBracing = i % 2 == 0,
                    HasRadialBracing = i % 2 != 0,
                    Column = GenerateColumn(i),
                    CrossBracing = GenerateCrossBracing(i),
                    RadialBracing = GenerateRadialBracing(i)
                });
            }

            return rows;
        }

        #endregion    
    }
}
