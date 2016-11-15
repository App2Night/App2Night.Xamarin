using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.DependencyService;
using App2Night.Service.Interface;
using App2Night.View;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class NavigationViewModel : MvvmNanoViewModel
    {
        

        public MvvmNanoCommand SyncCommand => new MvvmNanoCommand(async ()=> await Sync());

        public void OpenLogin()
        {
            //Check if user is loged in -> Token is available 
            //If Token is available -> Refresh token
            //If not -> Open Login! 
            NavigateTo<LoginViewModel>();
        }

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