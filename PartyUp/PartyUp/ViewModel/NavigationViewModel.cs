 

using System;
using System.Threading.Tasks;
using MvvmNano;
using PartyUp.DependencyService;
using PartyUp.Service.Interface;

namespace PartyUp.ViewModel
{
    public class NavigationViewModel : MvvmNanoViewModel
    {
        public string TestValue { get; set; } = Xamarin.Forms.DependencyService.Get<IConnectionService>().IsOnline()
            ? "Online"
            : "Offline";

        public MvvmNanoCommand SyncCommand => new MvvmNanoCommand(async ()=> await Sync());

        private async Task Sync()
        {
            await MvvmNanoIoC.Resolve<ICacheService>().RefreshPartys();
        }
    }
}