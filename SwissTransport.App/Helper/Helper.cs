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
    }
}
