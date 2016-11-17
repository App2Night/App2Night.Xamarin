using System.Linq;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class NavigationPage : MvvmNanoMasterDetailPage<NavigationViewModel>
    {
        Grid _layoutGrid;

        public NavigationPage()
        {
            if(Device.OS == TargetPlatform.iOS) Padding = new Thickness(0, 20, 0, 0);
            Title = "Menü";
            var syncButton = new CustomButton
            {
                Text = "Sync \uf0e2"
            };
            DetailListView.ItemTemplate = new DataTemplate(typeof(MenuTemplate));

            syncButton.ButtonLabel.FontSize = 40;
            syncButton.ButtonLabel.FontFamily = "FontAwesome";
            BindToViewModel(syncButton, CustomButton.CommandProperty, o => o.SyncCommand);
             
            _layoutGrid = new Grid
            {  
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(7, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star)} 
                },
                Children =
                { 
                    DetailListView,
                    { syncButton, 0, 1}
                }
            };

            MasterContent = _layoutGrid;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); 
            DetailListView.RowHeight =  (int) ( (Height/7.0*5) / ((MvvmNanoApplication)Application.Current).MasterDetails.Count);
        }
    }
}