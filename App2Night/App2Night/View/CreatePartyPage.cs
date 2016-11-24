using System;
using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using App2Night.ViewModel;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.View
{
    /// <summary>
    /// Create party page.
    /// </summary>
    public class CreatePartyPage : ContentPageWithInfo<CreatePartyViewModel>
    {
        #region Views

        private InputContainer<Entry> _entryName = new InputContainer<Entry>
        {
			Input = {Placeholder = AppResources.Name},
			ValidationVisible = true,
            FontSize = 35,
			Image = "\uf1ae"
        };

        private InputContainer<Entry> _descriptionEntry = new InputContainer<Entry>
        {
			Input = {Placeholder = AppResources.Description},
			Image = "\uf040",
            HeightRequest = 100,
            FontSize = 35,
        };

        private readonly InputContainer<DatePicker> _datePicker = new InputContainer<DatePicker>
        {
            Input =
            {
                MinimumDate = DateTime.Now,
                MaximumDate = DateTime.Now.AddMonths(12)
            },
            Image = "\uf073",
            FontSize = 35,
        };

        private InputContainer<TimePicker> _timePicker = new InputContainer<TimePicker> { Image = "\uf017", FontSize = 35, };

        private InputContainer<EnumBindablePicker<MusicGenre>> _musicGenreSwitch =
			new InputContainer<EnumBindablePicker<MusicGenre>>{ Image = "\uf001", FontSize = 35, Input = { SelectedIndex = 0}};

        private InputContainer<Entry> _cityNameEntry = new InputContainer<Entry>
        {
            Input = { Placeholder = AppResources.Cityname },
            FontSize = 35,
            Image = "\uf279"
        };

        private InputContainer<Entry> _streetEntry = new InputContainer<Entry>
        {
			Input = {Placeholder = AppResources.StrName},
            FontSize = 35,
            Image = "\uf0f3"
        };

        private InputContainer<Entry> _numberEntry = new InputContainer<Entry>
        {
			Input = {Keyboard = Keyboard.Numeric, Placeholder = AppResources.HNumber},
            FontSize = 35,
        };

        private InputContainer<Entry> _locationEntry = new InputContainer<Entry>
        {
			Input = {Placeholder = AppResources.Location},
            FontSize = 35,
            Image = "\uf015"
        };

        InputContainer<Entry> _zipCodetEntry = new InputContainer<Entry>
        {
			Input = {Keyboard = Keyboard.Numeric, Placeholder = AppResources.Zipcode},
            FontSize = 35,
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
            BindToViewModel(_tapGesture, TapGestureRecognizer.CommandProperty, vm => vm.LoadImageCommand);
            BindToViewModel(_image, Image.SourceProperty, vm => vm.Image);
            _image.GestureRecognizers.Add(_tapGesture);
            // set title of the page
            Title = AppResources.CreateParty;
			// bind to view models
			BindToViewModel(_entryName.Input, Entry.TextProperty, vm => vm.Name);
			BindToViewModel(_entryName, InputContainer<Entry>.InputValidateProperty, vm => vm.ValidName);
			BindToViewModel(_descriptionEntry.Input, Entry.TextProperty, vm => vm.Description);
			BindToViewModel(_descriptionEntry, InputContainer<Entry>.InputValidateProperty, vm => vm.ValidDescription);
			BindToViewModel(_musicGenreSwitch.Input, EnumBindablePicker<MusicGenre>.SelectedItemProperty, vm => vm.MusicGenre);
			// date and time
			BindToViewModel(_datePicker.Input, DatePicker.DateProperty, vm => vm.Date);
            BindToViewModel(_datePicker, InputContainer<DatePicker>.InputValidateProperty, vm => vm.ValidDate);

            BindToViewModel(_timePicker.Input, TimePicker.TimeProperty, vm => vm.Time);  
            // address
            BindToViewModel(_streetEntry.Input, Entry.TextProperty, vm => vm.StreetName);
			BindToViewModel(_streetEntry, InputContainer<Entry>.InputValidateProperty, vm => vm.ValidStreetname);

            BindToViewModel(_cityNameEntry.Input, Entry.TextProperty, vm => vm.CityName);
            BindToViewModel(_cityNameEntry, InputContainer<Entry>.InputValidateProperty, vm => vm.ValidCityname);

            BindToViewModel(_numberEntry.Input, Entry.TextProperty, vm => vm.HouseNumber);
            BindToViewModel(_numberEntry, InputContainer<Entry>.InputValidateProperty, vm => vm.ValidHousenumber);

            BindToViewModel(_locationEntry.Input, Entry.TextProperty, vm => vm.LocationName);
            BindToViewModel(_locationEntry, InputContainer<Entry>.InputValidateProperty, vm => vm.ValidLocationname);

            BindToViewModel(_zipCodetEntry.Input, Entry.TextProperty, vm => vm.Zipcode);
            BindToViewModel(_zipCodetEntry, InputContainer<Entry>.InputValidateProperty, vm => vm.ValidZipcode);

            //Buttons
            BindToViewModel(_acceptButton, CustomButton.CommandProperty, vm => vm.CreatePartyCommand);

            _image.IsVisible = false;
            // Change grid columns and rows if the device is windows
            if (Device.OS == TargetPlatform.Windows)
            {
            }
            // set Content
            _tableView.Root = new TableRoot
            {
                new TableSection()
                {
                    new ViewCell {View = _map }
                },
                new TableSection(AppResources.Description)
                {
                    new ViewCell {View = _entryName},
                    new ViewCell {View = _descriptionEntry},
                    new ViewCell {View = _musicGenreSwitch},
                    new ViewCell {View = _timePicker},
                    new ViewCell {View = _datePicker},
                },
				new TableSection(AppResources.Location)
                {
                    new ViewCell {View = _map},
                    new ViewCell { View = _locationEntry },
					// street name and number
                    new ViewCell {View = new Grid
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
                        }
                    }},
					// location name and zipcode
                    new ViewCell {View = new Grid
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
                        } 
                    }},
					// accept and remove btn
					new ViewCell {View = new Grid
					{
						ColumnDefinitions =
						{
							new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
							new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
						},
						Children =
						{
								{_acceptButton, 0,0},
								{_deleteButton, 1,0}
						} 
					}}, 
                }
            };


            Content = _tableView;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            _headerMap.HeightRequest = Width;
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
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)

            await DisplayAlert("File Location", file.Path, "OK");

            _image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
        /// <summary>
        /// Creates new party with the specific values of <see cref="T:App2Night.View.CreatePartyPage"/>.
        /// </summary>
        /// <param name="o">O.</param>
        /// <param name="e">E.</param>
        private void Accept(Object o, EventArgs e)
        {
            TappedAnimation(_acceptButton);
        }

        /// <summary>
        /// Resets all inputs of <see cref="T:App2Night.View.CreatePartyPage"/>.
        /// </summary>
        /// <param name="o">O.</param>
        /// <param name="e">E.</param>
        void Delete(Object o, EventArgs e)
        {
            TappedAnimation(_deleteButton);
            _entryName.Input.Text = "";
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


        /// <summary>
        /// Releases all resource used by the <see cref="T:App2Night.View.CreatePartyPage"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:App2Night.View.CreatePartyPage"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="T:App2Night.View.CreatePartyPage"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:App2Night.View.CreatePartyPage"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:App2Night.View.CreatePartyPage"/> was occupying.</remarks>
        public override void Dispose()
        {
            base.Dispose();
            // reset event handler
            _acceptButton.ButtonTapped -= Accept;
            _deleteButton.ButtonTapped -= Delete;
        }

        #endregion
    }
}