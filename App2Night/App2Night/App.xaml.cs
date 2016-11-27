using System.Threading.Tasks; 
using App2Night.Data.Language;
using App2Night.DependencyService;
using App2Night.Model.Model;
using App2Night.PageModel;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using FreshMvvm;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace App2Night
{
    public partial class App
    {
        public static bool MapAvailable { get; private set; } 

        public App()
        {
            InitializeComponent();

            //It is possible (even if it is unlikely) that Google Maps is not installed on the device.
            //Check if it is installed on start.
            if (Device.OS == TargetPlatform.Android)
            {
                MapAvailable =
                    Xamarin.Forms.DependencyService.Get<IAndroidAppLookupService>()
                        .DoesAppExist("com.google.android.apps.maps");
            }

            RegisterInterfaces();

            var masterDetailNav = new FreshMasterDetailNavigationContainer();
            masterDetailNav.Init("Menu");
            masterDetailNav.AddPage<DashboardPageModel>(AppResources.Dashboard, null);
            masterDetailNav.AddPage<PartyPickerViewModel>(AppResources.PickAParty, null);
            masterDetailNav.AddPage<CreatePartyViewModel>(AppResources.CreateAParty);
            masterDetailNav.AddPage<HistoryViewModel>(AppResources.History);
            masterDetailNav.AddPage<SettingViewModel>(AppResources.Settings);
            masterDetailNav.AddPage<AboutTabbedViewModel>(AppResources.About);

            MainPage = masterDetailNav; 

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                var ci = Xamarin.Forms.DependencyService.Get<ICultureService>().GetCurrentCultureInfo();
                AppResources.Culture = ci; // set the RESX for resource localization
                Xamarin.Forms.DependencyService.Get<ICultureService>().SetLocale(ci); // set the Thread for locale-aware methods
            }
        } 

        protected override void OnStart()
        {
            
            base.OnStart();   
           
            CrossGeolocator.Current.PositionChanged += CurrentOnPositionChanged;
            FreshIOC.Container.Resolve<IDataService>().PartiesUpdated +=
                (sender, args) => { CrossGeolocator.Current.StartListeningAsync(1, 100); };

            Task.Run(async ()=> await OnStartSync());
        }

        private async Task OnStartSync()
        {
            //Restore stored token 
            await FreshIOC.Container.Resolve<IStorageService>().OpenStorage();
            bool isLoggedIn = false;
            var storage = FreshIOC.Container.Resolve<IStorageService>().Storage;
            if (storage.Token != null)
            {
                //Set token from last session and update user information.
                isLoggedIn = await FreshIOC.Container.Resolve<IDataService>().SetToken(storage.Token);
            } 
            DebugHelper.PrintDebug(DebugType.Info, isLoggedIn ? "Log in from last session." : "User not logged in. No token available.");

            //Make an inital token refresh 
            Device.BeginInvokeOnMainThread(async ()=> await FreshIOC.Container.Resolve<IDataService>().RequestPartyWithFilter());

            if (!isLoggedIn)
            {
                //Prompt the login page if the user is not logged in
                Device.BeginInvokeOnMainThread(async ()=> await FreshIOC.Container.Resolve<DashboardPageModel>().OpenLogin());
            }
        } 
        private void CurrentOnPositionChanged(object sender, PositionEventArgs positionEventArgs)
        {
            var dataService = FreshIOC.Container.Resolve<IDataService>();
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

        private void RegisterInterfaces()
        {
            FreshIOC.Container.Register<IStorageService, StorageService>().AsSingleton();
            FreshIOC.Container.Register<IAlertService, AlertService>();
            FreshIOC.Container.Register<IClientService, ClientService>();
            FreshIOC.Container.Register<IDataService, DataService>().AsSingleton();
        }
    }
}
