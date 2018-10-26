using System;
using System.Collections.Generic;
using System.Linq;

namespace KNN
{
     internal class BasicNeighbor : INeighbor
     {
          private List<double> mParameters;
          public string Classification { get; set; }

          public BasicNeighbor(string classification, IEnumerable<double> parameters)
          {
               this.mParameters = new List<Double>(parameters);
               Classification = classification;
          }

          public BasicNeighbor(IEnumerable<double> parameters)
          {
               this.mParameters = new List<Double>(parameters);
               Classification = "undefined";
          }

          public double CalculateDistanceFrom(INeighbor other)
          {
               double distance;
               double[] distances = new double[mParameters.Count];

               for (int i = 0; i < mParameters.Count; i++)
               {
                    distances[i] = Math.Pow((mParameters[i] - other.GetParameter(i)), 2);
               }

               distance = Math.Sqrt(distances.Sum());

               return distance;
          }

          public double GetParameter(int index)
          {
               return mParameters[index];
          }
     }
}
