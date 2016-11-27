using App2Night.CustomView.View;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class NavigationPage : FreshBaseContentPage
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
            //DetailListView.ItemTemplate = new DataTemplate(typeof(MenuTemplate));

            syncButton.ButtonLabel.FontSize = 40;
            syncButton.ButtonLabel.FontFamily = "FontAwesome";
            syncButton.SetBinding(CustomButton.CommandProperty,"SyncCommand");
             
            _layoutGrid = new Grid
            {  
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(7, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star)} 
                },
                Children =
                { 
                    //DetailListView,
                    { syncButton, 0, 1}
                }
            };

            //MasterContent = _layoutGrid;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); 
          //  DetailListView.RowHeight =  (int) ( (Height/7.0*5) / ((MvvmNanoApplication)Application.Current).MasterDetails.Count);
        }
    }
}