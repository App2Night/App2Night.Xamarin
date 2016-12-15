using System.Threading.Tasks;
using Acr.UserDialogs;

using App2Night.DependencyService;
using App2Night.Model.Language;
using App2Night.Model.Model;
using App2Night.PageModel;
using App2Night.PageModel.SubPages;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using FreshMvvm;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace App2Night
{
    public partial class App
    {
        public static bool MapAvailable { get; private set; }

        public static CustomMasterDetailContainer _masterDetailNav; 

        public App()
        {
            MobileCenter.Start(typeof(Analytics), typeof(Crashes));
            CheckIfMapsIsAvailable();

            InitializeComponent();

            RegisterInterfaces();
            SetupGeolocator();
            _masterDetailNav = CreateMasterDetailContainerInstance();

            MainPage = _masterDetailNav;

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                var ci = Xamarin.Forms.DependencyService.Get<ICultureService>().GetCurrentCultureInfo();
                AppResources.Culture = ci; // set the RESX for resource localization
                Xamarin.Forms.DependencyService.Get<ICultureService>().SetLocale(ci); // set the Thread for locale-aware methods
            } 
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
            masterDetailNav.AddPage<CreatePartyViewModel>(AppResources.CreateAParty, "\uf271", requiresLogin: true);
            masterDetailNav.AddPage<MyPartysViewModel>(AppResources.MyParties, "\uf274", requiresLogin: true); 
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
			Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading());
            var alertService = FreshIOC.Container.Resolve<IAlertService>(); 

            var storage = FreshIOC.Container.Resolve<IStorageService>();
            await storage.OpenStorage(); 

            //Make an inital token refresh 
            await FreshIOC.Container.Resolve<IDataService>().BatchRefresh();

            if (!storage.IsLogIn)
            {
                ShowLoginModal();
            }
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading()); 
        }

        protected override void OnStart()
        {
            base.OnStart(); 
            Task.Run(async () => await GenerelSetup());
        }

        private void ShowLoginModal()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = FreshPageModelResolver.ResolvePageModel<LoginViewModel>();
                await _masterDetailNav.PushPage(page, null, true);
            }); 
        }

        private void SetupGeolocator()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await CoordinateHelper.HasGeolocationAccess())
                {
                    await CrossGeolocator.Current.StartListeningAsync(50, 100);
                    CrossGeolocator.Current.PositionChanged += CurrentOnPositionChanged; 
                }
            }); 
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
            FreshIOC.Container.Register<IDatabaseService>(Xamarin.Forms.DependencyService.Get<IDatabaseService>(), "IDatabaseService");
            FreshIOC.Container.Register<IStorageService, StorageService>().AsSingleton();
            FreshIOC.Container.Register<IAlertService, AlertService>().AsSingleton();
            FreshIOC.Container.Register<IClientService, ClientService>();
            FreshIOC.Container.Register<IDataService, DataService>().AsSingleton();
        }
    }
}
