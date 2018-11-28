using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SwissTransport
{
    public class Connections
    {
        [JsonProperty("connections")]
        public List<Connection> ConnectionList { get; set; } 
    }

    public class Connection
    {
        [JsonProperty("from")]
        public ConnectionPoint From  { get; set; }

        [JsonProperty("to")]
        public ConnectionPoint To { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }
    }

    public class ConnectionPoint
    {
        [JsonProperty("station")]
        public Station Station { get; set; }

        [JsonProperty("arrival")]
        public DateTime Arrival { get; set; }

        [JsonProperty("arrivalTimestamp")]
        public string ArrivalTimestamp { get; set; }

        [JsonProperty("departure")]
        public DateTime Departure { get; set; }

        [JsonProperty("departureTimestamp")]
        public string DepartureTimestamp { get; set; }

        [JsonProperty("delay")]
        public int? Delay { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("realtimeAvailability")]
        public string RealtimeAvailability { get; set; }
    }
}