using System.Windows;

using DesignStructs.FluentValidationExercise.Models;
using DesignStructs.FluentValidationExercise.Validators;

using FluentValidation.Results;

namespace DesignStructs.FluentValidationExercise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ThickenerInput input = new ThickenerInput();
            ThickenerInputValidator validator = new ThickenerInputValidator();

            MainWindow = new MainWindow() { DataContext = input };

            MainWindow.Show();

            ValidationResult results = validator.Validate(input);

            foreach (ValidationFailure f in results.Errors)
            {

            }
        }
    }
}
