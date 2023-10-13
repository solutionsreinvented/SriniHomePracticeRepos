using System.ComponentModel;
using System.Windows;

using FluentValidation.Results;

using ReInvented.FluentValidationExercise.Models;
using ReInvented.FluentValidationExercise.Validators;
using ReInvented.Shared.Stores;

namespace DesignStructs.FluentValidationExercise.ViewModels
{
    public class MainViewModel : ErrorsEnabledPropertyStore
    {

        public MainViewModel()
        {
            Input = new Input();
            Validator = new ThickenerInputValidator();
            ///AttachEvents();
        }

        private void AttachEvents()
        {
            if (Input.Shell != null)
            {
                Input.Shell.PropertyChanged -= OnShellPropertyChanged;
                Input.Shell.PropertyChanged += OnShellPropertyChanged;
            }
            if (Input.FeedWell != null)
            {
                Input.FeedWell.PropertyChanged -= OnFeedWellPropertyChanged;
                Input.FeedWell.PropertyChanged += OnFeedWellPropertyChanged;
            }
        }

        private void OnShellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateInputData();
        }
        private void OnFeedWellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateInputData();
        }

        public Input Input { get; private set; }

        public ThickenerInputValidator Validator { get; private set; }

        private void ValidateInputData()
        {
            ValidationResult results = Validator.Validate(Input);

            foreach (ValidationFailure f in results.Errors)
            {
                MessageBox.Show("Invalid data identified");
            }
        }

    }
}
