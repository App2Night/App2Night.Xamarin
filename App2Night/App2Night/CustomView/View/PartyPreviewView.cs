﻿using System;
using System.Collections.Generic;
using System.Globalization;
using App2Night.DependencyService;
using App2Night.Model.Model;
using App2Night.ViewModel;
using MvvmNano;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.CustomView.View
{
    public class PartyPreviewView : PreviewView
    {
        private Party Party => (Party) Item;
        private MapWrapper _map;
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
        private readonly IUserLocationService _userLocationService =
            Xamarin.Forms.DependencyService.Get<IUserLocationService>();

        private readonly TapGestureRecognizer _closeTapGestureRecognizer = new TapGestureRecognizer();

        public PartyPreviewView(Party party, double parentHeight, double parentWidth) : base(party.Name, party)
        { 
            BackgroundColor = Color.White;
            // set button to calculate route
            _routeBtn.Clicked += OpenNavigationToParty;
            Coordinates userCoordinates = _userLocationService.GetUserCoordinates(); 
            _map = new MapWrapper(new Map(MapSpan.FromCenterAndRadius(
                new Position(userCoordinates.Latitude, userCoordinates.Longitude),
                Distance.FromMiles(0.3)))
            {
                IsShowingUser = true 
            });
            Grid.SetColumnSpan(_map, 2);

            var dateLabel = new Label{Style = _infoLabelStyle};
            dateLabel.SetBinding(Label.TextProperty, "Date");
            var startTimeLabel = new Label { Style = _infoLabelStyle };
            startTimeLabel.SetBinding(Label.TextProperty, "Date");
            var genreLabel = new Label { Style = _infoLabelStyle };
            genreLabel.SetBinding(Label.TextProperty, "Genre");

            var views = new ResourceDictionary()
            {
                {"Date", dateLabel},
                {"Start time", startTimeLabel },
                {"Genre", genreLabel } 
            };

            var layoutGrid = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(200, GridUnitType.Absolute)}
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
                layoutGrid.Children.Add(new Label {Text = valuePair.Key, Style = _infoLabelStyle, HorizontalOptions = LayoutOptions.Start}, 0, rowCounter);
                layoutGrid.Children.Add((Xamarin.Forms.View) valuePair.Value, 1, rowCounter); 
                rowCounter++;
            }
            HeightRequest = parentWidth * 2/3.0;
            Content = new ScrollView { Content = layoutGrid };
        }

        public override void More()
        {
            base.More();
            MvvmNanoIoC.Resolve<NavigationViewModel>().OpenMore(Party);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var test = BindingContext;
        }

        private void CloseTapGestureRecognizerOnTapped(object sender, EventArgs eventArgs)
        {
            CloseView();
        }

        private void OpenNavigationToParty(object sender, EventArgs eventArgs)
        {
            
        }
    }
}