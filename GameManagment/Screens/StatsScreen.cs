#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TempleGardens
{
    public class StatsScreen : GameState
    {
        private int offsetX = 0;
        private int offsetY = 0;
        private Point centerScreen = new Point(1280 / 2, 720 / 2);

        private Vector2[] gridBacking = new Vector2[] {
            new Vector2(-64, -64),
            new Vector2(669, -64),
            new Vector2(-64, 384),
            new Vector2(669, 384)
        };

        private Rectangle[] rects;

        //private ImageButton endButton;

        private Rectangle[] SourceRectangles = new Rectangle[]
        {
            new Rectangle(544, 576, 416, 72),
            new Rectangle(544, 672, 400, 32),
            new Rectangle(544, 704, 448, 32),
            new Rectangle(544, 736, 412, 68),
            new Rectangle(544, 811, 391, 76),
            new Rectangle(544, 384, 200, 68),
            new Rectangle(544, 896, 256, 32),
            new Rectangle(544, 928, 288, 32),
            new Rectangle(544, 960, 288, 32),
            new Rectangle(32, 960, 279, 32)
        };


        private int helperText = -1;
            
        public StatsScreen(GameStateManager manager, Screens screenName)
            : base(manager, screenName)
        {
            rects = new Rectangle[10];

            for (var y = 0; y < 2; y++)
                for (var x = 0; x < 5; x++)
                {
                    rects[y * 5 + x] = new Rectangle(x * 128 + 320, y * 128 + 384, 96, 96);
                }


            //endButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(296, 1488, 280, 104), new Vector2(500, 128));
            //endButton.DrawLayer = 0.99f;
            //endButton.Color = Color.White;
            //endButton.Name = "End";
            //endButton.Selected += endButton_Selected;
            //endButton.HasFocus = false;
            //ControlManager.Add(endButton);
        }

        public override void Update(GameTime gameTime)
        {
            offsetX = (centerScreen.X - InputHandler.TouchVectorScaled.ToPoint().X) / 32;
            offsetY = (centerScreen.Y - InputHandler.TouchVectorScaled.ToPoint().Y) / 32;

            for (var i = 0; i < rects.Length; i++)
            {
                if (rects[i].Contains(InputHandler.TouchVectorScaled.ToPoint()))
                {
                    helperText = i;
                    break;
                }
                else
                    helperText = -1;
            }

//            if (InputHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
//                ScreenManager.GameReference.ScreenControl(ScreenName, "Back");

            if (InputHandler.FingerRaised())
                ScreenManager.GameReference.ScreenControl(ScreenName, "Back");

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                ScreenManager.GameReference.ManagePlacardState(gameTime);

            ControlManager.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < 4; i++)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(gridBacking[i].X + offsetX, gridBacking[i].Y + offsetY), new Rectangle(0, 736, 734, 448), Color.FromNonPremultiplied(65, 105, 199, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);

            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(128, 64), new Rectangle(144, 240, 304, 38), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(128, 128), new Rectangle(177, 208, 111, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);

            var toots = IOSNumberSpitter.UpdateNumberList(TempleMain.Player.TotalScore.ToString().ToCharArray());

            var teets = IOSNumberSpitter.UpdateNumberList(TempleMain.Player.Rank.ToString().ToCharArray());

            for (var i = 0; i < toots.Count; i++)
                spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(i * 24 + 432, 64), toots[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.77f);

            for (var i = 0; i < teets.Count; i++)
                spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(i + 24 + 256, 128), teets[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.77f);

            for (var y = 0; y < 2; y++)
                for (var x = 0; x < 5; x++)
                {
                    if (!TempleMain.Player.PlayerTrophies[y * 5 + x].Awarded)
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(x * 128 + 320, y * 128 + 384), new Rectangle(1728, 1792, 96, 96), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.77f);
                    else
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(x * 128 + 320, y * 128 + 384), GetBorderSource(TempleMain.Player.PlayerTrophies[y * 5 + x].Name), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.77f);
                }

            spriteBatch.Draw(SpriteLoader.IosMainSheet, Vector2.Zero, new Rectangle(760, 2032, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 712), new Rectangle(760, 2032, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 8), new Rectangle(2032, 1312, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1272, 8), new Rectangle(2032, 1312, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);

            if (helperText != -1)
                spriteBatch.Draw(SpriteLoader.IosTextSheet, new Vector2(320, 256), SourceRectangles[helperText], Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.88f);

            ControlManager.Draw(spriteBatch);

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(276, ScreenManager.GameReference.YPlacardOffset), new Rectangle(1296, 1680, 728, 104),
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        //private void endButton_Selected(object sender, System.EventArgs e)
        //{

        //}

        private Rectangle GetBorderSource(string name)
        {
            switch (name)
            {
                case "NoRocks10":
                    return new Rectangle(1248, 1792, 96, 96);
                case "Play15Norm":
                    return new Rectangle(1344, 1792, 96, 96);
                case "Played15Zen":
                    return new Rectangle(1440, 1792, 96, 96);
                case "NoAutoFill":
                    return new Rectangle(1536, 1792, 96, 96);
                case "CompleteOne":
                    return new Rectangle(1632, 1792, 96, 96);
                case "CompleteAll":
                    return new Rectangle(1248, 1888, 96, 96);
                case "Lvl5":
                    return new Rectangle(1344, 1888, 96, 96);
                case "Lvl10":
                    return new Rectangle(1440, 1888, 96, 96);
                case "Lvl15":
                    return new Rectangle(1536, 1888, 96, 96);
                case "Season100":
                    return new Rectangle(1632, 1888, 96, 96);
                default:
                    return new Rectangle(1728, 1792, 96, 96);
            }
        }
    }
}
