
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TempleGardens
{
    public class ImageButton : Control
    {
        public Rectangle ButtonBounds { get; set; }
        public Texture2D ButtonTexture { get; private set; }
        public Texture2D HighlightTexture { get; private set; }
        public Rectangle HighlightBounds { get; private set; }
        public Rectangle SourceRectangle { get; set; }

        public Color HighlightColor { get; set; }
        public Color FocusColor { get; set; }
        public bool Highlighted { get; private set; }

        public bool HasPlayed { get; private set; }

        public ImageButton(Rectangle sourceRect, Vector2 pos)
        {
            HighlightTexture = SpriteLoader.IosMainSheet;
            Position = pos;
            SourceRectangle = sourceRect;

            ButtonBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)sourceRect.Width, (int)sourceRect.Height);
            HighlightBounds = new Rectangle(ButtonBounds.X - 5, ButtonBounds.Y - 5,
                ButtonBounds.Width + 10, ButtonBounds.Height + 10);
            Highlighted = false;

            HighlightColor = Color.Red;
            FocusColor = Color.DimGray;
        }

		public ImageButton(Texture2D buttonImage, Vector2 pos)
		{
			ButtonTexture = buttonImage;
			Position = pos;

			HighlightBounds = new Rectangle(ButtonBounds.X - 5, ButtonBounds.Y - 5,
				ButtonBounds.Width + 10, ButtonBounds.Height + 10);
			Highlighted = false;

			HighlightColor = Color.FromNonPremultiplied(new Vector4(0.66f, 0.22f, 0.12f, 0.7f));
			FocusColor = Color.FromNonPremultiplied(new Vector4(0.5f, 0.5f, 0.5f, 0.4f));
		}

        public ImageButton(Texture2D buttonImage, Texture2D highlightImage, Rectangle sourceRect, Vector2 pos)
        {
            ButtonTexture = buttonImage;
            HighlightTexture = highlightImage;
            Position = pos;
            SourceRectangle = sourceRect;

            ButtonBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)sourceRect.Width,(int)sourceRect.Height);
            HighlightBounds = new Rectangle(ButtonBounds.X - 5, ButtonBounds.Y - 5,
                ButtonBounds.Width + 10, ButtonBounds.Height + 10);
            Highlighted = false;

            HighlightColor = Color.White;
            FocusColor = Color.DimGray;

        }



        //public override void PreDraw() { }
        public override void HandleInput() { }

        public override void Update(GameTime gameTime)
        {

            #region Mouse
            if (!ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()))
            {
                Highlighted = true;
                HasFocus = false;

                if (HasPlayed)
                    HasPlayed = false;

            }

            if (ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()))
            {
                Highlighted = true;
                HasFocus = true;

            }

            if (ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) &&
                InputHandler.FingerRaised())
            {
                HasFocus = true;
                base.OnSelected(null);
            }
            #endregion

            #region Touch
            //if (!ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()))
            //{
            //    Highlighted = true;
            //    HasFocus = false;

            //    if (HasPlayed)
            //        HasPlayed = false;

            //}


            //if (ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()))
            //{
            //    Highlighted = true;
            //    HasFocus = true;

            //}

            //if (ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) &&
            //    InputHandler.FingerRaised())
            //{
            //    HasFocus = true;
            //    base.OnSelected(null);
            //}

            #endregion

            //if (HasFocus && !HasPlayed)
            //{
            //    SoundBoard.ButtonHighlight.Play();
            //    HasPlayed = true;
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (HighlightTexture != null)
            {
                if (HasFocus)
                {
                    spriteBatch.Draw(ButtonTexture, Position, SourceRectangle, FocusColor, 0f, Vector2.Zero, 1f,
                        SpriteEffects.None, DrawLayer);
                    spriteBatch.Draw(HighlightTexture, HighlightBounds, new Rectangle(832, 608, 64,64), HighlightColor, 0f, Vector2.Zero,
                         SpriteEffects.None, DrawLayer - 0.01f);
                }
                else
                {
                    spriteBatch.Draw(ButtonTexture, Position, SourceRectangle, Color, 0f, Vector2.Zero, 1f,
                        SpriteEffects.None, DrawLayer);
                }
            }
            else
            {
                if (HasFocus)
                {
                    spriteBatch.Draw(ButtonTexture, Position, SourceRectangle, FocusColor, 0f, Vector2.Zero, 1f,
                        SpriteEffects.None, DrawLayer);
                }
                else
                {
                    spriteBatch.Draw(ButtonTexture, Position, SourceRectangle, HighlightColor, 0f, Vector2.Zero, 1f,
                        SpriteEffects.None, DrawLayer);
                }
            }
        }
    }
}
