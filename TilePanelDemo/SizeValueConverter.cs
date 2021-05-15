using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace TilePanelDemo
{
    public class SizeValueConverter : IValueConverter
    {
        public static readonly SizeValueConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Size size)
            {
                return size.ToString();
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return Size.Parse(s);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}