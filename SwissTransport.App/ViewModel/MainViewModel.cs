using System.Collections.ObjectModel;

namespace SwissTransport.App.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<object> TabChildren { private get; set; } = new ObservableCollection<object>();

        public MainViewModel()
        {
            TabChildren.Add(new ConnectionFinderViewModel());
            TabChildren.Add(new DepartureScheduleViewModel());
        }
    }
}
