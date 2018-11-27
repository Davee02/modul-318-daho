using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace SwissTransport
{
    public class Transport : ITransport
    {
        public Stations GetStations(string query)
        {
            var request = CreateWebRequest("http://transport.opendata.ch/v1/locations?query=" + query);
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            if (responseStream != null)
            {
                using (var reader = new StreamReader(responseStream))
                {
                    return JsonConvert.DeserializeObject<Stations>(reader.ReadToEnd(),
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }
            }

            return null;
        }

        public StationBoardRoot GetStationBoard(string station, string id)
        {
            var request = CreateWebRequest($"http://transport.opendata.ch/v1/stationboard?station={station}&id={id}");
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            if (responseStream != null)
            {
                using (var stream = new StreamReader(responseStream))
                {
                    return JsonConvert.DeserializeObject<StationBoardRoot>(stream.ReadToEnd());
                }
            }

            return null;
        }

        public Connections GetConnections(string fromStation, string toStation)
        {
            var request =
                CreateWebRequest($"http://transport.opendata.ch/v1/connections?from={fromStation}&to={toStation}");
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            if (responseStream != null)
            {
                using (var reader = new StreamReader(responseStream))
                {
                    return JsonConvert.DeserializeObject<Connections>(reader.ReadToEnd());
                }
            }

            return null;
        }

        private static WebRequest CreateWebRequest(string url)
        {
            var request = WebRequest.Create(url);
            var webProxy = WebRequest.DefaultWebProxy;

            webProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.Proxy = webProxy;

            return request;
        }
    }
}
