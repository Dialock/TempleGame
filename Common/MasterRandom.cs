#region Using Statements

#endregion

namespace TempleGardens
{
    public static class MasterRandom
    {
        public static FastRandom FRandom { get; private set; }

        public static void Init(int seed)
        {
            FRandom = new FastRandom(seed);
        }

        public static void Init()
        {
            FRandom = new FastRandom();
        }

    }
}
