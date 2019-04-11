using Microsoft.Xna.Framework;


namespace TempleGardens
{
    public class Pollen
    {
        public bool Visible { get; private set; }

        public Vector2 Position { get; private set; }
        public static Vector2 Direction { get; private set;}
        public Rectangle Bounds { get; private set; }

        public Color PollenColor { get; private set; }
        public int Size { get; private set; }

        public static int Speed { get; private set; }

        public Pollen()
        {
            Visible = false;
        }

        public void Activate(Vector2 pos)
        {
            Position = pos;
            Bounds = new Rectangle((int)Position.X - (64 * Speed), (int)Position.Y - (64 * Speed),
                                   64 * Speed * 2, 64 * Speed * 2);

            Visible = true;

            var choose = MasterRandom.FRandom.Next(0, 3);

            if (choose ==1)
                PollenColor = Color.FromNonPremultiplied(232, 208, 80, 255);
            else if (choose == 0)
                PollenColor = Color.FromNonPremultiplied(168, 108, 0, 255);
            else
                PollenColor = Color.FromNonPremultiplied(124, 64, 0, 255);

            Size = MasterRandom.FRandom.Next(1, 4);
        }

        public static void SetDirAndSpeed(Vector2 dir, int speed)
        {
            Direction = dir;
            Speed = speed;
        }

        public void Update(float elapsed)
        {
            Position = Position + Direction * elapsed * Speed * 45;

            if (!InWorldBounds(Position))
                Reset();

            if (!DistanceBounds())
                Reset();
           
        }

        public void Reset()
        {
            Visible = false;

        }

        private bool DistanceBounds()
        {
            return Position.X >= Bounds.X && Position.X < Bounds.X + Bounds.Width &&
                   Position.Y >= Bounds.Y && Position.Y < Bounds.Y + Bounds.Height;
        }

        private static bool InWorldBounds(Vector2 pos)
        {
            return pos.X >= 64 && pos.X < 1024 &&
                   pos.Y >= 64 && pos.Y < 704;
        }

    }
}
