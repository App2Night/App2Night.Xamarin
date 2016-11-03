using System.Threading.Tasks;
using App2Night.DependencyService;
using App2Night.Service.Interface;
using MvvmNano;
using PartyUp.Service.Interface;

namespace App2Night.ViewModel
{
    public class NavigationViewModel : MvvmNanoViewModel
    {
        public string TestValue { get; set; } = Xamarin.Forms.DependencyService.Get<IConnectionService>().IsOnline()
            ? "Online"
            : "Offline";

        public MvvmNanoCommand SyncCommand => new MvvmNanoCommand(async ()=> await Sync());

        private async Task Sync()
        {
            await MvvmNanoIoC.Resolve<IDataService>().RefreshPartys();
        }
    }
}