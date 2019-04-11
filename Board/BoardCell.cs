#region Using Statements
using Microsoft.Xna.Framework;
using System;
#endregion

namespace TempleGardens
{
    public enum CellMode : byte { Undefined = 0, None, Growing, Shadow, Flower, Weeding };
    public enum MateTypes : byte { Undefine = 0, None, Wind, Bee }; 

    public class BoardCell
    {
        public Rectangle Bounds { get; private set; }

        public ColorPair CellColors { get; private set; }
        public FlowerTypes PlantType { get; private set; }

        public byte CurrentNumber { get; private set; }
        public int CurrentID { get; private set; }

        public int Age { get; set; }
        public int MaxAge { get; private set; }
        public float Timer { get; private set; }
        private static float _timerDuration = 0.1f;

        public bool Ready { get; private set; }
        const float BeginScaling = 0.1f;
        const float FinalScaling = 1f;
        public float currentScale { get; private set; }
        private float flowerDrawTimer = 0f;
        private static float _flowerTimerDuration= 0.02f;
        private bool snitch = false;

        const float HoverScalngMax = 1.4f;

        public Point Mate { get; set; }
        public MateTypes MateType { get; set; }

        public bool DeFlowered { get; set; }

        public Point SourceReference { get; private set; }

        public CellMode Mode { get; set; }

        public float DrawLayer { get; private set; }
        private static float normalDrawLayer = 0.86f;
        private static float hoverDrawLayer = 0.90f;

        public float Rotation { get; private set; }
        private static float startRotation = MathHelper.ToRadians(90);

        public bool IsDark { get; private set; }

        public int MainAge { get; set; }
        public bool Blocked { get; set; }
        public int RockType { get; set; }
        public int BlockHP { get; set;}

        private static readonly Tuple<int, int>[] _neighbors = new[]
            {
                Tuple.Create(-1, 0),
                Tuple.Create(0, -1),
                Tuple.Create(1, 0),
                Tuple.Create(0, 1)
            };

        // A preset list that contains all possible outcomes of whether a square has a side or not.
        private static readonly Tuple<bool, bool, bool, bool>[] _sides = new[]
            {
              Tuple.Create(false,false,false,false), 
              Tuple.Create(false,true,false,false), 
              Tuple.Create(false,false,true,false),
              Tuple.Create(false,false,false,true),
              Tuple.Create(true,false,false,false), 
              Tuple.Create(false,true,true,false), 
              Tuple.Create(false,false,true,true),
              Tuple.Create(true,false,false,true), 
              Tuple.Create(true,true,false,false), 
              Tuple.Create(false,true,true,true),
              Tuple.Create(true,false,true,true),
              Tuple.Create(true,true,false,true),
              Tuple.Create(true,true,true,false),  
              Tuple.Create(true,true,true,true),
              Tuple.Create(true,false,true,false), 
              Tuple.Create(false,true,false,true) 
            };

        private static GamePlayScreen _gamePlayScreen;

        public BoardCell(int x, int y)
        {
            Bounds = new Rectangle(x * 64, y * 64, 64, 64);
            CellColors = new ColorPair(Color.White, Color.White);
            CurrentNumber = 0;
            CurrentID = 0;
            Age = 0;
            Timer = 0;
            PlantType = FlowerTypes.Undefined;

            SourceReference = new Point(1216, 1696);

            currentScale = BeginScaling;

            Ready = false;

            Mate = new Point(16, 16);
            MateType = MateTypes.None;
            DrawLayer = normalDrawLayer;

            MainAge = 0;
            Blocked = false;

            Rotation = startRotation;
        }

        public static void Init(GamePlayScreen gamePlay)
        {
            _gamePlayScreen = gamePlay;
        }

        public void FillCellData(byte number, int id, ColorPair colors, FlowerTypes flower, int max, bool dark)
        {
            CurrentNumber = number;
            CurrentID = id;
            Age = 0;
            CellColors = colors;
            PlantType = flower;

            IsDark = dark;

            MaxAge = max * 4;

            SourceReference = GetSource(CurrentNumber, Age, MaxAge, Bounds.X / 64 - 1, Bounds.Y / 64 - 1, CurrentID, IsDark);

            Mode = CellMode.Growing;

            MainAge = 0;
        }

