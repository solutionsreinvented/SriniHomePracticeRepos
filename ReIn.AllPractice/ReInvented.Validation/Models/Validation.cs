using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Collections;

namespace ReInvented.Validation.Models
{

    public class ValidationClass : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private OtherOne _anotherOne;
        private OtherTwo _anotherTwo;
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public OtherOne AnotherOne
        {
            get => _anotherOne;
            set
            {
                if (_anotherOne != value)
                {
                    _anotherOne = value;
                    OnPropertyChanged();
                    ValidateProperty(nameof(AnotherOne));
                }
            }
        }

        public OtherTwo AnotherTwo
        {
            get => _anotherTwo;
            set
            {
                if (_anotherTwo != value)
                {
                    _anotherTwo = value;
                    OnPropertyChanged();
                    ValidateProperty(nameof(AnotherTwo));
                }
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // INotifyDataErrorInfo implementation
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                return _errors[propertyName];
            }
            else
            {
                return null;
            }
        }

        private void ValidateProperty(string propertyName)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };

            Validator.TryValidateProperty(GetType().GetProperty(propertyName).GetValue(this), context, validationResults);

            if (validationResults.Count > 0)
            {
                _errors[propertyName] = new List<string>(validationResults.Select(r => r.ErrorMessage));
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    public class OtherOne : INotifyPropertyChanged
    {
        private double _propertyOne;
        private double _propertyTwo;

        public double PropertyOne
        {
            get => _propertyOne;
            set
            {
                if (_propertyOne != value)
                {
                    _propertyOne = value;
                    OnPropertyChanged();
                }
            }
        }

        public double PropertyTwo
        {
            get => _propertyTwo;
            set
            {
                if (_propertyTwo != value)
                {
                    _propertyTwo = value;
                    OnPropertyChanged();
                }
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OtherTwo : INotifyPropertyChanged
    {
        private double _propertyThree;
        private double _propertyFour;

        public double PropertyThree
        {
            get => _propertyThree;
            set
            {
                if (_propertyThree != value)
                {
                    _propertyThree = value;
                    OnPropertyChanged();
                }
            }
        }

        public double PropertyFour
        {
            get => _propertyFour;
            set
            {
                if (_propertyFour != value)
                {
                    _propertyFour = value;
                    OnPropertyChanged();
                }
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
