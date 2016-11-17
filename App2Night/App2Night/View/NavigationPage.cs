using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class NavigationPage : MvvmNanoMasterDetailPage<NavigationViewModel>
    {
        public NavigationPage()
        {
            if(Device.OS == TargetPlatform.iOS) Padding = new Thickness(0, 20, 0, 0);
            Title = "Menü";
            var syncButton = new CustomButton
            {
                Text = "Sync \uf0e2",
                
                Margin = new Thickness(0,20)
            };

            //DetailListView.RowHeight = 40; 

            DetailListView.ItemTemplate = new DataTemplate(typeof(MenuTemplate));

            syncButton.ButtonLabel.FontSize = 40;
            syncButton.ButtonLabel.FontFamily = "FontAwesome";
            BindToViewModel(syncButton, CustomButton.CommandProperty, o => o.SyncCommand);
            MasterContent = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(8, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)} 
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