        public void UpdateCell(float elapsed)
        {
            switch (Mode)
            {
                case CellMode.Growing:

                    if (Age > MaxAge || CurrentNumber == 0)
                        return;

                    Timer += elapsed;

                    if (Age <= MaxAge && Timer >= _timerDuration)
                    {
                        Age++;
                        Timer = 0;

                        SourceReference = GetSource(CurrentNumber, Age, MaxAge, Bounds.X / 64 - 1, Bounds.Y / 64 - 1, CurrentID, IsDark);
                    }

                    if (Age == MaxAge)
                        Mode = CellMode.Shadow;

                    break;
                case CellMode.Shadow:

                    flowerDrawTimer += elapsed;

                    if (flowerDrawTimer >= _flowerTimerDuration)
                    {
                        flowerDrawTimer = 0;

                        currentScale += 0.1f;

                        if (currentScale >= 0.3f)
                        {
                            Mode = CellMode.Flower;
                        }
                    }

                    break;
                case CellMode.Flower:

                    if (!Ready)
                    {
                        flowerDrawTimer += elapsed;

                        if (flowerDrawTimer >= _flowerTimerDuration)
                        {
                            flowerDrawTimer = 0;

                            if (!snitch)
                            {
                                currentScale += 0.1f;

                                Rotation -= MathHelper.ToRadians(10);

                                if (currentScale >= FinalScaling + 0.3f)
                                {
                                    snitch = true;

                                }
                            }
                            else
                            {
                                currentScale -= 0.1f;

                                if (currentScale <= FinalScaling)
                                {
                                    currentScale = FinalScaling;
                                    Ready = true;
                                    flowerDrawTimer = 0;

                                    Rotation += MathHelper.ToRadians(10);
                                }
                            }
                        }
                    }
                    else
                    {

                        if (Bounds.Contains(InputHandler.TouchVectorScaled.ToPoint()))
                            DrawLayer = hoverDrawLayer;
                        else
                            DrawLayer = normalDrawLayer;

                        if (Bounds.Contains(InputHandler.TouchVectorScaled.ToPoint()))
                        {
                            //DrawLayer = hoverDrawLayer;

                            flowerDrawTimer += elapsed;

                            if (flowerDrawTimer >= _flowerTimerDuration && currentScale <= HoverScalngMax)
                            {
                                flowerDrawTimer = 0;
                                currentScale += 0.1f;
                            }
                        }
                        else if (!Bounds.Contains(InputHandler.TouchVectorScaled.ToPoint()) && currentScale > FinalScaling)
                        {
                            //DrawLayer = normalDrawLayer;

                            flowerDrawTimer += elapsed;

                            if (flowerDrawTimer >= _flowerTimerDuration && currentScale != FinalScaling)
                            {
                                flowerDrawTimer = 0;
                                currentScale -= 0.1f;
                            }
                        }
                    }
                    break;
                case CellMode.Weeding:



                    break;
                case CellMode.None:
                default:

                    break;
            }

        }


        public void ClearCellData()
        {
            CurrentNumber = 0;
            CurrentID = 0;
            Age = 0;
            CellColors = new ColorPair(Color.White, Color.White);
            Timer = 0;
            PlantType = FlowerTypes.Undefined;
            SourceReference = new Point(1216, 1696);
            Ready = false;
            Mate = new Point(16, 16);
            currentScale = BeginScaling;
            Mode = CellMode.None;
            DeFlowered = false;
            snitch = false;
            flowerDrawTimer = 0;
            MainAge = 0;

            Rotation = startRotation;
        }

        public void ClearBlock()
        {
            Blocked = false;
        }

        public void ClearFlower()
        {
            DeFlowered = true;
        }

        public void ReevaluateSource()
        {

            if (CurrentNumber > 1)
            {
                CurrentNumber = 1;

                SourceReference = GetSource(CurrentNumber, Age, MaxAge, Bounds.X / 64 - 1, Bounds.Y / 64 - 1, CurrentID, IsDark);
            }
        }

