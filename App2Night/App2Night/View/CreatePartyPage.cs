using System;
using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using App2Night.ViewModel;
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

        private InputContainer<Entry> _streetEntry = new InputContainer<Entry>
        {
			Input = {Placeholder = AppResources.StrName},
            FontSize = 35,
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

        private Map _headerMap = new Map()
        {
            HeightRequest = 500
        };

        private Image _image = new Image
        {
            HeightRequest = 100,
            WidthRequest = 100
        };

        private MapWrapper _map;

        TableView _tableView = new TableView
        {
            HorizontalOptions = LayoutOptions.Center,
            RowHeight = 75,
        };

        private TapGestureRecognizer _tapGesture = new TapGestureRecognizer();

        #endregion

        public CreatePartyPage()
        {
            _map = new MapWrapper(_headerMap); 
            // set tap gesture reconizer
            _tapGesture.Tapped += LoadImage;
            // set title of the page
            Title = AppResources.CreateParty;
            _image.IsVisible = false;
            // Change grid columns and rows if the device is windows
            if (Device.OS == TargetPlatform.Windows)
            {
            }
            // set Content
            _tableView.Root = new TableRoot
            {
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
                    new ViewCell {View = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                        },
                        Children =
                        {
                            {_locationEntry, 0,0},
                            {_zipCodetEntry, 1,0}
                        }

                    }},
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


            Content = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(0, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(0, GridUnitType.Star)},
                },
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(0, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}, //Image 
                    new RowDefinition {Height = new GridLength(4, GridUnitType.Star)}, //TableView
                },
                Children =
                {
                    {_image, 1, 1},
                    {_tableView, 1, 2}
                }
            };
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

        /// <summary>
        /// Loads the image. If image is null, _contentLabel is visible.
        /// </summary>
        /// <param name="o">O.</param>
        /// <param name="e">E.</param>
        void LoadImage(Object o, EventArgs e)
        {
            /*ILoadImage galleryService = Xamarin.Forms.DependencyService.Get<ILoadImage>();
            galleryService.ImageSelected +=
                (i, imageSourceEventArgs) => _image.Source = imageSourceEventArgs.ImageSource;
            galleryService.LoadImage();
            if (_image.Source != null)
            {
                _ContentLabel.IsVisible = false;
                _image.IsVisible = true;
            }
            else
            {
                _ContentLabel.IsVisible = true;
                _image.IsVisible = false;
            }*/
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