using System;
using System.Device.Location;

namespace SwissTransport.App.Helper
{
    public class GeoLocator
    {
        private readonly GeoCoordinateWatcher m_watcher;

        public bool HasPermissions => m_watcher.Permission == GeoPositionPermission.Granted;
        public bool LocatorIsReady => m_watcher.Status == GeoPositionStatus.Ready;

        public GeoLocator()
        {
            m_watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
        }

        public bool StartLocator()
        {
            return m_watcher.TryStart(false, TimeSpan.FromSeconds(2));
        }

        public Coordinate GetLocation()
        {
            return new Coordinate
            {
                XCoordinate = m_watcher.Position.Location.Latitude,
                YCoordinate = m_watcher.Position.Location.Longitude
            };
        }
    }
}