using System;
using System.Collections.Generic;

namespace TempleGardens
{
    public enum Seasons: byte { Spring = 0, Summer, Fall, Winter }; 

    [Serializable]
    public class User
    {
        public string UserId { get; private set; }
        public int TotalScore { get; private set; }
        public int ScoreToLevel { get; private set; }
        public bool TutorialPlayed { get; set; }
        public int Rank { get; private set; }

        public List<Achievements> PlayerTrophies { get; private set; }

        // preferences
        public bool RegTilesPreferred { get; private set; }
        public string ColorPreference { get; private set; }

        public bool MusicActive { get; set; }
        public bool SoundActive { get; set; }
        public bool VibrateOn { get; set; }

// 		Stores ZenSave Data
		public int ZenSavedScore { get; set; }
		public byte[] BoardRocks { get; set; }
		public byte CurrentZenSeason { get; set; }
        public List<PieceTemplateSimplified> SavedZenPieces { get; set; }
        public int[] UsedPieceCount { get; set; }
        public int CurrentZenYear { get; set; }

        public int LangPreference { get; set; }

        public int ClassicCount { get; private set; }
        public int EnduranceCount { get; private set; }

        public User(string name)
		{
			UserId = name;
			TotalScore = 0;
			TutorialPlayed = false;
			InitTrophies();
			Rank = 1;

			RegTilesPreferred = true;
			ColorPreference = "Standard";

			MusicActive = true;
			SoundActive = true;
            VibrateOn = true;

            ZenSavedScore = 0;
            CurrentZenYear = 0;
            CurrentZenSeason = 0;
            SavedZenPieces = new List<PieceTemplateSimplified>();

            LangPreference = 0;


		}

        public void UpdateUser(int totScore, int score, int level)
        {
            TotalScore += totScore;
            ScoreToLevel = score;
            Rank = level;
        }

        /// <summary>
        /// Set color family used.
        /// </summary>
        /// <param name="colorPref"></param>
        public void SetData(string colorPref)
        {
            ColorPreference = colorPref;
        }

        /// <summary>
        /// Toggle tiles used.
        /// </summary>
        public void SetData()
        {
            RegTilesPreferred = !RegTilesPreferred;
        }

        public void IncrementCountClassic()
        {
            ClassicCount++;
        }

        public void IncrementCountEnduarnce()
        {
            EnduranceCount++;
        }

        /// <summary>
        /// Does not generate a picture of a cat.
        /// </summary>
        private void InitTrophies()
        {
            PlayerTrophies = new List<Achievements>();
            PlayerTrophies.Add(new Achievements("NoRocks10", "Played 10 Seasons without generating a rock.", 10));
            PlayerTrophies.Add(new Achievements("Play15Norm", "Played 15 Classic games.", 10));
            PlayerTrophies.Add(new Achievements("Played15Zen", "Played 15 Endurance games.", 10));
            PlayerTrophies.Add(new Achievements("NoAutoFill", "Filled board without using \nAuto Fill before timer expired \n10 Seasons in a row.", 20));
            PlayerTrophies.Add(new Achievements("CompleteOne", "Completed a single plant species in one game.", 20));
            PlayerTrophies.Add(new Achievements("CompleteAll", "Completed all plant species in one game.", 50));
            PlayerTrophies.Add(new Achievements("Lvl5", "Reached level 5.", 5));
            PlayerTrophies.Add(new Achievements("Lvl10", "Reached level 10", 20));
            PlayerTrophies.Add(new Achievements("Lvl15", "Reached level 15", 50));
            PlayerTrophies.Add(new Achievements("Season100", "Played 100 Seasons in one game.", 25));
        }
    }
}
