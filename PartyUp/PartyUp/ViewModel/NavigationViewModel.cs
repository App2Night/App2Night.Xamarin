 

using MvvmNano;
using PartyUp.DependencyService;

namespace PartyUp.ViewModel
{
    public class NavigationViewModel : MvvmNanoViewModel
    {
        public string TestValue { get; set; } = Xamarin.Forms.DependencyService.Get<IConnectionService>().IsOnline()
            ? "Online"
            : "Offline";
    }
}