using SwissTransport.App.Helper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SwissTransport.App.Model;

namespace SwissTransport.App.ViewModel
{
    public class StationBoardViewModel : ViewModelBase, ITransportResultViewModel
    {
        private readonly ITransport m_transport;

        private ObservableCollection<Station> m_stations = new ObservableCollection<Station>();
        private Station m_selectedStation = new Station();
        private DateTime m_selectedDateTime = DateTime.Now;
        private string m_searchText;
        private ObservableCollection<StationBoard> m_stationBoards = new ObservableCollection<StationBoard>();


        public ObservableCollection<StationBoard> StationBoards
        {
            get => m_stationBoards;
            set => SetProperty(ref m_stationBoards, value);
        }

        public DateTime SelectedDateTime
        {
            get => m_selectedDateTime;
            set
            {
                SetProperty(ref m_selectedDateTime, value);
                UpdateStationsBoard();
            }
        }

        public Station SelectedStation
        {
            get => m_selectedStation;
            set
            {
                m_selectedStation = value;
                UpdateStationsBoard();
            }
        }

        public StationBoard SelectedStationBoard { get; set; }
        public ObservableCollection<Station> Stations
        {
            get => m_stations;
            set => SetProperty(ref m_stations, value);
        }

        public string SearchText
        {
            get => m_searchText;
            set
            {
                if (!string.Equals(value, m_searchText))
                {

                    Stations = m_transport.GetStations(value).Result.StationList
                        .Where(x => x.Id != null)
                        .ToObservableCollection();

                    SetProperty(ref m_searchText, value);
                }
            }
        }

        public ICommand ShowStation { get; set; }

        public StationBoardViewModel()
        {
            m_transport = new Transport();
            ShowStation = new RelayCommand(x =>
                Helper.Helper.OpenGoogleMapsWithCoordinates(SelectedStationBoard.Stop.Station.Coordinate));
        }

        /// <summary>
        /// Set the collection, which holds all the stationboard-items, new
        /// </summary>
        private async void UpdateStationsBoard()
        {
            if (SelectedStation?.Id != null)
            {
                StationBoards = (await m_transport.GetStationBoard(SelectedStation.Id, SelectedDateTime)).Entries.ToObservableCollection();
            }
        }

        public void SendResultsAsMail()
        {
            throw new NotImplementedException();
        }
    }
}