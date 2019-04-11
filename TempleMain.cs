#region Using Statements
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Collections.Generic;
#endregion


namespace TempleGardens
{
    public enum Screens : byte
    {
        Undefined = 0, MainMenuScreen, EnduranceScreen, ClassicScreen, NewUserScreen,
        TitleScreen, OptionsScreen, EndStageScreen, TutorialScreen,
        PauseScreen, BonusScreen, StatsScreen, TransScreen
    };

    public class TempleMain : Game
    {
        GraphicsDeviceManager graphics;
        GameStateManager stateManager;

        #region Screens
        MainMenuScreen mainMenuScreen;
        GamePlayScreen gamePlayScreen;
        OptionsScreen optionsScreen;
        EndStageScreen endStageScreen;
        TitleScreen titleScreen;
        //TutorialScreen tutorialScreen;
        PauseScreen pauseScreen;
        StatsScreen statsScreen;
        #endregion

        public int YPlacardOffset { get; private set; }
        public enum PlacardSlideStates { Open, Closed, Opening, Closing }
        public PlacardSlideStates PlacardState = PlacardSlideStates.Closed;
        public static User Player { get; private set; }
        private float timer = 0f;

        public static Android.OS.Vibrator MainContext { get; private set;}

        public static int MainX { get; private set; }
        public static int MainY { get; private set; }

        public TempleMain(Android.OS.Vibrator vib)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            var MainX = graphics.PreferredBackBufferWidth;
            var MainY = graphics.PreferredBackBufferHeight;


            ResolutionHelper.Init(graphics, MainX, MainY, 1280, 720, false);

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            graphics.ToggleFullScreen();

            MainContext = vib;

        }

        #if WINDOWS || LINUX

//        public TempleMain()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//
//            ResolutionHelper.Init(graphics, 1280, 720, 1280, 720, false);
//
//            stateManager = new GameStateManager(this);
//            Components.Add(stateManager);
//
//            graphics.ToggleFullScreen();
//        }
#endif

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            YPlacardOffset = 800;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            MasterRandom.Init();
            SoundBoard.Init(this);

            Components.Add(new InputHandler(this));

            PieceImageFactory.Init(GraphicsDevice);
            PieceTemplate.Init("Standard");

            SpriteLoader.Init(this);

            InitUser("Player");

//            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//
//			var trip = Path.Combine(path, "Player.bin");
//
//			string[] files =  System.IO.Directory.GetFiles(path, "*.bin");
//
//            if (files.Length == 0)
                titleScreen = new TitleScreen(stateManager, true, Screens.TitleScreen);
//            else
//            {
//                titleScreen = new TitleScreen(stateManager, false, Screens.TitleScreen);
//                InitUser(null);
//            }
            stateManager.AddScreen(titleScreen);

            SoundBoard.MusicControl("StartProgram");
            this.IsMouseVisible = true;
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent(){}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        public void GetSomeContext(int rrr)
        {
            MainContext.Vibrate(rrr);
        }

        //public List<int> DrawASpoon()
        //{
        //    List<int> choc;


        //    using (ObjectSerializer cereal = new ObjectSerializer())
        //    {
        //        choc = cereal.DeserializeObject<List<int>>(@"Assets\Content\Data\shapeData.bin");
        //    }


        //    return choc;
        //}

        public void BackButton()
        {
            if (gamePlayScreen.ScreenState == ScreenStates.ACTIVE_DRAW)
                ScreenControl(gamePlayScreen.ScreenName, "Pause");
        }

