using Microsoft.Xna.Framework;

namespace TempleGardens
{
    public class TitleBee
    {
        public Vector2 Position { get; private set; }
        private Vector2 wanderPoint;
        private float wanderTimer;
        private float wanderDuration;
        private bool reachedWanderPoint = false;
        private float midWanderTimer = 0;
        private float midWanderTimerDuration = 0.2f;
        public byte Flap { get; private set; }
        private float flapTimer;
        private static float flapTimerDuration = 0.1f;
        private Vector2 direction;

        private byte easeDirection;

        public bool AllStop { get; set; }

        private float speed;

        public TitleBee()
        {
            Position = new Vector2(MasterRandom.FRandom.Next(32, 1300), MasterRandom.FRandom.Next(32, 732));

            wanderPoint = new Vector2(MasterRandom.FRandom.Next(32, 1300), MasterRandom.FRandom.Next(32, 732));
            reachedWanderPoint = false;

            wanderDuration = MasterRandom.FRandom.Next(2, 10);

            speed = (float)MasterRandom.FRandom.Next(15, 44);

            direction = wanderPoint - Position;
            direction.Normalize();

            Flap = (byte)MasterRandom.FRandom.Next(0, 2);

            easeDirection = (byte)MasterRandom.FRandom.Next(0, 4);
        }

        public void Update(GameTime gameTime)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!AllStop)
            {
                wanderTimer += elapsed;
                flapTimer += elapsed;

                if (flapTimer >= flapTimerDuration)
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
                    wanderPoint = new Vector2(MasterRandom.FRandom.Next(32, 1300), MasterRandom.FRandom.Next(32, 732));
                    reachedWanderPoint = false;

                    direction = wanderPoint - Position;
                    direction.Normalize();

                    midWanderTimer = 0;
                    wanderDuration = MasterRandom.FRandom.Next(2, 10);

                    speed = (float)MasterRandom.FRandom.Next(15, 44);
                    easeDirection = (byte)MasterRandom.FRandom.Next(0, 4);
                }
                else
                {
                    if (!reachedWanderPoint)
                    {

                        if(easeDirection == 0)
                        {
                            wanderPoint = new Vector2(wanderPoint.X - 9.5f, wanderPoint.Y);

                            direction = wanderPoint - Position;
                            direction.Normalize();
                        }
                        else if (easeDirection == 1)
                        {
                            wanderPoint = new Vector2(wanderPoint.X, wanderPoint.Y + 1.5f);

                            direction = wanderPoint - Position;
                            direction.Normalize();
                        }
                        else if (easeDirection == 2)
                        {
                            wanderPoint = new Vector2(wanderPoint.X + 1.5f, wanderPoint.Y - 1.5f);

                            direction = wanderPoint - Position;
                            direction.Normalize();
                        }
                        else
                        {
                            wanderPoint = new Vector2(wanderPoint.X - 1.5f, wanderPoint.Y + 1.5f);

                            direction = wanderPoint - Position;
                            direction.Normalize();
                        }

                        Position += direction * speed * elapsed;

                        if (Position.X >= wanderPoint.X - 0.3f && Position.X < wanderPoint.X + 0.3f &&
                            Position.Y >= wanderPoint.Y - 0.3f && Position.Y < wanderPoint.Y + 0.3f)
                            reachedWanderPoint = true;
                    }
                    else
                    {
                        midWanderTimer += elapsed;

                        if (midWanderTimer >= midWanderTimerDuration)
                        {
                            midWanderTimer = 0;
                            wanderPoint = new Vector2(MasterRandom.FRandom.Next((int)wanderPoint.X - 3, (int)wanderPoint.X + 4), MasterRandom.FRandom.Next((int)wanderPoint.Y - 3, (int)wanderPoint.Y + 4));

                            direction = wanderPoint - Position;
                            direction.Normalize();
                        }
                        else
                        {
                            Position += direction * 1.8f * elapsed;
                        }
                    }
                }
            }
            else
            {
                Position = new Vector2(Position.X, Position.Y + 50 * 5 * elapsed);
            }


        }

    }
}
