using System;
using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.PageModel.SubPages;
using App2Night.Service.Helper;
using FreshMvvm;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page.SubPages
{
    public class MyPartyDetailPage : FreshBaseContentPage
    {
        private static int _defaultFontSize = 16;
        private static Thickness _defaultMargin = new Thickness(5, 0);
        public static int CommandHeight = 70;

        #region Views

        private InputContainer<Entry> _nameEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.PartyName, IsEnabled = false},
            ValidationVisible = true,
            IconCode = "\uf0fc",
            Margin = _defaultMargin
        };

        private InputContainer<Editor> _descriptionEntry = new InputContainer<Editor>
        {
            Input =
            {
                HeightRequest = 100,
                IsEnabled = false
            },
            IconCode = "\uf040",
            Margin = _defaultMargin
        };

        private readonly InputContainer<DatePicker> _datePicker = new InputContainer<DatePicker>
        {
            Input =
            {
                MinimumDate = DateTime.Now,
                MaximumDate = DateTime.Now.AddMonths(12),
                IsEnabled = false
            },
            IconCode = "\uf073",
            Margin = _defaultMargin
        };

        private InputContainer<TimePicker> _startTimePicker = new InputContainer<TimePicker>
        {
            IconCode = "\uf017",
            Margin = _defaultMargin,
            Input = {IsEnabled = false}
        };

        private InputContainer<EnumBindablePicker<MusicGenre>> _musicGenrePicker =
            new InputContainer<EnumBindablePicker<MusicGenre>>
            {
                IconCode = "\uf001",
                Input = {IsEnabled = false},
                Margin = _defaultMargin
            };

        InputContainer<EnumBindablePicker<PartyType>> _partyTypePicker = new InputContainer
            <EnumBindablePicker<PartyType>>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            Input =
            {
                IsEnabled = false
            },
            ValidationVisible = false,
        };

        ToolbarItem _editToolbarItem = new ToolbarItem
        {
            Text = "Edit",
        };

        readonly Map _partyLocation = new Map()
        {
            HeightRequest = 200,
            IsShowingUser = true,
        };

        private Position _partyPosition;

        private CustomButton _cancelButton = new CustomButton
        {
            Text = "\uf00d",
            ButtonLabel =
            {
                FontFamily = "FontAwesome",
                FontSize = 50,
            },
            IsVisible = false,
            IsEnabled = true
        };

        private CustomButton _acceptButton = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel =
            {
                FontFamily = "FontAwesome",
                FontSize = 50,
            },
            IsVisible = false,
            IsEnabled = false
        };

        private InputContainer<Entry> _cityNameEntry = new InputContainer<Entry>
        {
            Input =
            {
                Placeholder = AppResources.Cityname,
                IsEnabled = false
            },
            IconCode = "\uf279"
        };

        private InputContainer<Entry> _streetEntry = new InputContainer<Entry>
        {
            Input =
            {
                Placeholder = AppResources.StrName,
                IsEnabled = false
            },
            IconCode = "\uf0f3"
        };

        private InputContainer<Entry> _houseNumberEntry = new InputContainer<Entry>
        {
            Input =
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = AppResources.HNumber,
                IsEnabled = false
            },
        };

        private InputContainer<Entry> _locationEntry = new InputContainer<Entry>
        {
            Input =
            {
                Placeholder = AppResources.Location,
                IsEnabled = false
            },
            IconCode = "\uf015",
            Margin = _defaultMargin
        };

        InputContainer<Entry> _zipCodetEntry = new InputContainer<Entry>
        {
            Input =
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = AppResources.Zipcode,
                IsEnabled = false
            },
        };

        BoxView _gradientLayer = new BoxView
        {
            Color = Color.White.MultiplyAlpha(0.3),
            IsVisible = false
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
                    ((MyPartyDetailPage) bindable).MapPinsSet((Pin) newValue);
                }
            });

        private void MapPinsSet(Pin pin)
        {
            _partyLocation.Pins.Add(pin);
        }

        public Pin MapPins
        {
            get { return (Pin) GetValue(MapPinsProperty); }
            set { SetValue(MapPinsProperty, value); }
        }

        #endregion

        public MyPartyDetailPage()
        {
            SetBindings();
            Device.BeginInvokeOnMainThread(async () => await InitializeMapCoordinates());
            ToolbarItems.Add(_editToolbarItem);
            _editToolbarItem.Clicked += SetEditEnable;
            _cancelButton.ButtonTapped += SetEditDisenable;
            var inputRows = CreateInputRows();
            //SKIA Replace with gradient layer
            Grid.SetRowSpan(inputRows, 2);
            Grid.SetColumnSpan(inputRows, 2);
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
                    {_cancelButton, 0, 1},
                    {_acceptButton, 1, 1}
                }
            };
        }

        /// <summary>
        /// Initializes ScrollView with Views. 
        /// </summary>
        /// <returns></returns>
        private ScrollView CreateInputRows()
        {
            return new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 5,
                    Children =
                    {
                        // Description of Party
                        new Frame
                        {
                            Content = new StackLayout
                            {
                                Children =
                                {
                                    _nameEntry,
                                    _descriptionEntry,
                                    _musicGenrePicker,
                                    _datePicker,
                                    _startTimePicker,
                                },
                                Spacing = 5
                            },
                        },
                        // Location of Party
                        new Frame
                        {
                            Content = new StackLayout
                            {
                                Children =
                                {
                                    _partyLocation,
                                    _locationEntry,
                                    new Grid
                                    {
                                        ColumnDefinitions =
                                        {
                                            new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                                        },
                                        Children =
                                        {
                                            {_streetEntry, 0, 0},
                                            {_houseNumberEntry, 1, 0}
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
                                            {_cityNameEntry, 0, 0},
                                            {_zipCodetEntry, 1, 0}
                                        },
                                        Margin = _defaultMargin
                                    },
                                },
                                Spacing = 5
                            },
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
        /// <summary>
        /// Sets editable of views true and start slide in animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SetEditEnable(object sender, EventArgs e)
        {
            _nameEntry.Input.IsEnabled = true;
            _descriptionEntry.Input.IsEnabled = true;
            _musicGenrePicker.Input.IsEnabled = true;
            _partyTypePicker.Input.IsEnabled = true;
            _datePicker.Input.IsEnabled = true;
            _startTimePicker.Input.IsEnabled = true;
            _streetEntry.Input.IsEnabled = true;
            _houseNumberEntry.Input.IsEnabled = true;
            _cityNameEntry.Input.IsEnabled = true;
            _zipCodetEntry.Input.IsVisible = true;

            await SlideInAnimtion();
        }
        /// <summary>
        /// Sets Views disenable and start slide out animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SetEditDisenable(object sender, EventArgs e)
        {
            _nameEntry.Input.IsEnabled = false;
            _descriptionEntry.Input.IsEnabled = false;
            _musicGenrePicker.Input.IsEnabled = false;
            _partyTypePicker.Input.IsEnabled = false;
            _datePicker.Input.IsEnabled = false;
            _startTimePicker.Input.IsEnabled = false;
            _streetEntry.Input.IsEnabled = false;
            _houseNumberEntry.Input.IsEnabled = false;
            _cityNameEntry.Input.IsEnabled = false;
            _zipCodetEntry.Input.IsVisible = false;
            await SlideOutAnimtion();
        }
        /// <summary>
        /// Slides out cancel and accept <see cref="CustomButton"/>
        /// </summary>
        /// <returns></returns>
        private async Task SlideOutAnimtion()
        {
            Animation slideOutAnimation = new Animation(d =>
            {
                _cancelButton.HeightRequest = d*70;
                _acceptButton.HeightRequest = d*70;
                _gradientLayer.HeightRequest = d*70;
            }, 1, 0);
            slideOutAnimation.Commit(this, "SlideOut", length: 1000U, easing: Easing.Linear, finished: (d, b) =>
            {
                _cancelButton.IsVisible = false;
                _acceptButton.IsVisible = false;
                _gradientLayer.IsVisible = false;
            });
        }
        /// <summary>
        /// Slides in cancel and accept <see cref="CustomButton"/>
        /// </summary>
        /// <returns></returns>
        private async Task SlideInAnimtion()
        {
            _gradientLayer.IsVisible = true;
            _cancelButton.IsVisible = true;
            _acceptButton.IsVisible = true;
            Animation slideOutAnimation = new Animation(d =>
            {
                _cancelButton.HeightRequest = d*70;
                _acceptButton.HeightRequest = d*70;
                _gradientLayer.HeightRequest = d*70;
            }, 0, 1);
            slideOutAnimation.Commit(this, "SlideOut", length: 1000U, easing: Easing.Linear);
        }

        private async Task InitializeMapCoordinates()
        {
            var coordinates = await CoordinateHelper.GetCoordinates();
            if (coordinates != null)
            {
                MoveMapToCoordinates(coordinates);
            }
        }

        private void MoveMapToCoordinates(Coordinates coordinates)
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(coordinates.Latitude, coordinates.Longitude),
                Distance.FromKilometers(2));
            _partyLocation.MoveToRegion(mapSpan);
        }

        private void SetBindings()
        {
            this.SetBinding(TitleProperty, "Party.Name");
            //Buttons
            _acceptButton.SetBinding(CustomButton.IsEnabledProperty, nameof(MyPartyDetailViewModel.AcceptButtonEnabled));
            _acceptButton.SetBinding(CustomButton.CommandProperty, nameof(MyPartyDetailViewModel.UpdatePartyCommand));
            _cancelButton.SetBinding(CustomButton.CommandProperty, nameof(MyPartyDetailViewModel.ClearFormCommand));
            // set name 
            _nameEntry.Input.SetBinding(Entry.TextProperty, nameof(MyPartyDetailViewModel.Name));
            _nameEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, nameof(MyPartyDetailViewModel.ValidName));
            // set description
            _descriptionEntry.Input.SetBinding(Editor.TextProperty, nameof(MyPartyDetailViewModel.Description));
            _descriptionEntry.SetBinding(InputContainer<Editor>.InputValidateProperty,
                nameof(MyPartyDetailViewModel.ValidDescription));
            // music genre
            _musicGenrePicker.Input.SetBinding(EnumBindablePicker<MusicGenre>.SelectedItemProperty,
                nameof(MyPartyDetailViewModel.MusicGenre));
            // date and time
            _datePicker.Input.SetBinding(DatePicker.DateProperty, nameof(MyPartyDetailViewModel.Date));
            // set start time
            _startTimePicker.Input.SetBinding(TimePicker.TimeProperty, nameof(MyPartyDetailViewModel.Time));
            // address
            _streetEntry.Input.SetBinding(Entry.TextProperty, nameof(MyPartyDetailViewModel.StreetName));
            _streetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(MyPartyDetailViewModel.ValidStreetname));
            // set city name
            _cityNameEntry.Input.SetBinding(Entry.TextProperty, nameof(MyPartyDetailViewModel.CityName));
            _cityNameEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(MyPartyDetailViewModel.ValidCityname));
            // set house number
            _houseNumberEntry.Input.SetBinding(Entry.TextProperty, nameof(MyPartyDetailViewModel.HouseNumber));
            _houseNumberEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(MyPartyDetailViewModel.ValidHousenumber));
            // set location
            _locationEntry.Input.SetBinding(Entry.TextProperty, nameof(MyPartyDetailViewModel.LocationName));
            _locationEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(MyPartyDetailViewModel.ValidLocationname));
            // set zipcode
            _zipCodetEntry.Input.SetBinding(Entry.TextProperty, nameof(MyPartyDetailViewModel.Zipcode));
            _zipCodetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(MyPartyDetailViewModel.ValidZipcode));
            // set MapPins
            this.SetBinding(DashboardPage.MapPinsProperty, nameof(PartyDetailViewModel.MapPins));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // remove events on button
            _editToolbarItem.Clicked -= SetEditEnable;
            _cancelButton.ButtonTapped -= SetEditDisenable;
        }
    }
}