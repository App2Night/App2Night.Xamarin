using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.DependencyService;
using App2Night.Helper;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using App2Night.ViewModel;
using MvvmNano;
using MvvmNano.Forms;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
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
            
            // Set MasterDetailPage
            SetUpMasterDetailPage<NavigationViewModel>();
            AddSiteToDetailPages(new CustomMasterDetailData(typeof(DashboardViewModel), "Dashboard", "\uf015"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof(PartyPickerViewModel), "Pick a party", "\uf29b"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof(CreatePartyViewModel), "Create", "\uf271"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof(HistoryViewModel), "History", "\uf187"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof(SettingViewModel), "Setting", "\uf085"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof(AboutTabbedViewModel), "About", "\uf05a"));

            //TODO Check if user is already loged in
            MvvmNanoIoC.Resolve<NavigationViewModel>().OpenLogin();

            //TODO Extract this part
            CrossGeolocator.Current.PositionChanged += CurrentOnPositionChanged;
            MvvmNanoIoC.Resolve<IDataService>().PartiesUpdated += (sender, args) =>
            {
                CrossGeolocator.Current.StartListeningAsync(1, 100);
            };
            Device.BeginInvokeOnMainThread((async () => await StartupSync()));
        }

        protected override void SetUpPresenter()
        {
            MvvmNanoIoC.RegisterAsSingleton<IPresenter>(
                new CustomPresenter(this)
            );
        }

        private void CurrentOnPositionChanged(object sender, PositionEventArgs positionEventArgs)
        {
            var dataService = MvvmNanoIoC.Resolve<IDataService>();
            var position = positionEventArgs.Position;
            Coordinates userPosition = new Coordinates((float) position.Longitude, (float) position.Latitude);
            //Update distance to all partys
            foreach (Party party in dataService.InterestingPartys)
            {
                double distance = userPosition.DistanceTo(party.Coordinates);
                party.DistanceToParty = distance;
            }
            foreach (Party party in dataService.SelectedPartys)
            {
                double distance = userPosition.DistanceTo(party.Coordinates);
                party.DistanceToParty = distance;
            }
        } 
        

        //Replace this with a serious sync 
        public async Task StartupSync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                //UserDialogs.Instance.ShowLoading("Starting session.");
                //var tokenResult = await MvvmNanoIoC.Resolve<IDataService>().RequestToken("test", "test");
                //UserDialogs.Instance.Toast(
                //    new ToastConfig("Token request finished " + (tokenResult.Success ? "" : "un") + "successfull.")
                //    {
                //        BackgroundColor = tokenResult.Success ? System.Drawing.Color.LawnGreen : System.Drawing.Color.LightCoral
                //    });
                UserDialogs.Instance.ShowLoading("Loading partys.");
                var result = await MvvmNanoIoC.Resolve<IDataService>().RefreshPartys();
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast(
                    new ToastConfig("Loading parties finished " + (result.Success ? "" : "un") + "successfull.")
                    {
                        BackgroundColor = result.Success ? System.Drawing.Color.MediumSeaGreen : System.Drawing.Color.LightCoral
                    });
            }
           
        }

        private void RegisterInterfaces()
        {
            MvvmNanoIoC.Register<IClientService, ClientService>();
            MvvmNanoIoC.RegisterAsSingleton<IDataService, DataService>();
            MvvmNanoIoC.RegisterAsSingleton<IImageFactory, ImageFactory>();
        }
    }
}
