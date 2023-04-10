namespace Practice.Animations.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            ThemeViewModel = new ThemeViewModel();
        }

        public ThemeViewModel ThemeViewModel { get; set; }
    }
}
