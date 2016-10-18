using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PartyUp.View
{
    public class DashboardPage : MvvmNanoContentPage<DashboardViewModel>
    {
        public DashboardPage()
        {
            var profilePictureView = new Map(
                    MapSpan.FromCenterAndRadius(
                    new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var lineBox = new BoxView {Color = Color.Aqua};

            var moreInfoLabel = new Label
            {
                Text = "More infos here.",
                StyleId = "Test"
            };

            var mainLayout = new Grid
            {
                Padding = new Thickness(0, 20,0,0),
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Absolute)}, 
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star)}, 
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star)},
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}, 
                },
                Children =
                {
                     {lineBox,0,1 },
                    { profilePictureView,1,0},
                   
                    {moreInfoLabel, 0, 3 }
                }
            }; 
            Grid.SetRowSpan(profilePictureView, 3);
            Grid.SetColumnSpan(lineBox, 3);
            Grid.SetColumnSpan(moreInfoLabel, 3); 
            Content = mainLayout;
        }
    }
}