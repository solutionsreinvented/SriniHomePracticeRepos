using System;
using System.Windows.Input;

using ReIn.VectorsPractice.Enums;
using ReIn.VectorsPractice.Factories;
using ReIn.VectorsPractice.Interfaces;
using ReIn.VectorsPractice.Models;

using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReIn.VectorsPractice.ViewModels
{
    public class MainViewModel : PropertyStore
    {
        public MainViewModel()
        {
            Bridge = new Bridge();
            AddGridCommand = new RelayCommand(OnAddGrid, true);
            RemoveGridCommand = new RelayCommand(OnRemoveGrid, true);
        }

        private void OnGridTypeChanged()
        {
            SelectedFrameGrid = FrameGridFactory.Create(SelectedFrameGrid.GridType);
        }

        private void OnRemoveGrid()
        {
            throw new NotImplementedException();
        }

        private void OnAddGrid()
        {
            SelectedFrameGrid = FrameGridFactory.Create(FrameGridType);
            Bridge.FrameGrids.Add(SelectedFrameGrid);
        }

        public Bridge Bridge { get => Get<Bridge>(); set => Set(value); }

        public IFrameGrid SelectedFrameGrid
        {
            get => Get<IFrameGrid>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(HasASelectedFrameGrid));
                SetEvents();
            }
        }

        private void SetEvents()
        {
            if (SelectedFrameGrid != null)
            {
                SelectedFrameGrid.GridTypeChanged += OnGridTypeChanged;
            }
        }

        public ICommand AddGridCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand RemoveGridCommand { get => Get<ICommand>(); set => Set(value); }

        public FrameGridType FrameGridType { get => Get<FrameGridType>(); set => Set(value); }

        public bool HasASelectedFrameGrid => SelectedFrameGrid != null;
    }
}
