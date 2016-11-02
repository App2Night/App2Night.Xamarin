using Android.App;
using Android.Content.PM;
using Android.OS;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;

namespace App2Night.Droid
{
    [Activity(Label = "App2Night", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrashManager.Register(this, "$(HockeyAppId)");
            UpdateManager.Register(this, "$(HockeyAppId)");
            MetricsManager.Register(Application, "$(HockeyAppId)");
            Xamarin.FormsMaps.Init(this, bundle);
            LoadApplication(new App());
            
          
        }

        protected override void OnResume()
        {
            base.OnResume();

            //Start Tracking usage in this activity
            Tracking.StartUsage(this);
        }

        protected override void OnPause()
        {
            //Stop Tracking usage in this activity
            Tracking.StopUsage(this);

            base.OnPause();
        }
    }
}

