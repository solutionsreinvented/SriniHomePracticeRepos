using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmDialogs.Interfaces;
using System.Windows;

namespace MvvmDialogs.Services
{
    public class DialogService : IDialogService
    {
        private readonly Window _owner;

        public DialogService(Window owner)
        {
            _owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        public IDictionary<Type, Type> Mappings { get; private set; }

        public void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
                                                  where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException(string.Format("Type {0} is already mapped to the type {1}.", typeof(TViewModel), typeof(TView)));
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            var viewType = Mappings[typeof(TViewModel)];

            IDialog dialog = (IDialog)Activator.CreateInstance(viewType);

            EventHandler<DialogCloseRequestedEventArgs> handler = null;

            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;

                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            };

            viewModel.CloseRequested += handler;
            dialog.DataContext = viewModel;
            dialog.Owner = _owner;

            return dialog.ShowDialog();
        }

        private void OnCloseRequested(object sender, DialogCloseRequestedEventArgs e)
        {

        }

    }
}
