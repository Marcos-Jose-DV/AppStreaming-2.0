using System.Globalization;

namespace AppAvaliacao.Converters;

internal class CardBackGroundColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var concluded = (bool)value;
        var color = Color.FromRgba("#3255e2");
        if (concluded)
        {
            return color;
        }

        color = Color.FromRgba("#e92525");
        return color;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
