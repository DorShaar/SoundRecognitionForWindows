using System;

namespace SoundRecognition
{
    internal interface IRecordStrategy : IDisposable
    {
        string RecordsDirectory { get; set; }

        void StartListeningToMicrophone(int audioDeviceNumber = 0);
    }
}
