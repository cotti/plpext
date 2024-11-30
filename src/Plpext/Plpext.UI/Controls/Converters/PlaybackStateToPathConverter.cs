using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Plpext.Core.Models;

namespace Plpext.UI.Controls.Converters;

public class PlaybackStateToPathConverter : IValueConverter
{
    public static readonly PlaybackStateToPathConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PlaybackState state)
        {
            return state == PlaybackState.Playing
                ? Geometry.Parse("M4,2 H7 V14 H4 Z M11,2 H14 V14 H11 Z")
                : Geometry.Parse("M5,2 L5,14 L15,8 Z");
        }

        return Geometry.Parse("M4,2 L4,14 L14,8 Z");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}