        public void ScreenControl(Screens screen, string command)
        {
            switch (screen)
            {
                case Screens.TitleScreen:
                    if (command == "Next")
                    {
                        titleScreen.ScreenState = ScreenStates.FINISHED;
                    }
                    else if (command == "Begin")
                    {
                        mainMenuScreen = new MainMenuScreen(stateManager, titleScreen.Tiling, Screens.MainMenuScreen);
                        stateManager.AddScreen(mainMenuScreen);

                        PieceTemplate.SetColorFamilies(Player.ColorPreference);
                    }
                    else if (command == "NewUser")
                    {
                        InitUser("Player");
                    }
                    break;
                case Screens.ClassicScreen:
                    if (command == "Lost")
                    {
                        var seasonz = (gamePlayScreen.Year - 1) * 4;
                        seasonz += (int)gamePlayScreen.Season + 1;

                        endStageScreen = new EndStageScreen(stateManager, Screens.EndStageScreen, command, gamePlayScreen.Score, gamePlayScreen.TotalMatches);
                        stateManager.AddScreen(endStageScreen);
                        endStageScreen.EndSeason = seasonz;

                        gamePlayScreen.ScreenState = ScreenStates.FINISHED;

                        SaveUser();
                    }
                    else if (command == "Save")
                        SaveUser();
                    else if (command == "Pause")
                    {
                        pauseScreen.ScreenState = ScreenStates.ACTIVE_DRAW;
                        pauseScreen.Caller = "Classic";
                        gamePlayScreen.ScreenState = ScreenStates.IDLE_NODRAW;
                    }
                    else if (command == "Won")
                    {
                        var seasonz = (gamePlayScreen.Year - 1) * 4;
                        seasonz += (int)gamePlayScreen.Season + 1;

                        endStageScreen = new EndStageScreen(stateManager, Screens.EndStageScreen, command, gamePlayScreen.Score, gamePlayScreen.TotalMatches);
                        stateManager.AddScreen(endStageScreen);

                        endStageScreen.EndSeason = seasonz;
                        gamePlayScreen.ScreenState = ScreenStates.FINISHED;

                        SaveUser();
                    }
                    else if (command == "InterSave")
                    {
                        gamePlayScreen.FinalizeGamePlay(Player);
                        SaveUser();
                    }

                    break;

                case Screens.OptionsScreen:
                    if (command == "Back")
                    {
                        optionsScreen.ScreenState = ScreenStates.FINISHED;
                        mainMenuScreen.ScreenState = ScreenStates.ACTIVE_DRAW;

                        optionsScreen = null;

                        SaveUser();
                    }
                    else if (command == "Tiles")
                    {
                        Player.SetData();
                        SaveUser();
                    }
                    else if (command == "BackPause")
                    {
                        optionsScreen.ScreenState = ScreenStates.FINISHED;
                        pauseScreen.ScreenState = ScreenStates.ACTIVE_DRAW;

                        SaveUser();

                        optionsScreen = null;
                    }
                    else if (command == "Reset")
                    {
                        Player.TutorialPlayed = false;

                        SaveUser();
                    }
                    else
                    {
                        PieceTemplate.SetColorFamilies(command);
                        Player.SetData(command);
                        SaveUser();
                    }

                    break;
                case Screens.EndStageScreen:

                    if (command == "Back")
                    {
                        endStageScreen.ScreenState = ScreenStates.FINISHED;
                        mainMenuScreen.ScreenState = ScreenStates.ACTIVE_DRAW;

                        Player.UpdateUser(endStageScreen.FullXPPool, endStageScreen.XPForThisLevel, endStageScreen.CurrentLvl);
                        SaveUser();

                        endStageScreen = null;
                        gamePlayScreen = null;
                        pauseScreen = null;

                        SoundBoard.MusicControl("ToMenu");
                    }

                    break;
                case Screens.MainMenuScreen:
                    
                    if (command == "Exit")
                    {
                        Exit();
                    }
                    else if (command == "Endurance")
                    {
                        mainMenuScreen.ScreenState = ScreenStates.IDLE_NODRAW;

                        pauseScreen = new PauseScreen(stateManager, Screens.PauseScreen);
                        pauseScreen.ScreenState = ScreenStates.IDLE_NODRAW;
                        stateManager.AddScreen(pauseScreen);


                        gamePlayScreen = new GamePlayScreen(stateManager, Screens.EnduranceScreen);
                        stateManager.AddScreen(gamePlayScreen);

                        SoundBoard.MusicControl("ToPlay");

                    }
                    else if (command == "Classic")
                    {
                        mainMenuScreen.ScreenState = ScreenStates.IDLE_NODRAW;

                        pauseScreen = new PauseScreen(stateManager, Screens.PauseScreen);
                        pauseScreen.ScreenState = ScreenStates.IDLE_NODRAW;
                        stateManager.AddScreen(pauseScreen);

                        if (Player.ZenSavedScore == 0)
                        {
                            gamePlayScreen = new GamePlayScreen(stateManager, Screens.ClassicScreen);
                            stateManager.AddScreen(gamePlayScreen);
                        }
                        else
                        {
                            gamePlayScreen = new GamePlayScreen(stateManager, Screens.ClassicScreen);
                            stateManager.AddScreen(gamePlayScreen);

                            gamePlayScreen.IntroduceSaveData(Player);
                        }

                        SoundBoard.MusicControl("ToPlay");
                    }
                    else if (command == "Stats")
                    {
                        mainMenuScreen.ScreenState = ScreenStates.IDLE_NODRAW;

                        statsScreen = new StatsScreen(stateManager, Screens.StatsScreen);
                        stateManager.AddScreen(statsScreen);

                        //tutorialScreen = new TutorialScreen(stateManager, Screens.TutorialScreen);
                        //stateManager.AddScreen(tutorialScreen);
                    }
                    else if (command == "Options")
                    {
                        optionsScreen = new OptionsScreen(stateManager, Screens.OptionsScreen, "Menu");
                        stateManager.AddScreen(optionsScreen);

                        mainMenuScreen.ScreenState = ScreenStates.IDLE_NODRAW;
                    }
                   
                    break;
                //case Screens.TutorialScreen:

                //    if (command == "Done")
                //    {
                //        tutorialScreen.ScreenState = ScreenStates.FINISHED;
                //        mainMenuScreen.ScreenState = ScreenStates.ACTIVE_DRAW;
                //    }

                    //break;
                case Screens.EnduranceScreen:

                    if (command == "Won")
                    {
                        var seasonz = (gamePlayScreen.Year - 1) * 4;
                        seasonz += (int)gamePlayScreen.Season + 1;

                        endStageScreen = new EndStageScreen(stateManager, Screens.EndStageScreen, command, gamePlayScreen.Score, gamePlayScreen.TotalMatches);
                        stateManager.AddScreen(endStageScreen);

                        endStageScreen.EndSeason = seasonz;
                        gamePlayScreen.ScreenState = ScreenStates.FINISHED;

                        Player.BoardRocks = null;
                        Player.CurrentZenSeason = 0;
                        Player.SavedZenPieces.Clear();
                        Player.UsedPieceCount = null;
                        Player.ZenSavedScore = 0;

                        SaveUser();
                    }
                    else if (command == "Lost")
                    {
                        var seasonz = (gamePlayScreen.Year - 1) * 4;
                        seasonz += (int)gamePlayScreen.Season + 1;

                        endStageScreen = new EndStageScreen(stateManager, Screens.EndStageScreen, command, gamePlayScreen.Score, gamePlayScreen.TotalMatches);
                        stateManager.AddScreen(endStageScreen);
                        endStageScreen.EndSeason = seasonz;

                        gamePlayScreen.ScreenState = ScreenStates.FINISHED;

                        SaveUser();
                    }
                    else if (command == "Pause")
                    {
                        pauseScreen.ScreenState = ScreenStates.ACTIVE_DRAW;
                        pauseScreen.Caller = "Zen";
                        gamePlayScreen.ScreenState = ScreenStates.IDLE_NODRAW;
                    }

                    break;
                case Screens.PauseScreen:

                    if (command == "ClassicUn")
                    {
                        pauseScreen.ScreenState = ScreenStates.IDLE_NODRAW;
                        gamePlayScreen.ScreenState = ScreenStates.ACTIVE_DRAW;
                    }
                    else if (command == "ZenUn")
                    {
                        pauseScreen.ScreenState = ScreenStates.IDLE_NODRAW;
                        gamePlayScreen.ScreenState = ScreenStates.ACTIVE_DRAW;
                    }
                    else if (command == "ZenEnd")
                    {
                        var seasonz = (gamePlayScreen.Year - 1) * 4;
                        seasonz += (int)gamePlayScreen.Season + 1;

                        pauseScreen.ScreenState = ScreenStates.FINISHED;
                        gamePlayScreen.ScreenState = ScreenStates.FINISHED;

                        endStageScreen = new EndStageScreen(stateManager, Screens.EndStageScreen, command, gamePlayScreen.Score, gamePlayScreen.TotalMatches);
                        stateManager.AddScreen(endStageScreen);

                        endStageScreen.EndSeason = seasonz;

                        SaveUser();
                    }
                    else if (command == "ClassicEnd")
                    {
                        pauseScreen.ScreenState = ScreenStates.FINISHED;

                        gamePlayScreen.FinalizeGamePlay(Player);
                        SaveUser();

                        gamePlayScreen.ScreenState = ScreenStates.FINISHED;

                        mainMenuScreen.ScreenState = ScreenStates.ACTIVE_DRAW;

                        gamePlayScreen = null;

                        SoundBoard.MusicControl("ToMenu");
                    }
                    else if (command == "OptionsGame")
                    {
                        pauseScreen.ScreenState = ScreenStates.IDLE_NODRAW;
					    
                        optionsScreen = new OptionsScreen(stateManager, Screens.OptionsScreen, "GamePaused");
                        stateManager.AddScreen(optionsScreen);
                    }


                    break;
                case Screens.StatsScreen:
                    if (command == "Back")
                    {
                        mainMenuScreen.ScreenState = ScreenStates.ACTIVE_DRAW;

                        statsScreen.ScreenState = ScreenStates.FINISHED;
                    }
                    break;
            }
        }

