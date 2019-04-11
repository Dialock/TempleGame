using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TempleGardens
{
    public class GameStateManager : DrawableGameComponent
    {
        #region Fields

        public TempleMain GameReference { get; private set; }

        // Three lists are used to manipultate the screens
        // screens is a master list of all ininitalized screens waiting for action.
        private List<GameState> screens = new List<GameState>();

        // updatingScreens deals with GameStates that are atleast updating
        private List<GameState> updatingScreens = new List<GameState>();

        // drawingScreens deals with screens that need to be drawn.
        private List<GameState> drawingScreens = new List<GameState>();

        // Default font set shared by all controls
        public SpriteFont ControlFont { get; private set; }

        // Seperate Spritebatch from main game.
        SpriteBatch ScreensSpriteBatch;

        private Color BackgroundColor;

        #endregion

        public GameStateManager(Game game)
            : base(game)
        {
			this.GameReference = (TempleMain)game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ScreensSpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            BackgroundColor = Color.Black;

            base.LoadContent();
        }
        
        #region Update and Draw

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Reverse for loop ensures each screen's state is properly set.
            // It then removes any finished screens from the screens list.
            for (var i = screens.Count - 1; i >= 0; i--)
            {
                if (screens[i].ScreenState == ScreenStates.FINISHED)
                    screens.RemoveAt(i);
            }

            // Add  active screens to updatingscreens list for processing
            foreach (GameState screen in screens)
            {
                if (screen.ScreenState == ScreenStates.ACTIVE_DRAW || screen.ScreenState == ScreenStates.ACTIVE_NODRAW)
                    updatingScreens.Add(screen);
            }

            // Iterate through the list updating each screen. until all have been
            // updated and removed.
            while (updatingScreens.Count > 0)
            {
                GameState currentScreen = updatingScreens[updatingScreens.Count - 1];

                currentScreen.Update(gameTime);

                updatingScreens.RemoveAt(updatingScreens.Count - 1);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Add drawable screens to drawingscreens list for drawing
            foreach (GameState screen in screens)
            {
                if (screen.ScreenState == ScreenStates.ACTIVE_DRAW || screen.ScreenState == ScreenStates.IDLE_DRAW)
                    drawingScreens.Add(screen);
            }

            ResolutionHelper.BeginDraw(BackgroundColor);

            ScreensSpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                 SamplerState.PointClamp, null, null, null,
                ResolutionHelper.GetTransformationMatrix());

            while (drawingScreens.Count > 0)
            {
                GameState screen = drawingScreens[drawingScreens.Count - 1];

                screen.Draw(ScreensSpriteBatch);

                drawingScreens.RemoveAt(drawingScreens.Count - 1);
            }

            ScreensSpriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        #region Screen Control

        public void AddScreen(GameState screen)
        {
            screens.Add(screen);
        }

        public void RemoveScreen(GameState screen)
        {
            screen = null;
            screens.Remove(screen);
        }


        #endregion
    }
}
