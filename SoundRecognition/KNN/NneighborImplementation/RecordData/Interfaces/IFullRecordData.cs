using System;
using System.Collections.Generic;

namespace KNN
{
     internal interface IFullRecordData
     {
          // TODO ask TOMER what is the diff between this and RecordInfoDescriptor?
          // are we using it?
          int getAmountOfSlices();
          int getAmountOfSections();
          //List<IRecordSliceData> getRecordSlicesList();
          List<IRecordSectionData> getRecordSectionsList();
          //IRecordSliceData getMaxRecognitionsSlice();
          //IRecordSliceData getMinRecognitionsSlice();
          IRecordSectionData getLastSection();
          Double getAvgInterval();
          Double getMaxInterval();
          Double getMinInterval();
          Double getAvgRecognitionsInSection();
          Double getAvgRecognitionsInSlice();
          Double getAvgRecognitionsInSecond();
          Double getTotalDuration();
          Double getTotalRecognitionsAmount();
          Boolean isTrendUp();
          Boolean isOverThePeak();
          void addRecordSection(IRecordSectionData recordSection);
          Double getLastRecognitionTime();
          INeighbor generateNeighborRepresentor();
          void addRecordSliceData(IRecordSliceData recordSliceData);
     }
}
