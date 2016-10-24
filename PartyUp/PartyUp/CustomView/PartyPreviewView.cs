using System;
using System.Linq;
using PartyUp.DependencyService;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PartyUp.CustomView
{
    public class PartyPreviewView : PreviewView
    {
        private readonly Party _party;
        private readonly IUserLocationService _userLocationService = Xamarin.Forms.DependencyService.Get<IUserLocationService>();
        private readonly TapGestureRecognizer _closeTapGestureRecognizer = new TapGestureRecognizer();

        public PartyPreviewView(Party party, double parentHeight, double parentWidth)
        {
            BackgroundColor = Color.White;
            _party = party;
            //Title label
            var titleLabel = new Label {Text = party.Name};
            //Close label
            var closeLabel = new Label {Text = "Close", HorizontalOptions = LayoutOptions.Start};
            //Set TabGesture
            closeLabel.GestureRecognizers.Add(_closeTapGestureRecognizer);
            _closeTapGestureRecognizer.Tapped += CloseTapGestureRecognizerOnTapped;
            // TODO Shows the current Position
            Coordinates userCoordinates = _userLocationService.GetUserCoordinates();
            Map map = new Map(MapSpan.FromCenterAndRadius(
                new Position(userCoordinates.Latitude, userCoordinates.Longitude),
                Distance.FromMiles(0.3)))
            {
                IsShowingUser = true
            };

            var layoutGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(40, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(parentWidth/2, GridUnitType.Absolute)}
                },
                Children =
                {
                    titleLabel,
                    closeLabel,
                    {map, 0, 1}
                }
            };
            //TODO Implments informations
            var stackLayout = new StackLayout
            {
                Children =
                {
                    layoutGrid,
                    new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                            new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                        },
                        Children =
                        {
                            {new Label
                            {
                                Text = "Name",
                                HorizontalOptions = LayoutOptions.Start,
                            },0,0},
                            {new Label
                            {
                                Text = _party.Name,
                                HorizontalOptions = LayoutOptions.End,
                            },1,0}
                        }
                    }
                }
            };

            HeightRequest = layoutGrid.RowDefinitions.Sum(o => o.Height.Value);
            Content = new ScrollView
            {
                Content = stackLayout
            };
        }

        private void CloseTapGestureRecognizerOnTapped(object sender, EventArgs eventArgs)
        {
            CloseView();
        }
    }
}