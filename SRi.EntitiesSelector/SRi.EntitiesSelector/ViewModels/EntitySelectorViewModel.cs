using SRi.EntitiesSelector.Base;
using SRi.EntitiesSelector.Commands;
using SRi.EntitiesSelector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SRi.EntitiesSelector.ViewModels
{
    public class EntitySelectorViewModel : Observable
    {
        private StaadModel _staadModel;
        private double _startDiameter;
        private double _endDiameter;

        public EntitySelectorViewModel()
        {
            StaadModel = new StaadModel();
            StartDiameter = 13.42779;
            EndDiameter = 24.85655;
            GetEntitiesCommand = new RelayCommand(GetEntities, true);
        }

        public StaadModel StaadModel
        {
            get => _staadModel;
            set
            {
                if (value != _staadModel)
                {
                    _staadModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public double StartDiameter
        {
            get => _startDiameter;
            set
            {
                if (value != _startDiameter)
                {
                    _startDiameter = value;
                    OnPropertyChanged();
                }
            }
        }

        public double EndDiameter
        {
            get => _endDiameter;
            set
            {
                if (value != _endDiameter)
                {
                    _endDiameter = value;
                    OnPropertyChanged();
                }

            }
        }

        public ICommand GetEntitiesCommand { get; set; }

        private void GetEntities()
        {
            StaadModel.GetEntities();

            HashSet<Plate> inRangePlates = GetPlatesInRange();

            if (inRangePlates.Any())
            {
                int[] platesList = (from p in inRangePlates select p.Id).ToArray();

                OpenStaadProvider.OSGeometry.SelectMultiplePlates(platesList);
            }
        }

        private HashSet<Plate> GetPlatesInRange()
        {
            HashSet<Plate> inRangePlates = new HashSet<Plate>();

            foreach (var p in StaadModel.Plates)
            {
                if (IsPlateInRange(p))
                {
                    inRangePlates.Add(p);
                }
            }

            return inRangePlates;

        }

        private bool IsPlateInRange(Plate p)
        {
            double diaA = DiaOf(p.A);
            double diaB = DiaOf(p.B);
            double diaC = DiaOf(p.C);

            return CheckDiameter(diaA) && CheckDiameter(diaB) && CheckDiameter(diaC) && (p.D == null || CheckDiameter(DiaOf(p.D)));
        }

        private double DiaOf(Node node) => 2 * (node.X * node.X + node.Z * node.Z);

        private bool CheckDiameter(double nodeDia) => nodeDia >= StartDiameter && nodeDia <= EndDiameter;


    }
}
