namespace ReIn.ViewModelPercolation.ObjectPassing
{
    public class BaseViewModel
    {
        public PassedObject PassedObject { get; set; }
    }

    public class OtherViewModel : BaseViewModel
    {

    }

    public class AnotherViewModel : BaseViewModel
    {

    }

    public class NavigationStore
    {
        public event 

        public BaseViewModel CurrentViewModel { get; set; }
    }


    public class PassedObject
    {
        public string Name { get; set; }

        public string EmailId { get; set; }

    }


}
