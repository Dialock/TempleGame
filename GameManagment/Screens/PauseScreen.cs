using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace TempleGardens
{
    public class PauseScreen : GameState
    {
        public string Caller { get; set; }

        ImageButton endButton;
        ImageButton resumeButton;

		ImageButton soundButton;
		ImageButton musicButton;
        ImageButton vibButton;

        private int offsetX = 0;
        private int offsetY = 0;
        private Point centerScreen = new Point(1280 / 2, 720 / 2);

        private Vector2[] gridBacking = new Vector2[] {
            new Vector2(-64, -64),
            new Vector2(669, -64),
            new Vector2(-64, 384),
            new Vector2(669, 384)
        };

        public PauseScreen(GameStateManager manager, Screens screenName)
            : base(manager, screenName)
        {
            Caller = "None";

            BuildControls();
        }

        public override void Update(GameTime gameTime)
        {

            offsetX = (centerScreen.X - InputHandler.TouchVectorScaled.ToPoint().X) / 32;
            offsetY = (centerScreen.Y - InputHandler.TouchVectorScaled.ToPoint().Y) / 32;

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                ScreenManager.GameReference.ManagePlacardState(gameTime);

            ControlManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteLoader.IosTextSheet, new Vector2(468, 176), new Rectangle(432, 16, 376, 72), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.55f);
            spriteBatch.Draw(SpriteLoader.IosSecondarySheet, new Vector2(436, 160), new Rectangle(8, 344, 440, 104), Color.LightSalmon, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.54f);

            spriteBatch.Draw(SpriteLoader.IosMainSheet, Vector2.Zero, new Rectangle(768, 2040, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 712), new Rectangle(768, 2040, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 8), new Rectangle(2040, 1320, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1272, 8), new Rectangle(2040, 1320, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(276, ScreenManager.GameReference.YPlacardOffset), new Rectangle(1296, 1680, 728, 104),
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            for (var i = 0; i < 4; i++)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(gridBacking[i].X + offsetX, gridBacking[i].Y + offsetY), new Rectangle(0, 736, 734, 448), Color.FromNonPremultiplied(178, 30, 104, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);

            ControlManager.Draw(spriteBatch);
        }

        private void BuildControls()
        {
//			optionsButton = new ImageButton(SpriteLoader.MenuButtonText, null, new Rectangle(8, 176, 408, 72), new Vector2(500, 304));
//			optionsButton.ButtonBounds = new Rectangle(500, 304, 408, 72);
//			optionsButton.HighlightColor = Color.White;
//			optionsButton.FocusColor = Color.Turquoise;
//			optionsButton.Selected += optionsButton_Selected;
//			optionsButton.HasFocus = false;
//			ControlManager.Add(optionsButton);


            resumeButton = new ImageButton(SpriteLoader.IosTextSheet, null, new Rectangle(432, 256, 376, 72), new Vector2(468, 304));
            resumeButton.ButtonBounds = new Rectangle(500, 304, 376, 72);
            resumeButton.HighlightColor = Color.White;
            resumeButton.FocusColor = Color.Turquoise;
            resumeButton.Selected += resumeButton_Selected;
            resumeButton.HasFocus = false;
            resumeButton.DrawLayer = 0.15f;
            ControlManager.Add(resumeButton);


            endButton = new ImageButton(SpriteLoader.IosTextSheet, null, new Rectangle(432, 176, 392, 72), new Vector2(468, 400));
            endButton.ButtonBounds = new Rectangle(492, 400, 392, 72);
            endButton.HighlightColor = Color.White;
            endButton.FocusColor = Color.Turquoise;
            endButton.Selected += endButton_Selected;
            endButton.HasFocus = false;
            endButton.DrawLayer = 0.15f;
            ControlManager.Add(endButton);

			if (TempleMain.Player.MusicActive)
                musicButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(576, 576, 64, 64), new Vector2(468, 504));
			else
                musicButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(576, 640, 64, 64), new Vector2(468, 504));

			musicButton.Selected += musicButton_Selected;
			musicButton.HighlightColor = Color.White;
			musicButton.DrawLayer = 0.44f;
			ControlManager.Add(musicButton);

			if (TempleMain.Player.SoundActive)
                soundButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(512, 576, 64, 64), new Vector2(796, 504));
			else
                soundButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(512, 640, 64, 64), new Vector2(796, 504));

			soundButton.Selected += soundButton_Selected;
			soundButton.HighlightColor = Color.White;
			soundButton.DrawLayer = 0.44f;
			ControlManager.Add(soundButton);

            if (TempleMain.Player.VibrateOn)
                vibButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(640, 576, 64, 64), new Vector2(632, 504));
            else
                vibButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(640, 640, 64, 64), new Vector2(632, 504));

            vibButton.Selected += vibButton_Selected;
            vibButton.HighlightColor = Color.White;
            vibButton.DrawLayer = 0.44f;
            ControlManager.Add(vibButton);
        }

//		void optionsButton_Selected(object sender, EventArgs e)
//		{
//			ScreenManager.GameReference.ScreenControl(ScreenName, "OptionsGame");
//		}

        void resumeButton_Selected(object sender, EventArgs e)
        {
            if (Caller == "Classic")
                ScreenManager.GameReference.ScreenControl(ScreenName, "ClassicUn");
            else if (Caller == "Zen")
                ScreenManager.GameReference.ScreenControl(ScreenName, "ZenUn");

        }

        void endButton_Selected(object sender, EventArgs e)
        {
            if (Caller == "Classic")
                ScreenManager.GameReference.ScreenControl(ScreenName, "ClassicEnd");
            else if (Caller == "Zen")
                ScreenManager.GameReference.ScreenControl(ScreenName, "ZenEnd");
        }

        void vibButton_Selected(object sender, EventArgs e)
        {
            TempleMain.Player.VibrateOn = !TempleMain.Player.VibrateOn;

            if (TempleMain.Player.VibrateOn)
                vibButton.SourceRectangle = new Rectangle(640, 576, 64, 64);
            else
                vibButton.SourceRectangle = new Rectangle(640, 640, 64, 64);
        }

		void soundButton_Selected(object sender, EventArgs e)
		{
			TempleMain.Player.SoundActive = !TempleMain.Player.SoundActive;

			if (TempleMain.Player.SoundActive)
                soundButton.SourceRectangle = new Rectangle(512, 576, 64, 64);
			else
                soundButton.SourceRectangle = new Rectangle(512, 640, 64, 64);
		}

		void musicButton_Selected(object sender, EventArgs e)
		{
			TempleMain.Player.MusicActive = !TempleMain.Player.MusicActive;

            if (TempleMain.Player.MusicActive)
            {
                musicButton.SourceRectangle = new Rectangle(576, 576, 64, 64);
                SoundBoard.MusicControl("UnSilence");
            }
            else
            {
                musicButton.SourceRectangle = new Rectangle(576, 640, 64, 64);
                SoundBoard.MusicControl("Silence");
            }
		}
    }
}