        public static Point GetSource(byte current, int age, int max, int x, int y, int id, bool isDark)
        {
            bool[] cddd = new bool[4];

            if (current == 1)
            {
                if (age < 1)
                    return new Point(768, 736);
                if (age >= 1)
                {
                    for (var i = 0; i < _neighbors.Length; i++)
                    {
                        var nX = x + _neighbors[i].Item1;
                        var nY = y + _neighbors[i].Item2;

                        if (!GamePlayScreen.InBounds(nX, nY))
                            cddd[i] = false;
                        else
                        {
                            if (id == _gamePlayScreen.BoardCells[nY * GamePlayScreen.Width + nX].CurrentID)
                                cddd[i] = true;
                            else
                                cddd[i] = false;
                        }
                    }

                    var index = 0;

                    for (var i = 0; i < _sides.Length; i++)
                    {
                        if (cddd[0] == _sides[i].Item1 &&
                            cddd[1] == _sides[i].Item2 &&
                            cddd[2] == _sides[i].Item3 &&
                            cddd[3] == _sides[i].Item4)
                            index = i;
                    }

                    var pointy = new Point();

                    return pointy = GetIndexStart(index, age, isDark);
                }
            }
            else
            {
                if (age < (current - 1) * 4)
                    return new Point(1216, 1696);
                else
                {
                    bool[] tips = new bool[4];

                    for (var i = 0; i < _neighbors.Length; i++)
                    {
                        var nX = x + _neighbors[i].Item1;
                        var nY = y + _neighbors[i].Item2;

                        if (!GamePlayScreen.InBounds(nX, nY))
                            cddd[i] = false;
                        else
                        {
                            if (_gamePlayScreen.BoardCells[nY * GamePlayScreen.Width + nX].CurrentID != id)
                            {
                                cddd[i] = false;
                                tips[i] = false;
                            }
                            else
                            {
                                if (current < _gamePlayScreen.BoardCells[nY * GamePlayScreen.Width + nX].CurrentNumber)
                                {
                                    cddd[i] = false;
                                    tips[i] = false;
                                }
                                else
                                {
                                    cddd[i] = true;
                                    tips[i] = true;
                                }

                                if (age >= (current - 1) * 4 + 4)
                                    cddd[i] = true;

                            }


                        }
                    }
                    var index = 0;
                    var plopper = 0;
                    for (var i = 0; i < _sides.Length; i++)
                    {
                        if (cddd[0] == _sides[i].Item1 &&
                            cddd[1] == _sides[i].Item2 &&
                            cddd[2] == _sides[i].Item3 &&
                            cddd[3] == _sides[i].Item4)
                            index = i;
                    }

                    for (var i = 0; i < 4; i++)
                    {
                        if (tips[i])
                        {
                            plopper = i;
                        }
                    }

                    var pointy = new Point();

                    return pointy = GetIndexEnd(index, age, plopper, current, isDark);
                }
            }

            return Point.Zero;
        }

