using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TempleGardens
{
    public class TitleScreen : GameState
    {
        public byte[] Tiling { get; private set; }

        private bool titleMoveDone = false;

        private float timer = 0;
        private float timerDuration = 0.01f;

        private float textInitTimer = 0;
        private float textInitTimerDuration = 1f;

        private Vector2 titleTextPosition = new Vector2(350, -400);

        bool bounceUp = false;

        List<FlowersTitle> flowerPool = new List<FlowersTitle>();

        private float newFlowerTimer;
        private float newFlowerTimerDuration = 0.4f;
        private int maxFlowers = 10;

        bool shiftMode = false;
        bool startingNextScreen = false;

        Vector2 helperNode = new Vector2(0, 64);

        private bool isNewUser;

        List<TitleBee> bees;

        private float initTimer;
        private bool ready = false;

        public TitleScreen(GameStateManager manager, bool newUser, Screens screenName)
            : base(manager, screenName)
        {
            Tiling = new byte[20 * 12];

            for (var i = 0; i < Tiling.Length; i++)
                Tiling[i] = (byte)MasterRandom.FRandom.Next(0, 9);

            for (var i = 0; i < maxFlowers; i++)
                flowerPool.Add(new FlowersTitle());

            bees = new List<TitleBee>();
            for (var i = 0; i < 25; i++)
                bees.Add(new TitleBee());

            isNewUser = newUser;

//            SoundBoard.MusicControl("StartProgram");
        }

        public override void Update(GameTime gameTime)
        {

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (ready)
            {
                newFlowerTimer += elapsed;
                textInitTimer += elapsed;

                if (!shiftMode)
                {
                    if (newFlowerTimer >= newFlowerTimerDuration)
                    {
                        newFlowerTimer = 0;
                        if (flowerPool.Any(r => !r.Active))
                        {
                            var pos = Vector2.Zero;

                            do
                            {
                                pos = new Vector2(MasterRandom.FRandom.Next(32, 1248), MasterRandom.FRandom.Next(32, 688));
                            }
                            while (flowerPool.Any(t => t.Position.X >= (pos.X - 64) && t.Position.X < (pos.X + 64) &&
                                       t.Position.Y >= (pos.Y - 64) && t.Position.Y < (pos.Y + 64)));

                            flowerPool.First(t => !t.Active).ActivateFlower(pos);
                        }
                    }

                    if (textInitTimer >= textInitTimerDuration)
                    {

                        timer += elapsed;
                        if (timer >= timerDuration && !titleMoveDone)
                        {
                            timer = 0;

                            if (!bounceUp)
                            {
                                titleTextPosition = new Vector2(titleTextPosition.X, titleTextPosition.Y + 7);

                                if (titleTextPosition.Y >= 248)
                                    bounceUp = true;
                            }
                            else
                            {
                                titleTextPosition = new Vector2(titleTextPosition.X, titleTextPosition.Y - 3);

                                if (titleTextPosition.Y <= 200)
                                    titleMoveDone = true;
                            }

                        }
                    }


                    foreach (var flower in flowerPool)
                    {
                        if (flower.Active)
                        {
                            flower.Update(gameTime);
                        }
                    }

                    foreach (var bee in bees)
                        bee.Update(gameTime);

                    if (InputHandler.FingerRaised() && titleMoveDone)
                    {
                        shiftMode = true;

                        for (var i = 0; i < flowerPool.Count; i++)
                            flowerPool[i].AllStop = true;

                        for (var i = 0; i < bees.Count; i++)
                            bees[i].AllStop = true;


                        ScreenManager.GameReference.ScreenControl(ScreenName, "NewUser");
                    }

                    if (InputHandler.FingerRaised() && titleMoveDone)
                    {
                        shiftMode = true;

                        for (var i = 0; i < flowerPool.Count; i++)
                            flowerPool[i].AllStop = true;

                        for (var i = 0; i < bees.Count; i++)
                            bees[i].AllStop = true;

                        if (isNewUser)
                            ScreenManager.GameReference.ScreenControl(ScreenName, "NewUser");
                    }


                }
                else
                {
                    if (timer >= timerDuration)
                    {
                        timerDuration = 0;

                        foreach (var flower in flowerPool)
                        {
                            if (flower.Active)
                            {
                                flower.Update(gameTime);
                            }
                        }

                        foreach (var bee in bees)
                            bee.Update(gameTime);

                        helperNode = new Vector2(helperNode.X, helperNode.Y + 55 * 5 * elapsed);

                        titleTextPosition = new Vector2(titleTextPosition.X, titleTextPosition.Y + 65 * 5 * elapsed);

                    }

                    if (helperNode.Y >= 256 && !startingNextScreen)
                    {

                        ScreenManager.GameReference.ScreenControl(ScreenName, "Begin");
                        Tiling = null;
                        startingNextScreen = true;
                    }

                    if (helperNode.Y >= 768)
                    {
                        ScreenManager.GameReference.ScreenControl(ScreenName, "Next");
                    }

                }


            }
            else
            {
                initTimer += elapsed;

                if (initTimer > 5)
                    ready = true;
            }
            ControlManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ready)
            {
                if (Tiling != null)
                {
                    for (var x = 0; x < 20; x++)
                        for (var y = 0; y < 12; y++)
                        {
                            spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(x * 64, y * 64), MainMenuScreen.GetTile(Tiling[y * 20 + x], 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.45f);

                        }
                }

                spriteBatch.Draw(SpriteLoader.IosMainSheet, titleTextPosition, new Rectangle(1280, 0, 672, 280), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.65f);
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(titleTextPosition.X + 32, titleTextPosition.Y + 32), new Rectangle(1280, 320, 672, 280), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.64f);


                foreach (var flower in flowerPool)
                {
                    if (flower.Active)
                    {
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, flower.Position, GetFlowerSource(flower.FlowerType, 1), flower.FlowerColor, 0f, FlowersTitle.Origin, flower.Scale, SpriteEffects.None, 0.62f);
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, flower.Position, GetFlowerSource(flower.FlowerType, 2), flower.FlowerColor, 0f, FlowersTitle.Origin, flower.Scale, SpriteEffects.None, 0.62f);
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(flower.Position.X + 6, flower.Position.Y + 6), GetFlowerSource(flower.FlowerType, 3), Color.White, 0f, FlowersTitle.Origin, flower.Scale, SpriteEffects.None, 0.61f);
                    }
                }

                foreach (var bee in bees)
                {
                    spriteBatch.Draw(SpriteLoader.IosMainSheet, bee.Position, Bee.GetSource(bee.Flap), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.63f);
                }

            }
            else
            {
                spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(368, 200), new Rectangle(0, 1664, 544, 320), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.44f);
            }
            ControlManager.Draw(spriteBatch);
        }

        public static Rectangle GetFlowerSource(FlowerTypes flower, byte flag)
        {
            var startX = 0;
            var startY = 0;

            if (flag == 0)
            {
                startX = 1280;
                startY = 896;
            }
            else if (flag == 1)
            {
                startX = 1280;
                startY = 1280;
            }
            else if (flag == 2)
            {
                startX = 1280;
                startY = 1472;
            }
            else if (flag == 3)
            {
                startX = 1280;
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
                    return new Rectangle(startX + 128, startY + 128, 64, 64);
                default:
                    return new Rectangle(0, 0, 0, 0);
            }
        }

        

    }
}
