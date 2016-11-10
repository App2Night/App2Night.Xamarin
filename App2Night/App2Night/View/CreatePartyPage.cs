using System;
using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.CustomViews;
using App2Night.Model.Enum;
using App2Night.ViewModel;
using Xamarin.Forms;

namespace App2Night.View
{
	/// <summary>
	/// Create party page.
	/// </summary>
    public class CreatePartyPage : ContentPageWithInfo<CreatePartyViewModel>
    {
        private Entry _entryName = new Entry();
        private Entry _descriptionEntry = new Entry();
        private readonly DatePicker _datePicker = new DatePicker
        {
            MinimumDate = DateTime.Now,
            MaximumDate = DateTime.Now.AddMonths(12)
        };
        StackLayout _noContentView = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(0, 20)
        };
        Label _noContentText = new Label { Text = "Click Here to Load image"};
        private TimePicker _timePicker = new TimePicker();
        private EnumBindablePicker<MusicGenre> _musicGenreSwitch = new EnumBindablePicker<MusicGenre>();
        private CustomButton _cancelButton = new CustomButton
        {
            Text = "\uf00d",
            ButtonLabel ={FontFamily = "FontAwesome", FontSize = 50},
        };
        private CustomButton _acceptButton = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel = { FontFamily = "FontAwesome",FontSize = 50},
        };

        private ImageFromPortable _image = new ImageFromPortable("App2Night.Data.Image.default.png")
        {
            HeightRequest = 100,
            WidthRequest = 100 
        };
		TapGestureRecognizer _tapGesture = new TapGestureRecognizer();
        public CreatePartyPage()
        {
            // add eventHandler to CustomBtn
            _acceptButton.ButtonTapped += Accept;
            _cancelButton.ButtonTapped += Cancel;
			// set tap gesture reconizer
			_tapGesture.Tapped += LoadImage;
            Title = "Create Party";
            _noContentView.Children.Add(new Label
            {
                FontFamily = "FontAwesome",
                Text = "\uf11a",
                FontSize = 100,
                FontAttributes = FontAttributes.Bold
            });
            _noContentView.Children.Add(_noContentText);
            _image.IsVisible = false;
            // set Content
            Content = new Grid
            {
                Padding = new Thickness(10),
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {
                        new Label
                    {
                        Text = "Name",
                        HorizontalOptions = LayoutOptions.Start
                    },0,1 },
                    {
                        new Label
                    {
                        Text = "Description",
                        HorizontalOptions = LayoutOptions.Start
                    },0,2 },
                    {
                        new Label
                    {
                        Text = "Date",
                        HorizontalOptions = LayoutOptions.Start
                    },0,3 },
                    {
                        new Label
                    {
                        Text = "Time",
                        HorizontalOptions = LayoutOptions.Start
                    },0,4 },
                    {
                        new Label
                    {
                        Text = "Music Genre",
                        HorizontalOptions = LayoutOptions.Start
                    },0,5 },

                    {_image,0,0 },

                    {_entryName,1,1 },
                    {_descriptionEntry,1,2 },
                    {_datePicker,1,3 },
                    {_timePicker,1,4 },
                    {_musicGenreSwitch,1,5 },
                    {_noContentView,0,0 },
                    {_cancelButton,1,6 },
                    {_acceptButton,0,6 }
                }
            };
            Grid.SetColumnSpan(_image,2);
            Grid.SetColumnSpan(_noContentView, 2);
			_noContentView.GestureRecognizers.Add(_tapGesture);
        }
		/// <summary>
		/// Loads the image.
		/// </summary>
		/// <param name="o">O.</param>
		/// <param name="e">E.</param>
		void LoadImage(Object o, EventArgs e) {
			_noContentText.Text = "is tapped";
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
        void Cancel(Object o, EventArgs e)
        {
            TappedAnimation(_cancelButton);
            _entryName.Text = "";
            _descriptionEntry.Text = "";
            _datePicker.Date  = DateTime.Now;
        }
		/// <summary>
		/// Tappeds the animation.
		/// </summary>
		/// <param name="view">View.</param>
        void TappedAnimation(Xamarin.Forms.View view)
        {
            var animation = new Animation(d =>
            {
                view.Scale = d;
            }, 1, 1.6);
            var nextAnimation = new Animation(d =>
            {
                view.Scale = d;
            }, 1.6, 1);
            animation.Commit(this, "Scale", finished: delegate
            {
                nextAnimation.Commit(this, "Descale");
            });
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
            _cancelButton.ButtonTapped -= Cancel;

        }
    }
}