using System;
using System.Collections.Generic;
using System.Linq;

namespace KNN
{
    internal class RecordNeighbor : INeighbor
    {
        public RecordNeighbor(RecordInfoDescriptor infoDescriptor) : this(infoDescriptor, "")
        {
        }

        public RecordNeighbor(RecordInfoDescriptor infoDescriptor, string classification)
        {
            this.Classification = classification;

            Intervals = infoDescriptor.IntervalsList;
            Recognitions = infoDescriptor.RecognitionsTimesList;

            MinInterval = infoDescriptor.MinInterval;
            MaxInterval = infoDescriptor.MaxInterval;
            AvgInterval = infoDescriptor.AvgInterval;
            Duration = infoDescriptor.Duration;

            LastSectionRecognitionsCount = infoDescriptor.LastSectionRecognitionsCount;
            LastSectionMinInterval = infoDescriptor.LastSectionMinInterval;
            LastSectionMaxInterval = infoDescriptor.LastSectionMaxInterval;
            LastSectionAvgInterval = infoDescriptor.LastSectionAvgInterval;
            LastSectionTimeSpan = infoDescriptor.LastSectionTimeSpan;

        }

        public double MinInterval { get; set; }
        public double MaxInterval { get; set; }
        public double AvgInterval { get; set; }
        public double Duration { get; set; }
        public List<double> Intervals { get; set; }
        public List<double> Recognitions { get; set; }
        public double LastSectionRecognitionsCount { get; set; }
        public double LastSectionMinInterval { get; set; }
        public double LastSectionMaxInterval { get; set; }
        public double LastSectionAvgInterval { get; set; }
        public double LastSectionTimeSpan { get; set; }

        public string Classification { get; set; }

        public double[] PrepareFixedParameters()
        {
            List<double> fixedParameters = new List<double>();
            fixedParameters.Add(Duration);
            fixedParameters.Add(Recognitions.Count);
            fixedParameters.Add(MinInterval);
            fixedParameters.Add(MaxInterval);
            fixedParameters.Add(AvgInterval);

            //optional: make those more effective.
            //may do so by adding weight, multiply values by constant>1, or simply by adding them to the list more than once
            fixedParameters.Add(LastSectionTimeSpan);
            fixedParameters.Add(LastSectionRecognitionsCount);
            fixedParameters.Add(LastSectionMinInterval);
            fixedParameters.Add(LastSectionMaxInterval);
            fixedParameters.Add(LastSectionAvgInterval);

            return fixedParameters.ToArray();

        }

        public double calculateFixedParametersDistanceFrom(RecordNeighbor other)
        {
            double distance;
            double[] myFixedParameters = PrepareFixedParameters();
            double[] otherFixedParameters = other.PrepareFixedParameters();
            int paramsCount = myFixedParameters.Length;
            double[] distances = new double[paramsCount];

            for (int i = 0; i < paramsCount; i++)
            {
                distances[i] = Math.Pow((myFixedParameters[i] - otherFixedParameters[i]), 2);
            }
            distance = Math.Sqrt(distances.Sum());

            return distance;
        }

        public double calculatRecognitionsDistanceFrom(RecordNeighbor other)
        {
            int myRecognitionsCount = Recognitions.Count;
            int otherRecognitionsCount = other.Recognitions.Count;
            List<double> longerRecognitionsList;
            List<double> shorterRecognitionsList;
            List<double> longerIntervalsList;
            List<double> shorterIntervalsList;
            int smallerCount, largerCount;

            //find the shorter lists and the longer lists
            if (myRecognitionsCount >= otherRecognitionsCount)
            {
                largerCount = myRecognitionsCount;
                smallerCount = otherRecognitionsCount;
                longerRecognitionsList = Recognitions;
                longerIntervalsList = Intervals;
                shorterRecognitionsList = other.Recognitions;
                shorterIntervalsList = other.Intervals;
            }
            else
            {
                largerCount = otherRecognitionsCount;
                smallerCount = myRecognitionsCount;
                longerRecognitionsList = other.Recognitions;
                longerIntervalsList = other.Intervals;
                shorterRecognitionsList = Recognitions;
                shorterIntervalsList = Intervals;
            }

            //trim the first part of the longer lists by skipping the first elements in each list
            //the amount of skipped-on elements is the diffrence between the longer and the shorter lists
            List<double> trimmedRecognitions =
                new List<double>(longerRecognitionsList.Skip(largerCount - smallerCount));
            List<double> trimmedIntervals =
                new List<double>(longerIntervalsList.Skip(largerCount - smallerCount));

            //compare the last recognitions and last intervals between the current object and the other object
            double recognitionsBasedDistance = compareDistanceBetweenEvenSizedLists(trimmedRecognitions, shorterRecognitionsList, true);
            double intervalsBasedDistance = compareDistanceBetweenEvenSizedLists(trimmedIntervals, shorterIntervalsList, false);
            double distance = Math.Sqrt(Math.Pow(recognitionsBasedDistance, 2) + Math.Pow(intervalsBasedDistance, 2));

            return distance;
        }

        public double compareDistanceBetweenEvenSizedLists(List<double> listA, List<double> listB, Boolean shouldModify)
        {
            double distance;
            int count = listA.Count;
            double modificationFactor = shouldModify ? 100 : 1; //used to make the infulence of lower range values as effective as higher range values
            double[] distances = new double[count];

            for (int i = 0; i < count; i++)
            {
                distances[i] = Math.Pow((listA[i] / modificationFactor - listB[i] / modificationFactor), 2);
            }
            distance = Math.Sqrt(distances.Sum());

            return distance;
        }

        public double distanceFrom(RecordNeighbor other)
        {
            double distance, fixedParamsDistance, listsDistance;

            fixedParamsDistance = calculateFixedParametersDistanceFrom(other);
            listsDistance = calculatRecognitionsDistanceFrom(other);
            distance = Math.Sqrt(Math.Pow(fixedParamsDistance, 2) + Math.Pow(listsDistance, 2));

            return distance;
        }



        public double GetParameter(int index)
        {
            throw new NotImplementedException("should use RecordNeighbor proerties instead");
        }

        public double CalculateDistanceFrom(INeighbor other)
        {
            throw new NotImplementedException("should use RecordNeighbor proerties instead");
        }

    }
}
