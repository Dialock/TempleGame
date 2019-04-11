#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#endregion

namespace TempleGardens
{
    public class OptionsScreen : GameState
    {
        ImageButton standardButton;
        ImageButton greyScaleButton;
        ImageButton boldButton;
        ImageButton pascalButton;
        ImageButton mysteryButton;

        //ImageButton languageButton;

        private int offsetX = 0;
        private int offsetY = 0;
        private Point centerScreen = new Point(1280 / 2, 720 / 2);

        ImageButton tilesUsed;

        //Label colorThemes;
        //Label tileType;
        
        //Label audio;

        ImageButton soundButton;
        ImageButton musicButton;
        ImageButton vibButton;

        ImageButton resetButton;
        ImageButton backButton;

		private string _caller;

        private Vector2[] gridBacking = new Vector2[] {
            new Vector2(-64, -64),
            new Vector2(669, -64),
            new Vector2(-64, 384),
            new Vector2(669, 384)
        };

        //private string[] langChoose = new string[]
        //{
        //        "English",
        //        "Español", // spanish
        //        "Português", // portuguese
        //        "Engelsk", // danish
        //        "Svenska", // swedish
        //        "Bokmål", //Norway
        //        "Français", // french
        //        "Italiano", //Italian 
        //        "Català" // calatalan
        //};

        //private int langPointer = 0;


