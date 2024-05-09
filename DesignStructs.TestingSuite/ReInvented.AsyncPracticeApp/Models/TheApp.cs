using ReInvented.AsyncPracticeApp.Views;

namespace ReInvented.AsyncPracticeApp.Models
{
    public interface IViewProvider
    {
        HomeView GetHomeView();
        ProgressView GetProgressView();
    }
    public class ViewProvider : IViewProvider
    {
        private readonly HomeView _homeView;
        private readonly ProgressView _progressView;

        public ViewProvider()
        {
            _homeView = new HomeView();
            _progressView = new ProgressView();
        }

        public HomeView GetHomeView()
        {
            return _homeView;
        }

        public ProgressView GetProgressView()
        {
            return _progressView;
        }
    }

    public class TheApp
    {

    }
}
