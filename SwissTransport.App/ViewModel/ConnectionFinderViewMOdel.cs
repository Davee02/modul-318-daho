using SwissTransport.App.Helper;
using System;
using System.Collections.ObjectModel;

namespace SwissTransport.App.ViewModel
{
    public class ConnectionFinderViewModel : ViewModelBase
    {
        private readonly ITransport m_transport;

        private ObservableCollection<Station> m_startStations = new ObservableCollection<Station>();
        private ObservableCollection<Station> m_stopStations = new ObservableCollection<Station>();
        private string m_startSearchText;
        private string m_stopSearchText;
        private Station m_startStation = new Station();
        private Station m_stopStation = new Station();
        private ObservableCollection<Connection> m_connections = new ObservableCollection<Connection>();
        private DateTime m_selectedDateTime = DateTime.Now;


        public DateTime SelectedDateTime
        {
            get => m_selectedDateTime;
            set
            {
                SetProperty(ref m_selectedDateTime, value);
                UpdateConnections();
            }
        }

        public Station StartStation
        {
            get => m_startStation;
            set
            {
                m_startStation = value; 
                UpdateConnections();
            }
        }

        public Station StopStation
        {
            get => m_stopStation;
            set
            {
                m_stopStation = value;
                UpdateConnections();
            }
        }

        public ObservableCollection<Connection> Connections
        {
            get => m_connections;
            set => SetProperty(ref m_connections, value);
        }

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
            m_transport = new Transport();
        }

        private void UpdateConnections()
        {
            if (StartStation?.Id != null && StopStation?.Id != null)
            {
                var allConnections = m_transport.GetConnections(StartStation.Id, StopStation.Id, 16, SelectedDateTime);
                Connections = allConnections.ConnectionList.ToObservableCollection();
            }
        }

    }
}
