
using Microsoft.Xna.Framework;

namespace TempleGardens
{
    public class Bee
    {
        public Vector2 Position { get; private set; }

		public static Vector2 Home = new Vector2(1033, 604);

        // small window
        private Vector2 wanderPoint;
        private float wanderTimer;
        private float wanderDuration = 3f;
        private bool reachedWanderPoint = false;
        private Vector2 direction;
        private float midWanderTimer = 0;
        private float midWanderDuration = 0.2f;
        public byte Flap { get; private set; }
        private float flapTimer;
        private static float flapDuration = 0.1f;

        // big window
        private float flapTimerActive;
        private static float flapDurationAcive = 0.1f;
        public byte ActiveFlap { get; private set; }
        public bool IsGoingRight { get; private set; }

        public int IndexOnBoard1 { get; set; }
        public int IndexOnBoard2 { get; set; }


        public Vector2 Destination1 { get { return new Vector2((DestPoint1.X + 1) * 64 + 32, (DestPoint1.Y + 1) * 64 + 32);  } }
        public Vector2 Destination2 { get { return new Vector2((DestPoint2.X + 1) * 64 + 32, (DestPoint2.Y + 1) * 64 + 32); } }
        public Point DestPoint1 { get; private set; }
        public Point DestPoint2 { get; private set; }

        private int currentPoint = 0;
        private bool reached = false;

        public bool FlowerOneGood { get; private set; }
        public bool FlowerTwoGood { get; private set; }

        public bool Active { get; set; }
        private bool ready = false;

        private static EffectsManager _effectManager;
        private static GamePlayScreen _gamePlayScreen;

        public Bee()
        {
            Position = new Vector2(MasterRandom.FRandom.Next(1033, 1214), MasterRandom.FRandom.Next(604, 703));

			wanderPoint = new Vector2(MasterRandom.FRandom.Next(1033, 1214), MasterRandom.FRandom.Next(604, 703));
            reachedWanderPoint = false;

            wanderDuration = (float)(MasterRandom.FRandom.NextDouble() + 3);

            direction = wanderPoint - Position;
            direction.Normalize();

            Flap = (byte)MasterRandom.FRandom.Next(0, 2);

            IndexOnBoard1 = 200;
            IndexOnBoard2 = 200;

            DestPoint1 = new Point(16, 16);
            DestPoint2 = new Point(16, 16);
        }

        public static void Init(EffectsManager effects, GamePlayScreen gameplay)
        {
            _effectManager = effects;
            _gamePlayScreen = gameplay;
        }


