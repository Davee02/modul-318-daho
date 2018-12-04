using SwissTransport.App.Helper;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SwissTransport.App.Model;

namespace SwissTransport.App.ViewModel
{
    public class NearStationsViewModel : ViewModelBase, ITransportResultViewModel
    {
        private readonly ITransport m_transport;
        private ObservableCollection<Station> m_nearStations = new ObservableCollection<Station>();
        private Coordinate m_foundCoordinates = new Coordinate();
        private Station m_selectedManualStation = new Station();
        private ObservableCollection<Station> m_stations = new ObservableCollection<Station>();
        private string m_searchText;
        private bool m_locaterIsActive = false;


        public Station SelectedStation { get; set; }
        public bool LocaterIsActive
        {
            get => m_locaterIsActive;
            set => SetProperty(ref m_locaterIsActive, value);
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
        public ObservableCollection<Station> Stations
        {
            get => m_stations;
            set => SetProperty(ref m_stations, value);
        }

        public Station SelectedManualStation
        {
            get => m_selectedManualStation;
            set
            {
                m_selectedManualStation = value;
                FoundCoordinates = m_selectedManualStation?.Coordinate;
                UpdateStations();
            }
        }

        public ObservableCollection<Station> NearStations
        {
            get => m_nearStations;
            set => SetProperty(ref m_nearStations, value);
        }

        public Coordinate FoundCoordinates
        {
            get => m_foundCoordinates;
            set => SetProperty(ref m_foundCoordinates, value);
        }

        public ICommand GetDevicePosition { get; set; }
        public ICommand ShowStation { get; set; }

        public NearStationsViewModel()
        {
            m_transport = new Transport();
            GetDevicePosition = new RelayCommand(x => GetCurrentPosition());
            ShowStation = new RelayCommand(x => Helper.Helper.OpenGoogleMapsWithCoordinates(SelectedStation.Coordinate));
        }

        /// <summary>
        /// Tries to get the current coordinates and displays a warning-messagebox, when it fails
        /// </summary>
        private async void GetCurrentPosition()
        {
            LocaterIsActive = true;
            bool success = true;
            await Task.Run(async () =>
                {
                    try
                    {
                        var geoLocator = new GeoLocator();
                        if (geoLocator.StartLocator() && geoLocator.HasPermissions)
                        {
                            await Task.Delay(500);

                            int i = 0;
                            // Try to get the current location 20 times with a delay of 0.2 seconds between the tries
                            while (i < 20)
                            {
                                if (geoLocator.LocatorIsReady)
                                {
                                    FoundCoordinates = geoLocator.GetLocation();
                                    break;
                                }

                                await Task.Delay(200);
                                i++;
                            }
                        }
                        else
                        {
                            MessageBox.Show(
                                "Die Position konnte leider nicht ermittelt werden. Bitte aktivieren Sie die Positionsdienste in Ihren Windows-Einstellungen und versuchen Sie es in ein paar Sekunden erneut.",
                                "Positionierung fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Warning);
                            success = false;
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Die Position konnte leider nicht ermittelt werden. Bitte aktivieren Sie die Positionsdienste in Ihren Windows-Einstellungen und versuchen Sie es in ein paar Sekunden erneut.",
                            "Positionierung fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Warning);
                        success = false;
                        return;
                    }
                }
            );
            LocaterIsActive = false;

            if(success)
                UpdateStations();
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

        /// <summary>
        /// Set the collection, which holds all the station-items, new
        /// </summary>
        private async void UpdateStations()
        {
            var stations = await m_transport.GetStations(FoundCoordinates);
            if (stations == null || stations.StationList?.Count == 0)
            {
                MessageBox.Show(
                    $"In der Nähe der Koordinaten \"{FoundCoordinates}\" konnte leider keine Station gefunden werden. Versuchen Sie die Suche erneut.",
                    "Keine Station gefunden", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                NearStations = stations.StationList
                    .Where(x => x.Id != null)
                    .ToObservableCollection();
            }
        }

        public void SendResultsAsMail()
        {
            var mailContentBuilder = new StringBuilder();
            mailContentBuilder.AppendLine("Hallo!</br>");
            mailContentBuilder.AppendLine(
                $"Sieh dir an, welche Stationen ich in der Nähe der Koordinaten {FoundCoordinates} gefunden habe:\n");
            mailContentBuilder.Append("<table border='1' style='border-collapse:collapse' cellpadding='5'");
            mailContentBuilder.Append("<tr><th>Name</th><th>Koordinaten</th></tr>");

            foreach (var station in NearStations)
            {
                mailContentBuilder.Append("<tr>");
                mailContentBuilder.Append($"<td>{station}</td>");
                mailContentBuilder.Append($"<td>{station.Coordinate}</td>");
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
