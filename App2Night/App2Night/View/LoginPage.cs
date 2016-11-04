using System; 
using App2Night.CustomView.View;
using App2Night.ViewModel; 
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class LoginPage : MvvmNanoContentPage<LoginViewModel>
    {
        private readonly CustomButton _loginButton = new CustomButton()
        {
            HorizontalOptions = LayoutOptions.Start,
            IsVisible = false,
            Text = "\uf061",
        };
        private readonly Entry _nameEntry = new Entry
        {
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 300,
            Placeholder = "Name"
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
            Placeholder = "Password"
        };

        private readonly Switch _signUpBtn = new Switch()
        {
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0,10,0,0)
        };

        private Label _registerLabel = new Label
        {
            Text = "Noch nicht registriert?",
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(10, 10, 0, 0)
        };

        private readonly ImageFromPortable _image = new ImageFromPortable("App2Night.Data.Image.default.png")
        {
            HeightRequest = 256,
            WidthRequest = 100,
            Margin = new Thickness(10),
        };

        public LoginPage()
        {
            Title = "Login";
            // set btn 
            _loginButton.ButtonLabel.FontSize = 30;
            _loginButton.ButtonLabel.FontFamily = "FontAwesome";
            _nameEntry.TextChanged += SetBtnVisible;
            _passwordEntry.TextChanged += SetBtnVisible;
            _loginButton.ButtonTapped += Login;
            _signUpBtn.Toggled += Register;
            _signUpBtn.Toggled += Toggle;
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
                    {_emailEntry,1,3 },
                    {_signUpBtn,2,4 },
                    {_registerLabel,1,4 },
                    {_loginButton,2,1}
                }
            };
            Grid.SetColumnSpan(_nameEntry,2);
            Grid.SetColumnSpan(_passwordEntry, 2);
            Grid.SetColumnSpan(_emailEntry, 2);

            Grid.SetColumnSpan(_image, 4);
            Content = grid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        private void SetBtnVisible(object o, TextChangedEventArgs s)
        {
            if (_nameEntry.Text != null && _passwordEntry.Text != null)
            {
                if (_nameEntry.Text.Length > 0 && _passwordEntry.Text.Length > 0)
                {
                    _loginButton.IsVisible = true;
                }
                else
                {
                    _loginButton.IsVisible = false;
                }
            }
            else
            {
                _loginButton.IsVisible = false;
            }
        }

        private void Login(object c, EventArgs args) 
        {
            Animation animation = new Animation(d =>
            {
                _loginButton.ButtonLabel.TranslationX = d*100;
            }, 0,1);
            animation.Commit(this, "StartOpeningAnimation");
            
        }

        private async void Register(object c, EventArgs args)
        {
            if (_signUpBtn.IsToggled)
            {
                _emailEntry.Scale = 1;
                _emailEntry.HeightRequest = 0;
                _emailEntry.IsVisible = true;
                _emailEntry.Opacity = 1;
                Animation fadeOutAnimation = new Animation(d =>
                {
                    _emailEntry.HeightRequest = d * 50;
                }, 0, 1);
                fadeOutAnimation.Commit(this, "StartOpeningAnimation", easing: Easing.BounceOut, length: 1000U);
                _emailEntry.IsVisible = true;
            }
            else
            {
                Animation fadeOutAnimation = new Animation(d =>
                {
                    _emailEntry.Rotation = d * 90;
                }, 0, 1);
                Animation moveAnimation = new Animation(d =>
                {
                    _emailEntry.TranslationX = d * -(_emailEntry.Width / 2 - _emailEntry.Height);
                    _emailEntry.TranslationY = d * (Height + 100);
                }, 0, 1);
                fadeOutAnimation.Commit(this, "StartOpeningAnimation", easing: Easing.Linear, length: 1000U);
                moveAnimation.Commit(this, "Move", easing:Easing.CubicIn, length: 1000U, finished: (d, b) =>
                {
                    _emailEntry.Opacity = 0.000000001;
                    _emailEntry.TranslationX = 0;
                    _emailEntry.TranslationY = 0;
                    _emailEntry.Rotation = 0;
                    var height = _emailEntry.HeightRequest;
                    var close = new Animation(d1 =>
                    {
                        _emailEntry.HeightRequest = height*d;
                    },1,0); 
                    close.Commit(this, "p", length:1000U);
                });
            }

        }

        private async void Toggle(object sender, ToggledEventArgs e)
        {
            await _signUpBtn.ScaleTo(1.3, easing: Easing.CubicIn);
            await _signUpBtn.ScaleTo(1, easing: Easing.CubicIn);
        }

        protected override void OnDisappearing()
        {
            _loginButton.ButtonTapped -= Login;
            _nameEntry.TextChanged -= SetBtnVisible;
            _passwordEntry.TextChanged -= SetBtnVisible;
            base.OnDisappearing();
        }
    }
}