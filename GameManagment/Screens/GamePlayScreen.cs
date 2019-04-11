#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace TempleGardens
{
    public class GamePlayScreen : GameState
    {
        enum Stages : byte { Undefined = 0, Stage1, Stage2, Stage3 };
        Stages mainStages = Stages.Stage1;

        private string[] stageNames = new string[]
        {
            "None",
            "Plant",
            "Match",
            "Clear"
        };

        private  string[] seasonNames = new string[]
        {
                "Spring",
                "Summer",
                "Autumn",
                "Winter"
        };
         
        enum Stage3Stages : byte { Undefined = 0, None, BeePollinate, ManageBreeding, PlayerWeeds, ManageOld, ProcessOld, Finalize };
        Stage3Stages stage3States = Stage3Stages.None;

        EffectsManager effectsManager;

        enum WindDirection : byte
        {
            None = 0, WestWeak, WestStrong, NorWestWeak, NorWestStrong, NorthWeak,
            NorthStrong, NorEastWeak, NorEastStrong, EastWeak, EastStrong, SouthEastWeak, SouthEastStrong,
            SouthWeak, SouthStrong, SouthWestWeak, SouthWestStrong
        };
        WindDirection wind = WindDirection.NorthWeak;

        List<PieceTemplate> GamePieces;

        public const int Width = 15;
        public const int Height = 10;

        public BoardCell[] BoardCells { get; private set; }
        PieceCell[] pieceCells;

        byte[,] nextShape;
        byte[,] currentShape;
        
        private bool occupied = false;

        // piece tracking
        int nextPiece = 0;
        int currentPiece = 0;

        // id tracking
        const int MaxPossibleId = 8888;
        private int currentUsedId = 2;

        // click tracking
        int click = 0;

        Texture2D nextImage;

        // Piece fields
        int[] currentPiecesUsed;
        int currentPointer = 0;
        FlowerTypes nextFlower;
        ColorPair nextColor;
        FlowerTypes currentFlower;
        ColorPair currentColor;

        // Wind
        int windspeed = 0;
        Vector2 windDirection = Vector2.Zero; 
        Wind[] windz = new Wind[25];
        float windTimer = 0;
        float windDuration = 0.3f;
        WindGage windGage = new WindGage(0);

        // pollen
        Pollen[] pollenz = new Pollen[150];

        // track newly unlocked pieces
        private List<int> newUnlocks = new List<int>();
        private float newPieceDisplayTimer = 0;
        private float newPieceDisplayDuration = 0.9f;

        // bees
        List<Bee> beez;
        int currentBee = 0;
        private int succesfulBeeUsageCount = 0;
        private int targetBeeUsage; 

        public int Year { get; set; }
        public Seasons Season { get; set; }
        public int Score { get; set; }

        private bool _matesId = false;

        public Color MoodColor { get; set; }

        private bool stage1Juststarted = false;
        private bool wrappingUpStage1 = false;

        private bool stage2JustStarted = false;
        public const int GageStart = 704;
		private float gageHeight = 0;
        private float currentGage = GageStart;

        const int TotalCells = 150;

        private ImageButton pauseButton;
        private enum ButtonSlide { Open, Closed, Opening, Closing }
        private ButtonSlide pauseButtonSlideState = ButtonSlide.Closed;
        private float pauseTimer = 0f;

        private static List<Vector2> BlindDraw = new List<Vector2>() {
            new Vector2(1032, 128),
			new Vector2(1127, 128),
			new Vector2(1032, 223),
			new Vector2(1127, 223),
			new Vector2(1032, 317),
			new Vector2(1127, 317),
			new Vector2(1032, 411),
			new Vector2(1127, 411),
			new Vector2(1076, 507)
        };

		/// <summary>
		/// Stores points for drawing flower gauges
		/// </summary>
        private static List<Vector2> DrawPositions = new List<Vector2>() {
			new Vector2(1032, 216),
			new Vector2(1127, 216),
			new Vector2(1032, 311),
			new Vector2(1127, 311),
			new Vector2(1032, 405),
			new Vector2(1127, 405),
			new Vector2(1032, 499),
			new Vector2(1127, 499),
			new Vector2(1076, 595)
		};

        private static Vector2[] WholeDrawPositions = new Vector2[] {
			new Vector2(1044, 140),
			new Vector2(1139, 140),
			new Vector2(1044, 235),
			new Vector2(1139, 235),
			new Vector2(1044, 329),
			new Vector2(1139, 329),
			new Vector2(1044, 423),
			new Vector2(1139, 423),
			new Vector2(1088, 519)
		};
             
        private ImageButton tapButton;
       
        private int[] yOffsetRectType = new int[]
        {
            0,  //0
            3,
            7,
            10,
            14, //4
            17,
            20,
            23,
            26,
            29,
            32, // 10
            36,
            40,
            43,
            46,
            51, // 15
            54,
            57,
            61,
            64,
            65,
            68, // 21
            71,
            73,
            76,
            80,
            83, // 26
            85,
            88 // 28
        };

        private int[] UsedFlowerCounter = new int[9];

        private Point CenterScreen = new Point(1280 / 2, 720 / 2);

        private Vector2[] gridBacking = new Vector2[] {
            new Vector2(-64, -64),
            new Vector2(669, -64),
            new Vector2(-64, 384),
            new Vector2(669, 384)
        };

        private int offsetX = 0;
        private int offsetY = 0;

        private byte[] tiling;

        private List<int> stage3Tracker;

        float _toRockTimer = 0f;

        private byte tutorialTracker = 0;
        private bool showHelp = false;

        private bool _buttonsVisisble = false;
        private ImageButton autoFillButton;
        private ImageButton skipHelpButton;
        private ImageButton nextHelpButton;
        private ImageButton endButton;

        public int TotalMatches { get; private set; }

        private Rectangle[] helpText = new Rectangle[]
        {
                new Rectangle(32, 352, 492, 115),
                new Rectangle(32, 480, 456, 77),
                new Rectangle(32, 576, 487, 116),
                new Rectangle(32, 704, 385, 113),
                new Rectangle(32, 832, 499, 116),
                new Rectangle(512, 461, 461, 96)
        };

        // track piece spread
        private List<int> masterUsageWeight = new List<int>();

        private int _zenHoldOverScore;

        private int[] acceptedSelections;
        private int[] notIncluded;

        IOSNumberSpitter numberSpitter;

        private bool _rockWasCreated = false;
        private int _lastNoRockCount = 0;

        private bool _autoClicked = false;
        private int _lastNoAutoClick = 0;

        private int _rockGenTracker = 0;

        public GamePlayScreen(GameStateManager manager, Screens screenName)
            : base(manager, screenName)
        {
            using (PieceFactory pieceFactory = new PieceFactory())
            {
                GamePieces = pieceFactory.BuildSpecialLibrary();
            }

            for (var i = 0; i < 9; i++)
                masterUsageWeight.Add(0);

            if (screenName == Screens.EnduranceScreen)
            {
                acceptedSelections = new int[4];
                var ttt = new int[9];

                for (var i = 0; i < ttt.Length; i++)
                    ttt[i] = i  + 1;

                acceptedSelections = ttt.PickRandom(4).ToArray();

                notIncluded = ttt.Except(acceptedSelections).ToArray();

                for (var t = 0; t < notIncluded.Length; t++)
                    notIncluded[t] -= 1;

                TempleMain.Player.IncrementCountEnduarnce();

                if (TempleMain.Player.EnduranceCount == 15)
                    ScreenManager.GameReference.EvaluateAchievements("EnduranceCount");
            }

            if (screenName == Screens.ClassicScreen)
            {
                TempleMain.Player.IncrementCountClassic();

                if (TempleMain.Player.ClassicCount == 15)
                    ScreenManager.GameReference.EvaluateAchievements("ClassicCount");
            }

            ChoosePieces();

            nextPiece = currentPiecesUsed[currentPointer];
            currentPointer++;
            currentPiece = currentPiecesUsed[currentPointer];
            currentPointer++;

            nextShape = GamePieces[nextPiece].Shape;
            currentShape = GamePieces[currentPiece].Shape;

            nextFlower = GamePieces[nextPiece].FlowerType;
            nextColor = new ColorPair(GamePieces[nextPiece].Colors.Color1, GamePieces[nextPiece].Colors.Color2);
            currentFlower = GamePieces[currentPiece].FlowerType;
            currentColor = new ColorPair(GamePieces[currentPiece].Colors.Color1, GamePieces[currentPiece].Colors.Color2);

			nextImage = PieceImageFactory.BuildImage(currentShape);

            BoardCell.Init(this);
            BoardCells = new BoardCell[Width * Height];
            pieceCells = new PieceCell[Width * Height];

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    BoardCells[y * Width + x] = new BoardCell(x + 1, y + 1);
                    pieceCells[y * Width + x] = new PieceCell(x + 1, y + 1);
                }

            for (var i = 0; i < windz.Length; i++)
                windz[i] = new Wind();

            for (var i = 0; i < pollenz.Length; i++)
                pollenz[i] = new Pollen();

            wind = (WindDirection)MasterRandom.FRandom.Next(1, 17);
            windDirection = GetWindRotation();

            Pollen.SetDirAndSpeed(windDirection, windspeed);
            Wind.SetDirAndSpeed(windDirection, windspeed);

            EffectsManager.Init(this);
            effectsManager = new EffectsManager();
            //BurstEffect.Init();

            Bee.Init(effectsManager, this);
            beez = new List<Bee>();
            beez.Add(new Bee());

            targetBeeUsage = beez.Count + 1;

            Year = 1;
            Season = Seasons.Spring;
            Score = 0;

            MoodColor = Color.PowderBlue;

            tiling = new byte[Width * Height];

            for (var i = 0; i < tiling.Length; i++)
                tiling[i] = (byte)MasterRandom.FRandom.Next(0, 9);

            stage3Tracker = new List<int>();

            BuildControls();

            TotalMatches = 0;
            numberSpitter = new IOSNumberSpitter(Screens.ClassicScreen);
        }
            
        private float _mainTimer = 0f;

        public override void Update(GameTime gameTime)
        {

            var _currentScaledMouse = new Point((InputHandler.TouchVectorScaled.ToPoint().X / 64) - 1, (InputHandler.TouchVectorScaled.ToPoint().Y / 64) - 1);

            if (!showHelp)
            {

                numberSpitter.Update(Year, Score);

                HandleInput();

                windGage.Update();

                var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                windTimer += elapsed;

                for (var i = 0; i < beez.Count; i++)
                    beez[i].Update(gameTime);


                if (windTimer >= windDuration)
                {
                    windTimer = 0;

                    var choose = MasterRandom.FRandom.Next(0, windz.Length);

                    if (!windz[choose].Visible)
                        windz[choose].Activate(new Vector2(MasterRandom.FRandom.Next(128, 1024), MasterRandom.FRandom.Next(64, 768)));
                }

                //var mousePos = new Point((InputHandler.MousePositionScaled.ToPoint().X / 64) - 1, (InputHandler.MousePositionScaled.ToPoint().Y / 64) - 1);

                if (InputHandler.FingerRaised() && InBounds(_currentScaledMouse.X, _currentScaledMouse.Y) && BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].Blocked)
                {
                    if (BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].RockType < 4)
                    {
                        BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].BlockHP--;

                        if (TempleMain.Player.MusicActive)
                        {
                            if (MasterRandom.FRandom.NextBool())
                                SoundBoard.RockTap1.Play();
                            else
                                SoundBoard.RockTap2.Play();
                        }
                    }
                }


                for (var i = 0; i < BoardCells.Length; i++)
                {
                    if (BoardCells[i].Blocked)
                    {
                        if (BoardCells[i].BlockHP == 0)
                        {
                            effectsManager.AddNewBurst(new Vector2(BoardCells[i].Bounds.X, BoardCells[i].Bounds.Y), new ColorPair(Absorb.color1, Absorb.color2));
                            SetCascade(i);

                            BoardCells[i].ClearBlock();
                            BoardCells[i].ClearCellData();
                        }
                    }

                    if (BoardCells[i].Ready)
                    {
                        var choose = MasterRandom.FRandom.Next(0, pollenz.Length);

                        if (!pollenz[choose].Visible)
                        {
                            if (MasterRandom.FRandom.Next(0, 3) == 1)
                            {
                                pollenz[choose].Activate(new Vector2(BoardCells[i].Bounds.X + MasterRandom.FRandom.Next(28, 48), BoardCells[i].Bounds.Y + MasterRandom.FRandom.Next(28, 48)));
                            }
                        }


                    }
                }

                // new unlocks
                if (newUnlocks.Count > 0)
                {
                    newPieceDisplayTimer += elapsed;

                    if (newPieceDisplayTimer >= newPieceDisplayDuration)
                    {
                        newPieceDisplayTimer = 0;

                        var flowerUnlocked = GamePieces[newUnlocks.Last()].FlowerType;

                        newUnlocks.Remove(newUnlocks.Last());

                        Score += 600;

                        effectsManager.AddScore(GetScoreDrawPoition(flowerUnlocked), 600);
                    }
                }

                for (var i = 0; i < pollenz.Length; i++)
                {
                    if (pollenz[i].Visible)
                        pollenz[i].Update(elapsed);
                }

                for (var i = 0; i < windz.Length; i++)
                {
                    if (windz[i].Visible)
                    {
                        windz[i].Update(elapsed);
                    }
                }

                for (var i = 0; i < BoardCells.Length; i++)
                {
                    BoardCells[i].UpdateCell(elapsed);
                }

                _mainTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (mainStages == Stages.Stage1)
                {
                    if (!stage1Juststarted)
                    {
                        _mainTimer = 0;
                        stage1Juststarted = true;
                        tapButton.Visible = true;
                        tapButton.Enabled = true;

                        if (!TempleMain.Player.PlayerTrophies[9].Awarded)
                        {
                            var seasonz = (Year - 1) * 4;
                            seasonz += (int)Season + 1;

                            if (seasonz == 100)
                                ScreenManager.GameReference.EvaluateAchievements("100");
                        }

                    }

                    if (!wrappingUpStage1 && stage1Juststarted && _mainTimer >= .5f)
                    {
                        currentGage -= 3.15f; // 3.15
                        gageHeight += 3.15f;
                        _mainTimer = 0;

                        // tut 1
                        if (!TempleMain.Player.TutorialPlayed && tutorialTracker == 0)
                        {
                            showHelp = true;
                        }

                        // TUT 2
                        if (!TempleMain.Player.TutorialPlayed && BoardCells.Any(t => t.CurrentID > 10) && tutorialTracker == 1)
                        {
                            showHelp = true;
                        }

                        var cellCheck = BoardCells.Count(r => r.Ready) + BoardCells.Count(e => e.Blocked);


                        if (cellCheck >= 112 && !autoFillButton.Enabled)
                        {
                            autoFillButton.Enabled = true;
                            autoFillButton.Visible = true;
                        }

                    }

                    if (stage1Juststarted && currentGage <= 8)
                    {
                        var t = 0;

                        for (var x = 0; x < Width; x++)
                            for (var y = 0; y < Height; y++)
                            {
                                if (PlaceShape(new Point(x, y), out t))
                                {
                                    currentPiece = nextPiece;

                                    nextPiece = currentPiecesUsed[currentPointer];
                                    currentPointer++;

                                    if (currentPointer == currentPiecesUsed.Length)
                                    {
                                        currentPointer = 0;
                                        currentPiecesUsed = currentPiecesUsed.Shuffle().ToArray();
                                    }

                                    currentShape = nextShape;
                                    nextShape = GamePieces[nextPiece].Shape;
                                    nextImage = PieceImageFactory.BuildImage(currentShape);

                                    nextFlower = GamePieces[nextPiece].FlowerType;
                                    nextColor = new ColorPair(GamePieces[nextPiece].Colors.Color1, GamePieces[nextPiece].Colors.Color2);
                                    currentFlower = GamePieces[currentPiece].FlowerType;
                                    currentColor = new ColorPair(GamePieces[currentPiece].Colors.Color1, GamePieces[currentPiece].Colors.Color2);
                                }
                            }
                    }

                    if (BoardCells.All(t => t.CurrentNumber > 0))
                        wrappingUpStage1 = true;
             
                    EvaluteCells(_currentScaledMouse);

                    if ((BoardCells.Count(t => t.Ready) + BoardCells.Count(r => r.Blocked)) == TotalCells)
                    {
                        if (!_matesId)
                        {
                            for (var x = 0; x < Width; x++)
                                for (var y = 0; y < Height; y++)
                                {
                                    IdMate(x, y);
                                }
                        }

                        mainStages = Stages.Stage2;

                        _mainTimer = 0;
                        currentGage = GageStart;
                        gageHeight = 0;
                        stage1Juststarted = false;
                        wrappingUpStage1 = false;

                        tapButton.Visible = false;
                        tapButton.Enabled = false;
                    }
                }
                else if (mainStages == Stages.Stage2)
                {
                    if (autoFillButton.Visible)
                    {
                        autoFillButton.Visible = false;
                        autoFillButton.Enabled = false;
                    }

                    if (!stage2JustStarted)
                    {
                        _mainTimer = 0;
                        stage2JustStarted = true;
                    }

                    if (currentGage > 8 && stage2JustStarted && _mainTimer >= 0.5f)
                    {
                        currentGage -= 8.15f; // 8.15
                        gageHeight += 8.15f;
                        _mainTimer = 0;
                    }

                    // tut 3
                    if (!TempleMain.Player.TutorialPlayed && tutorialTracker == 2)
                    {
                        showHelp = true;
                    }

                    if (currentBee < beez.Count && beez.All(g => !g.Active))
                    {
                        if (currentGage <= 8)
                        {
                            _mainTimer = 0;

                            for (var i = currentBee; i < beez.Count; i++)
                            {
                                var done = false;

                                do
                                {
                                    var temp1 = new Point(MasterRandom.FRandom.Next(0, Width), MasterRandom.FRandom.Next(0, Height));

                                    if (click == 0)
                                    {
                                        var alreadyApplied = beez.Any(r => r.DestPoint1 == temp1 ||
                                                                 r.DestPoint2 == temp1);

                                        if (!alreadyApplied)
                                        {
                                            click++;
                                            beez[currentBee].SetDestiantions(1, temp1);
                                            beez[currentBee].IndexOnBoard1 = temp1.Y * Width + temp1.X;
                                        }

                                    }
                                    else if (click == 1)
                                    {
                                        var alreadyApplied = beez.Any(r => r.DestPoint1 == temp1 ||
                                                                 r.DestPoint2 == temp1);

                                        if (!alreadyApplied)
                                        {
                                            click = 0;
                                            beez[currentBee].SetDestiantions(2, temp1);
                                            beez[currentBee].IndexOnBoard2 = temp1.Y * Width + temp1.X;
                                            done = true;
                                            currentBee++;
                                        }
                                    }


                                } while (!done);
                            }
                        }
                        else
                        {
                            //var temp = new Point((InputHandler.MousePositionScaled.ToPoint().X / 64) - 1, (InputHandler.MousePositionScaled.ToPoint().Y / 64) - 1);

                            var touchInBounds = GamePlayScreen.InBounds(_currentScaledMouse.X, _currentScaledMouse.Y);


                            if (click == 0)
                            {
                                if (InputHandler.FingerRaised() && touchInBounds)
                                {
                                    var alreadyApplied = beez.Any(r => r.DestPoint1 == _currentScaledMouse ||
                                                             r.DestPoint2 == _currentScaledMouse);

                                    if (!alreadyApplied)
                                    {
                                        click++;
                                        beez[currentBee].SetDestiantions(1, _currentScaledMouse);
                                        beez[currentBee].IndexOnBoard1 = _currentScaledMouse.Y * Width + _currentScaledMouse.X;
                                    }
                                }

                            }
                            else if (click == 1)
                            {
                                if (InputHandler.FingerRaised() && touchInBounds)
                                {
                                    var alreadyApplied = beez.Any(r => r.DestPoint1 == _currentScaledMouse ||
                                                             r.DestPoint2 == _currentScaledMouse);

                                    if (!alreadyApplied)
                                    {
                                        click++;
                                        beez[currentBee].SetDestiantions(2, _currentScaledMouse);
                                        beez[currentBee].IndexOnBoard2 = _currentScaledMouse.Y * Width + _currentScaledMouse.X;
                                    }
                                }
                            }
                            else if (click == 2)
                            {
                                currentBee++;
                                click = 0;
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < beez.Count; i++)
                            beez[i].Active = true;

                        mainStages = Stages.Stage3;
                        stage3States = Stage3Stages.BeePollinate;
                        currentBee = 0;
                        click = 0;
                        _mainTimer = 0;
                        currentGage = GageStart;
                        gageHeight = 0;
                        stage2JustStarted = false;
                    }

                }
                else if (mainStages == Stages.Stage3)
                {
                    switch (stage3States)
                    {
                        case Stage3Stages.BeePollinate:

                                // tut 4
                            if (!TempleMain.Player.TutorialPlayed && tutorialTracker == 3)
                                showHelp = true;

                            if (beez.All(t => t.Active == false))
                            {
                                foreach (var bee in beez)
                                {
                                    var success = bee.EvaluateBeeWork();

                                    if (success && BoardCells[bee.IndexOnBoard2].Mate == new Point(16, 16))
                                    {
                                        effectsManager.AddNewBurst(new Vector2(BoardCells[bee.IndexOnBoard1].Bounds.X, BoardCells[bee.IndexOnBoard1].Bounds.Y), BoardCells[bee.IndexOnBoard1].CellColors);
                                        effectsManager.AddNewBurst(new Vector2(BoardCells[bee.IndexOnBoard2].Bounds.X, BoardCells[bee.IndexOnBoard2].Bounds.Y), BoardCells[bee.IndexOnBoard2].CellColors);

                                        BoardCells[bee.IndexOnBoard2].Mate = new Point(BoardCells[bee.IndexOnBoard1].Bounds.X / 64 - 1, BoardCells[bee.IndexOnBoard1].Bounds.Y / 64 - 1);
                                        BoardCells[bee.IndexOnBoard2].MateType = MateTypes.Bee;
                                    }

                                    bee.Reset();
                                    stage3States = Stage3Stages.ManageBreeding;
                                }
                            }
                            break;
                        case Stage3Stages.ManageBreeding:

                            EvaluteMating();

                            for (var i = 0; i < BoardCells.Length; i++)
                            {

                                if (BoardCells[i].DeFlowered)
                                    BoardCells[i].ClearCellData();

                            }

                            _mainTimer = 0;
                            stage3States = Stage3Stages.PlayerWeeds;

                                // tut 5
                            if (!TempleMain.Player.TutorialPlayed && tutorialTracker == 4)
                                showHelp = true;

                            break;
                        case Stage3Stages.PlayerWeeds:

                            if (InputHandler.IsScreenBeingTouched() && InBounds(_currentScaledMouse.X, _currentScaledMouse.Y) && BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].CurrentID > 0)
                            {
                                effectsManager.AddNewBurst(new Vector2(BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].Bounds.X, BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].Bounds.Y), BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].CellColors);
                                BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].ClearCellData();
                            }

                            if (_mainTimer >= 0.01)
                            {
                                _mainTimer = 0;
                                currentGage -= 1.5f; // 1.5
                                gageHeight += 1.5f;
                            }

                            if (currentGage <= 8)
                            {
                                _mainTimer = 0;
                                stage3States = Stage3Stages.ManageOld;
                            }

                            break;
                        case Stage3Stages.ManageOld:

                            for (var i = 0; i < BoardCells.Length; i++)
                            {
                                if (BoardCells[i].CurrentID != 0)
                                    BoardCells[i].MainAge++;
                            }

                            if (stage3Tracker.Count == 0)
                                SelectOldFlowers();

                            for (var x = 0; x < Width; x++)
                            {
                                if (stage3Tracker[x] != -1)
                                {
                                    var element = stage3Tracker[x] * Width + x;

                                    effectsManager.CreateAbsorbEffect(new Vector2(BoardCells[element].Bounds.X + 32, BoardCells[element].Bounds.Y + 32));

                                }
                            }

                            stage3States = Stage3Stages.ProcessOld;

                            break;
                        case Stage3Stages.ProcessOld:

                            _toRockTimer += elapsed;

                            if (effectsManager.ActiveAbsorbs.Count == 0)
                            {
                                _toRockTimer = 0;

                                stage3States = Stage3Stages.Finalize;

                                stage3Tracker.Clear();
                            }
                            else if (effectsManager.ActiveAbsorbs.Count > 0 && _toRockTimer >= 0.2f && _toRockTimer < 0.3f)
                            {
                                for (var x = 0; x < Width; x++)
                                {
                                    if (stage3Tracker[x] != -1)
                                    {
                                        var startElement = stage3Tracker[x] * Width + x;

                                        var lastElement = 0;

                                        for (int y = Height - 1; y >= 0; y--)
                                        {
                                            if (!BoardCells[y * Width + x].Blocked)
                                            {
                                                lastElement = y * Width + x;

                                                break;
                                            }
                                        }

                                        if (BoardCells[startElement].CurrentID != 0)
                                        {
                                            BoardCells[startElement].ClearCellData();

                                            _rockGenTracker++;

                                            var choseMe = 0;

                                            if (_rockGenTracker > 3)
                                            {
                                                choseMe = MasterRandom.FRandom.Next(2, 8);
                                                _rockGenTracker = 0;
                                            }
                                            else
                                                choseMe = MasterRandom.FRandom.Next(0, 4);

                                            effectsManager.AddBlock(new Vector2(BoardCells[startElement].Bounds.X, BoardCells[startElement].Bounds.Y),
                                                new Vector2(BoardCells[lastElement].Bounds.X, BoardCells[lastElement].Bounds.Y), lastElement, choseMe);

                                            _rockWasCreated = true;

                                        }
                                    }
                                }
                            }

                            break;
                        case Stage3Stages.Finalize:

                            mainStages = Stages.Stage1;

                            wind = (WindDirection)MasterRandom.FRandom.Next(1, 17);
                            windDirection = GetWindRotation();
                            Pollen.SetDirAndSpeed(windDirection, windspeed);
                            Wind.SetDirAndSpeed(windDirection, windspeed);
                            windGage.WindChange = true;

                            foreach (var pollen in pollenz)
                                pollen.Reset();

                            ChoosePieces();

                            currentPointer = 0;

                            nextPiece = currentPiecesUsed[currentPointer];
                            currentPointer++;
                            currentPiece = currentPiecesUsed[currentPointer];
                            currentPointer++;

                            nextShape = GamePieces[nextPiece].Shape;
                            currentShape = GamePieces[currentPiece].Shape;

                            nextImage = PieceImageFactory.BuildImage(currentShape);

                            nextFlower = GamePieces[nextPiece].FlowerType;
                            nextColor = new ColorPair(GamePieces[nextPiece].Colors.Color1, GamePieces[nextPiece].Colors.Color2);
                            currentFlower = GamePieces[currentPiece].FlowerType;
                            currentColor = new ColorPair(GamePieces[currentPiece].Colors.Color1, GamePieces[currentPiece].Colors.Color2);

                            Season++;
                            if ((int)Season > 3)
                            {
                                Season = 0;
                                Year++;
                            }

                            _zenHoldOverScore = Score;
                            _matesId = false;

                            currentGage = GageStart;
                            gageHeight = 0;

                            for (var i = 0; i < BoardCells.Length; i++)
                            {
                                if (BoardCells[i].Blocked && BoardCells[i].BlockHP < 3)
                                    BoardCells[i].BlockHP = 3;

                                BoardCells[i].Age = 3;
                            }

                            for (var i = 0; i < BoardCells.Length; i++)
                            {
                                BoardCells[i].ReevaluateSource();
                            }

                            stage3States = Stage3Stages.None;

                            if (!TempleMain.Player.TutorialPlayed && tutorialTracker == 5)
                            {
                                showHelp = true;
                            }

                            SetMood();

                            if (BoardCells.All(t => t.Blocked))
                            {
                                TotalMatches = GetTotalMatches();
                                ScreenManager.GameReference.ScreenControl(ScreenName, "Lost");
                            }

                            if (!TempleMain.Player.PlayerTrophies[4].Awarded && UsedFlowerCounter.Any(r => r == 28))
                                ScreenManager.GameReference.EvaluateAchievements("OneComp");


                            if (ScreenName == Screens.ClassicScreen)
                            {
                                if (UsedFlowerCounter.All(v => v == 28))
                                {
                                    ScreenManager.GameReference.EvaluateAchievements("AllComp");

                                    TotalMatches = GetTotalMatches();

                                    ScreenManager.GameReference.ScreenControl(ScreenName, "Won");

                                }

                            }
                            else if (ScreenName == Screens.EnduranceScreen)
                            {
                                var trips = new bool[4];
                                for (var i = 0; i < acceptedSelections.Length; i++)
                                {
                                    if (UsedFlowerCounter[acceptedSelections[i] - 1] == 28)
                                        trips[i] = true;
                                }
                                if (trips.All(d => d == true))
                                {
                                    TotalMatches = GetTotalMatches();
                                    ScreenManager.GameReference.ScreenControl(ScreenName, "Won");
                                }
                            }

                            // trophie 0
                            if (!TempleMain.Player.PlayerTrophies[0].Awarded)
                            {

                                if (_rockWasCreated)
                                    CheckRockCount();
                                else
                                {
                                    _lastNoRockCount++;
                                }

                                if (_lastNoRockCount >= 10)
                                    ScreenManager.GameReference.EvaluateAchievements("RockTest");
                            }
                            // trophie 3

                            if (!TempleMain.Player.PlayerTrophies[3].Awarded)
                            {
                                if (_autoClicked)
                                    CheckAutoClickCount();
                                else
                                {
                                    _lastNoAutoClick++;
                                }

                                if (_lastNoAutoClick >= 10)
                                    ScreenManager.GameReference.EvaluateAchievements("AutoClick");
                            }
                            break;
                        default:

                            break;
                    }
                }

                if (InputHandler.FingerRaised() && InBounds(_currentScaledMouse.X, _currentScaledMouse.Y))
                {
                    //var here = new Point(InputHandler.MousePositionScaled.ToPoint().X / 64 - 1, InputHandler.MousePositionScaled.ToPoint().Y / 64 - 1);

                    if (BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].Mode == CellMode.Flower)
                    {
                        effectsManager.ActivateSpray(InputHandler.TouchVectorScaled, BoardCells[_currentScaledMouse.Y * Width + _currentScaledMouse.X].CellColors);
                    }
                }

                effectsManager.Update(gameTime);

            }

            if (showHelp && !_buttonsVisisble)
            {

                if (tutorialTracker <= 4)
                {
                    nextHelpButton.Visible = true;
                    nextHelpButton.Enabled = true;
                    skipHelpButton.Visible = true;
                    skipHelpButton.Enabled = true;

                    _buttonsVisisble = true;
                }
                else
                {
                    endButton.Visible = true;
                    endButton.Enabled = true;


                    _buttonsVisisble = true;
                }
            }

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                ScreenManager.GameReference.ManagePlacardState(gameTime);

            ControlManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
		{
            //Tutorial
			if(showHelp)
			{
				if(tutorialTracker < helpText.Length)
				{
                    spriteBatch.Draw(SpriteLoader.IosTextSheet, new Vector2(280, 424), helpText[tutorialTracker], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
					spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(248, 392), new Rectangle(96, 1216, 576, 200), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
				}
			}

			effectsManager.Draw(spriteBatch);

			// next shape
			if(mainStages == Stages.Stage1)
			{
				spriteBatch.Draw(nextImage, new Vector2(1112, 8), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
				spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1048, 40), GetFlowerSource(currentFlower, 1), currentColor.Color1, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
				spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1048, 40), GetFlowerSource(currentFlower, 2), currentColor.Color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
			}

			// backing
			for(var i = 0; i < 4; i++)
				spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(gridBacking[i].X + offsetX, gridBacking[i].Y + offsetY), new Rectangle(0, 736, 734, 448), MoodColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);

			// border
            spriteBatch.Draw(SpriteLoader.IosMainSheet, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.13f);
            for (var i = 0; i < 9; i++)
            {
                if (UsedFlowerCounter[i] == 28)
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, WholeDrawPositions[i], GetFlowerSource((FlowerTypes)(i + 1), 0), Color.DarkGoldenrod, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
                else
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, WholeDrawPositions[i], GetFlowerSource((FlowerTypes)(i + 1), 0), Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
            }

            if (ScreenName == Screens.EnduranceScreen)
            {
                for (var j = 0; j < notIncluded.Length; j++)
                {
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, BlindDraw[notIncluded[j]], new Rectangle(64, 576, 88, 88), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.97f);
                }
            }

			// gauge bar
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle(8, (int)currentGage, 48, (int)gageHeight), new Rectangle(832, 632, 48, 16), Color.FromNonPremultiplied(36, 84, 204, 255), 0f, Vector2.Zero, SpriteEffects.None, 0.12f);
            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle(8, 8, 48, 696), new Rectangle(832, 632, 48, 16), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.09f);

			for(var i = 0; i < 9; i++)
			{
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Rectangle((int)DrawPositions[i].X, (int)DrawPositions[i].Y - yOffsetRectType[UsedFlowerCounter[i]], 88, yOffsetRectType[UsedFlowerCounter[i]]),
                    new Rectangle(832, 632, 16, 16), Color.FromNonPremultiplied(200, 44, 44, 178), 0f, Vector2.Zero, SpriteEffects.None, 0.92f);
			}

			// Text data

            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(68, 16), new Rectangle(0, 8, 104, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);

            if (Year > 99)
                spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(248, 24), GetSeason(), Color.White, 0f, Vector2.Zero, .75f, SpriteEffects.None, 0.99f);
            else
                spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(232, 16), GetSeason(), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);

            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(449, 16), GetStage(), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
            //spriteBatch.DrawString(SpriteLoader.Font32, "Year " + Year.ToString() + ", " + seasonNames[(int)Season], new Vector2(72, 8), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
            //spriteBatch.DrawString(SpriteLoader.Font32, stageNames[(int)mainStages], new Vector2(449, 8), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
            spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(600, 16), new Rectangle(0, 440, 120, 40), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
            //spriteBatch.DrawString(SpriteLoader.Font32, "Score: " + Score.ToString(), new Vector2(619, 8), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);

            numberSpitter.Draw(spriteBatch);

			// wind
			for(var i = 0; i < windz.Length; i++)
			{
				if(windz[i].Visible)
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, windz[i].Position, new Rectangle(864, 640, windz[i].Size, windz[i].Size), windz[i].DebrisColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.94f);
			}

			// pollen
			for(var i = 0; i < pollenz.Length; i++)
			{
				if(pollenz[i].Visible)
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, pollenz[i].Position, new Rectangle(864, 640, pollenz[i].Size, pollenz[i].Size), pollenz[i].PollenColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.93f);
			}

			// wind marker
            spriteBatch.Draw(SpriteLoader.IosMainSheet, windGage.Position, new Rectangle(323, 576, 48, 48), Color.White, windGage.CurrentRotation, windGage.Origin, 1f, SpriteEffects.None, 0.98f);
                
			if(windspeed == 1)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1016, 32), new Rectangle(952, 168, 8, 24), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.97f);
			else
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(1016, 8), new Rectangle(960, 144, 8, 48), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.97f);

			for(var x = 0; x < Width; x++)
				for(var y = 0; y < Height; y++)
				{
					var element = y * Width + x;

                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(x * 64 + 64, y * 64 + 64), MainMenuScreen.GetTile(tiling[element], (int)Season), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.80f);

					// test
					if(BoardCells[element].Blocked)
					{
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
							GetRockSource(BoardCells[element].RockType), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);

						if(BoardCells[element].BlockHP < 3)
                            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
								GetCrackSource(BoardCells[element].BlockHP), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.89f);
					}

					// plant drawing
                    if (BoardCells[element].CurrentNumber != 0)
                    {
                        //if (BoardCells[element].CurrentNumber == 1)
                        //{
                            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                new Rectangle(BoardCells[element].SourceReference.X, BoardCells[element].SourceReference.Y, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.82f);
                        //}
                        //else
                        //{
                        //    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                        //        new Rectangle(BoardCells[element].SourceReference.X, BoardCells[element].SourceReference.Y, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.20f);
                        //}
                    }

					// flower drawing
					if(!BoardCells[element].DeFlowered)
					{
						if(BoardCells[element].Mode == CellMode.Flower || BoardCells[element].Mode == CellMode.Shadow)
						{
							if(BoardCells[element].Mode == CellMode.Shadow)
							{
                                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 37, BoardCells[element].Bounds.Y + 37),
									GetFlowerSource(BoardCells[element].PlantType, 3), Color.White, 0f, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer - 0.02f);
							}

							// todo:: fix flowers
							if(BoardCells[element].Mode == CellMode.Flower)
							{

								if(BoardCells[element].MainAge < 1)
								{
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 37, BoardCells[element].Bounds.Y + 37),
										GetFlowerSource(BoardCells[element].PlantType, 3), Color.White, BoardCells[element].Rotation, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer - 0.02f);

                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 32, BoardCells[element].Bounds.Y + 32),
										GetFlowerSource(BoardCells[element].PlantType, 1), BoardCells[element].CellColors.Color1, BoardCells[element].Rotation, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer);
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 32, BoardCells[element].Bounds.Y + 32),
										GetFlowerSource(BoardCells[element].PlantType, 2), BoardCells[element].CellColors.Color2, BoardCells[element].Rotation, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer);
								} else
								if(BoardCells[element].MainAge == 1)
								{
									var offsetx = (float)MasterRandom.FRandom.NextDouble();
									var offsety = (float)MasterRandom.FRandom.NextDouble();

                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 37 + offsetx, BoardCells[element].Bounds.Y + 37 + offsety),
										GetFlowerSource(BoardCells[element].PlantType, 3), Color.White, 0f, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer - 0.02f);

                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 32 + offsetx, BoardCells[element].Bounds.Y + 32 + offsety),
										GetFlowerSource(BoardCells[element].PlantType, 1), BoardCells[element].CellColors.Color1, 0f, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer);
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 32 + offsetx, BoardCells[element].Bounds.Y + 32 + offsety),
										GetFlowerSource(BoardCells[element].PlantType, 2), BoardCells[element].CellColors.Color2, 0f, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer);
								} else
								if(BoardCells[element].MainAge >= 2)
								{
									var offsetx = (float)MasterRandom.FRandom.Next(1, 3);
									var offsety = (float)MasterRandom.FRandom.Next(1, 3);

                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 37 + offsetx, BoardCells[element].Bounds.Y + 37 + offsety),
										GetFlowerSource(BoardCells[element].PlantType, 3), Color.White, 0f, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer - 0.02f);

                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 32 + offsetx, BoardCells[element].Bounds.Y + 32 + offsety),
										GetFlowerSource(BoardCells[element].PlantType, 1), BoardCells[element].CellColors.Color1, 0f, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer);
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X + 32 + offsetx, BoardCells[element].Bounds.Y + 32 + offsety),
										GetFlowerSource(BoardCells[element].PlantType, 2), BoardCells[element].CellColors.Color2, 0f, new Vector2(32, 32), BoardCells[element].currentScale, SpriteEffects.None, BoardCells[element].DrawLayer);
								}
							}
						}
					}

                    if (pieceCells[element].ShapeCellPresent && mainStages == Stages.Stage1)
                    {
                        if (occupied)
                        {
                            if (pieceCells[element].ShapeStart)
                            {

                                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                    new Rectangle(768, 640, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .87f);

                                if (!BoardCells[element].Blocked)
                                {
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                        GetFlowerSource(currentFlower, 1), currentColor.Color1, 0f, Vector2.Zero, 1f, SpriteEffects.None, .91f);
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                        GetFlowerSource(currentFlower, 2), currentColor.Color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, .91f);
                                }
                            }
                            else
                                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                    new Rectangle(768, 576, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .87f);
                        }
                        else
                        {
                            if (pieceCells[element].ShapeStart)
                            {
                                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                    new Rectangle(704, 640, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.87f);

                                if (!BoardCells[element].Blocked)
                                {
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                        GetFlowerSource(currentFlower, 1), currentColor.Color1, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.91f);
                                    spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                        GetFlowerSource(currentFlower, 2), currentColor.Color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.91f);
                                }
                            }
                            else
                                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y),
                                    new Rectangle(704, 576, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .87f);
                        }
                    }

					// determine what boardcell to highlight that a bee will visit
					var activeBee = beez.FindIndex(f => f.IndexOnBoard1 == element);

					if(activeBee != -1)
					{
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y), new Rectangle(448, 576, 64, 64), Color.White,
							0f, Vector2.Zero, 1f, SpriteEffects.None, 0.96f);
					}

					activeBee = beez.FindIndex(r => r.IndexOnBoard2 == element);

					if(activeBee != -1)
					{
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y), new Rectangle(448, 640, 64, 64), Color.White,
							0f, Vector2.Zero, 1f, SpriteEffects.None, 0.96f);
					}
				}

			// Bee Stuff
			for(var i = 0; i < beez.Count; i++)
			{
				if(!beez[i].Active)
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, beez[i].Position, Bee.GetSource(beez[i].Flap), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
				else
				{
					if(beez[i].IsGoingRight)
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, beez[i].Position, Bee.GetActiveSource(beez[i].ActiveFlap), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.96f);
					else
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, beez[i].Position, Bee.GetActiveSource(beez[i].ActiveFlap), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0.96f);
				}
			}

			occupied = false;

            if (ScreenManager.GameReference.PlacardState != TempleMain.PlacardSlideStates.Closed)
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(276, ScreenManager.GameReference.YPlacardOffset), new Rectangle(1296, 1680, 728, 104),
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

			ControlManager.Draw(spriteBatch);

            if (pauseButtonSlideState == ButtonSlide.Closing || pauseButtonSlideState == ButtonSlide.Opening)
                ChangeButtonState();
            
		}


        private Rectangle rectPauseButton = new Rectangle(1224, 120, 56, 432);

        private void ChangeButtonState()
        {
            if (pauseButtonSlideState == ButtonSlide.Closing)
            {
                pauseButton.Position += new Vector2(2,0);

                if (pauseButton.Position.X >= 1260)
                {
                    pauseButton.Position = new Vector2(1260, 120);
                    pauseButtonSlideState = ButtonSlide.Closed;
                    pauseButton.HasFocus = false;
                }

            }
            else if (pauseButtonSlideState == ButtonSlide.Opening)
            {
                pauseButton.Position -= new Vector2(2,0);

                if (pauseButton.Position.X <= 1172)
                {
                    pauseButton.Position = new Vector2( 1172,120);
                    pauseButton.Enabled = true;
                    pauseButtonSlideState = ButtonSlide.Open;
                }
            }
        }

        private void HandleInput()
		{
            if (pauseButtonSlideState == ButtonSlide.Closed)
            {
                if (rectPauseButton.Contains(InputHandler.TouchVectorScaled.ToPoint()))
                {
                    pauseTimer += 1f;
                }
                else
                {
                    pauseTimer = 0;
                }


                if (pauseTimer >= 5f)
                {
                    pauseButtonSlideState = ButtonSlide.Opening;
                    pauseTimer = 0;
                }
            }

            if (pauseButtonSlideState == ButtonSlide.Open)
            {
                if (!rectPauseButton.Contains(InputHandler.TouchVectorScaled.ToPoint()))
                {
                    pauseTimer += 1f;
                }
                else
                {
                    pauseTimer = 0;
                }

                if (pauseTimer >= 9f)
                {
                    pauseButtonSlideState = ButtonSlide.Closing;
                    pauseTimer = 0;
                    pauseButton.Enabled = false;
                }
            }

            if (InWorldBounds(InputHandler.TouchVectorScaled.ToPoint().X, InputHandler.TouchVectorScaled.ToPoint().Y))
            {
                offsetX = (CenterScreen.X - InputHandler.TouchVectorScaled.ToPoint().X) / 32;
                offsetY = (CenterScreen.Y - InputHandler.TouchVectorScaled.ToPoint().Y) / 32;
            }

			if(mainStages == Stages.Stage1)
			{

//                if (InputHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Q))
//                    currentShape = RotateMatrixLeft(currentShape, currentShape.GetLength(0));
//                if (InputHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.E))
//                    currentShape = RotateMatrixRight(currentShape, currentShape.GetLength(0));

                if (InputHandler.FingerRaised())
                {
                    var tr = 0;

                    if (PlaceShape(new Point(((InputHandler.TouchVectorScaled.ToPoint().X) / 64) - 1, ((InputHandler.TouchVectorScaled.ToPoint().Y) / 64) - 1), out tr))
                    {
                        currentPiece = nextPiece;

                        nextPiece = currentPiecesUsed[currentPointer];
                        currentPointer++;

                        if (currentPointer == currentPiecesUsed.Length)
                        {
                            currentPointer = 0;
                            currentPiecesUsed = currentPiecesUsed.Shuffle().ToArray();
                        }

                        currentShape = nextShape;
                        nextShape = GamePieces[nextPiece].Shape;
                        nextImage = PieceImageFactory.BuildImage(currentShape);

                        nextFlower = GamePieces[nextPiece].FlowerType;
                        nextColor = new ColorPair(GamePieces[nextPiece].Colors.Color1, GamePieces[nextPiece].Colors.Color2); 
                        currentFlower = GamePieces[currentPiece].FlowerType;
                        currentColor = new ColorPair(GamePieces[currentPiece].Colors.Color1, GamePieces[currentPiece].Colors.Color2);

                        if (tr > 0)
                        {
                            var sender = 0;

                            if (tr == 1)
                                sender = 25;
                            else
                            {
                                sender = 50 + (tr * 10);
                            }

                            if (TempleMain.Player.VibrateOn)
                                ScreenManager.GameReference.GetSomeContext(sender);

                        }
                        SoundBoard.DropShape.Play();

                    }

                }
			}

		}
        

        private static byte[,] RotateMatrixRight(byte[,] matrix, int n)
        {
            byte[,] shape = new byte[n, n];

            for (var i = 0; i < n; ++i)
                for (var j = 0; j < n; ++j)
                {
                    shape[i, j] = matrix[n - j - 1, i]; 
                }

            return shape;
        }

        private static byte[,] RotateMatrixLeft(byte[,] matrix, int n)
        {
            byte[,] shape = new byte[n, n];

            for (var i = 0; i < n; ++i)
                for (var j = 0; j < n; ++j)
                {
                    shape[i, j] = matrix[j, n - i - 1];
                }

            return shape;
        }

        private static Rectangle GetFlowerSource(FlowerTypes flower, byte flag)
        {
            var startX = 0;
            var startY = 0;

            var mod = 0;

			if (!TempleMain.Player.RegTilesPreferred)
                mod = 192;

            if (flag == 0)
            {
                startX = 1280 + mod;
                startY = 896;
            }
            else if (flag == 1)
            {
                startX = 1280 + mod;
                startY = 1280;
            }
            else if (flag == 2)
            {
                startX = 1280 + mod;
                startY = 1472;
            }
            else if (flag == 3)
            {
                startX = 1280 + mod;
                startY = 1088;
            }

            switch (flower)
            {
                case FlowerTypes.Orchid:
                    return new Rectangle(startX, startY, 64, 64);
                case FlowerTypes.Rose:
                    return new Rectangle(startX + 64, startY, 64, 64);
                case FlowerTypes.Anemone:
                    return new Rectangle(startX + 128, startY, 64, 64);
                case FlowerTypes.Begonia:
                    return new Rectangle(startX, startY + 64, 64, 64);
                case FlowerTypes.Gazania:
                    return new Rectangle(startX + 64, startY + 64, 64, 64);
                case FlowerTypes.Tulip:
                    return new Rectangle(startX + 128, startY + 64, 64, 64);
                case FlowerTypes.Petunia:
                    return new Rectangle(startX, startY + 128, 64, 64);
                case FlowerTypes.Poppy:
                    return new Rectangle(startX + 64, startY + 128, 64, 64);
                case FlowerTypes.Rhododendron:
                    return new Rectangle(startX + 128, startY+ 128, 64, 64);
                default:
                    return new Rectangle(0, 0, 0, 0);
            }
        }

        private bool PlaceShape(Point origin, out int blarg)
        {
            if (!InBounds(origin.X, origin.Y) ||
                BoardCells[origin.Y * Width + origin.X].CurrentNumber != 0 ||
                BoardCells[origin.Y * Width + origin.X].Blocked)
            {
                blarg = 0;
                return false;
            }

            Point startPoint = new Point();

            var flattenShape = currentShape.Flatten();
            var maxAge = flattenShape.Max();

            for (var x = 0; x < currentShape.GetLength(0); x++)
                for (var y = 0; y < currentShape.GetLength(1); y++)
                {
                    if (currentShape[x, y] == 1)
                    {
                        startPoint = new Point(x, y);
                    }
                }

            int backX, frontX, backY, frontY;

            backX = 0 - startPoint.X;
            frontX = currentShape.GetLength(0) - startPoint.X;
            backY = 0 - startPoint.Y;
            frontY = currentShape.GetLength(1) - startPoint.Y;

            // translate shape to overlay
            backX += origin.X;
            frontX += origin.X;
            backY += origin.Y;
            frontY += origin.Y;

            var u = 0;
            var v = 0;

            for (var x = backX; x < frontX; x++)
            {
                v = 0;
                for (var y = backY; y < frontY; y++)
                {
                    if (!InBounds(x, y) && currentShape[u, v] != 0)
                    {
                        BoardCells[origin.Y * Width + origin.X].FillCellData(
                            1, currentUsedId, GamePieces[currentPiece].Colors, GamePieces[currentPiece].FlowerType, maxAge, GamePieces[currentPiece].IsDark);

                        currentUsedId++;

                        if (currentUsedId >= MaxPossibleId)
                            currentUsedId = 2;

                        blarg = 1;

                        return true;
                    }

                    if (InBounds(x, y))
                    {
                        if (BoardCells[y * Width + x].CurrentNumber > 0 && currentShape[u, v] != 0)
                        {
                            BoardCells[origin.Y * Width + origin.X].FillCellData(
                                1, currentUsedId, GamePieces[currentPiece].Colors, GamePieces[currentPiece].FlowerType, maxAge, GamePieces[currentPiece].IsDark);

                            currentUsedId++;

                            if (currentUsedId >= MaxPossibleId)
                                currentUsedId = 2;

                            blarg = 1;

                            return true;
                        }

                        if (BoardCells[y * Width + x].Blocked && currentShape[u, v] != 0)
                        {
                            BoardCells[origin.Y * Width + origin.X].FillCellData(
                                1, currentUsedId, GamePieces[currentPiece].Colors, GamePieces[currentPiece].FlowerType, maxAge, GamePieces[currentPiece].IsDark);

                            currentUsedId++;

                            if (currentUsedId >= MaxPossibleId)
                                currentUsedId = 2;

                            blarg = 1;

                            return true;
                        }

                    }
                    v++;
                }
                u++;
            }

            var trips = 0;

            u = 0;
            v = 0;
            for (var x = backX; x < frontX; x++)
            {
                v = 0;
                for (var y = backY; y < frontY; y++)
                {
                    {
                        if (currentShape[u, v] > 0)
                        {
                            BoardCells[y * Width + x].FillCellData(
                                currentShape[u, v], currentUsedId, GamePieces[currentPiece].Colors,
                                GamePieces[currentPiece].FlowerType, maxAge, GamePieces[currentPiece].IsDark);

                            trips += 1;

                            Score += 50;
                            effectsManager.AddScore(new Vector2(BoardCells[y * Width + x].Bounds.X + 16, BoardCells[y * Width + x].Bounds.Y + 16), 50);
                        }
                    }
                    v++;
                }
                u++;
            }

            currentUsedId++;

            if (currentUsedId >= MaxPossibleId)
                currentUsedId = 2;

            blarg = trips;

            return true;
        }

        private Vector2 GetWindRotation()
        {
            switch (wind)
            {
                case WindDirection.WestWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(180);
                    return new Vector2(-1, 0);
                case WindDirection.WestStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(180);
                    return new Vector2(-2, 0);
                case WindDirection.NorWestWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(225);
                    return new Vector2(-1, -1);
                case WindDirection.NorWestStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(225);
                    return new Vector2(-2, -2);
                case WindDirection.NorthWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(270);
                    return new Vector2(0, -1);
                case WindDirection.NorthStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(270);
                    return new Vector2(0, -2);
                case WindDirection.NorEastWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(315);
                    return new Vector2(1, -1);
                case WindDirection.NorEastStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(315);
                    return new Vector2(2, -2);
                case WindDirection.EastWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(0);
                    return new Vector2(1, 0);
                case WindDirection.EastStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(0);
                    return new Vector2(2, 0);
                case WindDirection.SouthEastWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(45);
                    return new Vector2(1, 1);
                case WindDirection.SouthEastStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(45);
                    return new Vector2(2, 2);
                case WindDirection.SouthWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(90);
                    return new Vector2(0, 1);
                case WindDirection.SouthStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(90);
                    return new Vector2(0, 2);
                case WindDirection.SouthWestWeak:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(135);
                    return new Vector2(-1, 1);
                case WindDirection.SouthWestStrong:
                    windspeed = 2;
                    windGage.RealRotation = MathHelper.ToRadians(135);
                    return new Vector2(-2, 2);
                case WindDirection.None:
                default:
                    windspeed = 1;
                    windGage.RealRotation = MathHelper.ToRadians(0);
                    return new Vector2(2, 0);
            }
        }

        private void EvaluteCells(Point origin)
        {
            if (!InBounds(origin.X, origin.Y))
            {
                for (var i = 0; i < pieceCells.Length; i++)
                    pieceCells[i].ShapeCellPresent = false;
                return ;
            }

            for (var i = 0; i < pieceCells.Length; i++)
                pieceCells[i].ShapeCellPresent = false;

            Point startPoint = new Point();

            for (var x = 0; x < currentShape.GetLength(0); x++)
                for (var y = 0; y < currentShape.GetLength(1); y++)
                {
                    if (currentShape[x, y] == 1)
                    {
                        startPoint = new Point(x, y);
                    }
                }

            int backX, frontX, backY, frontY;

            backX = 0 - startPoint.X;
            frontX = currentShape.GetLength(0) - startPoint.X;
            backY = 0 - startPoint.Y;
            frontY = currentShape.GetLength(1) - startPoint.Y;

            // translate shape to overlay
            backX += origin.X;
            frontX += origin.X;
            backY += origin.Y;
            frontY += origin.Y;

            var u = 0;
            var v = 0;

            for (var x = backX; x < frontX; x++)
            {
                v = 0;
                for (var y = backY; y < frontY; y++)
                {
                    if (InBounds(x, y))
                    {
                        if (currentShape[u, v] > 0)
                            pieceCells[y * Width + x].ShapeCellPresent = true;
                        else
                            pieceCells[y * Width + x].ShapeCellPresent = false;

                        if (currentShape[u, v] == 1)
                            pieceCells[y * Width + x].ShapeStart = true;
                        else
                            pieceCells[y * Width + x].ShapeStart = false;
                    }
                    
                    v++;
                }
                u++;
            }

            u = 0;
            v = 0;

            for (var x = backX; x < frontX; x++)
            {
                v = 0;
                for (var y = backY; y < frontY; y++)
                {
                    if (!InBounds(x, y) && currentShape[u, v] > 0)
                        occupied = true;
                    v++;
                }
                u++;
            }

            if (!occupied)
            {
                for (var i = 0; i < BoardCells.Length; i++)
                {
                    if (BoardCells[i].CurrentNumber > 0 && pieceCells[i].ShapeCellPresent)
                        occupied = true;

                    if (BoardCells[i].Blocked && pieceCells[i].ShapeCellPresent)
                        occupied = true;
                }
            }

        }

        private void ChoosePieces()
        {

            if (ScreenName == Screens.EnduranceScreen)
            {
                currentPiecesUsed = new int[8];

                var trrer = new List<int>();

                for (var t = 0; t < 9; t++)
                    trrer.Add(t + 1);

                var result = trrer.Except(acceptedSelections).PickRandom(2).ToArray();

                var chopList = new List<int>();

                chopList = acceptedSelections.PickRandom(2).ToList();

                chopList.Add(result[0]);
                chopList.Add(result[1]);

                List<PieceTemplate> plantPool1;
                List<PieceTemplate> plantPool2;


                plantPool1 = (from a in GamePieces
                              where a.FlowerType == (FlowerTypes)chopList[0]
                              where a.Unlocked
                              select a).ToList();

                plantPool2 = (from b in GamePieces
                              where b.FlowerType == (FlowerTypes)chopList[1]
                              where b.Unlocked
                              select b).ToList();

                var clap = (from c in GamePieces
                            where c.FlowerType == (FlowerTypes)chopList[2]
                            select c).ToList().PickOne(1);

                var clap2 = (from d in GamePieces
                             where d.FlowerType == (FlowerTypes)chopList[3]
                             select d).ToList().PickOne(1);

                var select1 = plantPool1.OrderBy(x => MasterRandom.FRandom.Next()).Take(3).ToList();
                var select2 = plantPool2.OrderBy(x => MasterRandom.FRandom.Next()).Take(3).ToList();

                var currentUnlocks = new List<PieceTemplate>(select1.Count +  select2.Count + 2);

                currentUnlocks.AddRange(select1);
                currentUnlocks.AddRange(select2);
                currentUnlocks.Add(clap);
                currentUnlocks.Add(clap2);

                for (var i=0; i < currentUnlocks.Count;i++)
                    currentPiecesUsed[i] = GamePieces.FindIndex(
                        p => p.FlowerType == currentUnlocks[i].FlowerType &
                            p.Colors.Color1 == currentUnlocks[i].Colors.Color1 &
                            p.Colors.Color2 == currentUnlocks[i].Colors.Color2);

                currentPiecesUsed = currentPiecesUsed.Shuffle().ToArray();
            }
            else
            {
                currentPiecesUsed = new int[9];

                var assignmentOK = false;

                List<FlowerTypes> flowerAssignment = new List<FlowerTypes>();

                var maxI = masterUsageWeight.IndexOf(masterUsageWeight.Max());
                var minI = masterUsageWeight.IndexOf(masterUsageWeight.Min());

                var difference = masterUsageWeight[maxI] - masterUsageWeight[minI];

                if (difference < 2)
                    assignmentOK = true;

                List<FlowerTypes> flowerPool = PieceTemplate.Flowers.Shuffle().ToList();
                flowerAssignment = flowerPool.GetRange(0, 3);

                if (!assignmentOK)
                {
                    if (flowerAssignment.Contains(PieceTemplate.Flowers[maxI]) && !flowerAssignment.Contains(PieceTemplate.Flowers[minI]))
                    {
                        var targetIndex = flowerAssignment.IndexOf(PieceTemplate.Flowers[maxI]);
                        flowerAssignment[targetIndex] = PieceTemplate.Flowers[minI];
                    }
                    else if (flowerAssignment.Contains(PieceTemplate.Flowers[maxI]) && flowerAssignment.Contains(PieceTemplate.Flowers[minI]))
                    {
                        var targetIndex = flowerAssignment.IndexOf(PieceTemplate.Flowers[maxI]);
                        flowerAssignment[targetIndex] = flowerPool[3];
                    }

                }

                masterUsageWeight[(int)(flowerAssignment[0] - 1)]++;
                masterUsageWeight[(int)(flowerAssignment[1] - 1)]++;
                masterUsageWeight[(int)(flowerAssignment[2] - 1)]++;

                var unlocked1 = (from f in GamePieces
                                 where f.FlowerType == flowerAssignment[0]
                                 where f.Unlocked
                                 select f).ToList();

                unlocked1 = unlocked1.Shuffle().ToList();

                var pool1 = new List<PieceTemplate>();

                while (pool1.Count < 7)
                {
                    for (int i = unlocked1.Count - 1; i >= 0; i--)
                    {
                        if (unlocked1[i].Colors.Color1 == unlocked1[i].Colors.Color2 && !unlocked1[i].Used)
                        {
                            if (MasterRandom.FRandom.NextBool())
                            {
                                pool1.Add(unlocked1[i]);
                                unlocked1.Remove(unlocked1[i]);
                            }

                        }
                        else if (unlocked1[i].Colors.Color1 != unlocked1[i].Colors.Color2 && !unlocked1[i].Used)
                        {
                            if (MasterRandom.FRandom.Next(0, 4) == 2)
                            {
                                pool1.Add(unlocked1[i]);
                                unlocked1.Remove(unlocked1[i]);
                            }
                        }
                        else
                        {
                            if (MasterRandom.FRandom.Next(0, 4) == 2)
                            {
                                pool1.Add(unlocked1[i]);
                                unlocked1.Remove(unlocked1[i]);
                            }
                        }
                    }
                }

                var unlocked2 = (from g in GamePieces
                                 where g.FlowerType == flowerAssignment[1]
                                 where g.Unlocked
                                 select g).ToList();

                var pool2 = new List<PieceTemplate>();

                while (pool2.Count < 7)
                {
                    for (int i = unlocked2.Count - 1; i >= 0; i--)
                    {
                        if (unlocked2[i].Colors.Color1 == unlocked2[i].Colors.Color2 && !unlocked2[i].Used)
                        {
                            if (MasterRandom.FRandom.NextBool())
                            {
                                pool2.Add(unlocked2[i]);
                                unlocked2.Remove(unlocked2[i]);
                            }

                        }
                        else if (unlocked2[i].Colors.Color1 != unlocked2[i].Colors.Color2 && !unlocked2[i].Used)
                        {
                            if (MasterRandom.FRandom.Next(0, 4) == 2)
                            {
                                pool2.Add(unlocked2[i]);
                                unlocked2.Remove(unlocked2[i]);
                            }
                        }
                        else
                        {
                            if (MasterRandom.FRandom.Next(0, 4) == 2)
                            {
                                pool2.Add(unlocked2[i]);
                                unlocked2.Remove(unlocked2[i]);
                            }
                        }
                    }
                }

                var unlocked3 = (from h in GamePieces
                                 where h.FlowerType == flowerAssignment[2]
                                 where h.Unlocked
                                 select h).ToList();

                var pool3 = new List<PieceTemplate>();

                while (pool3.Count < 7)
                {
                    for (int i = unlocked3.Count - 1; i >= 0; i--)
                    {
                        if (unlocked3[i].Colors.Color1 == unlocked3[i].Colors.Color2 && !unlocked3[i].Used)
                        {
                            if (MasterRandom.FRandom.NextBool())
                            {
                                pool3.Add(unlocked3[i]);
                                unlocked3.Remove(unlocked3[i]);
                            }

                        }
                        else if (unlocked3[i].Colors.Color1 != unlocked3[i].Colors.Color2 && !unlocked3[i].Used)
                        {
                            if (MasterRandom.FRandom.Next(0, 4) == 2)
                            {
                                pool3.Add(unlocked3[i]);
                                unlocked2.Remove(unlocked3[i]);
                            }
                        }
                        else
                        {
                            if (MasterRandom.FRandom.Next(0, 4) == 2)
                            {
                                pool3.Add(unlocked3[i]);
                                unlocked3.Remove(unlocked3[i]);
                            }
                        }
                    }

                }

                unlocked1 = pool1.OrderBy(x => MasterRandom.FRandom.Next()).Take(3).ToList();
                unlocked2 = pool2.OrderBy(x => MasterRandom.FRandom.Next()).Take(3).ToList();
                unlocked3 = pool3.OrderBy(x => MasterRandom.FRandom.Next()).Take(3).ToList();

                var currentUnlocks = new List<PieceTemplate>(unlocked1.Count +
                    unlocked2.Count +
                    unlocked3.Count);

                currentUnlocks.AddRange(unlocked1);
                currentUnlocks.AddRange(unlocked2);
                currentUnlocks.AddRange(unlocked3);

                for (var i = 0; i < currentUnlocks.Count; i++)
                    currentPiecesUsed[i] = GamePieces.FindIndex(
                        p => p.FlowerType == currentUnlocks[i].FlowerType &
                            p.Colors.Color1 == currentUnlocks[i].Colors.Color1 &
                            p.Colors.Color2 == currentUnlocks[i].Colors.Color2);

                currentPiecesUsed = currentPiecesUsed.Shuffle().ToArray();

                //var ttt = new Dictionary<int, int>();

                //foreach (var value in currentPiecesUsed)
                //{
                //    if (ttt.ContainsKey(value))
                //        ttt[value]++;
                //    else
                //        ttt[value] = 1;
                //}
            }
        }

        private void EvaluteMating()
        {
            var badPoint = new Point(16, 16);

            for (var i = 0; i < BoardCells.Length; i++)
            {
                if (BoardCells[i].Mate != badPoint)
                {
                    var mateElement = BoardCells[i].Mate.Y * Width + BoardCells[i].Mate.X;

                    var flowerUsed = BoardCells[i].PlantType;
                    var flowerMate = BoardCells[mateElement].PlantType;

                    var mainFlowerColors = BoardCells[i].CellColors;
                    var mateFlowerColors = BoardCells[mateElement].CellColors;

                    var index = 0;


                    if (flowerUsed == flowerMate)
                    {
                        if (mainFlowerColors.Color1 == mainFlowerColors.Color2 &&                     // flower 1 colors same flower 2 colors same
                            mateFlowerColors.Color1 == mateFlowerColors.Color2)
                        {
                            var choose = MasterRandom.FRandom.Next(0, 5);

                            if (choose == 3)
                            {
                                index = GamePieces.FindIndex(
                                    r => r.FlowerType == flowerUsed &&
                                        (r.Colors.Color1 == mainFlowerColors.Color1 && r.Colors.Color2 == mateFlowerColors.Color1 ||
                                        r.Colors.Color1 == mateFlowerColors.Color1 && r.Colors.Color2 == mainFlowerColors.Color1));

                                if (index != -1)
                                {
                                    if (!GamePieces[index].Unlocked)
                                    {
                                        GamePieces[index].Unlocked = true;
                                        newUnlocks.Add(index);
                                    }
                                }
                            }
                        }
                    }
                    var index1 = GamePieces.FindIndex(y => y.FlowerType == flowerUsed &&
                        (y.Colors.Color1 == mainFlowerColors.Color1 && y.Colors.Color2 == mainFlowerColors.Color2));

                    if (!GamePieces[index1].Used)
                    {
                        GamePieces[index1].Used = true;
                        if (UsedFlowerCounter[(int)(flowerUsed - 1)] < 28)
                            UsedFlowerCounter[(int)(flowerUsed - 1)]++;
                    }

                    var index2 = GamePieces.FindIndex(w => w.FlowerType == flowerMate &&
                         w.Colors.Color1 == mateFlowerColors.Color1 && w.Colors.Color2 == mateFlowerColors.Color2);

                    if (!GamePieces[index2].Used)
                    {
                        GamePieces[index2].Used = true;
                        if (UsedFlowerCounter[(int)(flowerMate - 1)] < 28)
                            UsedFlowerCounter[(int)(flowerMate - 1)]++;
                    }

                }
            }

            for (var i = 0; i < BoardCells.Length; i++)
            {
                if (BoardCells[i].Mate != badPoint)
                {
                    var element = BoardCells[i].Mate.Y * Width + BoardCells[i].Mate.X;

                    effectsManager.AddNewBurst(new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y), BoardCells[element].CellColors);
                    effectsManager.AddNewBurst(new Vector2(BoardCells[i].Bounds.X, BoardCells[i].Bounds.Y), BoardCells[i].CellColors);

                    if (BoardCells[i].MateType == MateTypes.Bee)
                    {
                        Score += 500;
                        effectsManager.AddScore(new Vector2(BoardCells[i].Bounds.X + 16, BoardCells[i].Bounds.Y + 16), 500);

                        succesfulBeeUsageCount++;

                        if (succesfulBeeUsageCount >= targetBeeUsage)
                        {
                            if (beez.Count < 6)
                            {
                                beez.Add(new Bee());
                            }
                            targetBeeUsage = beez.Count + beez.Count + 2;
                            succesfulBeeUsageCount = 0;
                        }
                    }
                    else if (BoardCells[i].MateType == MateTypes.Wind)
                    {
                        Score += 90;
                        effectsManager.AddScore(new Vector2(BoardCells[i].Bounds.X + 16, BoardCells[i].Bounds.Y + 16), 90);
                    }
                    BoardCells[element].DeFlowered = true;
                    BoardCells[i].DeFlowered = true;
                }
            }

        }

        private void IdMate(int x, int y)
        {
            int x1 = 0;
            int y1 = 0;

            switch (wind)
            {
                case WindDirection.WestWeak:
                    x1 = x - 1;
                    y1 = y;
                    break;
                case WindDirection.WestStrong:
                    x1 = x - 2;
                    y1 = y;
                    break;
                case WindDirection.NorWestWeak:
                    x1 = x - 1;
                    y1 = y - 1;
                    break;
                case WindDirection.NorWestStrong:
                    x1 = x - 2;
                    y1 = y - 2;
                    break;
                case WindDirection.NorthWeak:
                    x1 = x;
                    y1 = y - 1;
                    break;
                case WindDirection.NorthStrong:
                    x1 = x;
                    y1 = y - 2;
                    break;
                case WindDirection.NorEastWeak:
                    x1 = x + 1;
                    y1 = y - 1;
                    break;
                case WindDirection.NorEastStrong:
                    x1 = x + 2;
                    y1 = y - 2;
                    break;
                case WindDirection.EastWeak:
                    x1 = x + 1;
                    y1 = y;
                    break;
                case WindDirection.EastStrong:
                    x1 = x + 2;
                    y1 = y;
                    break;
                case WindDirection.SouthEastWeak:
                    x1 = x + 1;
                    y1 = y + 1;
                    break;
                case WindDirection.SouthEastStrong:
                    x1 = x + 2;
                    y1 = y + 2;
                    break;
                case WindDirection.SouthWeak:
                    x1 = x;
                    y1 = y + 1;
                    break;
                case WindDirection.SouthStrong:
                    x1 = x;
                    y1 = y + 2;
                    break;
                case WindDirection.SouthWestWeak:
                    x1 = x - 1;
                    y1 = y + 1;
                    break;
                case WindDirection.SouthWestStrong:
                    x1 = x - 2;
                    y1 = y + 2;
                    break;
                case WindDirection.None:
                default:
                    break;

            }

            if (InBounds(x1, y1))
            {
                if (BoardCells[y * Width + x].PlantType == BoardCells[y1 * Width + x1].PlantType &
                    BoardCells[y * Width + x].CurrentID != BoardCells[y1 * Width + x1].CurrentID)
                {
                    BoardCells[y1 * Width + x1].Mate = new Point(x, y);
                    BoardCells[y1 * Width + x1].MateType = MateTypes.Wind;
                }
            }

            _matesId = true;
        }

        private static bool InWorldBounds(int x, int y)
        {
            return x >= 0 && x < 1280 &&
                   y >= 0 && y < 720;

        }

        private static bool InBoundsScreen(float x, float y)
        {
            return x >= 64 && x < 1024 &&
                   y >= 64 && y < 704;
        }

        public static bool InBounds(int x, int y)
        {
            return x >= 0 && x < Width &&
                   y >= 0 && y < Height;
        }

        private void SelectOldFlowers()
        {

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    if (BoardCells[y * Width + x].MainAge >= 1)
                    {
                        stage3Tracker.Add(y);
                        break;
                    }

                    if (y == (Height - 1))
                        stage3Tracker.Add(-1);
                }

        }

        private void SetMood()
        {
            MoodColor = Color.FromNonPremultiplied((byte)MasterRandom.FRandom.Next(0, 255), (byte)MasterRandom.FRandom.Next(0, 255), (byte)MasterRandom.FRandom.Next(0, 255), 255);
        }

        public void NowIsBlock(int element, int index)
        {
            BoardCells[element].ClearCellData();
            BoardCells[element].Blocked = true;
            BoardCells[element].RockType = index;
            BoardCells[element].BlockHP = 3;
        }

        public void TriggerPop(int element)
        {
            effectsManager.AddNewBurst(new Vector2(BoardCells[element].Bounds.X, BoardCells[element].Bounds.Y), BoardCells[element].CellColors);
            BoardCells[element].ClearCellData();
        }

        public static Rectangle GetRockSource(int index)
        {
            if (index == 0)
                return new Rectangle(192, 576, 64, 64);
            else if (index == 1)
                return new Rectangle(256, 576, 64, 64);
            else if (index == 2)
                return new Rectangle(192, 640, 64, 64);
            else if (index == 3)
                return new Rectangle(256, 640, 64, 64);
            else if (index == 4)
                return new Rectangle(736, 1408, 64, 64);
            else if (index == 5)
                return new Rectangle(800, 1408, 64, 64);
            else if (index == 6)
                return new Rectangle(736, 1472, 64, 64);
            else
                return new Rectangle(800, 1472, 64, 64);
        }

        public static Rectangle GetCrackSource(int hp)
        {
            if (hp == 2)
                return new Rectangle(320, 640, 64, 64);
            else if (hp == 1)
                return new Rectangle(384, 640, 64, 64);
            else
                return new Rectangle(64, 640, 64, 64);
                
        }

        private void SetCascade(int startElement)
        {
            if ((startElement - Width) < 0 || !BoardCells[startElement - Width].Blocked)
                return;

            BoardCells[startElement - Width].ClearBlock();
            BoardCells[startElement - Width].ClearCellData();
            
            effectsManager.AddBlock(new Vector2(BoardCells[startElement - Width].Bounds.X, BoardCells[startElement - Width].Bounds.Y),
                new Vector2(BoardCells[startElement].Bounds.X, BoardCells[startElement].Bounds.Y), startElement, BoardCells[startElement - Width].RockType);

            SetCascade(startElement - Width);
        }

        private Rectangle GetStage()
        {
            if (mainStages == Stages.Stage1)
                return new Rectangle(0, 200, 143, 40);
            else if (mainStages == Stages.Stage2)
                return new Rectangle(0, 240, 136, 40);
            else
                return new Rectangle(0, 280, 112, 40);
        }

        private Rectangle GetSeason()
        {
            if (Season == Seasons.Spring)
                return new Rectangle(0, 40, 144, 40);
            else if (Season == Seasons.Summer)
                return new Rectangle(0, 80, 192, 32);
            else if (Season == Seasons.Fall)
                return new Rectangle(0, 120, 192, 40);
            else
                return new Rectangle(0, 160, 152, 40);
        }

        private static Vector2 GetScoreDrawPoition(FlowerTypes type)
        {
            switch (type)
            {
                case FlowerTypes.Orchid:
                    return new Vector2(1050, 174);
                case FlowerTypes.Rose:
                    return new Vector2(1146, 174);
                case FlowerTypes.Anemone:
                    return new Vector2(1050, 270);
                case FlowerTypes.Begonia:
                    return new Vector2(1146, 270);
                case FlowerTypes.Gazania:
                    return new Vector2(1050, 366);
                case FlowerTypes.Tulip:
                    return new Vector2(1146, 366);
                case FlowerTypes.Petunia:
                    return new Vector2(1050, 462);
                case FlowerTypes.Poppy:
                    return new Vector2(1146, 462);
                case FlowerTypes.Rhododendron:
                    return new Vector2(1096, 558);
                default:
                    return new Vector2(782, 48);
            }
        }

        private void BuildControls()
        {
            tapButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(704, 250, 184, 111), new Vector2(1032, 9));
            tapButton.DrawLayer = 0.99f;
            tapButton.Color = Color.White;
            tapButton.FocusColor = Color.White;
            tapButton.HighlightColor = Color.NavajoWhite;
            tapButton.HasFocus = false;
            tapButton.Name = "Tap";
            tapButton.Selected += OnTapSelected;
            ControlManager.Add(tapButton);

            pauseButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(0, 1216, 88, 432), new Vector2(1248, 120));
			pauseButton.DrawLayer = 1f;
            pauseButton.Color = Color.White;
            pauseButton.HighlightColor = Color.FromNonPremultiplied(224, 204, 140, 175);
			pauseButton.Name = "Pause";
			pauseButton.ButtonBounds = new Rectangle(1172, 120, 88, 432);
			pauseButton.Selected += OnPauseSelected;
            pauseButton.Enabled = false;
			ControlManager.Add(pauseButton);

            autoFillButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(96, 1427, 183, 101), new Vector2(1032, 603));
            autoFillButton.DrawLayer = 0.89f;
			autoFillButton.Color = Color.White;
            autoFillButton.Name = "Auto";
            autoFillButton.Selected += autoFillButton_Selected;
            autoFillButton.Visible = false;
            autoFillButton.Enabled = false;
            ControlManager.Add(autoFillButton);

            nextHelpButton = new ImageButton(SpriteLoader.IosMainSheet, SpriteLoader.IosMainSheet, new Rectangle(450, 1416, 198, 66), new Vector2(626, 326));
            nextHelpButton.DrawLayer = 0.98f;
            nextHelpButton.FocusColor = Color.LawnGreen;
            nextHelpButton.Name = "Next";
            nextHelpButton.Selected += nextHelpButton_Selected;
            nextHelpButton.Visible = false;
            nextHelpButton.Enabled = false;
            ControlManager.Add(nextHelpButton);

            skipHelpButton = new ImageButton(SpriteLoader.IosMainSheet, SpriteLoader.IosMainSheet, new Rectangle(288, 1416, 162, 66), new Vector2(464, 326));
            skipHelpButton.DrawLayer = 0.98f;
            skipHelpButton.FocusColor = Color.LawnGreen;
            skipHelpButton.Name = "Skip";
            skipHelpButton.Selected += skipHelpButton_Selected;
            skipHelpButton.Enabled = false;
            skipHelpButton.Visible = false;
            ControlManager.Add(skipHelpButton);

            endButton = new ImageButton(SpriteLoader.IosMainSheet, null, new Rectangle(704, 128, 150, 66), new Vector2(674, 326));
            endButton.DrawLayer = 0.98f;
            endButton.FocusColor = Color.LawnGreen;
            endButton.Name = "End";
            endButton.Selected += endButton_Selected;
            endButton.Visible = false;
            endButton.Enabled = false;
            ControlManager.Add(endButton);
        }


        void OnTapSelected(object sender, System.EventArgs e)
        {
            if (mainStages == Stages.Stage1)
            {
                currentShape = RotateMatrixRight(currentShape, currentShape.GetLength(0));

                nextImage = PieceImageFactory.BuildImage(currentShape);
            }
        }

		void OnPauseSelected(object sender, System.EventArgs e)
		{
            TotalMatches = GetTotalMatches();

			ScreenManager.GameReference.ScreenControl(ScreenName, "Pause");
            pauseButton.Enabled = false;
            pauseButtonSlideState = ButtonSlide.Closing;
		}

        void endButton_Selected(object sender, System.EventArgs e)
        {
            showHelp = false;
            nextHelpButton.Visible = false;
            nextHelpButton.Enabled = false;
            skipHelpButton.Visible = false;
            skipHelpButton.Enabled = false;
            endButton.Visible = false;
            endButton.Enabled = false;

            _buttonsVisisble = false;

			TempleMain.Player.TutorialPlayed = true;
            ScreenManager.GameReference.ScreenControl(this.ScreenName, "Save");
        }

        void skipHelpButton_Selected(object sender, System.EventArgs e)
        {
            showHelp = false;
            nextHelpButton.Visible = false;
            nextHelpButton.Enabled = false;
            skipHelpButton.Visible = false;
            skipHelpButton.Enabled = false;
            endButton.Visible = false;
            endButton.Enabled = false;

            _buttonsVisisble = false;
            tutorialTracker = 6;

			TempleMain.Player.TutorialPlayed = true;
            ScreenManager.GameReference.ScreenControl(this.ScreenName, "Save");
        }

        void nextHelpButton_Selected(object sender, System.EventArgs e)
        {
            showHelp = false;
            nextHelpButton.Visible = false;
            nextHelpButton.Enabled = false;
            skipHelpButton.Visible = false;
            skipHelpButton.Enabled = false;
            endButton.Visible = false;
            endButton.Enabled = false;

            _buttonsVisisble = false;

            tutorialTracker++;

            if (tutorialTracker == 6)
            {
				TempleMain.Player.TutorialPlayed = true;
                ScreenManager.GameReference.ScreenControl(this.ScreenName, "Save");
            }
        }

        void autoFillButton_Selected(object sender, System.EventArgs e)
		{
            _autoClicked = true;

            var t = 0;

			for(var x = 0; x < Width; x++)
				for(var y = 0; y < Height; y++)
				{
                    if(PlaceShape(new Point(x, y), out t))
					{
						currentPiece = nextPiece;

						nextPiece = currentPiecesUsed[currentPointer];
						currentPointer++;

						if(currentPointer == currentPiecesUsed.Length)
						{
							currentPointer = 0;
							currentPiecesUsed = currentPiecesUsed.Shuffle().ToArray();
						}

						currentShape = nextShape;
						nextShape = GamePieces[nextPiece].Shape;
						nextImage = PieceImageFactory.BuildImage(currentShape);

						nextFlower = GamePieces[nextPiece].FlowerType;
						nextColor = new ColorPair(GamePieces[nextPiece].Colors.Color1, GamePieces[nextPiece].Colors.Color2);
						currentFlower = GamePieces[currentPiece].FlowerType;
						currentColor = new ColorPair(GamePieces[currentPiece].Colors.Color1, GamePieces[currentPiece].Colors.Color2);
					}
				}

			autoFillButton.Visible = false;
			autoFillButton.Enabled = false;
		}

        public void  FinalizeGamePlay(User user)
        {

            var tttt = new byte[150];
            for (var o = 0; o < 150; o++)
                tttt[o] = 10;

            if (ScreenName == Screens.ClassicScreen)
            {
                for (var i = 0; i < BoardCells.Length; i++)
                {
                    if (BoardCells[i].Blocked)

                        tttt[i] = (byte)BoardCells[i].RockType;
                }
            }


            user.CurrentZenSeason = (byte)Season;
            user.CurrentZenYear = Year;
            user.ZenSavedScore = _zenHoldOverScore;
            user.BoardRocks = tttt;
            user.UsedPieceCount = UsedFlowerCounter;

            user.SavedZenPieces.Clear();

            for (var i = 0; i < GamePieces.Count; i++)
            {
                user.SavedZenPieces.Add(new PieceTemplateSimplified(GamePieces[i].Used, GamePieces[i].Unlocked));
            }

        }

        public void IntroduceSaveData(User user)
        {
            Year = user.CurrentZenYear;
            Season = (Seasons)user.CurrentZenSeason;
            Score = user.ZenSavedScore;
            _zenHoldOverScore = user.ZenSavedScore;

            for (var i = 0; i < GamePieces.Count; i++)
            {
                if (user.SavedZenPieces[i].Used)
                    GamePieces[i].Used = true;

                if (user.SavedZenPieces[i].Unlocked)
                    GamePieces[i].Unlocked = true;
            }

            UsedFlowerCounter = user.UsedPieceCount;

            for ( var j =0; j < BoardCells.Length; j++)
            {
                if (user.BoardRocks[j] != 10)
                {
                    NowIsBlock(j, user.BoardRocks[j]);

                }
            }

            currentPointer = 0;

            ChoosePieces();

            nextPiece = currentPiecesUsed[currentPointer];
            currentPointer++;
            currentPiece = currentPiecesUsed[currentPointer];
            currentPointer++;

            nextShape = GamePieces[nextPiece].Shape;
            currentShape = GamePieces[currentPiece].Shape;

            nextImage = PieceImageFactory.BuildImage(currentShape);

            nextFlower = GamePieces[nextPiece].FlowerType;
            nextColor = new ColorPair(GamePieces[nextPiece].Colors.Color1, GamePieces[nextPiece].Colors.Color2);
            currentFlower = GamePieces[currentPiece].FlowerType;
            currentColor = new ColorPair(GamePieces[currentPiece].Colors.Color1, GamePieces[currentPiece].Colors.Color2);

        }

        public int GetTotalMatches()
        {
            var matches = 0;

            for (var i = 0; i < UsedFlowerCounter.Length; i++)
                matches += UsedFlowerCounter[i];

            return matches;
        }

        private void CheckAutoClickCount()
        {
            _autoClicked = false;

            _lastNoAutoClick = 0;
        }

        private void CheckRockCount()
        {
            _rockWasCreated = false;

            _lastNoRockCount = 0;
        }
	}
}