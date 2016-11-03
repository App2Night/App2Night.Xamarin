using System;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class LoginPage : MvvmNanoContentPage<LoginViewModel>
    {
        private CustomButton _regButton = new CustomButton()
        {
            HorizontalOptions = LayoutOptions.Start,
            IsVisible = false,
            Text = "\uf061",
            
        };
        private Entry _namEntry = new Entry
        {
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 300,
            Placeholder = "Name"
        };

        private Entry _passwordEntry = new Entry
        {
            IsPassword = true,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 300,
            Placeholder = "Email"
        };

        private CustomButton _signUpBtn = new CustomButton
        {
            HorizontalOptions = LayoutOptions.Center,
            Text = "Moechten Sie sich registrieren?",
            Margin = new Thickness(0,10,0,0)
        };

        private ImageFromPortable image = new ImageFromPortable("App2Night.Data.Image.default.png")
        {
            HeightRequest = 256,
            WidthRequest = 100,
            Margin = new Thickness(10),
        };

        public LoginPage()
        {
            Title = "Login";
            // set btn 
            _regButton.ButtonLabel.FontSize = 30;
            _regButton.ButtonLabel.FontFamily = "FontAwesome";
            _namEntry.TextChanged += (sender, args) => SetBtnVisible(sender, args);
            _passwordEntry.TextChanged += SetBtnVisible;
            _signUpBtn.ButtonLabel.TextColor = Color.Blue;
            var grid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(300, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    {image,0,0 },
                    {_namEntry, 1, 1},
                    {_passwordEntry, 1, 2},
                    {_signUpBtn,1,3 },
                    {_regButton,2,1}
                }
            };
            Grid.SetColumnSpan(image, 3);
            Content = grid;
        }

        private void SetBtnVisible(object o, TextChangedEventArgs s)
        {
            if (_namEntry.Text != null && _passwordEntry.Text != null)
            {
                if (_namEntry.Text.Length > 0 && _passwordEntry.Text.Length > 0)
                {
                    _regButton.IsVisible = true;
                }
                else
                {
                    _regButton.IsVisible = false;
                }
            }
            else
            {
                _regButton.IsVisible = false;
            }
        }
    }
}