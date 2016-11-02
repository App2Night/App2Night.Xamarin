using Android.App;
using Android.Content;
using Android.Net;
using App2Night.DependencyService;
using App2Night.Droid.Service;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidConnectionService))]
namespace App2Night.Droid.Service
{
    public class AndroidConnectionService : IConnectionService
    {
        public bool IsOnline()
        {
            ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);

            NetworkInfo activeNetwork = conMgr.ActiveNetworkInfo;
            if (activeNetwork == null) return false;
            return (activeNetwork.IsConnected);
        }
    }
}