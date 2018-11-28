using System;

namespace SwissTransport.App.ViewModel
{
    public class ConnectionFinderViewModel : ViewModelBase
    {
        public DateTime SelectedDateTime { get; set; } = DateTime.Now;
    }
}
