using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.DependencyService;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using App2Night.ViewModel;
using MvvmNano;
using MvvmNano.Forms;
using Plugin.Connectivity;
using Xamarin.Forms;
using Color = System.Drawing.Color;

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
                            Property = Label.HorizontalTextAlignmentProperty,
                            Value = TextAlignment.Center
                        },
                        new Setter
                        {
                            Property = Label.VerticalTextAlignmentProperty,
                            Value = TextAlignment.Center,
                        }
                    }

                },
                {new Style(typeof(ContentPage))
                {
                    Setters =
                    {
                         new Setter
                            {
                                Property =  ContentPage.BackgroundColorProperty,
                                Value = Color.Black,
                            }
                    }
}   
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
                    BackgroundColor = tokenResult.Success ?  Color.LawnGreen : Color.LightCoral
                });
            }  
            UserDialogs.Instance.ShowLoading("Loading partys.");
            var result = await MvvmNanoIoC.Resolve<IDataService>().RefreshPartys();
            UserDialogs.Instance.HideLoading();
            UserDialogs.Instance.Toast(new ToastConfig("Loading parties finished " + (result.Success ? "" : "un") + "successfull.")
            {
                BackgroundColor = result.Success ? System.Drawing.Color.LawnGreen : Color.LightCoral
            });
             
        }

        private void RegisterInterfaces()
        {
            MvvmNanoIoC.Register<IClientService, ClientService>();
            MvvmNanoIoC.RegisterAsSingleton<IDataService, DataService>();
        }
    }
}
