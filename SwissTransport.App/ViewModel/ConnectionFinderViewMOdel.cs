using SwissTransport.App.Helper;
using SwissTransport.App.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Input;

namespace SwissTransport.App.ViewModel
{
    public class ConnectionFinderViewModel : ViewModelBase, ITransportResultViewModel
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


        public Connection SelectedConnection { get; set; }
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
                SetProperty(ref m_startStation, value);
                UpdateConnections();
            }
        }

        public Station StopStation
        {
            get => m_stopStation;
            set
            {
                SetProperty(ref m_stopStation, value);
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

        public ICommand SwitchStations { get; set; }
        public ICommand ShowStartStation { get; set; }
        public ICommand ShowStopStation { get; set; }

        public string StartSearchText
        {
            get => m_startSearchText;
            set
            {
                if (!string.Equals(value, m_startSearchText))
                {
                    SetStartStationsMatchingToSearchText(value);

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
                    SetStopStationsMatchingToSearchText(value);

                    SetProperty(ref m_stopSearchText, value);
                }
            }
        }

        public ConnectionFinderViewModel()
        {
            m_transport = new Transport();
            SwitchStations = new RelayCommand(x => SwitchStartAndStopStations());
            ShowStartStation = new RelayCommand(x => Helper.Helper.OpenGoogleMapsWithCoordinates(SelectedConnection.From.Station.Coordinate));
            ShowStopStation = new RelayCommand(x => Helper.Helper.OpenGoogleMapsWithCoordinates(SelectedConnection.To.Station.Coordinate));
        }

        /// <summary>
        /// Set the collection, which holds all the connection-items, new
        /// </summary>
        private async void UpdateConnections()
        {
            if (StartStation?.Id != null && StopStation?.Id != null)
            {
                var allConnections = m_transport.GetConnections(StartStation.Id, StopStation.Id, 16, SelectedDateTime);
                Connections = (await allConnections).ConnectionList.ToObservableCollection();
            }
        }

        /// <summary>
        /// Switchs the Start- and Stopstation
        /// </summary>
        private void SwitchStartAndStopStations()
        {
            (StartStation, StopStation) = (StopStation, StartStation);

            var tmp = StartSearchText;
            StartSearchText = StopSearchText;
            StopSearchText = tmp;
        }

        /// <summary>
        /// Setting the StartStation-collection to the stations which are matching the provided filter
        /// </summary>
        /// <param name="searchText">The filter which is applied to the stations</param>
        private async void SetStartStationsMatchingToSearchText(string searchText)
        {
            StartStations = (await m_transport.GetStations(searchText)).StationList
                .Where(x => x.Id != null)
                .ToObservableCollection();
        }

        /// <summary>
        /// Setting the StopStation-collection to the stations which are matching the provided filter
        /// </summary>
        /// <param name="searchText">The filter which is applied to the stations</param>
        private async void SetStopStationsMatchingToSearchText(string searchText)
        {
            StopStations = (await m_transport.GetStations(searchText)).StationList
                .Where(x => x.Id != null)
                .ToObservableCollection();
        }

        public void SendResultsAsMail()
        {
            var mailContentBuilder = new StringBuilder();
            mailContentBuilder.AppendLine("Hallo!</br>");
            mailContentBuilder.AppendLine(
                $"Sieh dir an, welche Verbindungen ich von {StartStation} nach {StopStation} am {SelectedDateTime:D} um {SelectedDateTime:t} gefunden habe:\n");
            mailContentBuilder.Append("<table border='1' style='border-collapse:collapse' cellpadding='5'");
            mailContentBuilder.Append("<tr><th>Abfahrt</th><th>Gleis - Kante</th><th>Ankunft</th><th>Reisedauer</th></tr>");

            foreach (var connection in Connections)
            {
                mailContentBuilder.Append("<tr>");
                mailContentBuilder.Append($"<td>{connection.From.Station} - {connection.From.Departure:g}</td>");
                mailContentBuilder.Append($"<td>{connection.From.Platform}</td>");
                mailContentBuilder.Append($"<td>{connection.To.Station} - {connection.To.Arrival:g}</td>");
                mailContentBuilder.Append($"<td>{connection.Duration}</td>");
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
