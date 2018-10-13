using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundRecognition
{
     class SimpleRecordData : IFullRecordData
     {
          private const int SECTION_SIZE = 3;

          private List<IRecordSliceData> m_recordSlicesList;
          public List<Double> m_recognitionsTimesList; // TODO PRIVATE
          private Boolean m_isOverPeak;
          private Boolean m_isTrendUp;
          private Boolean m_isTrendDown;
          private Boolean m_hadTrendUp;
          private Double m_totalTime;
          private Double m_totalTargetSoundRecognitions;
          private Double m_currentMinInterval;
          private Double m_currentMaxInterval;
          private Double m_maxValueOfSectionMinIntervalAfterPeak;
          private Double m_avgInterval;   //TODO: should be easier to use intervals array and calculate when required
          private Double m_avgRecognitionsInSecond;
          private Double m_avgRecognitionsInSlice;

          //==================
          //utils funcs
          //==================

          //avg, min, max
          private void calculateIntervalsFields()
          {
               List<Double> intervals = new List<double>();
               for (int i = 0; i < m_recognitionsTimesList.Count - 1; i++)
               {
                    intervals.Add(m_recognitionsTimesList[i + 1] - m_recognitionsTimesList[i]);
               }
               if (intervals.Count > 0)
               {
                    m_avgInterval = intervals.Sum() / intervals.Count;
                    m_currentMinInterval = intervals.Min();
                    m_currentMaxInterval = intervals.Max();
               }
          }
          private void calculateRecognitionsAvgs()
          {
               m_avgRecognitionsInSlice = m_recognitionsTimesList.Count / m_recordSlicesList.Count;
               m_avgRecognitionsInSecond = m_recognitionsTimesList.Count / m_totalTime;
          }

          private void checkTrend(IRecordSectionData recordSection)
          {
               if (m_isTrendUp)
               {
                    m_hadTrendUp = true;
               }
               m_isTrendUp = recordSection.isTrendUp();
               m_isTrendDown = recordSection.isTrendDown();
               if (m_isTrendDown && m_hadTrendUp)
               {
                    m_isOverPeak = true;
                    m_maxValueOfSectionMinIntervalAfterPeak = m_currentMinInterval;
               }
          }

          //=================
          //interface funcs
          //=================

          public SimpleRecordData()
          {
               m_recordSlicesList = new List<IRecordSliceData>();
               m_recognitionsTimesList = new List<double>();
          }

          public void addRecordSliceData(IRecordSliceData recordSliceData)
          {
               m_recordSlicesList.Add(recordSliceData);
               m_recognitionsTimesList.AddRange(recordSliceData.getRecognitionsList());

               //update total time
               m_totalTime += recordSliceData.getDuration();

               //update pops count
               m_totalTargetSoundRecognitions += recordSliceData.getAmountOfTargetSoundRecognitions();

               //update intervals fields and avgs fields
               calculateIntervalsFields();
               calculateRecognitionsAvgs();

               //if there are at least SECTION_SIZE slices, 
               //check the last SECTION_SIZE slices to get their info as a section
               //(trend, maxValueOfSectionMinIntervalAfterPeak)
               calculateLastSectionInfo();
          }

          private void calculateLastSectionInfo()
          {
               int sliceCount = m_recordSlicesList.Count;
               List<IRecordSliceData> lastSlices;
               IRecordSectionData sectionData;
               if (sliceCount >= SECTION_SIZE)
               {
                    lastSlices = new List<IRecordSliceData>(SECTION_SIZE);
                    for (int i = 0; i < SECTION_SIZE; i++)
                    {
                         lastSlices.Add(m_recordSlicesList[sliceCount - SECTION_SIZE + i]);
                    }
                    //use last SECTION_SIZE slices to create temp sectionData object,
                    //first ctor param is index, should be ignored in this new implementation of recordData
                    //should be the only use of sectionData object in this new implementation of recordData
                    sectionData = new BasicSectionData(sliceCount / SECTION_SIZE, lastSlices);
                    checkTrend(sectionData);
                    if (m_isOverPeak && sectionData.getMinInterval() > m_maxValueOfSectionMinIntervalAfterPeak)
                    {
                         m_maxValueOfSectionMinIntervalAfterPeak = sectionData.getMinInterval();
                    }
               }
          }

          public void addRecordSection(IRecordSectionData recordSection)
          {
               //shouldn't be used in this new recordData implementation
               throw new NotImplementedException();
          }

          public INeighbor generateNeighborRepresentor()
          {
               BasicNeighbor representor;
               //List<Double> parameters = getParamsFromLastSections(3);
               double trendFactor, timeFromLastRecognition;
               timeFromLastRecognition = m_totalTime - m_recognitionsTimesList.Last();

               if (m_isTrendUp)
               {
                    trendFactor = -10;
               }
               else if (m_isTrendDown)
               {
                    trendFactor = 10;
               }
               else
               {
                    trendFactor = 0;
               }

               //representor = new BasicNeighbor(
               //    m_totalTime,
               //    m_totalTargetSoundRecognitions,
               //    trendFactor,
               //    m_isOverPeak ? 20 : 0,
               //    m_currentMinInterval,
               //    m_maxValueOfSectionMinIntervalAfterPeak,
               //    timeFromLastRecognition
               //    );

               representor = new BasicNeighbor(" ", new List<double>()); // TODO DELETE - just to make it compile.

               return representor;
          }

          public int getAmountOfSections()
          {
               //shouldn't be used in this new recordData implementation
               throw new NotImplementedException();
          }

          public int getAmountOfSlices()
          {
               return m_recordSlicesList.Count;
          }

          public double getAvgInterval()
          {
               return m_avgInterval;
          }

          public double getAvgRecognitionsInSecond()
          {
               return m_avgRecognitionsInSecond;
          }

          public double getAvgRecognitionsInSection()
          {
               //shouldn't be used in this new recordData implementation
               throw new NotImplementedException();
          }

          public double getAvgRecognitionsInSlice()
          {
               return m_avgRecognitionsInSlice;
          }

          public double getLastRecognitionTime()
          {
               return m_recognitionsTimesList.Last();
          }

          public IRecordSectionData getLastSection()
          {
               //shouldn't be used in this new recordData implementation
               throw new NotImplementedException();
          }

          public double getMaxInterval()
          {
               return m_currentMaxInterval;
          }

          public double getMinInterval()
          {
               return m_currentMinInterval;
          }

          public List<IRecordSectionData> getRecordSectionsList()
          {
               //shouldn't be used in this new recordData implementation
               throw new NotImplementedException();
          }

          public double getTotalDuration()
          {
               return m_totalTime;
          }

          public double getTotalRecognitionsAmount()
          {
               return m_totalTargetSoundRecognitions;
          }

          public bool isOverThePeak()
          {
               return m_isOverPeak;
          }

          public bool isTrendUp()
          {
               return m_isTrendUp;
          }
     }
}
