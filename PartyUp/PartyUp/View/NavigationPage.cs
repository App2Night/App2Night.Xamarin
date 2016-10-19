using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class NavigationPage : MvvmNanoMasterDetailPage<NavigationViewModel>
    {
        public NavigationPage()
        {
            var syncButton = new Button { Text = "Sync" }; 
            BindToViewModel(syncButton, Button.CommandProperty, o => o.SyncCommand);
            MasterContent = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)} 
                },
                Children =
                { 
                    DetailListView,
                    {syncButton, 0, 1},
                }
            };
        } 
    }
}