using Microsoft.Xna.Framework;
using System;

namespace TempleGardens
{
    public class Absorb
    {

        public Vector2[] ShardPositions { get; private set; }
        private Vector2[] shardDirections;
        public bool[] useColor1 { get; private set; }
        private float[] speed;

        private Rectangle _bounds;

        public static Color color1 = Color.FromNonPremultiplied(144, 136, 120, 255);
        public static Color color2 = Color.FromNonPremultiplied(144, 168, 172, 255);

        public bool Done { get; private set; }

        public Absorb(Vector2 origin, int max)
        {
            ShardPositions = new Vector2[max];
            shardDirections = new Vector2[max];
            useColor1 = new bool[max];
            speed = new float[max];

            for (var i = 0; i < max; i++)
            {
                var angle = (float)MathHelper.ToRadians(MasterRandom.FRandom.Next(0, 360));
                var distance = (float)MasterRandom.FRandom.Next(48, 96);


                var tempX = (float)(distance * Math.Cos(angle));
                var tempY =  (float)(distance * Math.Sin(angle));

                ShardPositions[i] = new Vector2(origin.X + tempX, origin.Y + tempY);

                var tempDir = origin - ShardPositions[i];

                tempDir.Normalize();

                shardDirections[i] = tempDir;

                useColor1[i] = MasterRandom.FRandom.NextBool();

                speed[i] = MasterRandom.FRandom.Next(112, 190);
            }
            _bounds = new Rectangle((int)origin.X - 6, (int)origin.Y - 6, 12, 12);
        }

        public void Update(GameTime gameTime)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (var i = 0; i < ShardPositions.Length; i++)
            {
                ShardPositions[i] += shardDirections[i] * speed[i] * elapsed;

                if (_bounds.Contains(ShardPositions[i].ToPoint()))
                    Done = true;
            }
        }

    }
}
