using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwissTransport
{
    [TestClass]
    public class TransportTest
    {
        private ITransport testee;

        [TestMethod]
        public void Locations()
        {
            testee = new Transport();
            var stations = testee.GetStations("Sursee,").Result;
            var stationsByCoordinates =
                testee.GetStations(new Coordinate {XCoordinate = 47.07164, YCoordinate = 8.34877}).Result;

            Assert.AreEqual(10, stations.StationList.Count);
            Assert.AreEqual(10, stationsByCoordinates.StationList.Count);
        }

        [TestMethod]
        public void StationBoard()
        {
            testee = new Transport();
            var stationBoard = testee.GetStationBoard("8502007", DateTime.Now).Result;

            Assert.IsNotNull(stationBoard);
        }

        [TestMethod]
        public void Connections()
        {
            testee = new Transport();
            var connections = testee.GetConnections("Sursee", "Luzern", 1, DateTime.Now).Result;

            Assert.IsNotNull(connections);
        }
    }
}
