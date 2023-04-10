using ReInvented.UI.Themes.Enums;
using ReInvented.UI.Themes.Models;

namespace Practice.Animations.ViewModels
{
    public class ThemeViewModel
    {

        public ThemeViewModel()
        {
            Theme = new Theme();

            Theme.LoadThemeType(ThemeType.Dark);
        }

        public Theme Theme { get; set; }
    }

}
