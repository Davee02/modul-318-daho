using System;

namespace SwissTransport
{
    public interface ITransport
    {
        Stations GetStations(string query);
        StationBoardRoot GetStationBoard(string id, DateTime departureDateTime);
        Connections GetConnections(string fromStation, string toStattion, int connectionsCount, DateTime departureDateTime);
    }
}