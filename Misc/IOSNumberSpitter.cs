#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TempleGardens
{
    public class IOSNumberSpitter
    {
        public List<Rectangle> YearList { get; private set; }
        private int _lastYear;

        public List<Rectangle> ScoreList { get; private set; }
        private int _lastScore;

        public List<Rectangle> DrainScoreList { get; private set; }
        private int _lastDrain;

        public List<Rectangle> ToLevelScoreList { get; private set; }
        private int _lastLevel;

        public List<Rectangle> MatchesList { get; private set; }
        private int _lastMatches;

        public List<Rectangle> SeasonsList { get; private set; }
        private int _lastSeason;

        public List<Rectangle> RankList { get; private set; }
        private int _lastRank;

        private Screens currentScreen;

        public IOSNumberSpitter() { }

        public IOSNumberSpitter(Screens screen)
        {
            currentScreen = screen;

            if (screen == Screens.ClassicScreen)
            {
                _lastYear = -1;
                YearList = new List<Rectangle>();
                _lastScore = -1;
                ScoreList = new List<Rectangle>();
            }
            else
            {
                DrainScoreList = new List<Rectangle>();
                _lastDrain = -1;
                ToLevelScoreList = new List<Rectangle>();
                _lastLevel = -1;
                MatchesList = new List<Rectangle>();
                _lastMatches = -1;
                SeasonsList = new List<Rectangle>();
                _lastSeason = -1;
                RankList = new List<Rectangle>();
                _lastRank = -1;
            }
        }

        public void Update(int year, int score)
        {
            if (year != _lastYear)
            {
                _lastYear = year;

                YearList.Clear();
                YearList = UpdateNumberList(year.ToString().ToCharArray());
            }

            if (score != _lastScore)
            {
                _lastScore = score;

                ScoreList.Clear();
                ScoreList = UpdateNumberList(score.ToString().ToCharArray());
            }
        }

        public void Update(int drainscore, int toLevelScore, int matches, int seasons, int rank)
        {
            if (toLevelScore != _lastLevel)
            {
                _lastLevel = toLevelScore;

                ToLevelScoreList.Clear();
                ToLevelScoreList = UpdateNumberList(toLevelScore.ToString().ToCharArray());
            }

            if (drainscore != _lastDrain)
            {
                _lastDrain = drainscore;

                DrainScoreList.Clear();
                DrainScoreList = UpdateNumberList(drainscore.ToString().ToCharArray());
            }

            if (seasons != _lastSeason)
            {
                _lastSeason = seasons;

                SeasonsList.Clear();
                SeasonsList = UpdateNumberList(seasons.ToString().ToCharArray());
            }

            if (matches != _lastMatches)
            {
                _lastMatches = matches;

                MatchesList.Clear();
                MatchesList = UpdateNumberList(matches.ToString().ToCharArray());
            }

            if (rank != _lastRank)
            {
                _lastRank = rank;

                RankList.Clear();
                RankList = UpdateNumberList(rank.ToString().ToCharArray());
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentScreen == Screens.ClassicScreen)
            {
                for (var i = 0; i < YearList.Count; i++)
                {
                    if (_lastYear > 99)
                    {
                        spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(175 + (i * 24), 24),
                            YearList[i], Color.White, 0f, Vector2.Zero, .75f, SpriteEffects.None, 0.99f);
                    }
                    else
                    {
                        spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(175 + (i * 24), 16),
                            YearList[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
                    }
                }

                for (var i = 0; i < ScoreList.Count; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(720 + (i * 24), 16),
                        ScoreList[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
                }
            }
            else
            {
                for (var i = 0; i < ToLevelScoreList.Count; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(512 + (i * 24), 394),
                        ToLevelScoreList[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                }

                for (var i = 0; i < DrainScoreList.Count; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(568 + (i * 24), 282),
                        DrainScoreList[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                }

                for (var i = 0; i < SeasonsList.Count; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(656 + (i * 24), 440),
                        SeasonsList[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                }

                for (var i = 0; i < MatchesList.Count; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(768 + (i * 24), 486),
                        MatchesList[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                }

                for (var i = 0; i < RankList.Count; i++)
                {
                    spriteBatch.Draw(SpriteLoader.IosEnglish, new Vector2(464 + (i * 24), 486),
                        RankList[i], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                }
            }

        }

        
        public static List<Rectangle> UpdateNumberList(char[] digits)
        {
            var tempHolder = new List<Rectangle>();

            for (var i = 0; i < digits.Length; i++)
            {
                switch (digits[i])
                {
                    case '1':
                        tempHolder.Add(new Rectangle(200, 8, 24, 32));
                        break;
                    case '2':
                        tempHolder.Add(new Rectangle(224, 8, 24, 32));
                        break;
                    case '3':
                        tempHolder.Add(new Rectangle(248, 8, 24, 32));
                        break;
                    case '4':
                        tempHolder.Add(new Rectangle(272, 8, 24, 32));
                        break;
                    case '5':
                        tempHolder.Add(new Rectangle(296, 8, 24, 32));
                        break;
                    case '6':
                        tempHolder.Add(new Rectangle(320, 8, 24, 32));
                        break;
                    case '7':
                        tempHolder.Add(new Rectangle(344, 8, 24, 32));
                        break;
                    case '8':
                        tempHolder.Add(new Rectangle(368, 8, 24, 32));
                        break;
                    case '9':
                        tempHolder.Add(new Rectangle(392, 8, 24, 32));
                        break;
                    case '0':
                    default:
                        tempHolder.Add(new Rectangle(176, 8, 24, 32));
                        break;
                }

            }


            return tempHolder;
        }
    }
}
