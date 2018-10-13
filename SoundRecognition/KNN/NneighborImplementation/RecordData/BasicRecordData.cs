using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundRecognition
{
     class BasicRecordData : IFullRecordData
     {
          private List<IRecordSliceData> m_recordSlicesList;
          //notice that sections overlap each other, while slices doesn't
          private List<IRecordSectionData> m_recordSectionsList;
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
          private Double m_avgRecognitionsInSection;
          private Double m_lastRecognitionTime;

          public BasicRecordData()
          {
               m_recordSlicesList = new List<IRecordSliceData>();
               m_recordSectionsList = new List<IRecordSectionData>();

          }
          public void addRecordSection(IRecordSectionData recordSection)
          {
               updateSlicesBeforeAddingSection(recordSection);
               m_recordSectionsList.Add(recordSection);
               //update all relevant fields
               updateFieldsAccordingToNewSection(recordSection);
          }

          private void updateFieldsAccordingToNewSection(IRecordSectionData recordSection)
          {
               calculateAvgInterval();
               updateAvgRecognitionsInSection(recordSection.getTotalRecognitions());
               calculateAvgRecognitionsInSecond();
               if (recordSection.getMaxInterval() > m_currentMaxInterval)
               {
                    m_currentMaxInterval = recordSection.getMaxInterval();
               }
               if (recordSection.getMinInterval() < m_currentMinInterval)
               {
                    m_currentMinInterval = recordSection.getMinInterval();
               }
               checkTrend(recordSection);
               m_lastRecognitionTime = recordSection.getLastRecognitionTime();
               if (m_isOverPeak && recordSection.getMinInterval() > m_maxValueOfSectionMinIntervalAfterPeak)
               {
                    m_maxValueOfSectionMinIntervalAfterPeak = recordSection.getMinInterval();
               }
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
                    setPeak();
               }
          }

          private void setPeak()
          {
               int indexOfLastTrendUpSection = findLastTrendUpSection();
               Boolean isFound = false;

               for (int i = indexOfLastTrendUpSection; i < m_recordSectionsList.Count && !isFound; i++)
               {
                    if (!m_recordSectionsList[i].isTrendUp() && !m_recordSectionsList[i].isTrendDown())
                    {
                         m_recordSectionsList[i].setAsPeak();
                         isFound = true;
                    }
               }
          }
          private int findLastTrendUpSection()
          {
               int result = m_recordSectionsList.Count;
               Boolean isFound = false;

               for (int i = m_recordSectionsList.Count; i > 0 && !isFound; i--)
               {
                    if (m_recordSectionsList[i - 1].isTrendUp())
                    {
                         result = i - 1;
                         isFound = true;
                    }
               }

               return result;
          }

          private void updateSlicesBeforeAddingSection(IRecordSectionData recordSection)
          {
               if (m_recordSectionsList.Count == 0) // keeps all slices of the first section
               {
                    foreach (IRecordSliceData slice in recordSection.getRecordSlicesList())
                    {//update total time and total recognitions count
                         addSliceAndUpdateFields(slice);
                    }
                    m_avgRecognitionsInSlice = recordSection.getAvgRecognitionsInSlice();
               }
               else
               { //sections overlap each other, thus only the last slice in the new section is new to the record
                    addSliceAndUpdateFields(recordSection.getRecordSlicesList().Last());//TODO: check
                    updateAvgRecognitionsInSlice(recordSection.getRecordSlicesList().Last().getAmountOfTargetSoundRecognitions());
               }
          }

          private void updateAvgRecognitionsInSection(double newSectionRecognitions)
          {
               int lastCount, currCount = m_recordSectionsList.Count;
               lastCount = currCount - 1;
               m_avgRecognitionsInSection =
                   (m_avgRecognitionsInSection * lastCount + newSectionRecognitions) / currCount;
          }

          private void updateAvgRecognitionsInSlice(double newSliceRecognitions)
          {
               int lastCount, currCount = m_recordSlicesList.Count;
               lastCount = currCount - 1;
               m_avgRecognitionsInSlice =
                   (m_avgRecognitionsInSlice * lastCount + newSliceRecognitions) / currCount;
          }

          //add slice to the list, update total time and total recognition
          private void addSliceAndUpdateFields(IRecordSliceData slice)
          {
               m_recordSlicesList.Add(slice);
               m_totalTime += slice.getDuration();
               m_totalTargetSoundRecognitions += slice.getAmountOfTargetSoundRecognitions();
          }

          public int getAmountOfSections()
          {
               return m_recordSectionsList.Count;
          }

          public int getAmountOfSlices()
          {
               int sum = 0;
               foreach (IRecordSectionData section in m_recordSectionsList)
               {
                    sum += section.getAmountOfSlices();
               }
               return sum;
          }
          private void calculateAvgInterval()
          {
               double count = 0, sum = 0;
               //TODO: notice that sections overlap each other, while slices doesn't
               foreach (IRecordSliceData slice in m_recordSlicesList)
               {
                    if (slice.getAmountOfTargetSoundRecognitions() > 0)
                    {
                         count += (double)slice.getAmountOfTargetSoundRecognitions() - 1; //one interval between each two recognitions
                         sum += (double)slice.getAvgInterval() * (slice.getAmountOfTargetSoundRecognitions() - 1);
                    }
               }
               m_avgInterval = sum / count;
          }

          public double getAvgInterval()
          {
               return m_avgInterval;
          }
          public double getAvgRecognitionsInSecond()
          {
               return m_avgRecognitionsInSecond;
          }
          private void calculateAvgRecognitionsInSecond()
          {
               m_avgRecognitionsInSecond = m_totalTargetSoundRecognitions / m_totalTime; //m_totalTime is in seconds
          }

          public double getAvgRecognitionsInSlice()
          {
               return m_avgRecognitionsInSlice;
          }

          public double getAvgRecognitionsInSection()
          {
               return m_avgRecognitionsInSection;
          }

          public IRecordSectionData getLastSection()
          {
               return m_recordSectionsList.Last();
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
               return m_recordSectionsList;
          }

          public double getTotalDuration()
          {
               return m_totalTime;
          }

          public double getTotalRecognitions()
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

          public double getLastRecognitionTime()
          {
               return m_lastRecognitionTime;
          }

          public INeighbor generateNeighborRepresentor()
          {
               BasicNeighbor representor;
               //List<Double> parameters = getParamsFromLastSections(3);
               double trendFactor;

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
               //    m_recordSectionsList.Last().getTotalRecognitions(),
               //    m_recordSectionsList.Last().getAvgRecognitionsInSlice(),
               //    m_recordSectionsList.Last().getAvgIntervals()
               //    );
               representor = new BasicNeighbor(" ", new List<double>()); // TODO DELETE - just to make it compile.

               return representor;
          }

          private List<Double> getParamsFromLastSections(int amount)
          {
               List<Double> parameters = new List<double>();
               int max = Math.Min(m_recordSectionsList.Count, amount);
               for (int i = 1; i <= max; i++)
               {
                    parameters.Add(m_recordSectionsList[m_recordSectionsList.Count - i].getTotalRecognitions());
                    parameters.Add(m_recordSectionsList[m_recordSectionsList.Count - i].getAvgRecognitionsInSlice());
                    parameters.Add(m_recordSectionsList[m_recordSectionsList.Count - i].getAvgIntervals());
               }
               return parameters;
          }

          public double getTotalRecognitionsAmount()
          {
               return m_totalTargetSoundRecognitions;
          }

          public void addRecordSliceData(IRecordSliceData recordSliceData)
          {
               throw new NotImplementedException();
          }
     }
}
