using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace App.Converters;

public class IsNotEqualsMultiConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values.Count < 2 ? true : !values[0]?.Equals(values[1]);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}