        public void Update(GameTime gameTime)
        {
            if (!Active)
            {
                var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                wanderTimer += elapsed;
                flapTimer += elapsed;

                if (flapTimer >= flapDuration)
                {
                    flapTimer = 0;
                    if (Flap == 0)
                        Flap = 1;
                    else
                        Flap = 0;
                }

                if (wanderTimer >= wanderDuration)
                {
                    wanderTimer = 0;
					wanderPoint = new Vector2(MasterRandom.FRandom.Next(1033, 1214), MasterRandom.FRandom.Next(604, 703));
                    reachedWanderPoint = false;

                    direction = wanderPoint - Position;
                    direction.Normalize();

                    midWanderTimer = 0;
                    wanderDuration = (float)(MasterRandom.FRandom.NextDouble() + 3);
                }
                else
                {
                    if (!reachedWanderPoint)
                    {
                        Position += direction * 25 * elapsed;

                        if (Position.X >= wanderPoint.X - 0.3f && Position.X < wanderPoint.X + 0.3f &&
                            Position.Y >= wanderPoint.Y - 0.3f && Position.Y < wanderPoint.Y + 0.3f)
                            reachedWanderPoint = true;
                    }
                    else
                    {
                        midWanderTimer += elapsed;

                        if (midWanderTimer >= midWanderDuration)
                        {
                            midWanderTimer = 0;
                            wanderPoint = new Vector2(MasterRandom.FRandom.Next((int)wanderPoint.X - 3, (int)wanderPoint.X + 4), MasterRandom.FRandom.Next((int)wanderPoint.Y - 3, (int)wanderPoint.Y + 4));

                            direction = wanderPoint - Position;
                            direction.Normalize();
                        }
                        else
                        {
                            Position += direction * 5 * elapsed;
                        }
                    }

                }

            }
            else
            {
                var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                flapTimerActive += elapsed;

                if (flapTimerActive >= flapDurationAcive)
                {
                    flapTimerActive = 0;
                    if (ActiveFlap == 0)
                        ActiveFlap = 1;
                    else if (ActiveFlap == 1)
                        ActiveFlap = 2;
                    else if (ActiveFlap == 2)
                        ActiveFlap = 0;
                }

                if (!ready)
                {
                    Position = Home;
                    ready = true;

                }
                else
                {
                    var futurePose = Vector2.Zero;

                    if (currentPoint == 0)
                    {
                        direction = Destination1 - Position;
                        direction.Normalize();
                        currentPoint = 1;
                    }
                    else if (currentPoint == 1)
                    {
                        if (!reached)
                        {

                            futurePose = Position + direction * 250 * elapsed;

                            if (futurePose.X > Position.X)
                                IsGoingRight = true;
                            else
                                IsGoingRight = false;

                            Position = futurePose;

                            if (Position.X >= Destination1.X - 7f && Position.X < Destination1.X + 7f &&
                                Position.Y >= Destination1.Y - 7f && Position.Y < Destination1.Y + 7f)
                                reached = true;
                        }
                        else
                        {
                            reached = false;
                            direction = Destination2 - Position;
                            direction.Normalize();
                            currentPoint++;
                            _effectManager.ActivateSpray(Destination1, _gamePlayScreen.BoardCells[IndexOnBoard1].CellColors);
                        }
                    }
                    else if (currentPoint == 2)
                    {
                        if (!reached)
                        {
                            futurePose = Position + direction * 250 * elapsed;

                            if (futurePose.X > Position.X)
                                IsGoingRight = true;
                            else
                                IsGoingRight = false;

                            Position = futurePose;

                            if (Position.X >= Destination2.X - 7f && Position.X < Destination2.X + 7f &&
                                Position.Y >= Destination2.Y - 7f && Position.Y < Destination2.Y + 7f)
                                reached = true;


                        }
                        else
                        {
                            reached = false;
                            direction = Home - Position;
                            direction.Normalize();
                            currentPoint++;
                            _effectManager.ActivateSpray(Destination2, _gamePlayScreen.BoardCells[IndexOnBoard2].CellColors);
                        }
                    }
                    else if (currentPoint == 3)
                    {
                        if (!reached)
                        {
                            futurePose = Position + direction * 250 * elapsed;

                            if (futurePose.X > Position.X)
                                IsGoingRight = true;
                            else
                                IsGoingRight = false;

                            Position = futurePose;

                            if (Position.X >= 1028)
                            {
                                reached = true;
								Position = new Vector2(1033, 604);
                                wanderTimer = 0;
								wanderPoint = new Vector2(MasterRandom.FRandom.Next(1033, 1214), MasterRandom.FRandom.Next(604, 703));
                                reachedWanderPoint = false;

                                direction = wanderPoint - Position;
                                direction.Normalize();

                                midWanderTimer = 0;
                                wanderDuration = (float)(MasterRandom.FRandom.NextDouble() + 3);
                            }
                        }
                        else
                        {
                            Active = false;

                        }
                    }

                }

            }

        }

        public void Reset()
        {
            DestPoint1 = new Point(16, 16);
            DestPoint2 = new Point(16, 16);
            IndexOnBoard1 = 200;
            IndexOnBoard2 = 200;
            reached = false;
            currentPoint = 0;
            ready = false;
            //GoodPoint1 = new Point(16, 16);
            //GoodPoint2 = new Point(16, 16);
            FlowerOneGood = false;
            FlowerTwoGood = false;
            wanderTimer = 0;
            midWanderTimer = 0;
        }

        public static Rectangle GetSource(byte index)
        {
            if (index == 0)
                return new Rectangle(384, 600, 7, 7);
            else
                return new Rectangle(391, 600, 7, 7);
        }

        public static Rectangle GetActiveSource(byte index)
        {
            if (index == 0)
                return new Rectangle(384, 576, 16, 16);
            else if (index == 1)
                return new Rectangle(400, 576, 16, 16);
            else
                return new Rectangle(416, 576, 16, 16);
        }

        public void SetDestiantions(int to, Point targetOrigin)
        {
            if (to == 1)
                DestPoint1 = targetOrigin;
            else
                DestPoint2 = targetOrigin;
        }
             

        public bool EvaluateBeeWork()
        {

            if (IndexOnBoard1 == 200 | IndexOnBoard2 == 200)
                return false;

            if (!_gamePlayScreen.BoardCells[IndexOnBoard1].DeFlowered && !_gamePlayScreen.BoardCells[IndexOnBoard2].DeFlowered)
            {
                if (_gamePlayScreen.BoardCells[IndexOnBoard1].PlantType == _gamePlayScreen.BoardCells[IndexOnBoard2].PlantType &&
                    _gamePlayScreen.BoardCells[IndexOnBoard1].CurrentID != _gamePlayScreen.BoardCells[IndexOnBoard2].CurrentID)
                {
                    FlowerOneGood = true;
                    FlowerTwoGood = true;
                }
                else
                {
                    FlowerOneGood = false;
                    FlowerTwoGood = false;
                }
            }
            else
                {
                    FlowerOneGood = false;
                    FlowerTwoGood = false;
                }

            if (FlowerOneGood && FlowerTwoGood)
                return true;
            else 
                return false;
        }
    }
}
