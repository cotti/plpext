using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Windows.Input;
using Avalonia.Data;
using Plpext.Core.Models;

namespace Plpext.UI;

public class AudioPlayerControl : TemplatedControl
{
    public static readonly StyledProperty<PlaybackState> PlaybackStateProperty =
        AvaloniaProperty.Register<AudioPlayerControl, PlaybackState>(nameof(PlaybackState), defaultValue: PlaybackState.Unknown, inherits: false, defaultBindingMode: BindingMode.OneWay);

    public PlaybackState PlaybackState
    {
        get {return GetValue(PlaybackStateProperty);}
        set {SetValue(PlaybackStateProperty, value);}
    }
    
    public static readonly StyledProperty<bool> IsPlayingProperty =
        AvaloniaProperty.Register<AudioPlayerControl, bool>(nameof(IsPlaying), false, false, Avalonia.Data.BindingMode.TwoWay);

    public bool IsPlaying
    {
        get { return GetValue(IsPlayingProperty); }
        set { SetValue(IsPlayingProperty, value); }
    }
    
    public static readonly StyledProperty<string> CurrentDurationProperty = AvaloniaProperty.Register<AudioPlayerControl, string>
    (
        name: nameof(CurrentDuration),
        defaultValue: "0:55",
        inherits: false,
        defaultBindingMode: Avalonia.Data.BindingMode.TwoWay,
        validate: (x) => x != "0"
    );

    public string CurrentDuration
    {
        get { return GetValue(CurrentDurationProperty); }
        set { SetValue(CurrentDurationProperty, value); }
    }

    public static readonly StyledProperty<string> TotalDurationProperty = AvaloniaProperty.Register<AudioPlayerControl, string>
    (
        name: nameof(TotalDuration),
        defaultValue: "1:07",
        inherits: false,
        defaultBindingMode: Avalonia.Data.BindingMode.TwoWay,
        validate: (x) => x != "0"
    );

    public string TotalDuration
    {
        get { return GetValue(TotalDurationProperty); }
        set { SetValue(TotalDurationProperty, value); }
    }

    public static readonly StyledProperty<double> ProgressProperty = AvaloniaProperty.Register<AudioPlayerControl, double>
    (
        name: nameof(Progress),
        defaultValue: 0,
        inherits: false,
        defaultBindingMode: Avalonia.Data.BindingMode.TwoWay,
        validate: (x) => x >= 0
    );

    public double Progress
    {
        get { return GetValue(ProgressProperty); }
        set { SetValue(ProgressProperty, value); }
    }

    public static readonly StyledProperty<ICommand?> PlayCommandProperty =
            AvaloniaProperty.Register<Button, ICommand?>(nameof(PlayCommand));
    public static readonly StyledProperty<object?> PlayCommandParameterProperty =
AvaloniaProperty.Register<Button, object?>(nameof(PlayCommandParameter));

    public ICommand? PlayCommand
    {
        get => GetValue(PlayCommandProperty);
        set => SetValue(PlayCommandProperty, value);
    }
    public object? PlayCommandParameter
    {
        get => GetValue(PlayCommandParameterProperty);
        set => SetValue(PlayCommandParameterProperty, value);
    }
    
    public static readonly StyledProperty<ICommand?> StopCommandProperty =
        AvaloniaProperty.Register<Button, ICommand?>(nameof(StopCommand));
    public static readonly StyledProperty<object?> StopCommandParameterProperty =
        AvaloniaProperty.Register<Button, object?>(nameof(StopCommandParameter));

    public ICommand? StopCommand
    {
        get => GetValue(StopCommandProperty);
        set => SetValue(StopCommandProperty, value);
    }
    public object? StopCommandParameter
    {
        get => GetValue(StopCommandParameterProperty);
        set => SetValue(StopCommandParameterProperty, value);
    }

}