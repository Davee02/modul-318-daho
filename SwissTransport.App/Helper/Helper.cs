using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows;

namespace SwissTransport.App.Helper
{
    public static class Helper
    {
        /// <summary>
        /// Converts generic collection to an ObservableCollection
        /// </summary>
        /// <param name="source">The source-collection</param>
        /// <returns>An ObservableCollection with the same items and type as the source</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }

        /// <summary>
        /// Generates a link to Google-Maps with a marker on the coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates of the desired marker</param>
        /// <returns>URL, which can be browsed in an internet browser</returns>
        public static string GetGoogleMapsLinkForCoordinates(Coordinate coordinates)
        {
            return $"http://www.google.com/maps/place/{coordinates.XCoordinate},{coordinates.YCoordinate}/data=!3m1!1e3";
        }

        /// <summary>
        /// Opens the link to Google-Maps with a marker in the standard-browser
        /// </summary>
        /// <param name="coordinates">The coordinates where the marker should be placed</param>
        public static void OpenGoogleMapsWithCoordinates(Coordinate coordinates)
        {
            if (coordinates != null)
            {
                Process.Start(SwissTransport.App.Helper.Helper.GetGoogleMapsLinkForCoordinates(coordinates));
            }
            else
            {
                MessageBox.Show(
                    "Die ausgewählte Station kann leider nicht angezeigt werden, da dafür keine Koordinaten gespeichert sind.",
                    "Station kann nicht angezeigt werden.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
