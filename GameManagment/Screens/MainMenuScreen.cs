#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
#endregion

namespace TempleGardens
{
    public class MainMenuScreen : GameState
    {
        private byte[] tiling;

        public static Rectangle GetTile(byte index, int multi)
        {
            var tapa = multi * 192;

            switch (index)
            {
                case 0:
                    default:
                    return new Rectangle(64 + tapa, 384, 64, 64);
                case 1:
                    return new Rectangle(128 + tapa, 384, 64, 64);
                case 2:
                    return new Rectangle(192 + tapa, 384, 64, 64);
                case 3:
                    return new Rectangle(64 + tapa, 448, 64, 64);
                case 4:
                    return new Rectangle(128 + tapa, 448, 64, 64);
                case 5:
                    return new Rectangle(192 + tapa, 448, 64, 64);
                case 6:
                    return new Rectangle(64 + tapa, 512, 64, 64);
                case 7:
                    return new Rectangle(128 + tapa, 512, 64, 64);
                case 8:
                    return new Rectangle(192 + tapa, 512, 64, 64);
            }
        }

        BuiltButton ClassicButton;
        BuiltButton EnduranceButton;
        BuiltButton OptionsButton;
        BuiltButton ExitButton;

        BuiltButton tutorialButton;

        List<FlowerMenu> flowers = new List<FlowerMenu>();

        private float accum;

        public MainMenuScreen(GameStateManager manager, byte[] incoming, Screens screenName)
            : base(manager, screenName)
        {
            tiling = new byte[20 * 12];

            for (var i = 0; i < tiling.Length; i++)
                tiling[i] = incoming[i];

            BuildControls();

            for (var i = 0; i < 15; i++)
                flowers.Add(new FlowerMenu());
        }

        public override void Update(GameTime gameTime)
        {
            accum += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (accum >= 1f)
            {
                accum = 0;

                var pp = flowers.FindLastIndex(t => !t.Active);

                flowers[pp].PrepFlower();
            }

            for (var i = 0; i < flowers.Count; i++)
            {
                if (flowers[i].Active)
                    flowers[i].Update(gameTime);
            }

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                ScreenManager.GameReference.ManagePlacardState(gameTime);

            ControlManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (var x = 0; x < 20; x++)
                for (var y = 0; y < 12; y++)
                {
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(x * 64, y * 64), MainMenuScreen.GetTile(tiling[y * 20 + x], 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.10f);
                }

            for (var i = 0; i < flowers.Count; i++)
            {
                if (flowers[i].Active)
                    flowers[i].Draw(spriteBatch);
            }

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(276, ScreenManager.GameReference.YPlacardOffset), new Rectangle(1296, 1680, 728, 104),
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            ControlManager.Draw(spriteBatch);
        }

        private void BuildControls()
        {
            ClassicButton = new BuiltButton("Classic", new Vector2(424, -128)); //- 512
            ClassicButton.Selected += ClassicButton_Selected;
            ClassicButton.DrawLayer = 0.33f;
            ControlManager.Add(ClassicButton);

            EnduranceButton = new BuiltButton("Endurance", new Vector2(424, -256)); // - 384
            EnduranceButton.Selected += EndruanceButton_Selected;
            EnduranceButton.DrawLayer = 0.34f;
            ControlManager.Add(EnduranceButton);

            OptionsButton = new BuiltButton("Options", new Vector2(424, -384)); // -256
            OptionsButton.Selected += OptionsButton_Selected;
            OptionsButton.DrawLayer = 0.35f;
            ControlManager.Add(OptionsButton);

            ExitButton = new BuiltButton("Exit", new Vector2(424, -512)); // -128
            ExitButton.Selected += ExitButton_Selected;
            ExitButton.DrawLayer = 0.36f;
            ControlManager.Add(ExitButton);

            tutorialButton = new BuiltButton("Help", new Vector2(256, -608));
            tutorialButton.DrawLayer = 0.55f;
            tutorialButton.Selected += tutorialButton_Selected;
            ControlManager.Add(tutorialButton);
        }

        void tutorialButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.GameReference.ScreenControl(ScreenName, "Stats");
        }


        void ExitButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.GameReference.ScreenControl(ScreenName, "Exit");
        }

        void OptionsButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.GameReference.ScreenControl(ScreenName, "Options");
        }

        void ClassicButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.GameReference.ScreenControl(ScreenName, "Classic");
        }

        void EndruanceButton_Selected(object sender, EventArgs e)
        {
            ScreenManager.GameReference.ScreenControl(ScreenName, "Endurance");
        }

    }
}
