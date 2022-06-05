using ReInvented.StylesExtractor.Models;
using ReInvented.StylesExtractor.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReInvented.StylesExtractor.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private IList<StructureGridRow> rows;

        public event PropertyChangedEventHandler PropertyChanged;


        public MainViewModel()
        {
            Rows = FakeDataService.GenerateRows();
        }

        public IList<StructureGridRow> Rows 
        { 
            get => rows;
            set
            {
                if (value != rows)
                {
                    rows = value; OnPropertyChanged(nameof(Rows));
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
