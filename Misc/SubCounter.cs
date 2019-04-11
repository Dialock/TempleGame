
namespace TempleGardens
{
    public class SubCounter
    {
        public FlowerTypes FlowerType { get; private set; }
        public int Used { get; set; }

        public SubCounter(FlowerTypes flowerUsed)
        {
            Used = 0;
            FlowerType = flowerUsed;
        }

    }
}