        public OptionsScreen(GameStateManager manager, Screens screenName, string caller)
            : base(manager, screenName)
        {
			_caller = caller;

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
            if (TempleMain.Player.ColorPreference != "Mystery")
                for (var i = 0; i < 7; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle(66, 128 + (i * 36), 32, 32), new Rectangle(832, 608, 16, 16), PieceTemplate.ColorFamilies[i], 0f, Vector2.Zero, SpriteEffects.None, 0.66f);
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle(64, 128 + (i * 36) - 2, 36, 36), new Rectangle(832, 608, 16, 16), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.65f);
                }
            else
                for (var i = 0; i < 7; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle(66, 128 + (i * 36), 32, 32), new Rectangle(928, 96, 32, 32), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.66f);
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle(64, 128 + (i * 36) - 2, 36, 36), new Rectangle(832, 608, 16, 16), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.65f);
                }

            for (var i = 0; i < 4; i++)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(gridBacking[i].X + offsetX, gridBacking[i].Y + offsetY), new Rectangle(0, 736, 734, 448), Color.FromNonPremultiplied(224, 204, 140, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);

            // ios Blech
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(66, 72), new Rectangle(0, 312, 288, 40), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.68f);
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(584, 208), new Rectangle(0, 400, 224, 40), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.68f);
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(584, 72), new Rectangle(0, 360, 136, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.68f);

            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(588, 248), new Rectangle(832, 608, 64, 64), Color.FromNonPremultiplied(201, 201, 201, 201), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.43f);

            //spriteBatch.DrawString(SpriteLoader.LangFont32, langChoose[langPointer], new Vector2(588, 412), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);

            spriteBatch.Draw(SpriteLoader.IosMainSheet, Vector2.Zero, new Rectangle(760, 2032, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 712), new Rectangle(760, 2032, 1280, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(0, 8), new Rectangle(2032, 1312, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1272, 8), new Rectangle(2032, 1312, 8, 704), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(276, ScreenManager.GameReference.YPlacardOffset), new Rectangle(1296, 1680, 728, 104),
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            ControlManager.Draw(spriteBatch);
        }

        private void BuildControls()
        {
            //colorThemes = new Label()
            //{
            //    Text = "Color Themes:",
            //    Color = Color.Black,
            //    Position = new Vector2(66, 72),
            //    Font = SpriteLoader.Font32,
            //    DrawLayer = 0.68f
            //};
            //ControlManager.Add(colorThemes);

            //tileType = new Label()
            //{
            //    Text = "Tile Type:",
            //    Color = Color.Black,
            //    Position = new Vector2(584, 192),
            //    Font = SpriteLoader.Font32,
            //    DrawLayer = 0.68f
            //};
            //ControlManager.Add(tileType);

            //audio = new Label()
            //{
            //    Text = "Audio/Misc:",
            //    Color = Color.Black,
            //    Position = new Vector2(584, 72),
            //    Font = SpriteLoader.Font32,
            //    DrawLayer = 0.68f
            //};
            //ControlManager.Add(audio);

            standardButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1280, 640, 378, 54), new Vector2(128, 128));
            standardButton.Color = Color.White;
            standardButton.HighlightColor = Color.NavajoWhite;
            standardButton.FocusColor = Color.Aquamarine;
            standardButton.Name = "Standard";
            standardButton.Selected += OptionsButtonSelected;
            standardButton.DrawLayer = 0.66f;
            ControlManager.Add(standardButton);

            greyScaleButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1280, 820, 186, 54), new Vector2(128, 209));
            greyScaleButton.Color = Color.White;
            greyScaleButton.HighlightColor = Color.NavajoWhite;
            greyScaleButton.FocusColor = Color.Aquamarine;
            greyScaleButton.Name = "Grey";
            greyScaleButton.Selected += OptionsButtonSelected;
            greyScaleButton.DrawLayer = 0.66f;
            ControlManager.Add(greyScaleButton);

            boldButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1280, 700, 186, 54), new Vector2(128, 290));
            boldButton.Color = Color.White;
            boldButton.HighlightColor = Color.NavajoWhite;
            boldButton.FocusColor = Color.Aquamarine;
            boldButton.Name = "Bold";
            boldButton.Selected += OptionsButtonSelected;
            boldButton.DrawLayer = 0.66f;
            ControlManager.Add(boldButton);

            pascalButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1280, 760, 282, 54), new Vector2(128, 371));
            pascalButton.Color = Color.White;
            pascalButton.HighlightColor = Color.NavajoWhite;
            pascalButton.FocusColor = Color.Aquamarine;
            pascalButton.Name = "Pastels";
            pascalButton.Selected += OptionsButtonSelected;
            pascalButton.DrawLayer = 0.66f;
            ControlManager.Add(pascalButton);

            mysteryButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1568, 820, 330, 54), new Vector2(128, 452));
            mysteryButton.Color = Color.White;
            mysteryButton.HighlightColor = Color.NavajoWhite;
            mysteryButton.FocusColor = Color.Aquamarine;
            mysteryButton.Name = "Mystery";
            mysteryButton.Selected += OptionsButtonSelected;
            mysteryButton.DrawLayer = 0.66f;
            ControlManager.Add(mysteryButton);

            resetButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1568, 760, 234, 54), new Vector2(880, 128));
            resetButton.Color = Color.White;
            resetButton.HighlightColor = Color.NavajoWhite;
            resetButton.FocusColor = Color.Aquamarine;
            resetButton.Name = "Reset";
            resetButton.Selected += reset_Selected;
            resetButton.DrawLayer = 0.66f;
            ControlManager.Add(resetButton);

            backButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1664, 640, 186, 54), new Vector2(640, 512));
            backButton.Color = Color.White;
            backButton.HighlightColor = Color.NavajoWhite;
            backButton.FocusColor = Color.Aquamarine;
            backButton.Name = "Back";
            backButton.Selected += backButton_Selected;
            backButton.DrawLayer = 0.66f;
            ControlManager.Add(backButton);

            //languageButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(282, 60, 378, 54), new Vector2(588, 384));
            //languageButton.Color = Color.White;
            //languageButton.HighlightColor = Color.NavajoWhite;
            //languageButton.FocusColor = Color.Aquamarine;
            //languageButton.Name = "Back";
            //languageButton.Selected += lang_Selected;
            //languageButton.DrawLayer = 0.66f;
            //ControlManager.Add(languageButton);

			if (TempleMain.Player.RegTilesPreferred)
                tilesUsed = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1408, 960, 64, 64), new Vector2(588, 248));
            else
                tilesUsed = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(1600, 1024, 64, 64), new Vector2(588, 248));

            tilesUsed.Name = "Tiles";
            tilesUsed.Selected += TilesSelected;
            tilesUsed.HighlightColor = PieceTemplate.ColorFamilies[2];
            tilesUsed.Color = Color.White;
            tilesUsed.DrawLayer = 0.44f;
            ControlManager.Add(tilesUsed);

			if (TempleMain.Player.MusicActive)
                musicButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(576, 576, 64, 64), new Vector2(588, 128));
            else
                musicButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(576, 640, 64, 64), new Vector2(588, 128));

            musicButton.Selected += musicButton_Selected;
            musicButton.HighlightColor = Color.White;
            musicButton.DrawLayer = 0.44f;
            ControlManager.Add(musicButton);

			if (TempleMain.Player.SoundActive)
                soundButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(512, 576, 64, 64), new Vector2(660, 128));
            else
                soundButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(512, 640, 64, 64), new Vector2(660, 128));

            soundButton.Selected += soundButton_Selected;
            soundButton.HighlightColor = Color.White;
            soundButton.DrawLayer = 0.44f;
            ControlManager.Add(soundButton);

            if (TempleMain.Player.VibrateOn)
                vibButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(640, 576, 64, 64), new Vector2(732, 128));
            else
                vibButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(640, 640, 64, 64), new Vector2(732, 128));

            vibButton.Selected += vibButton_Selected;
            vibButton.HighlightColor = Color.White;
            vibButton.DrawLayer = 0.44f;
            ControlManager.Add(vibButton);

        }

        //void lang_Selected(object sender, EventArgs e)
        //{
        //    langPointer++;

        //    if (langPointer == langChoose.Length)
        //        langPointer = 0;
        //}

        void reset_Selected(object sender, EventArgs e)
        {
            ScreenManager.GameReference.ScreenControl(ScreenName, "Reset");
        }

        void backButton_Selected(object sender, EventArgs e)
        {
			if(_caller == "Menu")
				ScreenManager.GameReference.ScreenControl(ScreenName, "Back");
			else
				ScreenManager.GameReference.ScreenControl(ScreenName, "BackPause");
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

        private void TilesSelected( object sender, EventArgs e)
        {
			if (TempleMain.Player.RegTilesPreferred)
                tilesUsed.SourceRectangle = new Rectangle(1600, 1024, 64, 64);
            else
                tilesUsed.SourceRectangle = new Rectangle(1408, 960, 64, 64);

            ScreenManager.GameReference.ScreenControl(ScreenName, "Tiles");
        }

        private void OptionsButtonSelected(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            if (button != null)
            {
                ScreenManager.GameReference.ScreenControl(ScreenName, button.Name);
            }
        }
    }
}
