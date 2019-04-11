#region Using Statements
using Microsoft.Xna.Framework;
using System;

#endregion

namespace TempleGardens
{
    public class SprayEffect
    {
        public Vector2[] SprayBitsPositions { get; private set; }
        public Rectangle SourceRect { get; private set; }
        private Vector2[] direction;
        private float[] speed;

        private float timer;
        private static float timerDurtation = 1f;

        public bool Active { get; private set; }

        public Color[] SprayColor { get; private set; }
        public bool[] IsBigger { get; private set; }

        public SprayEffect()
        {
            Active = false;
            timer = 0;

            SprayBitsPositions = new Vector2[13];
            speed = new float[13];
            direction = new Vector2[13];
            SprayColor = new Color[13];
            IsBigger = new bool[13];

        }

        public void SetData(Vector2 origin, ColorPair thisColor)
        {
            var mainAngle = MasterRandom.FRandom.Next(180, 300);

            var spreadMin = mainAngle - 15;
            var spreadMax = mainAngle + 15;

            var tempAngle = 0f;

            for (var i = 0; i < SprayBitsPositions.Length; i++)
            {
                SprayBitsPositions[i] = origin;
                speed[i] = MasterRandom.FRandom.Next(25, 55);

                tempAngle = MathHelper.ToRadians(MasterRandom.FRandom.Next(spreadMin, spreadMax + 1));

                direction[i] = new Vector2((float)Math.Cos(tempAngle), (float)Math.Sin(tempAngle));

                direction[i].Normalize();

                if (MasterRandom.FRandom.NextBool())
                    SprayColor[i] = thisColor.Color1;
                else
                    SprayColor[i] = thisColor.Color2;

                IsBigger[i] = MasterRandom.FRandom.NextBool();
            }

            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timer += elapsed;

            for (var i = 0; i < SprayBitsPositions.Length; i++)
                SprayBitsPositions[i]  += direction[i] * speed[i] * elapsed;

            if (timer >= timerDurtation)
            {
                timer = 0;
                Active = false;
            }
        }

    }
}
