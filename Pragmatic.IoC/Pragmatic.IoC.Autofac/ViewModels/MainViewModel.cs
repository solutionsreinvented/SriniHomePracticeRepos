using Pragmatic.IoC.Autofac.Models;

namespace Pragmatic.IoC.Autofac.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel(MainModel mainModel)
        {
            MainModel = mainModel;
        }
        public MainModel MainModel { get; set; }
    }
}
