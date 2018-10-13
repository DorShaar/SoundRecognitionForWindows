using System;
using System.Collections.Generic;

namespace SoundRecognition
{
     interface IRecordSectionData
     {
          // TODO ask TOMER what is the diff between this and RecordInfoDescriptor?
          // are we using it?
          int getAmountOfSlices();
          int getIndexInRecord();
          List<IRecordSliceData> getRecordSlicesList();
          //IRecordSliceData getMaxRecognitionsSlice();
          //IRecordSliceData getMinRecognitionsSlice();
          Double getAvgRecognitionsInSlice();
          Double getAvgRecognitionsInSecond();
          Double getAvgIntervals();
          Double getMaxInterval();
          Double getMinInterval();
          Double getTotalRecognitions();
          Double getTotalDuration();
          Double getStartTimeInRecord();
          Boolean isTrendUp(); //if slices' intervals getting shorter (slices' recognitions getting higher from one slice to the next)
          Boolean isTrendDown();
          Double getLastRecognitionTime();
          void setAsPeak();
     }
}
