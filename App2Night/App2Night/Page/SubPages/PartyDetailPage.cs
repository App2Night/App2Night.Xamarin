using System.Threading.Tasks;
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
        private static Thickness _defaultMargin = new Thickness(5, 0);
        public static int CommandHeight = 70;
        #region Views
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            IconCode = "\uf040",
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            IconCode = "\uf073",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _startDateTimeLabel = new InputContainer<Label>
        {
            IconCode = "\uf017",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _MusicGenreLabel = new InputContainer<Label>
        {
            IconCode = "\uf001",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _streetNameLabel = new InputContainer<Label>
        {
            IconCode = "\uf0f3",
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _houseNumberLabel = new InputContainer<Label>
        {
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _cityNameLabel = new InputContainer<Label>
        {
            IconCode = "\uf279",
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _zipcodeLabel = new InputContainer<Label>
        {
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Start, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        readonly Map _partyLocation = new Map()
        {
            HeightRequest = 200,
            IsShowingUser = true,
        };
        private Position _partyPosition;

        BoxView _gradientLayer = new BoxView
        {
            Color = Color.White.MultiplyAlpha(0.3),
            IsVisible = false
        };

        CustomButton _rateButton = new CustomButton
        {
            Text = "Rate",
            ButtonLabel =
            {
                FontSize = 50,
                FontFamily = "FontAwesome",
            },
            HorizontalOptions = LayoutOptions.Center

        };

        #endregion
        #region BindablePinProperty 
        public static BindableProperty MapPinsProperty = BindableProperty.Create(nameof(MapPins),
            typeof(Pin),
            typeof(DashboardPage),
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
        #endregion
        

        
        public PartyDetailPage()
        {
            SetBindings();
            Device.BeginInvokeOnMainThread(async () => await InitializeMapCoordinates());
            var inputRows = CreateInputRows();
            //SKIA Replace with gradient layer
            Grid.SetRowSpan(inputRows, 2);
            Grid.SetColumnSpan(inputRows, 2);
            Grid.SetColumn(_rateButton, 2);
            Content = new Grid
            {
                RowSpacing = 0,
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(CommandHeight, GridUnitType.Absolute)}
                },
                Children =
                {
                    inputRows,
                    {_gradientLayer, 0, 1},
                    {_rateButton, 0, 1},
                }
            };
        }

        private ScrollView CreateInputRows()
        {
            return new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 5,
                    Children =
                    {
                        _descriptionLabel,
                        _MusicGenreLabel,
                        _dateLabel,
                        _startDateTimeLabel,
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
                            Margin = _defaultMargin
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
                            Margin = _defaultMargin
                        },
                        new BoxView
                        {
                            HeightRequest = CommandHeight,
                            Color = Color.Transparent
                        }
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
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(coordinates.Latitude, coordinates.Longitude), Distance.FromKilometers(2));
            _partyLocation.MoveToRegion(mapSpan);
        }
        private void SetBindings()
        {
            this.SetBinding(TitleProperty, nameof(PartyDetailViewModel.Name));
            _descriptionLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.Description));
            _dateLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.Date), stringFormat: AppResources.Date);
            _MusicGenreLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.MusicGenre));
            _partyTypeLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.PartyType));
            _startDateTimeLabel.Input.SetBinding(Label.TextProperty, nameof(PartyDetailViewModel.CreationDateTime), stringFormat: AppResources.Time);
            _streetNameLabel.Input.SetBinding(Label.TextProperty, "Location.StreetName");
            _houseNumberLabel.Input.SetBinding(Label.TextProperty, "Location.HouseNumber");
            _cityNameLabel.Input.SetBinding(Label.TextProperty, "Location.CityName");
            _zipcodeLabel.Input.SetBinding(Label.TextProperty, "Location.Zipcode");
            this.SetBinding(DashboardPage.MapPinsProperty, nameof(PartyDetailViewModel.MapPins));
        }
    }
}