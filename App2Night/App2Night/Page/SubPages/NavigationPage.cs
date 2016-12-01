using System.Linq;
using App2Night.CustomView.View;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class NavigationPage : ContentPage
    {
        private readonly ListView _menuListView;
        Grid _layoutGrid;

        public NavigationPage(ListView menuListView)
        {
            var userInfoContainer = new ContentView();
            _menuListView = menuListView;
            if(Device.OS == TargetPlatform.iOS) Padding = new Thickness(0, 20, 0, 0);
            Title = "Menü";
            var syncButton = new CustomButton
            {
                Text = "Sync \uf0e2"
            };
            //DetailListView.ItemTemplate = new DataTemplate(typeof(MenuTemplate));

            syncButton.ButtonLabel.FontSize = 40;
            syncButton.ButtonLabel.FontFamily = "FontAwesome";
            syncButton.SetBinding(CustomButton.CommandProperty,"SyncCommand");
             
            _layoutGrid = new Grid
            {  
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(7, GridUnitType.Star)} 
                },
                Children =
                {
                    userInfoContainer,
                    { menuListView, 0, 1}
                }
            };

            Content = _layoutGrid;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            _menuListView.RowHeight =  (int) ((Height/9.0*7) / 6);
        }
    }
}