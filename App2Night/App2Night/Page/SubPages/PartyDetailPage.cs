using System.Threading.Tasks;
using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Model;
using App2Night.PageModel.SubPages;
using App2Night.ValueConverter; 
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page.SubPages
{
    public class PartyDetailPage : CustomContentPage
    {
        // Set values for views 
        private static int _defaultFontSize = 16;
        private static int _defaultIconSize = 50;

        #region Views
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            IconCode = "\uf040",
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            IconCode = "\uf073",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        InputContainer<Label> _startDateTimeLabel = new InputContainer<Label>
        {
            IconCode = "\uf017",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };

        InputContainer<Label> _musicGenreLabel = new InputContainer<Label>
        {
            IconCode = "\uf001",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };

        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
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
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _houseNumberLabel = new InputContainer<Label>
        {
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        InputContainer<Label> _cityNameLabel = new InputContainer<Label>
        {
            IconCode = "\uf279",
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _zipcodeLabel = new InputContainer<Label>
        {
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };
        readonly Map _partyLocation = new Map()
        {
            HeightRequest = 200,
            IsShowingUser = true,
        };

        private VerticalGallerieView _gallerieView = new VerticalGallerieView
        {

        };

        private Position _partyPosition;

        PartyRatingView _partyRating = new PartyRatingView();

        Label _generalRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            FontSize = _defaultFontSize,
        };  

        private CustomButton _defaultButton = new CustomButton {IsVisible = false};

        #endregion

        #region BindablePinProperty 

        public static BindableProperty MapPinsProperty = BindableProperty.Create(nameof(MapPins),
            typeof(Pin),
            typeof(PartyDetailPage),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                {
                    ((PartyDetailPage)bindable).MapPinsSet((Pin)newValue);
                }
            });

        public void MapPinsSet(Pin pin)
        {
            _partyLocation.Pins.Add(pin);
        }

        public Pin MapPins
        {
            get { return (Pin)GetValue(MapPinsProperty); }
            set { SetValue(MapPinsProperty, value); }
        }

        public static BindableProperty PartyProperty = BindableProperty.Create(nameof(Party), typeof(Party),
            typeof(PartyDetailPage),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                {
                    ((PartyDetailPage)bindable).Party = (Party)newValue;
                }
            });

        private Party _party;
        public Party Party { get { return _party; } set { _party = value; } }
        #endregion

        public PartyDetailPage()
        {
            _gallerieView.Template = typeof(ParticipantTemplate); 

            SetBindings();
            this.SetBinding(PartyProperty, nameof(PartyDetailViewModel.Party));
            Device.BeginInvokeOnMainThread(async () => await InitializeMapCoordinates());
            var inputRows = CreateInputRows();
            Content = inputRows;
            // Set ColumnSpan over all Columns to centrate view 
            _partyRating.RatePressedEvent += RateParty;

        }
        /// <summary>
        /// Opens seleted party with <see cref="PartyPreviewView"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        private void RateParty(object sender, Party o)
        {
            PreviewItemSelected<Party, RateView>(o, new object[] { Height, Width });
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
                        new Frame()
                        {
                            Margin = 5,
                            Padding = 5,
                            Content = _partyRating
                        },
                        new Frame //Contains generel information
                        {
                            Margin = 5,
                            Padding = 5,
                            Content = new StackLayout
                            {
                                Spacing = 15,
                                Children =
                                {
                                    _descriptionLabel,
                                    _musicGenreLabel,
                                    _partyTypeLabel,
                                    _dateLabel,
                                    _startDateTimeLabel,
                                    _priceLabel
                                }
                            },
                        },
                        // Set Frame with Location Label's
                        new Frame
                        {
                            Margin = 5,
                            Padding = 0,
                            Content = new StackLayout
                            {
                                Spacing = 15,
                                Children =
                                {
                                    new MapWrapper(_partyLocation),
                                    new Grid
                                    { 
                                        Margin = new Thickness(5, 0),
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
                                        Margin = new Thickness(5, 0),
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
                        _gallerieView 
                    }
                }
            };
        } 

        private async Task InitializeMapCoordinates()
        {
            // gets current position
            var coordinates = await CrossGeolocator.Current.GetPositionAsync();
            if (coordinates != null)
            {
                MoveMapToCoordinates(coordinates);
            }
        }

        private void MoveMapToCoordinates(Plugin.Geolocator.Abstractions.Position coordinates)
        {
            // adds current position to map 
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
            _musicGenreLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.MusicGenre));
            _partyTypeLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.PartyType));
            _startDateTimeLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.CreationDateTime), stringFormat: AppResources.Time);
            _priceLabel.Input.SetBinding(Label.TextProperty, "Party.Price");
            // Locationbinding of Party
            _streetNameLabel.Input.SetBinding(Label.TextProperty, "Location.StreetName");
            _houseNumberLabel.Input.SetBinding(Label.TextProperty, "Location.HouseNumber");
            _cityNameLabel.Input.SetBinding(Label.TextProperty, "Location.CityName");
            _zipcodeLabel.Input.SetBinding(Label.TextProperty, "Location.Zipcode");
            // set binding of rating
            _generalRateLabel.SetBinding(Label.TextProperty, "Party.GeneralAvg", converter:new PercentageValueConverter());
            _partyRating.SetBinding(PartyRatingView.PartyProperty, "Party");
            _partyRating.SetBinding(PartyRatingView.RateCommandProperty, "Party");
            _partyRating.SetBinding(PartyRatingView.RatingVisibleProperty, "ValidRate");

            this.SetBinding(MapPinsProperty, nameof(PartyDetailViewModel.MapPins));

            //Participants
            _gallerieView.SetBinding(GallerieView.ItemSourceProperty, "Party.Participants");
            _gallerieView.SetBinding(GallerieView.IsVisibleProperty, "ParticipantsVisible"); 
        } 

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // remove event of button
            _partyRating.RatePressedEvent -= RateParty;
        }
    }
}