using System.Collections.Generic;

namespace SoundRecognition
{
     class NeighborsComparer : Comparer<INeighbor>
     {
          private INeighbor mDistanceMeasureTarget;

          public NeighborsComparer(INeighbor distanceMeasureTarget)
          {
               this.mDistanceMeasureTarget = distanceMeasureTarget;
          }

          public override int Compare(INeighbor x, INeighbor y)
          {
               return (int)(x.CalculateDistanceFrom(mDistanceMeasureTarget) - y.CalculateDistanceFrom(mDistanceMeasureTarget)); //TODO: test
          }
     }
}
