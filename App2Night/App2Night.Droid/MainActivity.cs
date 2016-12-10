using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Azure.Mobile; 

namespace App2Night.Droid
{
    [Activity(Label = "App2Night", Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle); 
            UserDialogs.Init(this);
            Xamarin.FormsMaps.Init(this, bundle); 
            
            //Init analytics
            MobileCenter.Configure(Model.Credentials.AnalyticsKey);

            LoadApplication(new App()); 
        } 
    }
}

