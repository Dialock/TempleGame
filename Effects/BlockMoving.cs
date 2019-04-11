#region Using Statements
using Microsoft.Xna.Framework;

#endregion

namespace TempleGardens
{
    public class BlockMoving
    {
        public Vector2 DrawPosition { get; private set; }
        private Vector2 targetPosition;
        public bool Done { get; private set; }

        //private static GamePlayScreen _refGamePlayScreen;

        public int LastElement { get; private set; }

        public int ChoosenBlock { get; private set; }

        //public static void Init(GamePlayScreen gamePlayScreen)
        //{
        //    _refGamePlayScreen = gamePlayScreen;
        //}

        public BlockMoving(Vector2 start, Vector2 targetPos, int last, int chose)
        {
            DrawPosition = start;
            targetPosition = targetPos;

            LastElement = last;

            ChoosenBlock = chose;
        }

        public void Update(GameTime gameTime)
        {
            DrawPosition += new Vector2(0, 8);

            if (DrawPosition.Y >= targetPosition.Y)
                Done = true;
        }
    }
}
