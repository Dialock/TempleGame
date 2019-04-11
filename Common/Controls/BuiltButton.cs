#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TempleGardens
{
    public class BuiltButton : Control
    {
        public bool ContainsMouse { get; private set; }
        public Rectangle ButtonBounds { get; private set; }
        public Rectangle ButtonSource { get; private set; }
        public Rectangle BorderSource { get; private set; }
        public Rectangle BorderShadowSource { get; private set; } 

        private bool bounceUp = false;
        private int targetPoint = 0;

        private float moveTimer;
        private static float moveTimerDuration = 0.02f;

        public bool Active { get; private set; }

        public Color BorderColor { get; set; }

        public BuiltButton(string name, Vector2 startPosition)
        {
            SetBounds(name);
            Name = name;

            SetBounds(name);

            Position = startPosition;

            moveTimer = 0;

            BorderColor = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            if ((Name == "Resume" || Name == "End") && !Active)
                Active = true;

            if (Active)
            {
                //				if (ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) && !ContainsMouse)
                //                    ContainsMouse = true;
                //				else if (!ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) && ContainsMouse)
                //                {
                //                    HasFocus = false;
                //                    ContainsMouse = false;
                //                }
                //				if (ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) &&
                //					InputHandler.IsScreenBeingTouched())
                //                {
                //                    HasFocus = true;
                //                    base.OnSelected(null);
                //                }
                #region Mouse
                if (ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) && !ContainsMouse)
                    ContainsMouse = true;
                else if (!ButtonBounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) && ContainsMouse)
                {
                    HasFocus = false;
                    ContainsMouse = false;
                }

                if (ContainsMouse && InputHandler.FingerRaised())
                {
                    HasFocus = true;
                    base.OnSelected(null);
                #endregion

                    #region Touch
                    //if (ButtonBounds.Contains (InputHandler.TouchVectorScaled.ToPoint ()) && !ContainsMouse)
                    //    ContainsMouse = true;
                    //else if (!ButtonBounds.Contains (InputHandler.TouchVectorScaled.ToPoint ()) && ContainsMouse) {
                    //    HasFocus = false;
                    //    ContainsMouse = false;
                    //}

                    //if (ContainsMouse && InputHandler.FingerRaised ()) {
                    //    HasFocus = true;
                    //    base.OnSelected (null);
                }
                    #endregion
            }
            else
            {
                var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                moveTimer += elapsed;

                if (moveTimer >= moveTimerDuration)
                {
                    moveTimer = 0;

                    if (!bounceUp)
                    {
                        Position = new Vector2(Position.X, Position.Y + 10);

                        if (Position.Y >= targetPoint + 22)
                            bounceUp = true;
                    }
                    else
                    {
                        Position = new Vector2(Position.X, Position.Y - 3);
                        if (Position.Y <= targetPoint)
                        {
                            Enabled = true;
                            Active = true;

                            ButtonBounds = new Rectangle((int)Position.X, (int)Position.Y, ButtonBounds.Width, ButtonBounds.Height);


                        }
                    }
                }
            }

            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Name != "Help")
            {
                spriteBatch.Draw(SpriteLoader.IosTextSheet, Position, ButtonSource, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, DrawLayer);
                spriteBatch.Draw(SpriteLoader.IosSecondarySheet, new Vector2(Position.X + 16, Position.Y + 16), ButtonSource, Color.White, 0f, Vector2.Zero, 1F, SpriteEffects.None, DrawLayer - 0.11f);

                if (ContainsMouse)
                {
                    spriteBatch.Draw(SpriteLoader.IosSecondarySheet, new Vector2(Position.X - 16, Position.Y - 16), BorderSource, BorderColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, DrawLayer);
                    spriteBatch.Draw(SpriteLoader.IosSecondarySheet, new Vector2(Position.X, Position.Y), BorderShadowSource, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, DrawLayer - 0.11f);
                }
            }
            else
            {
                spriteBatch.Draw(SpriteLoader.IosMainSheet, Position, ButtonSource, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.81f);
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle((int)Position.X + 16, (int)Position.Y + 16, 72, 88), new Rectangle(832, 608, 16, 16), Color.FromNonPremultiplied(0, 0, 0, 64), 0f, Vector2.Zero, SpriteEffects.None, 0.80f);

                if (ContainsMouse)
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle((int)Position.X - 4, (int)Position.Y - 4, 80, 98), new Rectangle(832, 608, 16, 16), Color.FromNonPremultiplied(56, 211, 44, 99), 0f, Vector2.Zero, SpriteEffects.None, 0.81f);
            }
        }

        //public override void PreDraw() { }
        public override void HandleInput() { }

        private void SetBounds(string name)
        {
            if (name == "Classic")
            {
                ButtonBounds = new Rectangle(0, 0, 408, 72);
                ButtonSource = new Rectangle(8, 16, 408, 72);
                BorderSource = new Rectangle(8, 344, 440, 104);
                BorderShadowSource = new Rectangle(8, 696, 440, 104);

                targetPoint = 128;
            }
            else if (name == "Endurance")
            {
                ButtonBounds = new Rectangle(0, 0, 568, 72);
                ButtonSource = new Rectangle(8, 96, 568, 72);
                BorderSource = new Rectangle(272, 464, 600, 104);
                BorderShadowSource = new Rectangle(272, 816, 600, 104);

                targetPoint = 256;
            }
            else if (name == "Options")
            {
                ButtonBounds = new Rectangle(0, 0, 416, 72);
                ButtonSource = new Rectangle(8, 176, 416, 72);
                BorderSource = new Rectangle(8, 344, 440, 104);
                BorderShadowSource = new Rectangle(8, 696, 440, 104);

                targetPoint = 384;
            }
            else if (name == "Exit")
            {
                ButtonBounds = new Rectangle(0, 0, 216, 72);
                ButtonSource = new Rectangle(8, 256, 216, 72);
                BorderSource = new Rectangle(8, 464, 248, 104);
                BorderShadowSource = new Rectangle(8, 816, 248, 104);

                targetPoint = 512;
            }
            else if (name == "Help")
            {
                ButtonBounds = new Rectangle(0, 0, 96, 96);
                ButtonSource = new Rectangle(924, 404, 72, 88);

                targetPoint = 104;
            }
            else if (name == "End")
            {
                ButtonSource = new Rectangle(8, 496, 392, 72);
                ButtonBounds = new Rectangle(0, 0, 392, 72);
                BorderSource = new Rectangle(0, 0, 456, 120);

                targetPoint = (int)Position.Y;
            }
            else if (name == "Resume")
            {
                ButtonSource = new Rectangle(8, 416, 376, 72);
                ButtonBounds = new Rectangle(0, 0, 376, 72);
                BorderSource = new Rectangle(0, 0, 456, 120);

                targetPoint = (int)Position.Y;
            }


        }
    }
}
