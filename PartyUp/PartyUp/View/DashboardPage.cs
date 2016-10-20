using MvvmNano.Forms;
using PartyUp.CustomView;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class DashboardPage : MvvmNanoContentPage<DashboardViewModel>
    {
        private EnhancedContainer _userInfoContainer = new EnhancedContainer
        {
            Name = "User",
            ButtonText = "Edit" 
        };

        private EnhancedContainer _myPartiesContainer = new EnhancedContainer
        {
            Name = "MyEvents",
            ButtonText = "More"
        }; 

        private EnhancedContainer _historyContainer = new EnhancedContainer
        {
            Name = "History",
            ButtonText = "More"
        };


        public DashboardPage()
        { 
            BindToViewModel(_userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);
            

            BackgroundColor = Color.White;
            //    new Map(
            //    MapSpan.FromCenterAndRadius(
            //        new Position(37, -122), Distance.FromMiles(0.3)))
            //{
            //    IsShowingUser = true,
            //    HeightRequest = 100,
            //    WidthRequest = 960,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //}; 

            var headerContainer = new ContentView
            {
                BackgroundColor = Color.Aqua,
                HeightRequest = 150
            }; 
            var mainLayout = new StackLayout()
            {
                Spacing = 0,
                Children =
                {
                    headerContainer,
                    new BoxView
                    {
                        Color = Color.Black,
                        HeightRequest = 1
                    },
                    _userInfoContainer,
                    _myPartiesContainer,
                    _historyContainer
                }
            };

            Content = mainLayout;
        }
    }
}
