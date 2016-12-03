using System;
using System.Collections.Generic;
using App2Night.Data.Language;
using App2Night.Model.Model;
using App2Night.PageModel;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.CustomView.View
{
    /// <summary>
    /// Shows up a content view with selected party details. Contains a close and more btn.
    /// </summary>
    public class PartyPreviewView : PreviewView
    {
        #region Views
        private Party Party => (Party) Item;
        private readonly MapWrapper _map;

        Button _routeBtn = new Button
        {
            Text = "Route",
            HorizontalOptions = LayoutOptions.Center
        };

        Style _infoLabelStyle = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter
                {
                    Property = Label.FontSizeProperty,
                    Value = 20
                }
            }
        };
        #endregion

        public PartyPreviewView(Party party, double parentHeight, double parentWidth) : base(party.Name, party)
        {
            BackgroundColor = Color.White;
            // set button to calculate route
            _routeBtn.Clicked += OpenNavigationToParty;
            MoreEvent += OnMoreEventTapped;
            var position = new Position(party.Location.Latitude, party.Location.Longitude);
            _map = new MapWrapper(new Map(MapSpan.FromCenterAndRadius(
                position,
                Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                Pins =
                {
                    new Pin
                    {
                        Label = party.Name,
                        Position = position
                    }
                }
            });
            Grid.SetColumnSpan(_map, 2);
            var dateLabel = new Label {Style = _infoLabelStyle};
            dateLabel.SetBinding(Label.TextProperty, "Date", stringFormat: AppResources.Date);
            var startTimeLabel = new Label {Style = _infoLabelStyle};
            startTimeLabel.SetBinding(Label.TextProperty, "Date", stringFormat: AppResources.Time);
            var genreLabel = new Label {Style = _infoLabelStyle};
            genreLabel.SetBinding(Label.TextProperty, "MusicGenre");

            var views = new ResourceDictionary()
            {
                {AppResources.DateTime, dateLabel},
                {AppResources.StartTime, startTimeLabel},
                {AppResources.Genre, genreLabel}
            };

            var layoutGrid = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(200, GridUnitType.Absolute)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    _map
                }
            };
            int rowCounter = 1;
            foreach (KeyValuePair<string, object> valuePair in views)
            {
                layoutGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
                layoutGrid.Children.Add(
                    new Label {Text = valuePair.Key, Style = _infoLabelStyle, HorizontalOptions = LayoutOptions.Start},
                    0, rowCounter);
                layoutGrid.Children.Add((Xamarin.Forms.View) valuePair.Value, 1, rowCounter);
                rowCounter++;
            }
            HeightRequest = parentWidth*2/3.0;
            Content = new ScrollView {Content = layoutGrid};
        }

        private void OnMoreEventTapped(object sender, EventArgs eventArgs)
        {
             FreshIOC.Container.Resolve<DashboardPageModel>().OpenMore(Party);

        } 
         
        /// <summary>
        /// Closes <code>PartyPreviewView</code>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void CloseTapGestureRecognizerOnTapped(object sender, EventArgs eventArgs)
        {
            MoreEvent -= OnMoreEventTapped;
            CloseView();
        }
        /// <summary>
        /// Shows up the route from user to party.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OpenNavigationToParty(object sender, EventArgs eventArgs)
        {
            
        }  
    }
}