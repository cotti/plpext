using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.Models;

public class PlaybackStoppedEventArgs : EventArgs
{
    public AudioFile AudioFile { get; init; }
}

public class PlaybackStartedEventArgs : EventArgs
{
    public AudioFile AudioFile { get; init; }
}
