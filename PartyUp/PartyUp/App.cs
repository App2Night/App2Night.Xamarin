
using System.Threading.Tasks;
using MvvmNano;
using MvvmNano.Forms;
using PartyUp.Service.Interface;
using PartyUp.Service.Service;
using PartyUp.ViewModel;
using Xamarin.Forms; 

namespace PartyUp
{
    public class App : MvvmNanoApplication 
    {
        public App()
        {
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
            AddSiteToDetailPages(new MasterDetailData(typeof(EventPickerViewModel), "Pick a party"));
            AddSiteToDetailPages(new MasterDetailData(typeof(CreatePartyViewModel), "Create"));
            AddSiteToDetailPages(new MasterDetailData(typeof(HistoryViewModel), "History"));
            AddSiteToDetailPages(new MasterDetailData(typeof(SettingViewModel), "Setting"));
            AddSiteToDetailPages(new MasterDetailData(typeof(AboutViewModel),"About"));
            
            Device.BeginInvokeOnMainThread(async ()=>
            {
                await StartupSync();
            });


        } 

        private async Task StartupSync()
        {
            await MvvmNanoIoC.Resolve<ICacheService>().RefreshPartys();
        }

        private void RegisterInterfaces()
        {
            MvvmNanoIoC.Register<IClientService, ClientService>();
            MvvmNanoIoC.RegisterAsSingleton<ICacheService, CacheService>();
        }
    }
}
