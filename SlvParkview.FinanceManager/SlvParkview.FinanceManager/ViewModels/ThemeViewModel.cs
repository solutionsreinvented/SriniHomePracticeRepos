using ReInvented.Shared.Stores;
using ReInvented.UI.Themes.Enums;
using ReInvented.UI.Themes.Models;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ThemeViewModel : PropertyStore
    {
        public ThemeViewModel()
        {
            SelectedTheme = ThemeType.Blue;
        }

        public ThemeType SelectedTheme { get => Get<ThemeType>(); set { Set(value); UpdateSelectedThemeChanged(); } }

        private void UpdateSelectedThemeChanged()
        {
            Theme.LoadThemeType(SelectedTheme);
        }
    }
}
