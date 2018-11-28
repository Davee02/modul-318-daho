using System;
using System.Collections.ObjectModel;
using SwissTransport.App.Helper;

namespace SwissTransport.App.ViewModel
{
    public class ConnectionFinderViewModel : ViewModelBase
    {
        private readonly Transport m_transport = new Transport();

        private ObservableCollection<Station> m_startStations = new ObservableCollection<Station>();
        private ObservableCollection<Station> m_stopStations = new ObservableCollection<Station>();
        private string m_startSearchText;
        private string m_stopSearchText;


        public DateTime SelectedDateTime { get; set; } = DateTime.Now;
        public Station StartStation { get; set; } = new Station();
        public Station StopStation { get; set; } = new Station();

        public ObservableCollection<Station> StartStations
        {
            get => m_startStations;
            set => SetProperty(ref m_startStations, value);
        }

        public ObservableCollection<Station> StopStations
        {
            get => m_stopStations;
            set => SetProperty(ref m_stopStations, value);
        }

        public string StartSearchText
        {
            get => m_startSearchText;
            set
            {
                if (!string.Equals(value, m_startSearchText))
                {
                    StartStations = m_transport.GetStations(value).StationList.ToObservableCollection();
                    SetProperty(ref m_startSearchText, value);
                }
            }
        }

        public string StopSearchText
        {
            get => m_stopSearchText;
            set
            {
                if (!string.Equals(value, m_stopSearchText))
                {
                    StopStations = m_transport.GetStations(value).StationList.ToObservableCollection();
                    SetProperty(ref m_stopSearchText, value);
                }
            }
        }

        public ConnectionFinderViewModel()
        {

        }


    }
}
