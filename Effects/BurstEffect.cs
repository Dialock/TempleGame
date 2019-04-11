using Microsoft.Xna.Framework;

namespace TempleGardens
{
    public class BurstEffect
    {
        public const int MaxBursts = 36;
        public Vector2[] BurstPositions { get; private set; }
        private Vector2[] directions;
        public bool[] isColor1 { get; private set; }
        private int[] _speed;
        public bool Done { get; private set; }

        public ColorPair ColorUsed { get; private set; }
        //private static int _minAlpha;

        private float _frameTimer;
        private static float _frameTimerDuration;

        private int alphaValue = 255;
        private int fadeIncrement = 6;
        private float fadeDelay = 0.035f;

        public BurstEffect(ColorPair startColors, Vector2 originPos)
        {
            Init();

            BurstPositions = new Vector2[MaxBursts];
            _speed = new int[MaxBursts];
            isColor1 = new bool[MaxBursts];

            ColorUsed = startColors;

            for (var x = 0; x < 4; x++)
                for (var y = 0; y < 4; y++)
                {
                    BurstPositions[y * 4 + x] = new Vector2(
                        (x * 2) + 22 + (int)originPos.X,
                        (y * 2) + 22 + (int)originPos.Y);


                    _speed[y * 4 + x] = MasterRandom.FRandom.Next(25, 45);

                    isColor1[y * 4 + x] = MasterRandom.FRandom.NextBool();
                }

            _frameTimer = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (Done)
                return;

            var elasped = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameTimer += elasped;
            fadeDelay -= elasped;

            if (fadeDelay <= 0 && alphaValue > 0)
            {
                fadeDelay = 0.035f;

                alphaValue -= fadeIncrement;

                var colr1 = Color.FromNonPremultiplied(ColorUsed.Color1.R, ColorUsed.Color1.G, ColorUsed.Color1.B, (byte)MathHelper.Clamp(alphaValue, 1, 255));
                var colr2 = Color.FromNonPremultiplied(ColorUsed.Color2.R, ColorUsed.Color2.G, ColorUsed.Color2.B, (byte)MathHelper.Clamp(alphaValue, 1, 255));

                ColorUsed = new ColorPair(colr1, colr2);
            }

            if (_frameTimer >= _frameTimerDuration)
            {
                Done = true;
            }
            

            for (var i = 0; i < BurstPositions.Length; i++)
            {
                BurstPositions[i] += directions[i] * _speed[i] * elasped;
            }
        }

        public void Init()
        {
            _frameTimerDuration = 1.4f;
        
            directions = new Vector2[MaxBursts];
            for (var t = 0; t < MaxBursts; t++)
            {
                var i = 0.0f;
                var j = 0.0f;

                i = MasterRandom.FRandom.Next(-9, 10) * .1f;
                j = MasterRandom.FRandom.Next(-9, 10) * .1f;

                if (i == 0)
                {
                    if (MasterRandom.FRandom.NextBool())
                        i += 0.1f;
                    else
                        i -= 0.1f;
                }

                if (j == 0)
                {
                    if (MasterRandom.FRandom.NextBool())
                        j += 0.1f;
                    else
                        j -= 0.1f;
                }

                directions[t] = new Vector2(i, j);
                directions[t].Normalize();
            }
        }

    }
}