        private static Point GetIndexStart(int index, int age, bool isDark)
        {
            if (age > 3)
                age = 3;

            if (age == 0)
            {
                if (isDark)
                    return new Point(768, 736);
                else
                    return new Point(768, 1056);
            }
            else if (age == 1)
            {
                if (isDark)
                    return new Point(832, 736);
                else
                    return new Point(832, 1056);
            }
            else if (age == 2)
            {
                if (isDark)
                    return new Point(896, 736);
                else
                    return new Point(896, 1056);
            }
            else if (age == 3)
            {
                switch (index)
                {
                    case 0:
                        if (isDark)
                            return new Point(960, 736);
                        else
                            return new Point(960, 1056);
                    case 1:
                        if (isDark)
                            return new Point(768, 800);
                        else
                            return new Point(768, 1120);
                    case 2:
                        if (isDark)
                            return new Point(832, 800);
                        else
                            return new Point(832, 1120);
                    case 3:
                        if (isDark)
                            return new Point(896, 800);
                        else
                            return new Point(896, 1120);
                    case 4:
                        if (isDark)
                            return new Point(960, 800);
                        else
                            return new Point(960, 1120);
                    case 5:
                        if (isDark)
                            return new Point(768, 864);
                        else
                            return new Point(768, 1184);
                    case 6:
                        if (isDark)
                            return new Point(832, 864);
                        else
                            return new Point(832, 1184);
                    case 7:
                        if (isDark)
                            return new Point(896, 864);
                        else
                            return new Point(896, 1184);
                    case 8:
                        if (isDark)
                            return new Point(960, 864);
                        else
                            return new Point(960, 1184);
                    case 9:
                        if (isDark)
                            return new Point(768, 928);
                        else
                            return new Point(768, 1248);
                    case 10:
                        if (isDark)
                            return new Point(832, 928);
                        else
                            return new Point(832, 1248);
                    case 11:
                        if (isDark)
                            return new Point(896, 928);
                        else
                            return new Point(896, 1248);
                    case 12:
                        if (isDark)
                            return new Point(960, 928);
                        else
                            return new Point(960, 1248);
                    case 13:
                        if (isDark)
                            return new Point(768, 992);
                        else
                            return new Point(768, 1312);
                    case 14:
                        if (isDark)
                            return new Point(832, 992);
                        else
                            return new Point(832, 1312);
                    case 15:
                    default:
                        if (isDark)
                            return new Point(896, 992);
                        else
                            return new Point(896, 1312);
                }
            }
            else
                return new Point(192, 256);



            //switch (index)
            //{
            //    case 0:
            //        return new Point(64 * age, 0);
            //    case 1:
            //        return new Point(64 * age, 64);
            //    case 2:
            //        return new Point(64 * age, 128);
            //    case 3:
            //        return new Point(64 * age, 192);
            //    case 4:
            //        return new Point(64 * age, 256);
            //    case 5:
            //        return new Point(64 * age, 320);
            //    case 6:
            //        return new Point(64 * age, 384);
            //    case 7:
            //        return new Point(64 * age, 448);
            //    case 8:
            //        return new Point(64 * age, 512);
            //    case 9:
            //        return new Point(64 * age, 576);
            //    case 10:
            //        return new Point(64 * age, 640);
            //    case 11:
            //        return new Point(64 * age, 704);
            //    case 12:
            //        return new Point(64 * age, 768);
            //    case 13:
            //        return new Point(64 * age, 832);
            //    case 14:
            //        return new Point(64 * age, 896);
            //    case 15:
            //        return new Point(64 * age, 960);
            //    default:
            //        return new Point();
            //}
        }

