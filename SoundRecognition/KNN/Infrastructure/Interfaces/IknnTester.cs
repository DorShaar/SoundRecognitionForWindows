
namespace KNN
{
    internal interface IKnnTester
    {
        string TestAndClassify(RecordNeighbor toTest, int k);
    }
}
