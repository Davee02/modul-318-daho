using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    }
}
