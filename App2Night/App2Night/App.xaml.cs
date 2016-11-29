using System.Threading.Tasks;
using Acr.UserDialogs;
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

        CustomMasterDetailContainer _masterDetailNav = new CustomMasterDetailContainer();
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
            _masterDetailNav.Init("Menu");
            _masterDetailNav.AddPage<DashboardPageModel>(AppResources.Dashboard, null);
            _masterDetailNav.AddPage<PartyPickerViewModel>(AppResources.PickAParty, null);
            _masterDetailNav.AddPage<CreatePartyViewModel>(AppResources.CreateAParty);
            _masterDetailNav.AddPage<HistoryViewModel>(AppResources.History);
            _masterDetailNav.AddPage<SettingViewModel>(AppResources.Settings);
            _masterDetailNav.AddPage<AboutTabbedViewModel>(AppResources.About);

            MainPage = _masterDetailNav;

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                var ci = Xamarin.Forms.DependencyService.Get<ICultureService>().GetCurrentCultureInfo();
                AppResources.Culture = ci; // set the RESX for resource localization
                Xamarin.Forms.DependencyService.Get<ICultureService>().SetLocale(ci); // set the Thread for locale-aware methods
            } 

            Device.BeginInvokeOnMainThread(async ()=>await Setup());
        }

        async Task Setup()
        {
            using (UserDialogs.Instance.Loading())
            {
                CrossGeolocator.Current.PositionChanged += CurrentOnPositionChanged;
                FreshIOC.Container.Resolve<IDataService>().PartiesUpdated +=
                    (sender, args) => { CrossGeolocator.Current.StartListeningAsync(1, 100); };

                await FreshIOC.Container.Resolve<IStorageService>().OpenStorage();
                bool isLoggedIn = false;
                var storage = FreshIOC.Container.Resolve<IStorageService>().Storage;
                if (storage.Token != null)
                {
                    //Set token from last session and update user information.
                    isLoggedIn = await FreshIOC.Container.Resolve<IDataService>().SetToken(storage.Token);
                }
                DebugHelper.PrintDebug(DebugType.Info,
                    isLoggedIn ? "Log in from last session." : "User not logged in. No token available.");

                //Make an inital token refresh 
                await FreshIOC.Container.Resolve<IDataService>().RequestPartyWithFilter();

                if (!isLoggedIn)
                {
                    var page = FreshPageModelResolver.ResolvePageModel<LoginViewModel>();
                    await _masterDetailNav.PushPage(page, null, true);
                     
                }
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
