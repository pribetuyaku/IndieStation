using Android.App;
using Android.Content;
using Android.OS;
using AppFinal.Droid;
using AppFinal.Interfaces;
//assembles the activity
[assembly: Xamarin.Forms.Dependency(typeof(GameOpenerActivity))]

namespace AppFinal.Droid
{

    [Activity(Label = "GameOpenerActivity")]
    //class that implements IOpenGame interface to open the game via App
    public class GameOpenerActivity : Activity, IOpenGame
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        /// <summary>
        /// checks if the game is installed in the phone, if so opens it , other wise send user to the PlayStore
        ///
        /// </summary>
        /// <param name="game"></param>
        public void OpenGame(string game)
        {
            
            Intent intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(game);

            // If not NULL run the app, if not, take the user to the app store
            if (intent != null)
            {
                intent.AddFlags(ActivityFlags.NewTask);
                //starts the package
                Android.App.Application.Context.StartActivity(intent);
            }
            else //won't work because our game is not in PlayStore
            {
                // intent = new Intent(Intent.ActionView);
                // intent.AddFlags(ActivityFlags.NewTask);
                //
                // intent.SetData(Android.Net.Uri.Parse($"market://details?id={game}"));
                // Android.App.Application.Context.StartActivity(intent);
            }
        }
    }
}