using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SwissTransport.App.Helper
{
    public static class Helper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }

        public static string GetGoogleMapsLinkForCoordinates(Coordinate coordinates)
        {
            return $"http://www.google.com/maps/place/{coordinates.XCoordinate},{coordinates.YCoordinate}/data=!3m1!1e3";
        }
    }
}
