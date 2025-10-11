using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace InbresTest.Views.Converters;

public class HandleOffsetConverter : IValueConverter
{
    private const double HandleSizeRatio = 5.0; 

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!(value is double dimension) || targetType != typeof(double))
        {
            return new BindingNotification(new ArgumentException("Input must be a double dimension."), BindingErrorType.Error);
        }
        
        // parameter = "Corner" (для угловых маркеров)
        if ((string)parameter! == "Corner")
        {
            // Вычисляем половину размера маркера: Dimension / 10 / 2
            double handleHalfSize = dimension / (HandleSizeRatio * 2.0); 
            // Позиция для угла (0,0) - сдвиг назад
            return -handleHalfSize; 
        }
        
        // parameter = "Center" (для центральных маркеров по оси, используя Width или Height)
        if ((string)parameter! == "Center")
        {
            // Размер маркера: dimension / HandleSizeRatio
            double handleSize = dimension / HandleSizeRatio;
                
            // Позиция
            return dimension - (handleSize * HandleSizeRatio/2) - HandleSizeRatio; 
        }

        return new BindingNotification(new ArgumentException("Invalid parameter for HandleOffsetConverter."), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