        private void SaveUser()
        {
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

			var trip = Path.Combine(path, "Player.bin");

            //string[] files =  System.IO.Directory.GetFiles(path, "*.bin");

            using (ObjectSerializer cereal = new ObjectSerializer())
            {
                cereal.SerializeObject(trip, Player);
            }
        }


        private void InitUser(string fileName)
        {
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

			var trip = Path.Combine(path, "Player.bin");

			string[] files =  System.IO.Directory.GetFiles(path, "*.bin");

            if (files.Length == 0)
            {
                using (ObjectSerializer cereal = new ObjectSerializer())
                {
                    Player = new User(fileName);

                    cereal.SerializeObject(trip, Player);
                }
            }
            else
            {
                using (ObjectSerializer cereal = new ObjectSerializer())
                {
                    Player = cereal.DeserializeObject<User>(files[0]);
                }
            }
        }

        public void ManagePlacardState(GameTime gametime)
        {
            if (PlacardState == PlacardSlideStates.Closing)
            {
                YPlacardOffset += 2;

                if (YPlacardOffset == 800)
                {
                    PlacardState = PlacardSlideStates.Closed;
                }
            }

            if (PlacardState == PlacardSlideStates.Opening)
            {
                YPlacardOffset -= 2;

                if (YPlacardOffset == 600)
                    PlacardState = PlacardSlideStates.Open;
            }

            if (PlacardState == PlacardSlideStates.Open)
            {
                timer += (float)gametime.ElapsedGameTime.TotalSeconds;

                if (timer >= 2f)
                {
                    timer = 0f;
                    PlacardState = PlacardSlideStates.Closing;
                }
            }
        }

