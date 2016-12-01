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

        CustomMasterDetailContainer _masterDetailNav; 

        public App()
        {
            InitializeComponent();
            CheckIfMapsIsAvailable();

            RegisterInterfaces();
            _masterDetailNav = CreateMasterDetailContainerInstance();

            MainPage = _masterDetailNav;

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                var ci = Xamarin.Forms.DependencyService.Get<ICultureService>().GetCurrentCultureInfo();
                AppResources.Culture = ci; // set the RESX for resource localization
                Xamarin.Forms.DependencyService.Get<ICultureService>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            Device.BeginInvokeOnMainThread(async () => await GenerelSetup());
        }

        /// <summary>
        /// Creates an instance of the <see cref="CustomMasterDetailContainer"/> containing all pages for this session.
        /// </summary>
        /// <returns></returns>
        private CustomMasterDetailContainer CreateMasterDetailContainerInstance()
        {
            var masterDetailNav = new CustomMasterDetailContainer();
            masterDetailNav.Init("App2Night");
            masterDetailNav.AddPage<DashboardPageModel>(AppResources.Dashboard, "\uf015");
            masterDetailNav.AddPage<PartyPickerViewModel>(AppResources.PickAParty, "\uf29b");
            masterDetailNav.AddPage<CreatePartyViewModel>(AppResources.CreateAParty, "\uf271");
            masterDetailNav.AddPage<HistoryViewModel>(AppResources.History, "\uf187");
            masterDetailNav.AddPage<SettingViewModel>(AppResources.Settings, "\uf085");
            masterDetailNav.AddPage<AboutTabbedViewModel>(AppResources.About, "\uf05a");
            return masterDetailNav;
        }

        /// <summary>
        /// Validates if Google Maps is installed if the app is executed on an android device.
        /// </summary>
        private static void CheckIfMapsIsAvailable()
        { 
            if (Device.OS == TargetPlatform.Android)
            {
                MapAvailable =
                    Xamarin.Forms.DependencyService.Get<IAndroidAppLookupService>()
                        .DoesAppExist("com.google.android.apps.maps");
            }
        }

        /// <summary>
        /// Resumes the last session, makes an initial sync and opens the login page if needed.
        /// </summary>
        /// <returns></returns>
        private async Task GenerelSetup()
        {
            using (UserDialogs.Instance.Loading())
            {
                SetupGeolocator();

                await FreshIOC.Container.Resolve<IStorageService>().OpenStorage();
                bool isLoggedIn = await ResumeLastSession();

                //Make an inital token refresh 
                await FreshIOC.Container.Resolve<IDataService>().RequestPartyWithFilter();

                if (!isLoggedIn)
                {
                    await ShowLoginModal();
                }
            }
        }

        private async Task ShowLoginModal()
        {
            var page = FreshPageModelResolver.ResolvePageModel<LoginViewModel>();
            await _masterDetailNav.PushPage(page, null, true);
        }

        private async Task<bool> ResumeLastSession()
        {
            bool isLoggedIn = false;
            var storage = FreshIOC.Container.Resolve<IStorageService>().Storage;
            if (storage.Token != null)
            {
                //Set token from last session and update user information.
                isLoggedIn = await FreshIOC.Container.Resolve<IDataService>().SetToken(storage.Token);
            }
            DebugHelper.PrintDebug(DebugType.Info,
                isLoggedIn ? "Log in from last session." : "User not logged in. No token available.");
            return isLoggedIn;
        }

        private void SetupGeolocator()
        {
            CrossGeolocator.Current.PositionChanged += CurrentOnPositionChanged;
            FreshIOC.Container.Resolve<IDataService>().PartiesUpdated +=
                (sender, args) => { CrossGeolocator.Current.StartListeningAsync(1, 100); };
        }

        /// <summary>
        /// Handles position changes.
        /// Calculates distance to all nearby parties.
        /// </summary> 
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

        /// <summary>
        /// Registers all interfaces.
        /// </summary>
        private void RegisterInterfaces()
        {
            FreshIOC.Container.Register<IStorageService, StorageService>().AsSingleton();
            FreshIOC.Container.Register<IAlertService, AlertService>();
            FreshIOC.Container.Register<IClientService, ClientService>();
            FreshIOC.Container.Register<IDataService, DataService>().AsSingleton();
        }
    }
}
