using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace KNN
{
    internal class KnnTester : IKnnTester
    {
        private readonly string CLASSIFICATION_A;
        private readonly string CLASSIFICATION_B;
        private readonly string mClassADataFilesPath;
        private readonly string mClassBDataFilesPath;

        private List<RecordNeighbor> mNeighbors;

        public KnnTester(string recordsDataPath, string recognizerType, string itemCategory,
             string classificationAName, string classificationBName)
        {
            CLASSIFICATION_A = classificationAName;
            CLASSIFICATION_B = classificationBName;

            mClassADataFilesPath = Path.Combine(recordsDataPath, recognizerType, itemCategory, classificationAName);
            Directory.CreateDirectory(mClassADataFilesPath);

            mClassBDataFilesPath = Path.Combine(recordsDataPath, recognizerType, itemCategory, classificationBName);
            Directory.CreateDirectory(mClassBDataFilesPath);

            GenerateKnnDataSet();
        }

        public string TestAndClassify(RecordNeighbor toTest, int k)
        {
            Dictionary<string, double> classifications = new Dictionary<string, double>();
            NeighborsComparer comparer = new NeighborsComparer(toTest);

            // Sorting the neighbors set by distance from tested object.
            mNeighbors.Sort(comparer);

            // Keeping the classifications of the k closest neighbors
            // (first k objects in the sorted list) in a dictionary, 
            // counting the repeatitions of each one of them.
            for (int i = 0; i < k; i++)
            {
                if (classifications.ContainsKey(mNeighbors[i].Classification))
                {
                    classifications[mNeighbors[i].Classification]++;
                }
                else
                {
                    classifications.Add(mNeighbors[i].Classification, 1 / mNeighbors[i].distanceFrom(toTest)); //test!!!!!!!!!!!!!!!!!!
                }
            }

            // Returns the classification of the most common of them
            // pay attention to cases of a few different classifications 
            // with equal amount of repeatitons.
            List<KeyValuePair<string, double>> maxpairs = new List<KeyValuePair<string, double>>();
            double max = classifications.Values.Max();
            foreach (KeyValuePair<String, double> pair in classifications)
            {
                if (pair.Value == max)
                {
                    maxpairs.Add(pair);
                }
            }

            return maxpairs[0].Key;
        }

        private void GenerateKnnDataSet()
        {
            mNeighbors = new List<RecordNeighbor>();

            // Load info descriptors of two classifications.
            List<RecordInfoDescriptor> classificationARecordInfoDescriptors = LoadRecordInfoDescriptors(mClassADataFilesPath);
            List<RecordInfoDescriptor> classificationBRecordInfoDescriptors = LoadRecordInfoDescriptors(mClassBDataFilesPath);

            // Creates neighbor from each descriptor with classificationA.
            foreach (RecordInfoDescriptor recordInfoDescriptor in classificationARecordInfoDescriptors)
            {
                if (recordInfoDescriptor.LastSectionTimeSpan <= 0 || recordInfoDescriptor.AvgInterval <= 0)
                {
                    recordInfoDescriptor.UpdateReloadedProperties(4.0);
                }

                mNeighbors.Add(GenerateNeighborFromRecordInfoDescriptor(recordInfoDescriptor, CLASSIFICATION_A));
            }

            // Creates neighbor from each descriptor with classificationB.
            foreach (RecordInfoDescriptor recordInfoDescriptor in classificationBRecordInfoDescriptors)
            {
                if (recordInfoDescriptor.LastSectionTimeSpan <= 0 || recordInfoDescriptor.AvgInterval <= 0)
                {
                    recordInfoDescriptor.UpdateReloadedProperties(4.0);
                }

                mNeighbors.Add(GenerateNeighborFromRecordInfoDescriptor(recordInfoDescriptor, CLASSIFICATION_B));
            }
        }

        private List<RecordInfoDescriptor> LoadRecordInfoDescriptors(string xmlFilesDirectoryPath)
        {
            List<RecordInfoDescriptor> recordDescriptors = new List<RecordInfoDescriptor>();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecordInfoDescriptor));
            foreach (string file in Directory.GetFiles(xmlFilesDirectoryPath))
            {
                TextReader textReader = new StreamReader(file);
                RecordInfoDescriptor recordInfo = (RecordInfoDescriptor)xmlSerializer.Deserialize(textReader);
                recordDescriptors.Add(recordInfo);
            }

            return recordDescriptors;
        }

        public RecordNeighbor GenerateNeighborFromRecordInfoDescriptor(
             RecordInfoDescriptor recordInfoDescriptor, string classification = "none")
        {
            return new RecordNeighbor(recordInfoDescriptor, classification);
        }
    }
}
