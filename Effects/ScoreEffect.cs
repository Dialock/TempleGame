using Microsoft.Xna.Framework;

namespace TempleGardens
{
    public class ScoreEffect
    {

        public Vector2 GlyphPostion { get; private set; }
        public Rectangle[] GlyphRectangles { get; private set; }

        public bool Done { get; private set; }

        private float fadeDelay = 0.04f;
        public Color FontColor { get; private set; }

        private float a, g, b, r;

        public ScoreEffect(int score, Vector2 start)
        {
            Done = false;

            var size = ("+" + score.ToString()).ToCharArray();

            GlyphPostion = start;
            GlyphRectangles = GetNumberSources(size);

            a = 1f;
            r = 0f;
            b = 0f;
            g = 0f;

            FontColor = new Color(r, g, b, a);

        }

        public void Update(GameTime gameTime)
        {
            if (Done)
                return;

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            fadeDelay -= elapsed;

            if (fadeDelay <= 0)
            {
                fadeDelay = 0.04f;

                a -= 0.05f;
                r += 0.05f;
                g += 0.005f;
                b += 0.005f;

                FontColor = new Color(r, g, b, a);

                GlyphPostion += new Vector2(0, -1);
            }

            if (a <= 0.0f)
                Done = true;

        }

        public static Rectangle[] GetNumberSources(char[] score)
        {
            var tempRect = new Rectangle[score.Length];

            for (var i = 0; i < score.Length; i++)
            {
                switch (score[i])
                {
                    case '1':
                        tempRect[i] = new Rectangle(704, 64, 16, 16);
                        break;
                    case '2':
                        tempRect[i] = new Rectangle(720, 64, 16, 16);
                        break;
                    case '3':
                        tempRect[i] = new Rectangle(736, 64, 16, 16);
                        break;
                    case '4':
                        tempRect[i] = new Rectangle(752, 64, 16, 16);
                        break;
                    case '5':
                        tempRect[i] = new Rectangle(768, 64, 16, 16);
                        break;
                    case '6':
                        tempRect[i] = new Rectangle(784, 64, 16, 16);
                        break;
                    case '7':
                        tempRect[i] = new Rectangle(704, 80, 16, 16);
                        break;
                    case '8':
						tempRect[i] = new Rectangle(720, 80, 16, 16);
                        break;
                    case '9':
						tempRect[i] = new Rectangle(736, 80, 16, 16);
                        break;
                    case '0':
						tempRect[i] = new Rectangle(752, 80, 16, 16);
                        break;
                    case '+':
						tempRect[i] = new Rectangle(768, 80, 16, 16);
                        break;
                    default:
                       tempRect[i] = new Rectangle(80, 16, 16, 16);
                        break;
                }

            }

            return tempRect;
        }
    }
}
