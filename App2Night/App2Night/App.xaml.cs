using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Data.Language;
using App2Night.DependencyService;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using App2Night.ViewModel;
using MvvmNano;
using MvvmNano.Forms;
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
            MainPage = new ContentPage();
            //It is possible (even if it is unlikely) that Google Maps is not installed on the device.
            //Check if it is installed on start.
            if (Device.OS == TargetPlatform.Android)
            {
                MapAvailable =
                    Xamarin.Forms.DependencyService.Get<IAndroidAppLookupService>()
                        .DoesAppExist("com.google.android.apps.maps");
            }

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                var ci = Xamarin.Forms.DependencyService.Get<ICultureService>().GetCurrentCultureInfo();
                AppResources.Culture = ci; // set the RESX for resource localization
                Xamarin.Forms.DependencyService.Get<ICultureService>().SetLocale(ci); // set the Thread for locale-aware methods
            }
        }

        async Task SyncOnStartup()
        {
            
        }

        protected override void OnStart()
        {
            RegisterInterfaces();
            base.OnStart();

            // Set MasterDetailPage
            SetUpMasterDetailPage<NavigationViewModel>();
            AddSiteToDetailPages(new CustomMasterDetailData(typeof (DashboardViewModel), AppResources.Dashboard, "\uf015"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof (PartyPickerViewModel), AppResources.PickAParty, "\uf29b"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof (CreatePartyViewModel), AppResources.CreateAParty, "\uf271"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof (HistoryViewModel), AppResources.PartyHistory, "\uf187"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof (SettingViewModel), AppResources.Settings, "\uf085"));
            AddSiteToDetailPages(new CustomMasterDetailData(typeof (AboutTabbedViewModel), AppResources.About, "\uf05a"));

            //TODO Check if token is available and refresh it to test if it is still valid
            var loggedIn = false;
            if (!loggedIn)
            {
                //Prompt the login page if the user is not logged in
                MvvmNanoIoC.Resolve<NavigationViewModel>().OpenLogin();
            }
            //Sync the parties
            //Task.Run(async () =>
            //{
            //    try
            //    {
            //        await MvvmNanoIoC.Resolve<IDataService>().RefreshPartys();
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.WriteLine(e);
            //    }
            //});

            //TODO Extract this part
            CrossGeolocator.Current.PositionChanged += CurrentOnPositionChanged;
            MvvmNanoIoC.Resolve<IDataService>().PartiesUpdated +=
                (sender, args) => { CrossGeolocator.Current.StartListeningAsync(1, 100); };
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

        private void RegisterInterfaces()
        {
            MvvmNanoIoC.Register<IStorageService, StorageService>();
            MvvmNanoIoC.Register<IAlertService, AlertService>();
            MvvmNanoIoC.Register<IClientService, ClientService>();
            MvvmNanoIoC.RegisterAsSingleton<IDataService, DataService>();
            MvvmNanoIoC.RegisterAsSingleton<IImageFactory, ImageFactory>();
        }
    }
}