        public void EvaluateAchievements(string command)
        {

            // check that no rocks have formed in ten seasons

            switch (command)
            {
                case "RockTest":
                    Player.PlayerTrophies[0].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;

                // check that 15 normal games have been started

                case "EnduranceCount":
                    Player.PlayerTrophies[1].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;

                // check that 15 endurance games have been started

                case "ClassicCount":
                    Player.PlayerTrophies[2].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;

                // check that the auto fill button wasnt pressed in 10 seasons

                case "AutoClick":
                    Player.PlayerTrophies[3].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;

                // check that a flower species has been completed
                case "OneComp":
                    Player.PlayerTrophies[4].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;
                // check that all flower species have been cmpleted

                case "AllComp":
                    Player.PlayerTrophies[5].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;
                // check that lvl 5 has been reached

                case "Lvl5":
                    Player.PlayerTrophies[6].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;
                // check that lvl 10 has been reached
                case "Lvl10":
                    Player.PlayerTrophies[7].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;
                // check that lvl 15 has been reached
                case "Lvl15":
                    Player.PlayerTrophies[8].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;
                // check that 100 seasons have been played in one game
                case "100":
                    Player.PlayerTrophies[9].Award();
                    PlacardState = PlacardSlideStates.Opening;
                    break;
                default:
                    break;

            }

            SaveUser();
        }
    }

#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new TempleMain())
                game.Run();
        }
    }
#endif
}
