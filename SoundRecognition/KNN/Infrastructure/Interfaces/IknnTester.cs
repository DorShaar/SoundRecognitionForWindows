
namespace SoundRecognition
{
     interface IKnnTester
     {
          INeighbor GetNeighbor(int index);
          void AddNeighbor(INeighbor neighbor);
          string TestAndClassify(INeighbor toTest, int k);
     }
}
