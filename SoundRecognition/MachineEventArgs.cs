using System;

namespace SoundRecognition
{
    class RecognizerFinishedEventArgs : EventArgs
    {
    }

    class RecorderUpdateEventArgs : EventArgs
    {
        public string Message { get; set; }
        public byte[] AudioBytes { get; }

        public RecorderUpdateEventArgs(string message, byte[] audioBytes)
        {
            Message = message;
            AudioBytes = audioBytes;
        }
    }
}
