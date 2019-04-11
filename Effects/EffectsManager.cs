using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TempleGardens
{
    public class EffectsManager
    {
        private List<BurstEffect> activeBursts = new List<BurstEffect>();
        private List<SprayEffect> activeSprays = new List<SprayEffect>();
        public List<Absorb> ActiveAbsorbs { get; private set; }
        public List<BlockMoving> ActiveBlocks { get; private set; }
        public List<ScoreEffect> ActiveScores { get; private set; }

      
        private static GamePlayScreen _gamePlayScreen;

        public EffectsManager()
        {
            for (var i =0; i < 4; i++)
                activeSprays.Add(new SprayEffect());

            ActiveAbsorbs = new List<Absorb>();
            ActiveBlocks = new List<BlockMoving>();
            ActiveScores = new List<ScoreEffect>();
        }

        public static void Init(GamePlayScreen gamePlayScreen)
        {
                _gamePlayScreen = gamePlayScreen;
        }

        /// <summary>
        /// Adds the score effect
        /// </summary>
        /// <param name="startPos">Score's initial position</param>
        /// <param name="score">the score to display</param>
        public void AddScore(Vector2 startPos, int score)
        {
            ActiveScores.Add(new ScoreEffect(score, startPos));
        }

        /// <summary>
        /// Adds the block effect.  Used to make tile appear to move on board
        /// </summary>
        /// <param name="startPos">Blocks initial position</param>
        /// <param name="targetPos">Blocks destination</param>
        /// <param name="lastElement">The id of the final boardcell to be occupied</param>
        public void AddBlock(Vector2 startPos, Vector2 targetPos, int lastElement, int chosenBlock)
        {
            ActiveBlocks.Add(new BlockMoving(startPos, targetPos, lastElement, chosenBlock));
        }

        /// <summary>
        /// Adds the spray effect to pointer when a flower is touched
        /// </summary>
        /// <param name="pos">effects start position</param>
        /// <param name="colors">colors used in effect</param>
        public void ActivateSpray(Vector2 pos, ColorPair colors)
        {
            for (var i = 0; i < activeSprays.Count; i++)
            {
                if (!activeSprays[i].Active)
                {
                    activeSprays[i].SetData(pos, colors);
                    break;
                }
            }
        }

        public void AddNewBurst( Vector2 position, ColorPair colors)
        {
            activeBursts.Add(new BurstEffect(colors, position));
        }

        public void CreateAbsorbEffect(Vector2 position)
        {
            ActiveAbsorbs.Add(new Absorb(position, 64));
        }

        public void Update(GameTime gameTime)
        {

            foreach (var effect in activeSprays)
            {
                effect.Update(gameTime);
            }

            for (int i = activeBursts.Count - 1; i >= 0; i--)
            {
                activeBursts[i].Update(gameTime);

                if (activeBursts[i].Done)
                    activeBursts.Remove(activeBursts[i]);
            }

            for (int i = ActiveAbsorbs.Count - 1; i >= 0; i--)
            {
                ActiveAbsorbs[i].Update(gameTime);

                if (ActiveAbsorbs[i].Done)
                    ActiveAbsorbs.Remove(ActiveAbsorbs[i]);
            }

            for (int i = ActiveBlocks.Count - 1; i >= 0; i--)
            {
                ActiveBlocks[i].Update(gameTime);

                if (ActiveBlocks[i].Done)
                {
                    _gamePlayScreen.NowIsBlock(ActiveBlocks[i].LastElement, ActiveBlocks[i].ChoosenBlock);
                    ActiveBlocks.Remove(ActiveBlocks[i]);
                }
            }
        
            for (int i = ActiveScores.Count - 1; i >= 0; i--)
            {
                ActiveScores[i].Update(gameTime);

                if (ActiveScores[i].Done)
                {
                    ActiveScores.Remove(ActiveScores[i]);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var effect in activeBursts)
            {
                for (var i = 0; i < BurstEffect.MaxBursts; i++)
                {
                    if (effect.isColor1[i])
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, effect.BurstPositions[i],
                            new Rectangle(864, 640, 3, 3), effect.ColorUsed.Color1,
                            0f, Vector2.Zero, 1f, SpriteEffects.None, 0.90f);
                    else
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, effect.BurstPositions[i],
                            new Rectangle(864, 640, 2, 2), effect.ColorUsed.Color2,
                            0f, Vector2.Zero, 1f, SpriteEffects.None, 0.90f);
                }
            }

            foreach (var effect in activeSprays)
            {
                if (effect.Active)
                {
                    for (var i = 0; i < effect.SprayBitsPositions.Length; i++)
                    {
                        if (effect.IsBigger[i])
                            spriteBatch.Draw(SpriteLoader.IosMainSheet, effect.SprayBitsPositions[i], new Rectangle(864, 640, 4, 4), effect.SprayColor[i], 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.90f);
                        else
                            spriteBatch.Draw(SpriteLoader.IosMainSheet, effect.SprayBitsPositions[i], new Rectangle(864, 640, 2, 3), effect.SprayColor[i], 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.90f);
                    }
                }
            }

            foreach (var effect in ActiveAbsorbs)
            {
                for (var i = 0; i < effect.ShardPositions.Length; i++)
                {
                    if (effect.useColor1[i])
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, effect.ShardPositions[i], new Rectangle(864, 640, 4, 4), Absorb.color1, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.90f);
                    else
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, effect.ShardPositions[i], new Rectangle(864, 640, 2, 2), Absorb.color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.90f);
                }
            }

            foreach (var effect in ActiveBlocks)
            {
                spriteBatch.Draw(SpriteLoader.IosMainSheet, effect.DrawPosition, GamePlayScreen.GetRockSource(effect.ChoosenBlock), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
            }

            foreach (var effect in ActiveScores)
            {
                    for (var i = 0; i < effect.GlyphRectangles.Length; i++)
                        spriteBatch.Draw(SpriteLoader.IosMainSheet, new Vector2(effect.GlyphPostion.X + (i * 16), effect.GlyphPostion.Y), effect.GlyphRectangles[i], effect.FontColor, 0f,
                            Vector2.Zero, 1f, SpriteEffects.None, 0.96f);
            }

        }
    }
}
