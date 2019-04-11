using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;

namespace TempleGardens
{
    [Activity(Label = "Temple Gardens"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        TempleMain game;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);  

            TempleMain.Activity = this;
            Vibrator v = (Vibrator)GetSystemService(Context.VibratorService);
           

            game = new TempleMain(v);
            SetContentView(game.Window);
            game.Run();
        }


        protected override void OnPause()
        {
            base.OnPause();
            SoundBoard.MusicControl("Silence");
        }

        protected override void OnResume()
        {
            base.OnResume();
            SoundBoard.MusicControl("UnSilence");
        }
    }
}

