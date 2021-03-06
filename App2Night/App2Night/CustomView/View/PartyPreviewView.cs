﻿using System;
using System.Collections.Generic;
using App2Night.Model.Language;
using App2Night.Model.Model;
using App2Night.PageModel;
using App2Night.PageModel.SubPages;
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
        private static int _defaultFontSize = 16;
        private static int _defaultIconSize = 50;
        #region Views
        private Party Party => (Party) Item;
        private readonly MapWrapper _map;
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
        PartyRatingView _ratingView = new PartyRatingView{ RatingVisible = false};
         
        Label _dateLabel, _startTimeLabel, _genreLabel, _priceLabel; 
        #endregion

        public PartyPreviewView(Party party, double parentHeight, double parentWidth) : base(party.Name, party)
        {
            BackgroundColor = Color.White;
            // Set Label for Information of party
            DescriptionLabel(out _dateLabel, out _startTimeLabel, out _genreLabel, out _priceLabel);
            SetBindings();
            MoreEvent += OnMoreEventTapped;
            _map = SetMap(party);
            // Set HeightRepuest
            _map.Map.HeightRequest = 3*(parentWidth/5);
            var layoutGrid = CreateInputs();
            // Set columnSpan of rateButton to centrate 
            // Set HeightRequest 
            HeightRequest = parentWidth * 2 / 3.0;
            Content = new ScrollView { Content = layoutGrid };
        }

        /// <summary>
        /// Sets MapWrapper with Poisitions of Party.
        /// </summary>
        /// <param name="party"></param>
        /// <returns><see cref="MapWrapper"/> with Party position.</returns>
        private MapWrapper SetMap(Party party)
        {
            var position = new Position(party.Location.Latitude, party.Location.Longitude);
            return new MapWrapper(new Map(MapSpan.FromCenterAndRadius(
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
        }

        private void SetBindings()
        {
            _dateLabel.SetBinding(Label.TextProperty, "Date", stringFormat: AppResources.Date);
            _startTimeLabel.SetBinding(Label.TextProperty, "Date", stringFormat: AppResources.Time);
            _genreLabel.SetBinding(Label.TextProperty, "MusicGenre");
            _priceLabel.SetBinding(Label.TextProperty, "Price");
            _ratingView.SetBinding(PartyRatingView.PartyProperty, ".");
        }
        /// <summary>
        /// Initializes description label.
        /// </summary>
        /// <param name="dateLabel"></param>
        /// <param name="startTimeLabel"></param>
        /// <param name="genreLabel"></param>
        /// <param name="priceLabel"></param>
        private void DescriptionLabel(out Label dateLabel, out Label startTimeLabel, out Label genreLabel, out Label priceLabel)
        {
            dateLabel = new Label { Style = _infoLabelStyle, VerticalOptions = LayoutOptions.Center};
            startTimeLabel = new Label { Style = _infoLabelStyle, VerticalOptions = LayoutOptions.Center };
            genreLabel = new Label { Style = _infoLabelStyle, VerticalOptions = LayoutOptions.Center };
            priceLabel = new Label { Style = _infoLabelStyle, VerticalOptions = LayoutOptions.Center };
        }
        /// <summary>
        /// Opens either <see cref="PartyDetailViewModel"/> or <see cref="MyPartyDetailViewModel"/> if party if hosted by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnMoreEventTapped(object sender, EventArgs eventArgs)
        {
             FreshIOC.Container.Resolve<DashboardPageModel>().OpenMore(Party); 
        } 

        /// <summary>
        /// Creates <see cref="StackLayout"/> with map, rating and description of party.
        /// </summary>
        /// <returns></returns>
        private StackLayout CreateInputs()
        {
            // Set ResourceDictionary 
            var views = new ResourceDictionary()
            {
                {"\uf073", _dateLabel},
                {"\uf017", _startTimeLabel},
                {"\uf001", _genreLabel},
                {"\uf155", _priceLabel}
            };
            var descriptionGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
                },
            };
            int rowCounter = 1;
            foreach (KeyValuePair<string, object> valuePair in views)
            {
                descriptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                descriptionGrid.Children.Add(new Label { Text = valuePair.Key, Style = _infoLabelStyle, HorizontalOptions = LayoutOptions.Start, FontFamily = "FontAwesome", FontSize = _defaultIconSize, VerticalOptions = LayoutOptions.Center }, 0, rowCounter);
                descriptionGrid.Children.Add((Xamarin.Forms.View)valuePair.Value, 1, rowCounter);
                rowCounter++;
            }
            return new StackLayout
            {
                Children =
                {
                    _map,
                    _ratingView,
                    new Frame
                    {
                        Content = descriptionGrid,
                        Margin = 5,
                        Padding = 5,
                    },
                }
            };
        }
    }
}