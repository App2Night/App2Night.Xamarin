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
        private static Thickness _defaultMargin = new Thickness(5, 0);

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

        Label _gpsSettingHeader = new Label
        {
            Text = AppResources.GpsSettingHeader
        };

        Label _clearCacheLabel = new Label
        {
            Text = AppResources.ClearCacheSettingsHeader,
            Margin = new Thickness(0,25,0,0)
        };
        Button _clearCacheButton = new Button
        {
            Text = AppResources.ClearCache,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        #endregion

        public SettingPage()
        {
            SetBindings();
            _gpsEnabledSwitch.Toggled += LocationChanger;
            _clearCacheButton.Clicked += ClearCache;
            _grid = CreateInputRows();
            var stackLayout = new StackLayout
            {
                Padding = new Thickness(5),
                Children =
                {
                    _gpsSettingHeader,
                    _gpsRangeSlider,
                    new Grid
                    {
                        Children =
                        {
                            _gpsEnabledSwitch,
                            new Label {Text = AppResources.EnableGps, HorizontalOptions = LayoutOptions.Start}
                        }
                    },
                    _grid,
                    new Label {Text = AppResources.GpsUsage},
                    _clearCacheLabel,
                    _clearCacheButton,
                    new Label {Text = AppResources.ClearCacheUsage},
                }
            };
            Content = stackLayout;
        }

        private void SetBindings()
        {
            _gpsRangeSlider.SetBinding(Slider.ValueProperty, nameof(SettingViewModel.SelectedRange));
            _gpsEnabledSwitch.SetBinding(Switch.IsToggledProperty, nameof(SettingViewModel.GpsEnabled));
            _zipCodetEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.Zipcode));
            _cityNameEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.CityName));
            _numberEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.HouseNumber));
            _streetEntry.SetBinding(Entry.TextProperty, nameof(SettingViewModel.StreetName));

            _zipCodetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, nameof(SettingViewModel.ValidZipcode));
            _cityNameEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, nameof(SettingViewModel.ValidCityname));
            _numberEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, nameof(SettingViewModel.ValidHousenumber));
            _streetEntry.SetBinding(InputContainer<Entry>.InputValidateProperty, nameof(SettingViewModel.ValidHousenumber));
        }

        private void ClearCache(object sender, EventArgs e)
        {
            UserDialogs.Instance.Confirm(new ConfirmConfig().SetMessage("").SetAction(b => {}));
        }

        private async void LocationChanger(object sender, ToggledEventArgs e)
        {
            if (((Switch) sender).IsToggled)
            {
                _gpsRangeSlider.IsEnabled = true;
                _gpsSettingHeader.IsEnabled = true;
                await CollapseLocationChanger();
            }
            else
            {
                _gpsRangeSlider.IsEnabled = false;
                _gpsSettingHeader.IsEnabled = false;
                await ExpandLocationChanger();
            }
        }

        private async Task ExpandLocationChanger()
        {
            _zipCodetEntry.IsVisible = true;
            _cityNameEntry.IsVisible = true;
            _streetEntry.IsVisible = true;
            _numberEntry.IsVisible = true;
            Animation entryAnimation = new Animation(d => { _grid.HeightRequest = d*100; }, 0, 1);
            entryAnimation.Commit(this, "Animation", easing: Easing.BounceOut, length: 500U);
        }

        private async Task CollapseLocationChanger()
        {
            Animation entryAnimation = new Animation(d => { _grid.HeightRequest = d*100; }, 1, 0);
            entryAnimation.Commit(this, "Animation", easing: Easing.Linear, length: 500U, finished: (d, b) =>
            {
                _zipCodetEntry.IsVisible = false;
                _cityNameEntry.IsVisible = false;
                _streetEntry.IsVisible = false;
                _numberEntry.IsVisible = false;
            });
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