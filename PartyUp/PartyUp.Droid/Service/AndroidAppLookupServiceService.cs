
using Android.App;
using Android.Content.PM;
using PartyUp.DependencyService;
using PartyUp.Droid.Service;[assembly: Xamarin.Forms.Dependency(typeof(AndroidAppLookupServiceService))]

namespace PartyUp.Droid.Service
{
    public class AndroidAppLookupServiceService : IAndroidAppLookupService
    {
        public bool DoesAppExist(string packageName)
        {
            PackageManager pm = Application.Context.PackageManager;
            bool installed = false;
            try
            {
                pm.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                installed = true;
            }
            catch (PackageManager.NameNotFoundException e)
            {
                installed = false;
            }
            return installed;
        }
    }
}