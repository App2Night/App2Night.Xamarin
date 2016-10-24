using System;
using System.Collections.Generic;
using System.Diagnostics;
using MvvmNano.Forms;
using PartyUp.CustomView;
using PartyUp.Model.Model;
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
                //HeightRequest = 100,
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
            ButtonText = "More" 
        }; 

        private EnhancedContainer _historyContainer = new EnhancedContainer
        {
            Name = "History",
            ButtonText = "More",
            Content = new GallerieView
            { 
                Rows = 3,
                ElementSize = 100,
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

        GallerieView partieGallerie = new GallerieView
        {
            //HeightRequest = 200,
            ElementSize = 200,
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
        }; 

        public DashboardPage()
        {
            _myPartiesContainer.Content = partieGallerie;
            partieGallerie.ElementTapped += PartieSelected;

            BindToViewModel(_userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);
            BindToViewModel(_myPartiesContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToMyPartiesCommand);
            BindToViewModel(_historyContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToHistoryCommand);

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

            var profilePictureHeight = 200;

            //TODO Replace with actual map
            var mapView = new BoxView
            {
                Color = Color.Green,
                IsVisible = false
            };

            var profilePicture = new RoundView()
            {
                //TODO Replace with actuel profile picture
                Content = new BoxView() { Color = Color.Red },
                BackgroundColor = Color.White
            };

            var headerContainer = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(0, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(profilePictureHeight, GridUnitType.Absolute)},
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(profilePictureHeight*1.2, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)} 
                },
                Children =
                {
                    {profilePicture, 1,1 },
                    mapView
                }
            };
            Grid.SetColumnSpan(mapView,3);
            Grid.SetRowSpan(mapView,2); 

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

        /// <summary>
        /// Gets fired if a partie item gets tapped on.
        /// Will display the party info view.
        /// </summary> 
        private void PartieSelected(object sender, object o)
        {
            var party = (Party)o; 
        }
    }
}
