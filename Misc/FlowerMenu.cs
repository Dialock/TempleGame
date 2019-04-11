
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TempleGardens
{
    public class FlowerMenu
    {
        public Vector2 Position { get; private set; }
        public float Speed { get; private set; }
        public float SpinSpeed { get; private set; }
        public float Rotation { get; private set; }

        public bool Active { get; private set; }

        private static Vector2 origin = new Vector2(32, 32);

        private float timer;
        private static float timerDuration = 0.02f;

        private FlowerTypes flowerType;

        private float[] chooseMe = new float[]
        {
            0.01f,
            -0.01f,
            0.05f,
            0.07f,
            -0.05f,
            -0.03f,
            0.09f,
            -0.09f
        };

        public FlowerMenu()
        {
            Position = new Vector2(-16, -16);
            Speed = 88;

        }

        public void PrepFlower()
        {
            Position = new Vector2(MasterRandom.FRandom.Next(32, 1330), -32);
            Speed = MasterRandom.FRandom.Next(6, 15);
            SpinSpeed = chooseMe[MasterRandom.FRandom.Next(0, chooseMe.Length)];
            flowerType = PieceTemplate.Flowers[MasterRandom.FRandom.Next(0, PieceTemplate.Flowers.Length)];
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timer += elapsed;

            if (timer >= timerDuration)
            {
                timer = 0;

                Position = new Vector2(Position.X, Position.Y + Speed);

                Rotation += SpinSpeed;
            }

            if (Position.Y > 770)
                Active = false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            spriteBatch.Draw(SpriteLoader.IosMainSheet, Position, TitleScreen.GetFlowerSource(flowerType, 3), Color.White,
                Rotation, origin, 1f, SpriteEffects.None, 0.86f);
        }
    }
}
