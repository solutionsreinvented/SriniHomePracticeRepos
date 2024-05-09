using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

using ReInvented.AsyncPracticeApp.Base;
using ReInvented.AsyncPracticeApp.Interfaces;
using ReInvented.AsyncPracticeApp.Models;
using ReInvented.Shared.Commands;

namespace ReInvented.AsyncPracticeApp.ViewModels
{
    public class ProgressViewModel : BaseViewModel, IProgressViewModel
    {
        #region Default Constructor

        public ProgressViewModel(Application application, IViewProvider viewProvider) : base(application, viewProvider)
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public ObservableCollection<IActionItem> Actions { get; set; }

        #endregion

        #region Commands

        public ICommand AddNewActionItemCommand { get; private set; }

        #endregion

        #region Command Handlers

        private void OnAddNewActionItem()
        {

        }

        #endregion

        #region Public Functions

        public IActionItem CreateNewActionItem(string caption, string description)
        {
            return new ActionItem() { Caption = caption, Description = description };
        }

        public void AddNewActionItem(string caption, string description)
        {
            Actions.Add(CreateNewActionItem(caption, description));
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Actions = new ObservableCollection<IActionItem>();
            Actions.CollectionChanged += OnActionsCollectionChanged;
            AddNewActionItemCommand = new RelayCommand(OnAddNewActionItem, true);
        }

        #endregion

        #region Event Handlers

        private void OnActionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<IActionItem> collection = sender as ObservableCollection<IActionItem>;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (collection.Count > 1)
                {
                    IActionItem newItem = collection[collection.Count - 1];
                    IActionItem prevItem = collection[collection.Count - 2];
                    prevItem.FinishedAt = newItem.StartedAt;
                }
            }
        }

        #endregion
    }
}
