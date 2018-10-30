using SoundRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KNN
{
    // Must be public since it should be XmlSerilizied (it accept only public types).
    public class RecordInfoDescriptor : IRecordInfoDescriptor
    {
        private readonly string XML_EXTENSION = ".xml";
        private Logger mLogger = new Logger(nameof(KNN), ConsoleColor.White);

        public List<double> IntervalsList { get; set; } = new List<double>();
        public List<double> RecognitionsTimesList { get; set; } = new List<double>();

        public double AvgInterval { get; set; }
        public double MinInterval { get; set; }
        public double MaxInterval { get; set; }
        public double Duration { get; set; } 
        // TODO answer - set by UpdateDurationAndLastSection, used to set the following knn parameters and is also used as knn parameter
        public double LastSectionTimeSpan { get; set; }
        public double LastSectionRecognitionsCount { get; set; }
        public double LastSectionMinInterval { get; set; }
        public double LastSectionMaxInterval { get; set; }
        public double LastSectionAvgInterval { get; set; }

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

        //TODO: remove method from interface, or leave it not implemented.
        //the knn creates a RecordNeighbor out of the descriptor instead of using this method
        public INeighbor GenerateNeighborRepresentor()
        {
            //BasicNeighbor representor;
            //double recognitionsTimeSpan = RecognitionsTimesList.Last() - RecognitionsTimesList.First();
            //double timeFromLastRecognition = Duration - RecognitionsTimesList.Last();
            //List<double> parameters = new List<double>();
            //parameters.Add(recognitionsTimeSpan);
            //parameters.Add(timeFromLastRecognition);
            //parameters.Add(AvgInterval);
            //parameters.Add(MaxInterval);
            //parameters.Add(MinInterval);
            //parameters.Add(RecognitionsTimesList.Count);
            //parameters.AddRange(IntervalsList);
            //parameters.AddRange(RecognitionsTimesList);
            //representor = new BasicNeighbor(parameters);


            //return representor;
            
            throw new NotImplementedException();
        }

        public void Save(string directoryPath)
        {
            string xmlFilePath = Path.Combine(directoryPath, $"{Guid.NewGuid().ToString()}{XML_EXTENSION}");
            SerializationMachine.XmlSerialize(xmlFilePath, this, typeof(RecordInfoDescriptor));
            mLogger.WriteLine($"Record data saved");
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

        public void UpdateReloadedProperties(double sectionTimeSpan)
        {
            UpdateIntervalsFields();
            updateLastSectionFields(Duration - sectionTimeSpan);

        }

        public void UpdateDurationAndLastSection(double duration)
        {
            double previousDuration = Duration;
            Duration = duration;

            updateLastSectionFields(previousDuration);
        }

        void updateLastSectionFields(double previousDuration)
        {
            LastSectionTimeSpan = Duration - previousDuration;
            LastSectionRecognitionsCount =
                RecognitionsTimesList.Count(d => (d > previousDuration && d <= Duration));//new Func<double, bool>(d => (d > previousDuration && d <= duration))
            int firstIndexInSection = RecognitionsTimesList.IndexOf(
                RecognitionsTimesList.Where(d => (d > previousDuration && d <= Duration)).FirstOrDefault());
            List<double> intervalsInLastSection = new List<double>(IntervalsList.Skip(firstIndexInSection));
            //validation test: the above list's count shuold be equal to lastSectionRecognitionsCount-1
            if (intervalsInLastSection.Count > 0)
            {
                LastSectionMinInterval = intervalsInLastSection.Min();
                LastSectionMaxInterval = intervalsInLastSection.Max();
                LastSectionAvgInterval = intervalsInLastSection.Sum() / intervalsInLastSection.Count;
            }
            else
            {
                LastSectionMinInterval = LastSectionMaxInterval = LastSectionAvgInterval = -10; //test
            }
        }
    }
}
