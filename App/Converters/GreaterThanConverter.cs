using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace App.Converters;

public class GreaterThanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        if (value == AvaloniaProperty.UnsetValue)
            return false;

        try
        {
            var currentValue = System.Convert.ToDouble(value);
            var compareValue = System.Convert.ToDouble(parameter);

            return currentValue > compareValue;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Conversion error: {ex.Message}");
            return false;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}