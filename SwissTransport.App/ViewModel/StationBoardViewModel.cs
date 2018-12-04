using SwissTransport.App.Helper;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
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

                    SetStationsMatchingToSearchText(value);

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

        /// <summary>
        /// Setting the Station-collection to the stations which are matching the provided filter
        /// </summary>
        /// <param name="searchText">The filter which is applied to the stations</param>
        private async void SetStationsMatchingToSearchText(string searchText)
        {
            Stations = (await m_transport.GetStations(searchText)).StationList
                .Where(x => x.Id != null)
                .ToObservableCollection();
        }

        public void SendResultsAsMail()
        {
            var mailContentBuilder = new StringBuilder();
            mailContentBuilder.AppendLine("Hallo!</br>");
            mailContentBuilder.AppendLine(
                $"Sieh dir an, welche Verbindungen von {SelectedStation} am {SelectedDateTime:D} ab {SelectedDateTime:t} abfahren:\n");
            mailContentBuilder.Append("<table border='1' style='border-collapse:collapse' cellpadding='5'");
            mailContentBuilder.Append("<tr><th>Reiseziel</th><th>Linie</th><th>Abfahrtszeit</th></tr>");

            foreach (var stationBoard in StationBoards)
            {
                mailContentBuilder.Append("<tr>");
                mailContentBuilder.Append($"<td>{stationBoard.To}</td>");
                mailContentBuilder.Append($"<td>{stationBoard.Category} {stationBoard.Number}</td>");
                mailContentBuilder.Append($"<td>{stationBoard.Stop.Departure}</td>");
                mailContentBuilder.Append("</tr>");
            }
            mailContentBuilder.Append("</table>");

            var mailMessage = new MailMessage
            {
                Subject = "Resultate von TransportGate",
                IsBodyHtml = true,
                Body = mailContentBuilder.ToString(),
                From = new MailAddress("resultate@transportgate.ch")
            };

            var filename = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".eml");
            mailMessage.Save(filename);

            Process.Start(filename);
        }
    }
}