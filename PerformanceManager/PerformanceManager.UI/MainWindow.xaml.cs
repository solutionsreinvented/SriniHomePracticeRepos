using System.Windows;
///using ProdActivity.Domain.Extensions;

namespace PerformanceManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //DateTime dateTime = new(2022, 05, 19, 9, 10, 0);

            //var holidays = JsonConvert.DeserializeObject<HashSet<DateTime>>(File.ReadAllText(@"C:\Source\ProdActivity\ProdActivity.Domain\Data\Holidays.json"));

            //MessageBox.Show($"Next business hour: {dateTime.NextBusinessHour(9, holidays)}");

            /////StoreResourcesData();

            //RetrieveResourcesData();

        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        /*        private void RetrieveResourcesData()
                {

                    EngineeringResourcesRepository engineeringResourcesRepository = new();
                    List<IEngineeringResource> engineeringResources = engineeringResourcesRepository.GetAll();

                    IEngineeringResource srinivas = engineeringResources[0];

                    DesignActivity newActivity = new(1)
                    {
                        ActivityType = EngineeringActivityType.Design,
                        InitiatedOn = DateTime.Now,
                        ProjectType = ProjectType.Order,
                        Description = "Bridge Design 33m Thickener",
                        CurrentStatus = CompletionStatus.OnTrack
                    };

                    _ = newActivity.AllocatedResources.Add(srinivas);

                    newActivity.SetCompletionInHours(72);

                    srinivas.Activities.Add(newActivity);

                } */

    }
}
