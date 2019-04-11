using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TempleGardens
{
    public class EndStageScreen : GameState
    {
        private Color rectColor = Color.Red;

        public Color MoodColor { get; set; }

        private int offsetX = 0;
        private int offsetY = 0;
        private Point centerScreen = new Point(1366 / 2, 768 / 2);

        public int EndGameScore { get; set; }

		private ImageButton endButton;

        private Vector2[] gridBacking = new Vector2[] {
            new Vector2(-64, -64),
            new Vector2(669, -64),
            new Vector2(-64, 384),
            new Vector2(669, 384)
        };

        private int xpNeededToLevel;
        private int currentEXPPool;
        public int CurrentLvl { get; private set; }
        public int XPForThisLevel { get; private set; }
        public int FullXPPool { get; private set; }
        private int tempDrainPool;
        private int _currentWidth = 0;

        private const int GaugeWidth = 544;
        private float timer = 0f;

        private bool gaugeDone = false;

        public int EndSeason { get; set; }
        private int matches;

        IOSNumberSpitter numberSpitter;

        public EndStageScreen(GameStateManager manager, Screens screenName, string endType, int endScore, int matches)
            : base(manager, screenName)
        {
            this.matches = matches;

            endButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(296, 1488, 280, 104), new Vector2(500, 128));
			endButton.DrawLayer = 0.99f;
			endButton.Color = Color.White;
			endButton.Name = "End";
			endButton.Selected += endButton_Selected;
            endButton.HasFocus = false;
			ControlManager.Add (endButton);

            switch (endType)
            {
                case "ClassicEnd":
                case "ZenEnd":
                    MoodColor = Color.FromNonPremultiplied(255, 242, 0, 255);
                    break;
                case "Won":
                    MoodColor = Color.LightGreen;
                    break;
                case "Lost":
                default:
                    MoodColor = Color.Tomato;
                    break;
            }

            currentEXPPool = endScore;
			CurrentLvl = TempleMain.Player.Rank;
			XPForThisLevel = TempleMain.Player.ScoreToLevel;
            tempDrainPool = XPForThisLevel;

            Evalute();

            numberSpitter = new IOSNumberSpitter(Screens.EndStageScreen);
                
        }

        public override void Update(GameTime gameTime)
        {
            numberSpitter.Update(currentEXPPool, xpNeededToLevel, matches, EndSeason, CurrentLvl);

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timer += elapsed;

            if (timer >= 0.04f && !gaugeDone)
            {
                if (currentEXPPool > 0)
                {
                    var taker = 0;

                    var diff = xpNeededToLevel - tempDrainPool;

                    if (diff < 2000)
                        taker = 125;
                    else if (diff >= 2001 && diff < 5000)
                        taker = 200;
                    else if (diff >= 5001 && diff < 10000)
                        taker = 300;
                    else if (diff >= 10001 && diff < 15000)
                        taker = 900;
                    else
                        taker = 2000;

                    var buffer = currentEXPPool - taker;

                    if (buffer < 0)
                        taker = currentEXPPool;

                    buffer = (int)tempDrainPool + taker;

                    if (buffer > xpNeededToLevel)
                        taker = (int)xpNeededToLevel - (int)tempDrainPool;

                    currentEXPPool -= taker;

                    tempDrainPool += taker;

                    FullXPPool += taker;

                    if (tempDrainPool != 0)
                        _currentWidth = (int)(GaugeWidth * ((double)tempDrainPool / xpNeededToLevel));
                    else
                        _currentWidth = 0;

                    if (tempDrainPool >= xpNeededToLevel)
                    {
                        tempDrainPool = 0;

                        CurrentLvl++;

                        if (CurrentLvl == 5)
                            ScreenManager.GameReference.EvaluateAchievements("Lvl5");
                        else if (CurrentLvl == 10)
                            ScreenManager.GameReference.EvaluateAchievements("Lvl10");
                        else if (CurrentLvl == 15)
                            ScreenManager.GameReference.EvaluateAchievements("Lvl15");

                        Evalute();
                    }

                    timer = 0;
                }
                else
                {
                    gaugeDone = true;
                    XPForThisLevel = (int)tempDrainPool;
                }
            }

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                ScreenManager.GameReference.ManagePlacardState(gameTime);

            ControlManager.Update(gameTime);

            offsetX = (centerScreen.X - InputHandler.TouchVectorScaled.ToPoint().X) / 32;
            offsetY = (centerScreen.Y - InputHandler.TouchVectorScaled.ToPoint().Y) / 32;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < 4; i++)
				spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(gridBacking[i].X + offsetX, gridBacking[i].Y + offsetY), new Rectangle(0, 736, 734, 448), MoodColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);

            numberSpitter.Draw(spriteBatch);

            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(320, 256), new Rectangle(64, 64, 640, 312), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle(368, 336, _currentWidth, 40), new Rectangle(928, 640, 64, 40), rectColor, 0f, Vector2.Zero, SpriteEffects.None, 0.55f);

            // drain score
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(336, 282), new Rectangle(200, 168, 224, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);

            //spriteBatch.DrawString(SpriteLoader.Font20, "Session EXP: " + currentEXPPool.ToString(), new Vector2(470, 282), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);

            // to level 
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(336, 394), new Rectangle(168, 48, 170, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            //spriteBatch.DrawString(SpriteLoader.Font20, "To Level: " + tempDrainPool.ToString() + "/" + xpNeededToLevel.ToString(), new Vector2(470, 390), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            // seasons played
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(336, 440), new Rectangle(200, 88, 314, 38), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            // rank
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(336, 486), new Rectangle(200, 128, 128, 40), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            //matches
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(584, 486), new Rectangle(336, 128, 175, 34), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            //spriteBatch.DrawString(SpriteLoader.Font20, "Rank: " + CurrentLvl.ToString(), new Vector2(770, 282), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            //spriteBatch.DrawString(SpriteLoader.Font20, "Seasons Played: " + EndSeason.ToString(), new Vector2(470, 412), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);

            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(460, 600), new Rectangle(896, 256, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(532, 600), new Rectangle(960, 256, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(604, 600), new Rectangle(896, 320, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.66f);

            spriteBatch.Draw(SpriteLoader.IosMainSheet, Vector2.Zero, new Rectangle(760, 2032, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 712), new Rectangle(760, 2032, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 8), new Rectangle(2032, 1312, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1272, 8), new Rectangle(2032, 1312, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(276, ScreenManager.GameReference.YPlacardOffset), new Rectangle(1296, 1680, 728, 104),
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            ControlManager.Draw(spriteBatch);
        }


		private void endButton_Selected(object sender, System.EventArgs e)
		{
			ScreenManager.GameReference.ScreenControl(this.ScreenName, "Back");
		}

        private void Evalute()
        {
            var soup = Math.Pow(CurrentLvl, 1.95) * 10000;

            xpNeededToLevel = (int)soup;

            if (tempDrainPool != 0)
                _currentWidth = (int)(GaugeWidth * ((double)tempDrainPool / xpNeededToLevel));
            else
                _currentWidth = 0;




        }
    }
}
