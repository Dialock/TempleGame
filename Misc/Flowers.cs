#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace TempleGardens
{
    public class Flowers
    {
        public FlowerTypes FlowerType { get; private set; }
        public Color FlowerColor { get; private set; }

        public Vector2 Position { get; private set; }

        public static Vector2 Origin = new Vector2(32, 32);

        public float Scale { get; private set; }
        private static float beginScale = 0.1f;
        private static float highScale = 1.4f;
        private static float finalScale = 1f;

        private bool atPeak = false;
        private bool ready = false;

        public bool Active { get; set; }
        public bool Leave { get; set; }

        private float growTimer;
        private static float growTimerDuration = 0.02f;

        private float maxLifeTimer;
        private static float maxLifeTimerDuration = 5f;

        public bool AllStop { get; set; }

        public Flowers()
        {
            FlowerColor = PieceTemplate.ColorFamilies[0];
            FlowerType = PieceTemplate.Flowers[0];
            Position = new Vector2(-200, 200);
            Scale = beginScale;
            Active = false;
        }

        public void ActivateFlower(Vector2 newPos)
        {
            Position = newPos;
            Active = true;
            ready = false;
			FlowerColor = PieceTemplate.ColorFamilies[MasterRandom.FRandom.Next(0, PieceTemplate.ColorFamilies.Count)];
            FlowerType = PieceTemplate.Flowers[MasterRandom.FRandom.Next(0, PieceTemplate.Flowers.Length)];
        }

        public void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!AllStop)
            {
                maxLifeTimer += elapsed;

                if (maxLifeTimer >= maxLifeTimerDuration && !Leave)
                    Leave = true;

                if (!ready)
                {

                    growTimer += elapsed;

                    if (growTimer >= growTimerDuration)
                    {
                        growTimer = 0;

                        if (!atPeak)
                        {
                            Scale += 0.1f;
                            if (Scale >= highScale)
                                atPeak = true;
                        }
                        else
                        {
                            Scale -= 0.1f;

                            if (Scale <= finalScale)
                            {
                                Scale = finalScale;
                                ready = true;
                                atPeak = false;
                            }
                        }
                    }
                }

                if (ready && Leave)
                {
                    growTimer += elapsed;

                    if (growTimer >= growTimerDuration)
                    {
                        growTimer = 0;

                        Scale -= 0.1f;

                        if (Scale <= beginScale)
                        {
                            Scale = beginScale;
                            Active = false;
                            Leave = false;
                            maxLifeTimer = 0;
                        }
                    }
                }

            }
            else
            {
                Position = new Vector2(Position.X, Position.Y + 55 * 5 * elapsed);
            }

        }
    }
}
