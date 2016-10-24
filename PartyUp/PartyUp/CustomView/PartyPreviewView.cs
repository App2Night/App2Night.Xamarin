using System;
using System.Linq;
using PartyUp.Model.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PartyUp.CustomView
{
    public class PartyPreviewView : PreviewView
    {
        private readonly Party _party;

      

        public PartyPreviewView(Party party, double parentHeight, double parentWidth)
        { 
            BackgroundColor = Color.White;
             
            _party = party;
            //Title label
            var titleLabel = new Label {Text = party.Name};

            //Close label
            var closeLabel = new Label {Text = "Close", HorizontalOptions = LayoutOptions.Start};
            TapGestureRecognizer closeTapGestureRecognizer = new TapGestureRecognizer();
            closeLabel.GestureRecognizers.Add(closeTapGestureRecognizer);
            closeTapGestureRecognizer.Tapped += CloseTapGestureRecognizerOnTapped;

            //TODO Set coordinates, customize the map
            Map map = new Map(MapSpan.FromCenterAndRadius(
                new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true
            };

            Grid layoutGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(parentWidth / 2, GridUnitType.Absolute) }
                },
                Children =
                {
                    titleLabel,
                    closeLabel,
                    {map, 0, 1 }
                }
            };
            Content = layoutGrid;

            //TODO Add more party information

            HeightRequest = layoutGrid.RowDefinitions.Sum(o => o.Height.Value);
        }

        private void CloseTapGestureRecognizerOnTapped(object sender, EventArgs eventArgs)
        {
            CloseView();
        } 
    }
}