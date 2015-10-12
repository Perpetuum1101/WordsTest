using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WordTes.UI.Models
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var visible = (bool)value;

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visible = (Visibility)value;

            return visible == Visibility.Visible;
        }
    }
}
