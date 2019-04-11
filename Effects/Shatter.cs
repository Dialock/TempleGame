using Microsoft.Xna.Framework;
using System.Linq;

namespace TempleGardens
{
    public class Shatter
    {
        public static Rectangle[] ShardsSource { get; private set; }

        public Rectangle[] ShardsFrame { get; private set; }
        public Vector2[] ShardLocations { get; private set; }
        public int ShardType { get; private set; }
        public Point[] GravityAndVariance { get; private set; }
        public Color ColorUsed { get; private set; }

        private float _yMax;
        private bool[] isDropping;
        public bool Done { get; private set; }

        public Shatter(int shardId, Vector2 origin ,Color color)
        {
            ShardType = shardId;
            ShardsFrame = new Rectangle[ShardsSource.Length];
            ShardLocations = new Vector2[ShardsSource.Length];
            GravityAndVariance = new Point[ShardsSource.Length];
            isDropping = new bool[ShardsSource.Length];
            ColorUsed = color;

            for (var i = 0; i < ShardsFrame.Length; i++)
            {
                int x = 0;
                int y = 0;

                GetMeh(ShardType, ref x, ref y);

                ShardsFrame[i] = new Rectangle(x + ShardsSource[i].X, y + ShardsSource[i].Y,ShardsSource[i].Width, ShardsSource[i].Height);
                ShardLocations[i] = new Vector2(x + origin.X + ShardsSource[i].X, y + origin.Y + ShardsSource[i].Y);
                GravityAndVariance[i] = new Point(MasterRandom.FRandom.Next(10, 16), MasterRandom.FRandom.Next(-4, 5));
                isDropping[i] = false;
            }

            _yMax = ShardLocations.Min(t => t.Y) - 65;
            Done = false;
        }

        public void Update()
        {
            if (Done)
                return;

            for (var i = ShardLocations.Length - 1; i >= 0; i--)
            {
                if (!isDropping[i])
                {
                    if (ShardLocations[i].Y >= _yMax)
                        ShardLocations[i] = new Vector2(ShardLocations[i].X + GravityAndVariance[i].Y, ShardLocations[i].Y - GravityAndVariance[i].X);
                    else
                        isDropping[i] = true;
                }
                else
                {
                    if (ShardLocations[i].Y <= 704)
                        ShardLocations[i] = new Vector2(ShardLocations[i].X + GravityAndVariance[i].Y, ShardLocations[i].Y + GravityAndVariance[i].X);
                }


            }

            if (ShardLocations.All(t => t.Y >= 704))
                Done = true;
        }

        public static void GetMeh(int type, ref int x, ref int y)
        {

            switch (type)
            {
                case 1:
                    x = 0;
                    y = 0;
                    break;
                case 2:
                    x = 64;
                    y = 0;
                    break;
                case 3:
                    x = 128;
                    y = 0;
                    break;
                case 4:
                    x = 0;
                    y = 64;
                    break;
                case 5:
                    x= 64;
                    y = 64;
                    break;
                case 6:
                    x = 128;
                    y = 64;
                    break;
                case 7:
                    x = 0;
                    y = 128;
                    break;
                case 8:
                    x = 64;
                    y = 128;
                    break;
                case 9:
                    x = 128;
                    y = 128;
                    break;
            }

        }

        public static void Init()
        {
            ShardsSource = new Rectangle[]
            {
                new Rectangle(17, 0, 10,15),
                new Rectangle(34, 9, 8, 10),
                new Rectangle(24, 27, 10, 15),
                new Rectangle(6, 27, 18, 10),
                new Rectangle(37, 36, 16, 14),
                new Rectangle(10, 43, 14, 10),
                new Rectangle(29, 42, 8, 16)
            };
        }
    }
}
