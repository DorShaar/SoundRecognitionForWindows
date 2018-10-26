using System.Collections.Generic;

namespace KNN
{
     internal class NeighborsComparer : Comparer<RecordNeighbor>
     {
          private RecordNeighbor mDistanceMeasureTarget;

          public NeighborsComparer(RecordNeighbor distanceMeasureTarget)
          {
               this.mDistanceMeasureTarget = distanceMeasureTarget;
          }

          public override int Compare(RecordNeighbor x, RecordNeighbor y)
          {
               return (int)(x.distanceFrom(mDistanceMeasureTarget) - y.distanceFrom(mDistanceMeasureTarget)); //TODO: test
          }
     }
}
