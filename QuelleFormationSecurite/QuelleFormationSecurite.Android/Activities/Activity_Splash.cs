using Android.App;
using Android.Content.PM;
using Android.OS;
using QuelleFormationSecurite.Droid;

namespace Marche_Digitale.Droid.Activities
{
    [Activity(Theme = "@style/Theme.AppCompat.Light", Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class Activity_Splash : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            StartActivity(typeof(Activity_Accueil));
        }
    }
}