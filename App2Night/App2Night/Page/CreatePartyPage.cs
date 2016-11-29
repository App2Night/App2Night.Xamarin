﻿using System;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page
{
    /// <summary>
    /// Create party page.
    /// </summary>
    public class CreatePartyPage : FreshBaseContentPage
    {
        //Default thicknes
        private static Thickness _defaultMargin = new Thickness(5, 0);

        #region Views

        private InputContainer<Entry> _nameEntry = new InputContainer<Entry>
        {
			Input = {Placeholder = AppResources.PartyName},
			ValidationVisible = true, 
			IconCode = "\uf0fc",
            Margin = _defaultMargin
        };

        private InputContainer<Entry> _descriptionEntry = new InputContainer<Entry>
        {
			Input = {Placeholder = AppResources.Description},
			IconCode = "\uf040",
            HeightRequest = 100,
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

        private InputContainer<TimePicker> _timePicker = new InputContainer<TimePicker> { IconCode = "\uf017" };

        private InputContainer<EnumBindablePicker<MusicGenre>> _musicGenreSwitch =
			new InputContainer<EnumBindablePicker<MusicGenre>>{ IconCode = "\uf001",   Input = { SelectedIndex = 0},
                Margin = _defaultMargin
            };

        private InputContainer<Entry> _cityNameEntry = new InputContainer<Entry>
        {
            Input = { Placeholder = AppResources.Cityname }, 
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

        private CustomButton _deleteButton = new CustomButton
        {
            Text = "\uf1f8",
            ButtonLabel = {FontFamily = "FontAwesome", FontSize = 50},
        };

        private CustomButton _acceptButton = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel = {FontFamily = "FontAwesome", FontSize = 50},
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
            

            _map = new MapWrapper(_headerMap);
            // set tap gesture reconizer
            _tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "LoadImageCommand");
            _image.SetBinding(Image.SourceProperty, "Image");
            _image.GestureRecognizers.Add(_tapGesture);
            // set title of the page
            Title = AppResources.CreateParty;
			// bind to view models
			_nameEntry.Input.SetBinding( Entry.TextProperty, "Name");

            _nameEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidName");

            _descriptionEntry.Input.SetBinding( Entry.TextProperty, "Description");

            _descriptionEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidDescription");

            _musicGenreSwitch.Input.SetBinding(EnumBindablePicker<MusicGenre>.SelectedItemProperty, "MusicGenre");
            // date and time
            _datePicker.Input.SetBinding(DatePicker.DateProperty, "Date");
            _datePicker.SetBinding( InputContainer<DatePicker>.InputValidateProperty, "ValidDate");

            _timePicker.Input.SetBinding(TimePicker.TimeProperty, "Time");  
            // address
            _streetEntry.Input.SetBinding( Entry.TextProperty, "StreetName");

            _streetEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidStreetname");

            _cityNameEntry.Input.SetBinding( Entry.TextProperty, "CityName");
            _cityNameEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidCityname");

            _numberEntry.Input.SetBinding( Entry.TextProperty, "HouseNumber");
            _numberEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidHousenumber");

            _locationEntry.Input.SetBinding( Entry.TextProperty, "LocationName");
            _locationEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidLocationname");

            _zipCodetEntry.Input.SetBinding( Entry.TextProperty, "Zipcode");
            _zipCodetEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidZipcode");

            //Buttons
            _acceptButton.SetBinding( CustomButton.CommandProperty, "CreatePartyCommand");

            _image.IsVisible = false;
            // Change grid columns and rows if the device is windows
            if (Device.OS == TargetPlatform.Windows)
            {
            }

            var layout = new StackLayout
            {
                Spacing = 5,
                Children =
                { 
                    _nameEntry,
                    _descriptionEntry,
                    _musicGenreSwitch,
                    _datePicker,
                    _timePicker,
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
                            {_streetEntry, 0,0},
                            {_numberEntry, 1,0}
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
                            {_cityNameEntry, 0,0},
                            {_zipCodetEntry, 1,0}
                        },
            Margin = _defaultMargin
                    }
                }
            };

            Content = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)}
                },
                Children =
                {
                    new ScrollView
                    {
                        Content = layout
                    },
                    { new BoxView { Color = Color.Accent }, 0, 1  },
                     
                }
            }; 
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            _headerMap.HeightRequest = width;
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

        #region Events
        private async void MediaPicker(Object o, EventArgs e)
        {
            //await CrossMedia.Current.Initialize();

            //if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //{
            //    await DisplayAlert("No Camera", ":( No camera available.", "OK");
            //}

            //var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //{
            //    Directory = "Sample",
            //    Name = "test.jpg"
            //});

            //if (file == null)

            //await DisplayAlert("File Location", file.Path, "OK");

            //_image.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});
        }
        /// <summary>
        /// Creates new party with the specific values of <see cref="T:App2Night.Page.CreatePartyPage"/>.
        /// </summary>
        /// <param name="o">O.</param>
        /// <param name="e">E.</param>
        private void Accept(Object o, EventArgs e)
        {
            TappedAnimation(_acceptButton);
        }

        /// <summary>
        /// Resets all inputs of <see cref="T:App2Night.Page.CreatePartyPage"/>.
        /// </summary>
        /// <param name="o">O.</param>
        /// <param name="e">E.</param>
        void Delete(Object o, EventArgs e)
        {
            TappedAnimation(_deleteButton);
            _nameEntry.Input.Text = "";
            _descriptionEntry.Input.Text = "";
            _datePicker.Input.Date = DateTime.Now;
        }

        /// <summary>
        /// Tappeds the animation.
        /// </summary>
        /// <param name="view">View.</param>
        void TappedAnimation(Xamarin.Forms.View view)
        {
            var animation = new Animation(d => { view.Scale = d; }, 1, 1.6);
            var nextAnimation = new Animation(d => { view.Scale = d; }, 1.6, 1);
            animation.Commit(this, "Scale", length: 250, finished: delegate { nextAnimation.Commit(this, "Descale"); });
        }

        protected override void OnDisappearing()
        {
            // reset event handler
            _acceptButton.ButtonTapped -= Accept;
            _deleteButton.ButtonTapped -= Delete;
            base.OnDisappearing();
        } 
        #endregion
    }
}