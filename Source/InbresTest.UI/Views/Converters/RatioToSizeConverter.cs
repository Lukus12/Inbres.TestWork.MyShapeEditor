using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace InbresTest.Views.Converters;

public class RatioToSizeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double dimension && parameter is string ratioString)
        {
            if (double.TryParse(ratioString, out double ratio) && ratio != 0)
            {
                return dimension / ratio; // Возвращаем размер маркера
            }
        }
        return Avalonia.Data.BindingErrorType.Error;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}