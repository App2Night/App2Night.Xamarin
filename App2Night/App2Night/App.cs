using System.Threading.Tasks;
using App2Night.DependencyService;
using App2Night.ViewModel;
using MvvmNano;
using MvvmNano.Forms;
using PartyUp.Service.Interface;
using PartyUp.Service.Service;
using Xamarin.Forms;

namespace App2Night
{
    public class App : MvvmNanoApplication
    {
        public static bool MapAvailable { get; private set; }


        public App()
        {
            //It is possible (even if it is unlikely) that Google Maps is not installed on the device.
            //Check if it is installed on start.
            if (Device.OS == TargetPlatform.Android)
            { 
                MapAvailable = Xamarin.Forms.DependencyService.Get<IAndroidAppLookupService>().DoesAppExist("com.google.android.apps.maps");
            }
            Resources = new ResourceDictionary
            {
                new Style(typeof(Label))
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = Xamarin.Forms.View.VerticalOptionsProperty,
                            Value = LayoutOptions.Center
                        },
                        new Setter
                        {
                            Property = Xamarin.Forms.View.HorizontalOptionsProperty,
                            Value = LayoutOptions.Center, 
                        },
                        new Setter
                        {
                            Property = Xamarin.Forms.View.MarginProperty,
                            Value = 0,
                        },
                    },
                   
                },
                {"Test", new Style(typeof(Label))
                }
            };
        }

        protected override void OnStart()
        {
            RegisterInterfaces();

            base.OnStart();
            SetUpMasterDetailPage<NavigationViewModel>();
            AddSiteToDetailPages(new MasterDetailData(typeof(DashboardViewModel), "Dashboard"));
            AddSiteToDetailPages(new MasterDetailData(typeof(PartyPickerViewModel), "Pick a party"));
            AddSiteToDetailPages(new MasterDetailData(typeof(CreatePartyViewModel), "Create"));
            AddSiteToDetailPages(new MasterDetailData(typeof(HistoryViewModel), "History"));
            AddSiteToDetailPages(new MasterDetailData(typeof(SettingViewModel), "Setting"));
            AddSiteToDetailPages(new MasterDetailData(typeof(AboutViewModel), "About"));

            //Task.Run(async () => { await StartupSync(); });


        } 

        private async Task StartupSync()
        {
            var token = await MvvmNanoIoC.Resolve<IClientService>().GetToken("test", "test");
            await MvvmNanoIoC.Resolve<ICacheService>().RefreshPartys();
        }

        private void RegisterInterfaces()
        {
            MvvmNanoIoC.Register<IClientService, ClientService>();
            MvvmNanoIoC.RegisterAsSingleton<ICacheService, CacheService>();
        }
    }
}
