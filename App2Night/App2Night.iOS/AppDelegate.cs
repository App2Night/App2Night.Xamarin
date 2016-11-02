using System;
using System.Collections.Generic;
using System.Linq;
using App2Night;
using Foundation;
using HockeyApp.iOS;
using UIKit;

namespace PartyUp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            LoadApplication(new App());

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("3aca0e171a5443c090b3e064f2e5ce4b");
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation(); 
            return base.FinishedLaunching(app, options);

        }
    }
}
