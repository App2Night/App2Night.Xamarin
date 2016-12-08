﻿using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page.SubPages
{
    public class PartyDetailPage : FreshBaseContentPage
    {
        private static int _defaultFontSize = 16;
        private static int _defaultIconSize = 50;
        private static Thickness _defaultMargin = new Thickness(5, 0);
        public static int CommandHeight = 70;

        #region Views
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            IconCode = "\uf040",
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            IconCode = "\uf073",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        InputContainer<Label> _startDateTimeLabel = new InputContainer<Label>
        {
            IconCode = "\uf017",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        InputContainer<Label> _MusicGenreLabel = new InputContainer<Label>
        {
            IconCode = "\uf001",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        Label _creationPartyLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Color.Gray.MultiplyAlpha(0.3),
        };
        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        InputContainer<Label> _priceLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        InputContainer<Label> _streetNameLabel = new InputContainer<Label>
        {
            IconCode = "\uf0f3",
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _houseNumberLabel = new InputContainer<Label>
        {
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center},
            ValidationVisible = false
        };
        InputContainer<Label> _cityNameLabel = new InputContainer<Label>
        {
            IconCode = "\uf279",
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _zipcodeLabel = new InputContainer<Label>
        {
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        readonly Map _partyLocation = new Map()
        {
            HeightRequest = 200,
            IsShowingUser = true,
        };

        private Position _partyPosition;

        CustomButton _rateButton = new CustomButton
        {
            Text = "Rate",
            ButtonLabel =
            {
                FontSize = 25,
                FontFamily = "FontAwesome",
            },
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        Label _generalRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            FontSize = _defaultFontSize,
            Text = "1"
        };

        Label _priceRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            FontSize = _defaultFontSize,
            Text = "1"
        };

        Label _locationRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            FontSize = _defaultFontSize,
            Text = "1"
        };

        Label _moodRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            FontSize = _defaultFontSize,
            Text = "1"
        };

        private Frame _ratingGrid;

        #endregion

        #region BindablePinProperty 

        public static BindableProperty MapPinsProperty = BindableProperty.Create(nameof(MapPins),
            typeof(Pin),
            typeof(DashboardPage),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                {
                    ((PartyDetailPage) bindable).MapPinsSet((Pin) newValue);
                }
            });

        public void MapPinsSet(Pin pin)
        {
            _partyLocation.Pins.Add(pin);
        }

        public Pin MapPins
        {
            get { return (Pin) GetValue(MapPinsProperty); }
            set { SetValue(MapPinsProperty, value); }
        }

        #endregion

        public PartyDetailPage()
        {
            SetBindings();
            Device.BeginInvokeOnMainThread(async () => await InitializeMapCoordinates());
            var inputRows = CreateInputRows();
            Content = inputRows;
            // Set ColumnSpan over all Columns to centrate view
            Grid.SetColumnSpan(_rateButton, 4);
        }
        /// <summary>
        /// Initialize ScrollView with all Views.
        /// </summary>
        /// <returns><see cref="ScrollView"/> for Content.</returns>
        private ScrollView CreateInputRows()
        {
            return new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 10,
                    Children =
                    {
                        // Add Party Rating
                        (_ratingGrid = CreateRatingColumns()),
                        new Frame
                        {
                            Content = new StackLayout
                            {
                                Spacing = 10,
                                Children =
                                {
                                    _descriptionLabel,
                                    _MusicGenreLabel,
                                    _partyTypeLabel,
                                    _dateLabel,
                                    _startDateTimeLabel,
                                    _priceLabel
                                }
                            },
                        },
                        new Frame
                        {
                            Content = new StackLayout
                            {
                                Spacing = 10,
                                Children =
                                {
                                    _partyLocation,
                                    new Grid
                                    {
                                        ColumnDefinitions =
                                        {
                                            new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                                        },
                                        Children =
                                        {
                                            {_streetNameLabel, 0, 0},
                                            {_houseNumberLabel, 1, 0}
                                        },
                                    },
                                    new Grid
                                    {
                                        ColumnDefinitions =
                                        {
                                            new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                                        },
                                        Children =
                                        {
                                            {_cityNameLabel, 0, 0},
                                            {_zipcodeLabel, 1, 0}
                                        },
                                    }
                                }
                            },
                        },
                        _creationPartyLabel,
                        new BoxView
                        {
                            HeightRequest = CommandHeight,
                            Color = Color.Transparent
                        }
                    }
                }
            };
        }
        /// <summary>
        /// Initialize Frame for Rating of Party.
        /// </summary>
        /// <returns><see cref="Frame"/></returns>
        private Frame CreateRatingColumns()
        {
            return new Frame
            {
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                    },
                    RowDefinitions = new RowDefinitionCollection
                    {
                      new RowDefinition {Height = new GridLength(3, GridUnitType.Star)},
                      new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                    },
                    Children =
                    {
                        // Location Rating 
                        {
                            new Label
                            {
                                Text = "\uf015",
                                FontFamily = "FontAwesome",
                                TextColor = Color.Gray.MultiplyAlpha(0.3),
                                FontSize = _defaultIconSize,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Start,
                            },
                            0, 0
                        },
                        {_locationRateLabel, 0, 0},
                        // Price Rating
                        {
                            new Label
                            {
                                Text = "\uf155",
                                FontFamily = "FontAwesome",
                                TextColor = Color.Gray.MultiplyAlpha(0.3),
                                FontSize = _defaultIconSize,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Start,
                            },
                            1, 0
                        },
                        {_priceRateLabel, 1, 0},
                        // Mood Rating 
                        {
                            new Label
                            {
                                Text = "\uf118",
                                FontFamily = "FontAwesome",
                                TextColor = Color.Gray.MultiplyAlpha(0.3),
                                FontSize = _defaultIconSize,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Start,
                            },
                            2, 0
                        },
                        {_moodRateLabel, 2, 0},
                        // General Rating
                        {
                            new Label
                            {
                                Text = "\uf29b",
                                FontFamily = "FontAwesome",
                                TextColor = Color.Gray.MultiplyAlpha(0.3),
                                FontSize = _defaultIconSize,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Start,
                            },
                            3, 0
                        },
                        {_generalRateLabel, 3, 0},
                        {_rateButton,0,1 }
                    }
                }
            };
        }

        private async Task InitializeMapCoordinates()
        {
            var coordinates = await CrossGeolocator.Current.GetPositionAsync();
            if (coordinates != null)
            {
                MoveMapToCoordinates(coordinates);
            }
        }

        private void MoveMapToCoordinates(Plugin.Geolocator.Abstractions.Position coordinates)
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(coordinates.Latitude, coordinates.Longitude),
                Distance.FromKilometers(2));
            _partyLocation.MoveToRegion(mapSpan);
        }

        private void SetBindings()
        {
            // Set Bindings of all Views
            this.SetBinding(TitleProperty, nameof(PartyDetailViewModel.Name));
            // general Information of Party
            _descriptionLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.Description));
            _dateLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.Date), stringFormat: AppResources.Date);
            _MusicGenreLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.MusicGenre));
            _partyTypeLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.PartyType));
            _startDateTimeLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.CreationDateTime), stringFormat: AppResources.Time);
            _creationPartyLabel.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.CreationDateTime), stringFormat: "Created on " + AppResources.Date);
            _priceLabel.Input.SetBinding(Label.TextProperty, "Party.Price");
            // Locationbinding of Party
            _streetNameLabel.Input.SetBinding(Label.TextProperty, "Location.StreetName");
            _houseNumberLabel.Input.SetBinding(Label.TextProperty, "Location.HouseNumber");
            _cityNameLabel.Input.SetBinding(Label.TextProperty, "Location.CityName");
            _zipcodeLabel.Input.SetBinding(Label.TextProperty, "Location.Zipcode");

            //_generalRateLabel.SetBinding(Label.TextProperty, );
            this.SetBinding(DashboardPage.MapPinsProperty, nameof(PartyDetailViewModel.MapPins));
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            // Set Size of Frame 
            _ratingGrid.HeightRequest = Width/3;
        }
    }
}