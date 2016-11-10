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
		StackLayout _ContentView = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(0, 20)
        };
		Label _ContentText = new Label { Text = "Click Here to Load image"};
		Label _ContentLabel = new Label
		{
			FontFamily = "FontAwesome",
			Text = "\uf11a",
			FontSize = 100,
			FontAttributes = FontAttributes.Bold
		};
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

		private Image _image = new Image
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
			// set title of the page
            Title = "Create Party";
			// set content of _contentView
			_ContentView.Children.Add(_ContentLabel);
			_ContentView.Children.Add(_image);
            _ContentView.Children.Add(_ContentText);
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
                    {_entryName,1,1 },
                    {_descriptionEntry,1,2 },
                    {_datePicker,1,3 },
                    {_timePicker,1,4 },
                    {_musicGenreSwitch,1,5 },
                    {_ContentView,0,0 },
                    {_cancelButton,1,6 },
                    {_acceptButton,0,6 }
                }
            };
            Grid.SetColumnSpan(_image,2);
            Grid.SetColumnSpan(_ContentView, 2);
			_ContentView.GestureRecognizers.Add(_tapGesture);
        }
		/// <summary>
		/// Loads the image. If image is null, _contentLabel is visible.
		/// </summary>
		/// <param name="o">O.</param>
		/// <param name="e">E.</param>
		void LoadImage(Object o, EventArgs e) {
			ILoadImage galleryService = Xamarin.Forms.DependencyService.Get<ILoadImage>();
			galleryService.ImageSelected += (i, imageSourceEventArgs) => _image.Source = imageSourceEventArgs.ImageSource;
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
			}
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