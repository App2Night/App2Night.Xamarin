 

using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.DependencyService;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using App2Night.ViewModel;
using MvvmNano;
using MvvmNano.Forms;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace App2Night
{
    public partial class App : MvvmNanoApplication
    {
        public static bool MapAvailable { get; private set; }
 
        public App()
        {
            InitializeComponent();
            MainPage = new ContentPage();
            //It is possible (even if it is unlikely) that Google Maps is not installed on the device.
            //Check if it is installed on start.
            if (Device.OS == TargetPlatform.Android)
            {
                MapAvailable =
                    Xamarin.Forms.DependencyService.Get<IAndroidAppLookupService>()
                        .DoesAppExist("com.google.android.apps.maps");
            } 
        }

        protected override void OnStart()
        {
            RegisterInterfaces();

            base.OnStart();
            SetUpMasterDetailPage<NavigationViewModel>();
            AddSiteToDetailPages(new MasterDetailData(typeof (DashboardViewModel), "Dashboard"));
            AddSiteToDetailPages(new MasterDetailData(typeof (PartyPickerViewModel), "Pick a party"));
            AddSiteToDetailPages(new MasterDetailData(typeof (CreatePartyViewModel), "Create"));
            AddSiteToDetailPages(new MasterDetailData(typeof (HistoryViewModel), "History"));
            AddSiteToDetailPages(new MasterDetailData(typeof (SettingViewModel), "Setting"));
            AddSiteToDetailPages(new MasterDetailData(typeof (AboutViewModel), "About"));

            //SetUpMainPage<LoginViewModel>();
            Device.BeginInvokeOnMainThread((async () => await StartupSync()));
        }

        public async Task StartupSync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                UserDialogs.Instance.ShowLoading("Starting session.");
                var tokenResult = await MvvmNanoIoC.Resolve<IDataService>().RequestToken("test", "test");
                UserDialogs.Instance.Toast(
                    new ToastConfig("Token request finished " + (tokenResult.Success ? "" : "un") + "successfull.")
                    {
                        BackgroundColor = tokenResult.Success ? System.Drawing.Color.LawnGreen : System.Drawing.Color.LightCoral
                    });
            }
            UserDialogs.Instance.ShowLoading("Loading partys.");
            var result = await MvvmNanoIoC.Resolve<IDataService>().RefreshPartys();
            UserDialogs.Instance.HideLoading();
            UserDialogs.Instance.Toast(
                new ToastConfig("Loading parties finished " + (result.Success ? "" : "un") + "successfull.")
                {
                    BackgroundColor = result.Success ? System.Drawing.Color.LawnGreen : System.Drawing.Color.LightCoral
                });
        }

        private void RegisterInterfaces()
        {
            MvvmNanoIoC.Register<IClientService, ClientService>();
            MvvmNanoIoC.RegisterAsSingleton<IDataService, DataService>();
        }
    }
}
