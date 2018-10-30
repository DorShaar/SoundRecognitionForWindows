using System;

namespace SoundRecognition
{
    interface IRecorder : IDisposable
    {
        void Record();
    }
}