        private static Point GetIndexEnd(int index, int age, int plop, int current, bool isDark)
        {
            if (age < (current - 1) * 4 + 4)
            {
                if (plop == 0)
                {
                    if (age >= (current - 1) * 4 && age < (current - 1) * 4 + 1)
                        if (isDark)
                            return new Point(1024, 992);
                        else
                            return new Point(1024, 1504);
                    else if (age >= (current - 1) * 4 + 1 && age < (current - 1) * 4 + 2)
                        if (isDark)
                            return new Point(1088, 992);
                        else
                            return new Point(1088, 1504);
                    else if (age >= (current - 1) * 4 + 2 && age < (current - 1) * 4 + 3)
                        if (isDark)
                            return new Point(1152, 992);
                        else
                            return new Point(1152, 1504);
                    else if (age >= (current - 1) * 4 + 3 && age < (current - 1) * 4 + 4)
                        if (isDark)
                            return new Point(1216, 992);
                        else
                            return new Point(1216, 1504);
                }
                else if (plop == 1)
                {
                    if (age >= (current - 1) * 4 && age < (current - 1) * 4 + 1)
                        if (isDark)
                            return new Point(1024, 800);
                        else
                            return new Point(1024, 1312);
                    else if (age >= (current - 1) * 4 + 1 && age < (current - 1) * 4 + 2)
                        if (isDark)
                            return new Point(1088, 800);
                        else
                            return new Point(1088, 1312);
                    else if (age >= (current - 1) * 4 + 2 && age < (current - 1) * 4 + 3)
                        if (isDark)
                            return new Point(1152, 800);
                        else
                            return new Point(1152, 1312);
                    else if (age >= (current - 1) * 4 + 3 && age < (current - 1) * 4 + 4)
                        if (isDark)
                            return new Point(1216, 800);
                        else
                            return new Point(1216, 1312);
                }
                else if (plop == 2)
                {
                    if (age >= (current - 1) * 4 && age < (current - 1) * 4 + 1)
                        if (isDark)
                            return new Point(1024, 864);
                        else
                            return new Point(1024, 1376);
                    else if (age >= (current - 1) * 4 + 1 && age < (current - 1) * 4 + 2)
                        if (isDark)
                            return new Point(1088, 864);
                        else
                            return new Point(1088, 1376);
                    else if (age >= (current - 1) * 4 + 2 && age < (current - 1) * 4 + 3)
                        if (isDark)
                            return new Point(1152, 864);
                        else
                            return new Point(1152, 1376);
                    else if (age >= (current - 1) * 4 + 3 && age < (current - 1) * 4 + 4)
                        if (isDark)
                            return new Point(1216, 864);
                        else
                            return new Point(1216, 1376);
                }
                else
                {
                    if (age >= (current - 1) * 4 && age < (current - 1) * 4 + 1)
                        if (isDark)
                            return new Point(1024, 928);
                        else
                            return new Point(1024, 1440);
                    else if (age >= (current - 1) * 4 + 1 && age < (current - 1) * 4 + 2)
                        if (isDark)
                            return new Point(1088, 928);
                        else
                            return new Point(1088, 1440);
                    else if (age >= (current - 1) * 4 + 2 && age < (current - 1) * 4 + 3)
                        if (isDark)
                            return new Point(1152, 928);
                        else
                            return new Point(1152, 1440);
                    else if (age >= (current - 1) * 4 + 3 && age < (current - 1) * 4 + 4)
                        if (isDark)
                            return new Point(1216, 928);
                        else
                            return new Point(1216, 1440);
                }

            }

            switch (index)
            {
                case 0:
                    return new Point(1216, 1696);
                case 1:
                    if (isDark)
                        return new Point(1024, 736);
                    else
                        return new Point(1024, 1248);
                case 2:
                    if (isDark)
                        return new Point(1088, 736);
                    else
                        return new Point(1088, 1248);
                case 3:
                    if (isDark)
                        return new Point(1152, 736);
                    else
                        return new Point(1152, 1248);
                case 4:
                    if (isDark)
                        return new Point(1216, 736);
                    else
                        return new Point(1216, 1248);
                case 5:
                    if (isDark)
                        return new Point(1024, 1056);
                    else
                        return new Point(1024, 1568);
                case 6:
                    if (isDark)
                        return new Point(1088, 1056);
                    else
                        return new Point(1088, 1568);
                case 7:
                    if (isDark)
                        return new Point(1152, 1056);
                    else
                        return new Point(1152, 1568);
                case 8:
                    if (isDark)
                        return new Point(1216, 1056);
                    else
                        return new Point(1216, 1568);
                case 9:
                    if (isDark)
                        return new Point(1024, 1120);
                    else
                        return new Point(1024, 1632);
                case 10:
                    if (isDark)
                        return new Point(1088, 1120);
                    else
                        return new Point(1088, 1632);
                case 11:
                    if (isDark)
                        return new Point(1152, 1120);
                    else
                        return new Point(1152, 1632);
                case 12:
                    if (isDark)
                        return new Point(1216, 1120);
                    else
                        return new Point(1216, 1632);
                case 13:
                    if (isDark)
                        return new Point(1024, 1184);
                    else
                        return new Point(1024, 1696); 
                case 14:
                    if (isDark)
                        return new Point(1088, 1184);
                    else
                        return new Point(1088, 1696);
                case 15:
                    if (isDark)
                        return new Point(1152, 1184);
                    else
                        return new Point(1152, 1696);
                default:
                    return new Point(1216, 1696);
            }
        }
    }
}
