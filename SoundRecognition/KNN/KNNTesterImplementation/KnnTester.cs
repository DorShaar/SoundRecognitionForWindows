using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundRecognition
{
     class KnnTester : IKnnTester
     {
          private List<INeighbor> mNeighbors;

          public KnnTester(List<INeighbor> initializedList)
          {
               mNeighbors = initializedList ?? throw new Exception("KnnTester's ctor must get initialized list");
          }

          public void AddNeighbor(INeighbor neighbor)
          {
               mNeighbors.Add(neighbor);
          }

          public INeighbor GetNeighbor(int index)
          {
               return mNeighbors[index];
          }

          public string TestAndClassify(INeighbor toTest, int k)
          {
               Dictionary<string, int> classifications = new Dictionary<string, int>();
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
                         classifications.Add(mNeighbors[i].Classification, 1);
                    }
               }

               // Returns the classification of the most common of them
               // TODO: pay attention to cases of a few different classifications 
               // with equal amount of repeatitons.
               List<KeyValuePair<string, int>> maxpairs = new List<KeyValuePair<string, int>>();
               int max = classifications.Values.Max();
               foreach (KeyValuePair<String, int> pair in classifications)
               {
                    if (pair.Value == max)
                    {
                         maxpairs.Add(pair);
                    }
               }

               return maxpairs[0].Key;
          }
     }
}
