using Android.App;
using Android.Content;
using Android.Net;
using PartyUp.DependencyService;
using PartyUp.Droid.Service;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidConnectionService))]
namespace PartyUp.Droid.Service
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