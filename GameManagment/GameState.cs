#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TempleGardens
{
    public enum ScreenStates
    {
        IDLE_DRAW, IDLE_NODRAW, ACTIVE_DRAW, ACTIVE_NODRAW, FINISHED
    }

    public abstract class GameState
    {
        #region Fields

        // These fields are than main flags for determining if this screen needs to be draw, updated,
        // or removed fromn screens list.   
        //public bool IsActive { get; private set; }
        //public bool IsCovered { get; private set; }
        //public bool IsExiting { get; private set; }

        public Screens ScreenName { get; private set; }

        public ScreenStates ScreenState { get; set; }

        // All Screens get a controlManager though it might be empty.
        protected ControlManager controlManager;

        // Each screen has a reference to the GameStateManager for messaging.
        public GameStateManager ScreenManager { get; private set; }

        #endregion

        #region Properties

        //public GameStateManager ScreenManager
        //{
        //    get { return screenManager; }
        //    protected set { screenManager = value; }
        //}

        // read-only
        public ControlManager ControlManager
        {
            get { return controlManager; }
        }

        #endregion

        public GameState(GameStateManager manager, Screens screenName)
        {
            ScreenState = ScreenStates.ACTIVE_DRAW;
            ScreenManager = manager;

            controlManager = new ControlManager(ScreenManager.ControlFont);

            ScreenName = screenName;
        }

        #region Abstract Methods

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        #endregion

    }
}
