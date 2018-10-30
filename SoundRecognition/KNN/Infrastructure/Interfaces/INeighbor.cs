
namespace KNN
{
    public interface INeighbor
    {
        string Classification { get; }
        double GetParameter(int index);
        double CalculateDistanceFrom(INeighbor other);
    }
}
