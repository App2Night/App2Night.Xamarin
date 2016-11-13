using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.DependencyService;
using App2Night.Service.Interface;
using MvvmNano;
using PartyUp.Service.Interface;

namespace App2Night.ViewModel
{
    public class NavigationViewModel : MvvmNanoViewModel
    {
        

        public MvvmNanoCommand SyncCommand => new MvvmNanoCommand(async ()=> await Sync());

        private async Task Sync()
        {
            UserDialogs.Instance.ShowLoading("Sync");
            var result = await MvvmNanoIoC.Resolve<IDataService>().RefreshPartys();
            UserDialogs.Instance.HideLoading();
            UserDialogs.Instance.Toast(
            new ToastConfig("Sync finished " + (result.Success ? "" : "un") + "successfull.")
            {
                BackgroundColor = result.Success ? System.Drawing.Color.LawnGreen : System.Drawing.Color.LightCoral
            });
        }
    }
}