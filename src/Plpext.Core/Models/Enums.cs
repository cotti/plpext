using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.Models;

public enum AudioFormat
{
    Unknown,
    Mono16,
    Stereo16
}

public enum PlaybackState
{
    Unknown,
    Stopped,
    Playing,
    Paused
}