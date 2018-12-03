using System;
using System.Device.Location;

namespace SwissTransport.App.Helper
{
    public class GeoLocator
    {
        private readonly GeoCoordinateWatcher m_watcher;

        /// <summary>
        /// If the GeoCoordinateWatcher has the permissions to use the location-services
        /// </summary>
        public bool HasPermissions => m_watcher.Permission == GeoPositionPermission.Granted;

        /// <summary>
        /// If the GeoCoordinateWatcher is ready to locate
        /// </summary>
        public bool LocatorIsReady => m_watcher.Status == GeoPositionStatus.Ready;

        public GeoLocator()
        {
            m_watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
        }

        /// <summary>
        /// Tries to start the GeoCoordinateWatcher within 3 seconds
        /// </summary>
        /// <returns>The Success of the start</returns>
        public bool StartLocator()
        {
            return m_watcher.TryStart(false, TimeSpan.FromSeconds(3));
        }

        /// <summary>
        /// Asks the GeoCoordinateWatcher for the current location.
        /// If the location is unknown, the X- and Y-Coordinates are "NaN"
        /// </summary>
        /// <returns>The coordinates of the current location.</returns>
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