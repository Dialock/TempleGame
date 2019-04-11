
using Microsoft.Xna.Framework;


namespace TempleGardens
{
    public class Wind
    {
        public bool Visible { get; set; }

        public float Timer { get; private set; }
        private float timerDuration;

        public byte Age { get; private set; }

        public Vector2 Position { get; private set; }
        public static Vector2 Direction { get; private set; }

        public Color DebrisColor { get; private set; }
        public int Size { get; private set; }

        public static int Speed { get; private set; }

        public Wind()
        {
            Visible = false;
            Timer = 0;
            timerDuration = 0.2f;
            Age = 0;

            DebrisColor = Color.White;
        }

        public void Activate(Vector2 pos)
        {
            Position = pos;
            Visible = true;

            Age = 0;

            DebrisColor = SetColor();

            Size = MasterRandom.FRandom.Next(2, 6);

        }

        public void Update(float elapsed)
        {
            Timer += elapsed;
            Position = Position + Direction * elapsed * Speed * 45;

            if (Timer >= timerDuration)
            {
                Timer = 0;
                Age++;
            }

            if (Age > 9)
                Reset();

            if (!WindBounds(Position))
                Reset();
        }

        public static void SetDirAndSpeed(Vector2 dir, int speed)
        {
            Direction = dir;
            Speed = speed;
        }

        private void Reset()
        {
            Visible = false;
            Timer = 0;
            Age = 0;
        }

        private static bool WindBounds(Vector2 pos)
        {
            return pos.X >= 64 && pos.X < 1024 &&
                    pos.Y >= 128 && pos.Y < 768;
        }

        private static Color SetColor()
        {
            var choose = MasterRandom.FRandom.Next(0, 5);

            if (choose == 0)
                return Color.FromNonPremultiplied(83, 72, 35, 255);
            else if (choose == 1)
                return Color.FromNonPremultiplied(105, 95, 60, 255);
            else if (choose == 2)
                return Color.FromNonPremultiplied(91, 81, 48, 255);
            else
                return Color.Tan;

        }
    }
}
