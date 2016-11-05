using App2Night.iOS.Service;

[assembly: Xamarin.Forms.Dependency(typeof(iOSConnectionService))]
namespace App2Night.iOS.Service
{
    public class iOSConnectionService : IConnectionService
    {
        public bool IsOnline()
        {
            return Reachability.IsHostReachable(Reachability.HostName);
        }
    }
}