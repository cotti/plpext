using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plpext.Core.AudioPlayer
{
    public class AudioContext : IDisposable
    {
        private ALDevice device;
        private ALContext context;
        private bool disposedValue;

        public AudioContext()
        {

        }

        public void Initialize()
        {
            device = ALC.OpenDevice(null!);
            context = ALC.CreateContext(device, (int[])null!);
            ALC.MakeContextCurrent(context);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ALC.DestroyContext(context);
                    ALC.CloseDevice(device);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
