using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.PageModel;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class SettingPage : CustomContentPage
    {
        private static int _defaultFontSize = 18;

        #region Views

        private InputContainer<Entry> _cityNameEntry = new InputContainer<Entry>
        {
            Input =
            {
                Placeholder = AppResources.Cityname,
            },
            IconCode = "\uf279"
        };

        private InputContainer<Entry> _streetEntry = new InputContainer<Entry>
        {
            Input =
            {
                Placeholder = AppResources.StrName,
            },
            IconCode = "\uf0f3"
        };

        private InputContainer<Entry> _numberEntry = new InputContainer<Entry>
        {
            Input =
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = AppResources.HNumber,
            },
        };

        InputContainer<Entry> _zipCodetEntry = new InputContainer<Entry>
        {
            Input =
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = AppResources.Zipcode,
            },
        };

        Switch _gpsEnabledSwitch = new Switch
        {
            HorizontalOptions = LayoutOptions.End
        };

        Slider _gpsRangeSlider = new Slider(5, 200, 1);

        private Grid _grid;

        Button _clearCacheButton = new Button
        {
            Text = AppResources.ClearCache,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        CustomButton _readAgButton = new CustomButton
        {
            ButtonLabel = {FontFamily = "FontAwesome", Text = "\uf061", FontSize = 25},
            HorizontalOptions = LayoutOptions.End,
        };

        #endregion

        public SettingPage()
        {
            SetBindings();
            _gpsEnabledSwitch.Toggled += LocationChanger;
            _grid = CreateInputRows();
            StackLayout stackLayout = CreateFrames();
            Content = new ScrollView { Content = stackLayout };
        }
        /// <summary>
        /// Creates Frames on Page.
        /// </summary>
        /// <returns></returns>
        private StackLayout CreateFrames()
        {
            return new StackLayout
            { 
                Children =
                {
                    // Gps Setting Section
                    new Frame
                    {
                        Margin = 5,
                        Padding = 5,
                        Content = new StackLayout
                        {
                            Children =
                            {
                                new Label {Text = AppResources.GpsSettingHeader, FontSize = _defaultFontSize},
                                _gpsRangeSlider,
                                new Grid
                                {
                                    Children =
                                    {
                                        _gpsEnabledSwitch,
                                        new Label
                                        {
                                            Text = AppResources.EnableGps,
                                            HorizontalOptions = LayoutOptions.Start
                                        }
                                    }
                                },
                                _grid,
                                new Label {Text = AppResources.GpsUsage}
                            }
                        }
                    },
                    // Clear Cache Section
                    new Frame
                    {
                        Margin = 5,
                        Padding = 5,
                        Content = new StackLayout
                        {
                            Children =
                            {
                                new Label
                                {
                                    Text = AppResources.ClearCacheSettingsHeader,
                                    FontSize = _defaultFontSize
                                },
                                _clearCacheButton,
                                new Label {Text = AppResources.ClearCacheUsage}
                            }
                        }
                    },
                    // Agb Section
                    new Frame
                    {
                        Margin = 5,
                        Padding = 5,
                        Content = new StackLayout
                        {
                            Children =
                            {
                                new Label
                                {
                                    Text = AppResources.AgbHeader,
                                    FontSize = _defaultFontSize
                                },
                                new Grid
                                {
                                    Children =
                                    {
                                        _readAgButton,
                                        new Label
                                        {
                                            Text = AppResources.AgbContent,
                                            HorizontalOptions = LayoutOptions.Start
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private void SetBindings()
        {
            // configure GPS range 
            _gpsRangeSlider.SetBinding(Slider.ValueProperty, nameof(SettingViewModel.SelectedRange));
            _gpsEnabledSwitch.SetBinding(Switch.IsToggledProperty, nameof(SettingViewModel.GpsEnabled));
            // bind entries to validate input
            _zipCodetEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.Zipcode));
            _cityNameEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.CityName));
            _numberEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.HouseNumber));
            _streetEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.StreetName));
            // checks if current input is position on map and change symbol
            _zipCodetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, nameof(SettingViewModel.ValidZipcode));
            _cityNameEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(SettingViewModel.ValidCityname));
            _numberEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(SettingViewModel.ValidHousenumber));
            _streetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty,
                nameof(SettingViewModel.ValidHousenumber));
            // checks if user want to delete cache
            _clearCacheButton.SetBinding(Button.CommandProperty, nameof(SettingViewModel.ValidateClearCacheCommand));
            _readAgButton.SetBinding(CustomButton.CommandProperty, nameof(SettingViewModel.MoveToReadAgbCommand));
        }

        /// <summary>
        /// Expands entries for manuel location input if <see cref="Switch.IsToggled"/>, otherwise entries collapse and hidden for user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationChanger(object sender, ToggledEventArgs e)
        {
            if (((Switch) sender).IsToggled)
            {
                _gpsRangeSlider.IsEnabled = true;
                CollapseLocationChanger();
            }
            else
            {
                _gpsRangeSlider.IsEnabled = false;
                ExpandLocationChanger();
            }
        }

        /// <summary>
        ///  Animates <see cref="Grid.HeightRequest"/> from 0 to 1 to show it. Changes visibility to true.
        /// </summary>
        /// <returns></returns>
        private void ExpandLocationChanger()
        {
            _zipCodetEntry.IsVisible = true;
            _cityNameEntry.IsVisible = true;
            _streetEntry.IsVisible = true;
            _numberEntry.IsVisible = true;
            Animation entryAnimation = new Animation(d => { _grid.HeightRequest = d*100; }, 0, 1);
            entryAnimation.Commit(this, "Animation", easing: Easing.BounceOut, length: 500U);
        }

        /// <summary>
        /// Animates <see cref="Grid.HeightRequest"/> from 1 to 0 to hide it. Finally changes visibility to false.
        /// </summary>
        /// <returns></returns>
        private void CollapseLocationChanger()
        {
            Animation entryAnimation = new Animation(d => { _grid.HeightRequest = d*100; }, 1, 0);
            entryAnimation.Commit(this, "Animation", easing: Easing.Linear, finished: (d, b) =>
            {
                _zipCodetEntry.IsVisible = false;
                _cityNameEntry.IsVisible = false;
                _streetEntry.IsVisible = false;
                _numberEntry.IsVisible = false;
            });
        }

        /// <summary>
        /// Initializes Grid for <see cref="Entry"/>'s
        /// </summary>
        /// <returns></returns>
        private Grid CreateInputRows()
        {
            return new Grid
            {
                HeightRequest = 0,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                },
                Children =
                {
                    {
                        // Street name and house number entry
                        new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                            },
                            Children =
                            {
                                {_streetEntry, 0, 0},
                                {_numberEntry, 1, 0}
                            }
                        },
                        0, 0
                    },
                    {
                        new Grid
                        {
                            // City name and zipcode
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                            },
                            Children =
                            {
                                {_cityNameEntry, 0, 0},
                                {_zipCodetEntry, 1, 0}
                            }
                        },
                        0, 1
                    }
                }
            };
        }
    }
}