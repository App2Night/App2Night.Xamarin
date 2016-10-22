using System.Collections.Generic;
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
            ButtonText = "Edit",
            Content = new GallerieView
            {
                HeightRequest = 100,
                ItemSource = new List<string>()
                {
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7"
                }
            }
        };

        private EnhancedContainer _myPartiesContainer = new EnhancedContainer
        {
            Name = "MyEvents",
            ButtonText = "More",
            Content = new GallerieView
            {
                HeightRequest = 200,
                MaxElementSize = 200,
                ItemSource = new List<string>()
                {
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7"
                }
            }
        }; 

        private EnhancedContainer _historyContainer = new EnhancedContainer
        {
            Name = "History",
            ButtonText = "More",
            Content = new GallerieView
            {
                HeightRequest = 190,
                MaxElementSize = 100,
                ItemSource = new List<string>()
                {
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7",
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7",
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7"
                }
            }
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

            var headerContainer = new RoundView()
            {
                Content = new BoxView() { Color = Color.Red},
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

            Content = new ScrollView
            {
                Content = mainLayout,
                Orientation  = ScrollOrientation.Vertical
            };
        }
    }
}
