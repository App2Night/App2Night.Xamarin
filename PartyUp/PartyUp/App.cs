
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
                            Property = Label.VerticalOptionsProperty,
                            Value = LayoutOptions.Center
                        },
                        new Setter
                        {
                            Property = Label.HorizontalOptionsProperty,
                            Value = LayoutOptions.Center,
                            
                        },
                    },
                   
                },
                {"Test", new Style(typeof(Label))
                {
                    
                }}
            };
        }

        protected override void OnStart()
        {
            RegisterInterfaces();

            base.OnStart(); 
            SetUpMasterDetailPage<NavigationViewModel>();
            AddSiteToDetailPages(new MasterDetailData(typeof(DashboardViewModel), "Dashboard")); 
            AddSiteToDetailPages(new MasterDetailData(typeof(EventPickerViewModel), "Pick a party"));
        }

        private void RegisterInterfaces()
        {
            MvvmNanoIoC.Register<ICacheService, CacheService>();
            MvvmNanoIoC.Register<ICacheService, CacheService>();
        }
    }
}
