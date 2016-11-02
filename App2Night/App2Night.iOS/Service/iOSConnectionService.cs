using App2Night.DependencyService;
using PartyUp.iOS.Service;

[assembly: Xamarin.Forms.Dependency(typeof(iOSConnectionService))]
namespace PartyUp.iOS.Service
{
    public class iOSConnectionService : IConnectionService
    {
        public bool IsOnline()
        {
            return Reachability.IsHostReachable(Reachability.HostName);
        }
    }
}