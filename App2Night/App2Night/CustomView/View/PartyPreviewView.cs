using System;
using System.Globalization;
using App2Night.DependencyService;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.CustomView.View
{
    public class PartyPreviewView : PreviewView
    {
        private Party Party => (Party) Item;
        private MapWrapper map;

        private readonly IUserLocationService _userLocationService =
            Xamarin.Forms.DependencyService.Get<IUserLocationService>();

        private readonly TapGestureRecognizer _closeTapGestureRecognizer = new TapGestureRecognizer();

        public PartyPreviewView(Party party, double parentHeight, double parentWidth) : base(party.Name, party)
        { 
            BackgroundColor = Color.White;
            // set button to calculate route
            var routeBtn = new Button
            {
                Text = "Route",
                HorizontalOptions = LayoutOptions.Center
            };
            // TODO Handle BtnClicked
            routeBtn.Clicked += CalculateRoute;
            Coordinates userCoordinates = _userLocationService.GetUserCoordinates(); 
            map = new MapWrapper(new Map(MapSpan.FromCenterAndRadius(
                new Position(userCoordinates.Latitude, userCoordinates.Longitude),
                Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 200
            }); 

            var view = new ScrollView
            {
                //TODO Header
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                    },
                    Children =
                    {
                        map,
                        {
                            new Label
                            {
                                Text = "Name",
                                HorizontalOptions = LayoutOptions.Start,
                            }
                            , 0, 1
                        },
                        {
                            new Label {Text = Party.Name, HorizontalOptions = LayoutOptions.End,}
                            , 1, 1
                        },
                        {
                            new Label
                            {
                                Text = "Date",
                                HorizontalOptions = LayoutOptions.Start,
                            },
                            0, 2
                        },
                        {
                            //TODO Handle DateFormating for system
                            new Label
                            {
                                Text = Party.Date.Date.ToString("HH:mm:ss dd.MM.yyyy",
                                    CultureInfo.CurrentUICulture),
                                HorizontalOptions = LayoutOptions.End,
                            },
                            1,2 
                        },
                        {
                            new Label
                            {
                                Text = "Music Genre",
                                HorizontalOptions = LayoutOptions.Start,
                            },
                            0, 3
                        },
                        {
                            new Label
                            {
                                Text = Party.MusicGenre.ToString(),
                                HorizontalOptions = LayoutOptions.End,
                            },
                            1, 3
                        },
                        {
                            new Label
                            {
                                Text = "Created",
                                HorizontalOptions = LayoutOptions.Start,
                            },
                            0, 4
                        },
                        {
                            new Label
                            {
                                Text = Party.CreationDateTime.ToString("dd MMMM yyyy",
                                    CultureInfo.CurrentUICulture),
                                HorizontalOptions = LayoutOptions.End,
                            },
                            1, 4
                        },
                        {
                            routeBtn, 0, 5
                        }
                    }
                }
            };
            //HeightRequest = layoutGrid.RowDefinitions.Sum(o => o.Height.Value);

            Grid.SetColumnSpan(map, 2);
            HeightRequest = parentWidth * 2/3.0;
            Content = view;
        }

        private void CloseTapGestureRecognizerOnTapped(object sender, EventArgs eventArgs)
        {
            CloseView();
        }

        private void CalculateRoute(object sender, EventArgs eventArgs)
        {
        }
    }
}