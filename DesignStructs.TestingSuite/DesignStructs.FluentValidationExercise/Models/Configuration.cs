using System.Windows.Media;

using Newtonsoft.Json;

using ReInvented.Shared.Stores;

namespace DesignStructs.FluentValidationExercise.Models
{
    public enum ThemeType
    {
        Light,
        Dark,
        Custom,
        Default
    }

    public class Configuration : ValidatablePropertyStore
    {
        #region Default Constructor

        public Configuration()
        {
            ApplicationDirectory = @"\\10.67.12.30\product_engineering\Civil_Structural\ThickenerModelGenerator\ApplicationData";
        }

        #endregion

        #region Public Properties

        [JsonProperty]
        public string ApplicationDirectory { get => Get<string>(); private set => Set(value); }

        public UserPreferences UserPreferences { get => Get<UserPreferences>(); set => Set(value); }

        #endregion
    }

    public class UserPreferences : ValidatablePropertyStore
    {
        #region Default Constructor

        public UserPreferences()
        {

        }

        #endregion

        #region Public Properties

        public ThemeType ThemeType { get => Get<ThemeType>(); set => Set(value); }

        public FontFamily FontFamily { get => Get<FontFamily>(); set => Set(value); }

        public double NormalFontSize { get => Get<double>(); set => Set(value); }

        public double SubFontSize { get => Get<double>(); set => Set(value); }

        #endregion

    }
}
