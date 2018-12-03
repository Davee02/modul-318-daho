using System;
using System.Threading.Tasks;

namespace SwissTransport
{
    public interface ITransport
    {
        Task<Stations> GetStations(string query);
        Task<Stations> GetStations(Coordinate coordinates);
        Task<StationBoardRoot> GetStationBoard(string id, DateTime departureDateTime);
        Task<Connections> GetConnections(string fromStation, string toStattion, int connectionsCount, DateTime departureDateTime);
    }
}