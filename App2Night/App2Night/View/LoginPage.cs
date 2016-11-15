using System;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class LoginPage : MvvmNanoContentPage<LoginViewModel>
    {
        #region Nodes
        private readonly CustomButton _loginButton = new CustomButton()
        {
            HorizontalOptions = LayoutOptions.Start,
			ButtonLabel = { FontFamily = "FontAwesome", FontSize = 30 },
            IsVisible = false,
            Text = "\uf061",
        };

		private readonly CustomEntry _nameEntry = new CustomEntry
		{
			ImageString = "\uf003",
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			WidthRequest = 300

        };

        private readonly Entry _emailEntry = new Entry
        {
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 300,
            Placeholder = "Email",
            IsVisible = false
        };

        private readonly Entry _passwordEntry = new Entry
        {
            IsPassword = true,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 300,
            Placeholder = "Password",

        };

        private readonly Switch _signUpSwitch = new Switch()
        {
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 10, 0, 0)
        };

        private readonly Label _registerLabel = new Label
        {
            Text = "Noch nicht registriert?",
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(10, 10, 0, 0)
        };

        private readonly Label _acceptLabel = new Label
        {
            Text = "Hiermit bestätige ich das bla bla bla bla bla bla viel Recht,bla bla bla bla bla bla, ob du behindert bist hab ich gefragt",
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 25, 0, 0),
            IsVisible = false
        };

        private readonly ImageFromPortable _image = new ImageFromPortable("App2Night.Data.Image.default.png")
        {
            HeightRequest = 256,
            WidthRequest = 100,
            Margin = new Thickness(10),
        };

        private readonly Switch _acceptSwitch = new Switch
        {
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 25, 0, 0),
            IsVisible = false
        };

        private readonly CustomButton _moreInfButton = new CustomButton
        {
            HorizontalOptions = LayoutOptions.Start,
			IsVisible = false,
			Text = "Mehr Informationen",
        };
        #endregion
        public LoginPage()
        { 
            Title = "Login";
			BackgroundColor = Color.White;
            // set bindings
            BindToViewModel(_loginButton, CustomButton.CommandProperty, vm => vm.StartLoginCommand); 
            BindToViewModel(_nameEntry, Entry.TextProperty, vm => vm.Username);
            BindToViewModel(_passwordEntry, Entry.TextProperty, vm => vm.Password);
			// set event handler 
			_nameEntry.Entry.TextChanged += SetBtnVisible;
            _passwordEntry.TextChanged += SetBtnVisible;
            _loginButton.ButtonTapped += Login;
            _signUpSwitch.Toggled += Register;
            _signUpSwitch.Toggled += Toggle;
            var grid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(150, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(150, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    //{_image,0,0 },
                    {_nameEntry, 1, 1},
                    {_passwordEntry, 1, 2},
                    {_emailEntry, 1, 3},
                    {_acceptLabel,1, 4 },
					//{_moreInfButton,2,4},
                    {_acceptSwitch,1,5 },
                    {_registerLabel, 1, 6},
                    {_signUpSwitch, 2, 6},
                    {_loginButton, 3, 1}
                }
            };
            Grid.SetColumnSpan(_nameEntry, 2);
            Grid.SetColumnSpan(_passwordEntry, 2);
            Grid.SetColumnSpan(_emailEntry, 2);
            Grid.SetColumnSpan(_acceptLabel,2);
            Grid.SetColumnSpan(_image, 4);
            Grid.SetColumnSpan(_acceptSwitch, 2);
            Content = grid;
        }
        #region Parties
        /// <summary>
        /// Sets the login btn visible. Only if the necassary inputs available, the <code>_loginBtn</code> is visible.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        private void SetBtnVisible(object o, TextChangedEventArgs s)
        {
            if (_nameEntry.Entry.Text != null && _passwordEntry.Text != null)
            {
				_loginButton.IsVisible = (_nameEntry.Entry.Text.Length > 0 && _passwordEntry.Text.Length > 0 ? true : false);
            }
            else
            {
                _loginButton.IsVisible = false;
            }
        }

        /// <summary>
        /// Login the user.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="args"></param>
        private void Login(object c, EventArgs args)
        {
            Animation animation = new Animation(d => { _loginButton.ButtonLabel.TranslationX = d*100; }, 0, 1);
            animation.Commit(this, "StartOpeningAnimation");
            if (_signUpSwitch.IsToggled)
            {
                // TODO Handle Input
            }
            else
            {
                // TODO Handle Login
            }
        }

        /// <summary>
        /// Sets the LoginPage to register the user.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="args"></param>
        private void Register(object c, EventArgs args)
        {
            // check if the switch is triggerd, then it shows up the email entry
            if (_signUpSwitch.IsToggled)
            {
                _emailEntry.IsVisible = true;
                _acceptLabel.IsVisible = true;
                _acceptSwitch.IsVisible = true;
				_moreInfButton.IsVisible = true;
                Animation fadeInAnimation = new Animation(d =>
                {
                    // set value 50, the movement of the animation set the HeightRequest
                    _emailEntry.HeightRequest = d * 50;
                    _acceptSwitch.HeightRequest = d * 50;
                    _acceptLabel.HeightRequest = d * 50;
					_moreInfButton.HeightRequest = d * 50;
                }, 0, 1);
                fadeInAnimation.Commit(this, "StartOpeningAnimation", easing: Easing.BounceOut, length: 1000U);
                _emailEntry.IsVisible = true;
            }
            else
            {
				Animation fadeOutAnimation = new Animation(d =>
                {
                    // set value 50, the movement of the animation set the HeightRequest
                    _emailEntry.HeightRequest = d * 50;
					_acceptSwitch.HeightRequest = d * 50;
					_acceptLabel.HeightRequest = d * 50;
					_moreInfButton.HeightRequest = d * 50;
                }, 1, 0);
                fadeOutAnimation.Commit(this, "Move", easing: Easing.CubicIn, length: 1000U, finished: (d, b) =>
                {
					_emailEntry.IsVisible = false;
					_acceptLabel.IsVisible = false;
					_acceptSwitch.IsVisible = false;
					_moreInfButton.IsVisible = false ;
                });
            }
        }
        #endregion
        /// <summary>
        /// Changes the scale of the switch, if its triggered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Toggle(object sender, ToggledEventArgs e)
        {
            await _signUpSwitch.ScaleTo(1.3, easing: Easing.CubicIn);
            await _signUpSwitch.ScaleTo(1, easing: Easing.CubicIn);
        }

      

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //TODO Remove after mvvm framework gets its update to support modal pages and masterDetailPage
            ViewModel.CloseViewEvent += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                });
            };
        }

        public override void Dispose()
        {
            _loginButton.ButtonTapped -= Login;
            _nameEntry.Entry.TextChanged -= SetBtnVisible;
            _passwordEntry.TextChanged -= SetBtnVisible;
            base.Dispose();
        } 
    }
}