using System;
using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using App2Night.PageModel;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page
{
    /// <summary>
    /// Create party page.
    /// </summary>
    public class CreatePartyPage : CustomContentPage
    {
        //Default thicknes
        private static Thickness _defaultMargin = new Thickness(5, 0);
        public static int CommandHeight = 70;

        #region Views

        private InputContainer<Entry> _nameEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.PartyName},
            ValidationVisible = true,
            IconCode = "\uf0fc",
            Margin = _defaultMargin
        };

        private InputContainer<Editor> _descriptionEntry = new InputContainer<Editor>
        {
            Input =
            {
                HeightRequest = 100
            },
            IconCode = "\uf040",
            Margin = _defaultMargin
        };

        private readonly InputContainer<DatePicker> _datePicker = new InputContainer<DatePicker>
        {
            Input =
            {
                MinimumDate = DateTime.Now,
                MaximumDate = DateTime.Now.AddMonths(12)
            },
            IconCode = "\uf073",
            Margin = _defaultMargin
        };

        private InputContainer<TimePicker> _timePicker = new InputContainer<TimePicker>
        {
            IconCode = "\uf017",
            Margin = _defaultMargin
        };

        private InputContainer<EnumBindablePicker<MusicGenre>> _musicGenreSwitch =
            new InputContainer<EnumBindablePicker<MusicGenre>>
            {
                IconCode = "\uf001",
                Input = {SelectedIndex = 0},
                Margin = _defaultMargin
            };

        private InputContainer<Entry> _cityNameEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.Cityname},
            IconCode = "\uf279"
        };

        private InputContainer<Entry> _streetEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.StrName},
            IconCode = "\uf0f3"
        };

        private InputContainer<Entry> _numberEntry = new InputContainer<Entry>
        {
            Input = {Keyboard = Keyboard.Numeric, Placeholder = AppResources.HNumber},
        };

        private InputContainer<Entry> _locationEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.Location},
            IconCode = "\uf015",
            Margin = _defaultMargin
        };

        InputContainer<Entry> _zipCodetEntry = new InputContainer<Entry>
        {
            Input = {Keyboard = Keyboard.Numeric, Placeholder = AppResources.Zipcode},
        };

        private InputContainer<Entry> _priceContainer = new InputContainer<Entry>
        {
            Input = {Keyboard = Keyboard.Numeric, Placeholder = "Price"}, //RESOURCE
            IconCode = "\uf155",
            Margin = _defaultMargin
        };

        private CustomButton _clearButton = new CustomButton
        {
            Text = "\uf1f8",
            ButtonLabel = {FontFamily = "FontAwesome", FontSize = 50},
            IsEnabled = false
        };

        private CustomButton _acceptButton = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel = {FontFamily = "FontAwesome", FontSize = 50},
            IsEnabled = false
        };

        private Map _headerMap = new Map();

        private Image _image = new Image
        {
            HeightRequest = 100,
            WidthRequest = 100
        };

        private MapWrapper _map;

        readonly TableView _tableView = new TableView
        {
            HorizontalOptions = LayoutOptions.Center,
            RowHeight = 75,
            HasUnevenRows = true
        };

        private readonly TapGestureRecognizer _tapGesture = new TapGestureRecognizer();

        #endregion

        public CreatePartyPage()
        {
            Title = AppResources.CreateParty;

            _map = new MapWrapper(_headerMap);
            // set tap gesture reconizer
            SetBindings();

            _image.IsVisible = false;
            // Change grid columns and rows if the device is windows
            if (Device.OS == TargetPlatform.Windows)
            {
            }

            ScrollView inputRows = CreateInputRows();
            Grid.SetRowSpan(inputRows, 2);
            Grid.SetColumnSpan(inputRows, 2);

            //SKIA Replace with gradient layer
            var gradientLayer = new BoxView
            {
                Color = Color.White.MultiplyAlpha(0.3)
            };


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
                    {gradientLayer, 0, 1},
                    {_clearButton, 0, 1},
                    {_acceptButton, 1, 1}
                }
            };

            Grid.SetColumnSpan(gradientLayer, 2);
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
                        new Frame
                        {
                            Content = new StackLayout
                            {
                                Children =
                                {
                                    _nameEntry,
                                    _descriptionEntry,
                                    _musicGenreSwitch,
                                    _datePicker,
                                    _timePicker,
                                },
                                Spacing = 5
                            },
                        },
                        new Frame
                        {
                            Content = new StackLayout
                            {
                                Children =
                                {
                                    _map,
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
                                            {_numberEntry, 1, 0}
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

        private void SetBindings()
        {
            //Buttons
            _acceptButton.SetBinding(CustomButton.IsEnabledProperty, nameof(CreatePartyViewModel.AcceptButtonEnabled));
            _clearButton.SetBinding(CustomButton.IsEnabledProperty, nameof(CreatePartyViewModel.ClearButtonEnabled));

            _acceptButton.SetBinding(CustomButton.CommandProperty, "CreatePartyCommand");
            _clearButton.SetBinding(CustomButton.CommandProperty, nameof(CreatePartyViewModel.ClearFormCommand));

            _tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "LoadImageCommand");
            _image.SetBinding(Image.SourceProperty, "Image");
            _image.GestureRecognizers.Add(_tapGesture);

            _priceContainer.Input.SetBinding(Entry.TextProperty, "Price");

            _nameEntry.Input.SetBinding(Entry.TextProperty, "Name");

            _nameEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, "ValidName");

            _descriptionEntry.Input.SetBinding(Editor.TextProperty, "Description");

            _descriptionEntry.SetBinding(InputContainer<Editor>.InputValidateProperty, "ValidDescription");

            _musicGenreSwitch.Input.SetBinding(EnumBindablePicker<MusicGenre>.SelectedItemProperty, "MusicGenre");
            // date and time
            _datePicker.Input.SetBinding(DatePicker.DateProperty, "Date");

            _timePicker.Input.SetBinding(TimePicker.TimeProperty, "Time");
            // address
            _streetEntry.Input.SetBinding(Entry.TextProperty, "StreetName");

            _streetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, "ValidStreetname");

            _cityNameEntry.Input.SetBinding(Entry.TextProperty, "CityName");
            _cityNameEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, "ValidCityname");

            _numberEntry.Input.SetBinding(Entry.TextProperty, "HouseNumber");
            _numberEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, "ValidHousenumber");

            _locationEntry.Input.SetBinding(Entry.TextProperty, "LocationName");
            _locationEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, "ValidLocationname");

            _zipCodetEntry.Input.SetBinding(Entry.TextProperty, "Zipcode");
            _zipCodetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, "ValidZipcode");
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            _headerMap.HeightRequest = width/2.0;
        }

        private void TextLength(object sender, TextChangedEventArgs e)
        {
            int _limit = 6; //Enter text limit

            string text = ((InputContainer<Entry>) sender).Input.Text; //Get Current Text
            if (text.Length > _limit) //If it is more than your character restriction
            {
                text = text.Remove(text.Length - 1); // Remove Last character
                ((InputContainer<Entry>) sender).Input.Text = text; //Set the Old value
            }
        }
    }
}