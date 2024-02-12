using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using FluentValidation;

namespace AllCorePracticeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Launder launder = new Launder();
            launder.PropertyChanged += OnLaunderPropertyChanged;
            launder.Height = 6.0;
        }

        private void OnLaunderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Launder launder = sender as Launder;
            var results = LaunderValidator.Instance.Validate(launder);
            throw new NotImplementedException();
        }
    }


    public class LaunderValidator : AbstractValidator<Launder>
    {
        private static LaunderValidator _instance;

        public static LaunderValidator Instance => _instance ?? (_instance = new LaunderValidator());

        private LaunderValidator()
        {
            RuleFor(launder => launder.Height).GreaterThan(0.0).WithMessage("Launder height shall be greater than 0.0").LessThan(5.5).WithMessage("Launder height shall be less than 5.5");
        }

    }





















}
