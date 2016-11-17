using System;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class LoginPage : MvvmNanoContentPage<LoginViewModel>
    {
        #region Views
        private readonly CustomButton _loginButton = new CustomButton()
        {
            HorizontalOptions = LayoutOptions.Start,
			ButtonLabel = { FontFamily = "FontAwesome", FontSize = 30 },
            IsVisible = false,
            Text = "\uf061",
        };

		private readonly InputContainer<Entry> _nameEntry = new InputContainer<Entry>
        {
			Image = "\uf2bd",
            Input = { Placeholder = "Name"},
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
			WidthRequest = 300

        };

        private readonly InputContainer<Entry> _emailEntry = new InputContainer<Entry>
        {
            Image = "\uf003",
            Input = { Placeholder = "Email Address", Keyboard = Keyboard.Email },
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 300,
            IsVisible = false,
        };

        private readonly InputContainer<Entry> _passwordEntry = new InputContainer<Entry>
        {
            Input = { Placeholder = "Password", IsPassword = true, },
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 300,
			Image = "\uf023"
        };

        private readonly InputContainer<Switch> _signUpSwitch = new InputContainer<Switch>()
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

        private readonly CachedImage _image = new CachedImage()
        {
            HeightRequest = 256,
            WidthRequest = 100,
            Margin = new Thickness(10) 
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
            _image.SetImage("App2Night.Data.Image.default.png", SourceOrigin.File);
            Title = "Login";

            //Make sure that the page does not merge in to the status bar on iOS.
            if(Device.OS == TargetPlatform.iOS) 
                Padding = new Thickness(0,20,0,0);

			BackgroundColor = Color.White;
            // set bindings
            BindToViewModel(_loginButton, CustomButton.CommandProperty, vm => vm.StartLoginCommand); 
            BindToViewModel(_nameEntry.Input, Entry.TextProperty, vm => vm.Username);
            BindToViewModel(_passwordEntry.Input, Entry.TextProperty, vm => vm.Password);
			// set event handler 
			_nameEntry.Input.TextChanged += SetBtnVisible;
			_passwordEntry.Input.TextChanged += SetBtnVisible;
            _loginButton.ButtonTapped += Login;
            _signUpSwitch.Input.Toggled += Register;
            _signUpSwitch.Input.Toggled += Toggle;
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
                    {_image,0,0 },
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
            if (_nameEntry.Input.Text != null && _passwordEntry.Input.Text != null)
            {
				_loginButton.IsVisible = (_nameEntry.Input.Text.Length > 0 && _passwordEntry.Input.Text.Length > 0 ? true : false);
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
            if (_signUpSwitch.Input.IsToggled)
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
            if (_signUpSwitch.Input.IsToggled)
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

        public override void Dispose()
        {
            _loginButton.ButtonTapped -= Login;
            _nameEntry.Input.TextChanged -= SetBtnVisible;
			_passwordEntry.Input.TextChanged -= SetBtnVisible;
            base.Dispose();
        } 
    }
}