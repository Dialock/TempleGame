#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion

namespace TempleGardens
{
    public static class SoundBoard
    {
        //private static SoundEffect music1;
        //private static SoundEffect music2;
        //private static SoundEffect music3;
//        public static SoundEffectInstance MusicInstance { get; private set; }

        public static SoundEffect DropShape { get; private set; }
        public static SoundEffect RockTap1 { get; private set; }
        public static SoundEffect RockTap2 { get; private set; }

        private static Song song1;
        private static Song song2;


//        private static SoundEffect music1;
//        public static SoundEffectInstance Music1Instance { get; private set; }
//        public static SoundEffect GetShape { get; private set; }
//        public static SoundEffect SetShape { get; private set;}
//        public static SoundEffect BadPlace { get; private set; }
//        public static SoundEffect GoodPlace { get; private set; }
//        public static SoundEffect PieceDie { get; private set; }
//
//        public static SoundEffect Click1 { get; private set; }
//        public static SoundEffect Click2 { get; private set; }
//
//        public static SoundEffect ButtonClicked { get; private set; }
//        public static SoundEffect ButtonHighlight { get; private set;}
//        public static SoundEffectInstance ButtonHighlightInsnce { get; private set; }
//
//        public static SoundEffect ScoreDrain { get; private set; }
//        public static SoundEffectInstance ScoreDrainInstance { get; private set; }
//
//        public static SoundEffect LayerChange { get; private set; }
//
//        public static SoundEffect AdminGain { get; private set; }

        public static ContentManager Content { get; private set; }



        public static void Init(Game game)
        {
            Content = new ContentManager(game.Services, "Content");

            DropShape = GetCalledSound("plantDrop1");

            RockTap1 = GetCalledSound("rockTap_01");
            RockTap2 = GetCalledSound("rockTap_02");


            song1 = Content.Load<Song>(@"Sound\KaraSquarePicnic32");
            song2 = Content.Load<Song>(@"Sound\SquarePelucheTryI32");
//            music1 = GetCalledSound("Kara Square - Picnic by the Waterfall");

            //music1 = GetCalledMusic("playBacking1");
            //music2 = GetCalledMusic("drugged");

            //GetShape = GetCalledSound("getPiece");
            ////SetShape = GetCalledSound("normalDrop1");
            //SetShape = GetCalledSound("setPiece");
            //BadPlace = GetCalledSound("placePowerBad");
            //GoodPlace = GetCalledSound("placePower");
            //PieceDie = GetCalledSound("pieceDie");

            //Click1 = GetCalledSound("click1");
            //Click2 = GetCalledSound("click2");

            //ButtonHighlight = GetCalledSound("buttonHighlight");
            //ButtonClicked = GetCalledSound("buttonSelect");

            //AdminGain = GetCalledSound("adminGain");
            //ScoreDrain = GetCalledSound("scoreDrain");
            //ScoreDrainInstance = ScoreDrain.CreateInstance();
            //ScoreDrainInstance.IsLooped = true;

            //LayerChange = GetCalledSound("layerSlide");

            //ButtonHighlightInsnce = ButtonHighlight.CreateInstance();

        }

        public static void MusicControl(string command)
        {
            if (command == "StartProgram")
            {
                if (TempleMain.Player.MusicActive)
                {
                    MediaPlayer.Play(song1);
                    MediaPlayer.Volume = 0.55f;
                    MediaPlayer.IsRepeating = true;
                }
                else
                {
                    MediaPlayer.Play(song1);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.55f;
                    MediaPlayer.Pause();
                }
            }
            else if (command == "ToMenu")
            {
                if (TempleMain.Player.MusicActive)
                {
                    MediaPlayer.Play(song1);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.55f;
                }
                else
                {
                    MediaPlayer.Play(song1);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.55f;
                    MediaPlayer.Pause();
                }

            }
            else if (command == "Silence")
            {
                MediaPlayer.Pause();
            }
            else if (command == "UnSilence")
            {
                MediaPlayer.Resume();
            }
            else if (command == "ToPlay")
            {
                if (TempleMain.Player.MusicActive)
                {
                    MediaPlayer.Play(song2);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.55f;
                }
                else
                {
                    MediaPlayer.Play(song2);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.55f;
                    MediaPlayer.Pause();
                }
            }
        }

        private static SoundEffect GetCalledMusic(string musicName)
        {
            return Content.Load<SoundEffect>(@"Music\" + musicName);
        }

        private static SoundEffect GetCalledSound(string soundName)
        {
            return Content.Load<SoundEffect>(@"Sound\" + soundName);
        }


        //public static void SetSoundInstance(int i)
        //{
        //    if (i == 1)
        //    {
        //        MusicInstance = music1.CreateInstance();
        //        MusicInstance.Volume = 0.55f;
        //        MusicInstance.IsLooped = true;
        //    }
        //    else if (i == 2)
        //    {
        //        MusicInstance = music2.CreateInstance();
        //        MusicInstance.Volume = 0.55f;
        //        MusicInstance.IsLooped = true;
        //    }
        //    else
        //    {
        //        MusicInstance = music3.CreateInstance();
        //        MusicInstance.Volume = 0.55f;
        //        MusicInstance.IsLooped = true;
        //    }
        //}
    }
}
