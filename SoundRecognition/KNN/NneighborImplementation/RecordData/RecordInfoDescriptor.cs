using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoundRecognition
{
     // Must be public since it should be XmlSerilizied (it accept only public types).
     public class RecordInfoDescriptor : IRecordInfoDescriptor
     {
          private readonly string XML_EXTENSION = ".xml";

          public List<double> IntervalsList { get; set; } = new List<double>();
          public List<double> RecognitionsTimesList { get; set; } = new List<double>();

          public double AvgInterval { get; set; }
          public double MinInterval { get; set; }
          public double MaxInterval { get; set; }
          public double Duration { get; set; } // TODO What is that? who update that field (if should)  or who give that parameter?

          public void AddRecognitionTime(double recognitionTime)
          {
               double previous = -1;
               if (RecognitionsTimesList.Count > 0)
               {
                    previous = RecognitionsTimesList.Last();
               }

               RecognitionsTimesList.Add(recognitionTime);
               if (previous > 0)
               {
                    IntervalsList.Add(recognitionTime - previous);
               }

               UpdateIntervalsFields();
          }

          // TODO: if neigbour gets recognitions list and intervals list,
          // should find a way to compare neighbours with different amount of parameters
          public INeighbor GenerateNeighborRepresentor()
          {
               BasicNeighbor representor;
               double recognitionsTimeSpan = RecognitionsTimesList.Last() - RecognitionsTimesList.First();
               double timeFromLastRecognition = Duration - RecognitionsTimesList.Last();
               List<double> parameters = new List<double>();
               parameters.Add(recognitionsTimeSpan);
               parameters.Add(timeFromLastRecognition);
               parameters.Add(AvgInterval);
               parameters.Add(MaxInterval);
               parameters.Add(MinInterval);
               parameters.Add(RecognitionsTimesList.Count);
               parameters.AddRange(IntervalsList);
               parameters.AddRange(RecognitionsTimesList);
               representor = new BasicNeighbor(parameters);

               //params to neighbor:
               ///pops
               ///intervals
               ///total duration?
               ///pops count
               ///time span from first pop to last pop?
               /// - maybe better then total duration in case of checking partial record 
               /// (example: in case of trimming half of the non-pops part at the beginning)
               /// trend?
               /// avg?
               /// trend,avgInterval,popsCount and\or pops and intervals sublists in last few seconds
               /// cookState (may be set according to some of the above parameters, 
               ///     which may be calculated every few seconds, using updateState func


               //double trendFactor, timeFromLastRecognition;
               //timeFromLastRecognition = m_totalTime - m_recognitionsTimesList.Last();

               //if (m_isTrendUp)
               //{
               //    trendFactor = -10;
               //}
               //else if (m_isTrendDown)
               //{
               //    trendFactor = 10;
               //}
               //else
               //{
               //    trendFactor = 0;
               //}

               //representor = new BasicNeighbor(
               //    m_totalTime,
               //    m_totalTargetSoundRecognitions,
               //    trendFactor,
               //    m_isOverPeak ? 20 : 0,
               //    m_currentMinInterval,
               //    m_maxValueOfSectionMinIntervalAfterPeak,
               //    timeFromLastRecognition
               //    );

               return representor;
               //TODO:maybe use RecordNeighbors to compare better.
          }

          public void Save(string directoryPath)
          {
               string xmlFilePath = Path.Combine(directoryPath, $"{Guid.NewGuid().ToString()}{XML_EXTENSION}");
               SerializationMachine.XmlSerialize(xmlFilePath, this, typeof(RecordInfoDescriptor));
          }

          private void UpdateIntervalsFields()
          {
               if (IntervalsList.Count > 0)
               {
                    AvgInterval = IntervalsList.Sum() / IntervalsList.Count;
                    MinInterval = IntervalsList.Min();
                    MaxInterval = IntervalsList.Max();
               }
          }
     }
}
