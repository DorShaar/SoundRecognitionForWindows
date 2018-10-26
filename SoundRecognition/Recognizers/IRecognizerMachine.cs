using System;

namespace SoundRecognition
{
     interface IRecognizerMachine
     {
          event EventHandler<RecognizerFinishedEventArgs> RecognizerFinished;
          void LoadProcessedData(string recognizerType, string itemCategory);
          void ProcessNewData(IItemInfo item);
          void Stop(string stopReason);
     }

     public enum eRecognitionStatus
     {
          Recognized = 0,
          UnRecognized = 1
     }
}
