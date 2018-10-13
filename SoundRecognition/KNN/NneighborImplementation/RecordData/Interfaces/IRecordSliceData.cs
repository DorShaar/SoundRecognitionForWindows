using System;
using System.Collections.Generic;

namespace SoundRecognition
{
     interface IRecordSliceData
     {
          // TODO ask TOMER what is the diff between this and RecordInfoDescriptor?
          // are we using it?
          Double getDuration();
          Double getTimeInRecord();
          int getIndexInRecord();
          int getAmountOfTargetSoundRecognitions(); //in case of popcorn, targetSound is pop
          Double? getAvgInterval();
          Double? getMaxInterval();
          Double? getMinInterval();
          Double? getLastRecognitionTime();
          List<double> getRecognitionsList();
          void addRecognition(Double recognitionTime);
     }
}
