﻿using System;
using System.Threading.Tasks;
using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.Model.Language;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class LoginPage : CustomContentPage
    {
        #region Views  
		private readonly InputContainer<Entry> _usernameEntry = new InputContainer<Entry>
        {
			IconCode = "\uf2bd",
            Input =
            {
                Placeholder = AppResources.Username,
                AutomationId = "UsernameInput"
            },
            ValidationVisible = true

        };

        private readonly InputContainer<Entry> _emailEntry = new InputContainer<Entry>
        {
            IconCode = "\uf003",
            Input = { Placeholder = AppResources.EmailAdress, Keyboard = Keyboard.Email,
                AutomationId = "EmailInput" }, 
            IsVisible = false,
            ValidationVisible = true
        };

        private readonly InputContainer<Entry> _passwordEntry = new InputContainer<Entry>
        {
            Input =
            {
                Placeholder = AppResources.Password,
                IsPassword = true,
                AutomationId = "PasswordInput"
            },
			IconCode = "\uf023",
            ValidationVisible = true
        };

        private readonly Switch  _signUpSwitch = new Switch()
        {
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 10),
            AutomationId = "SignUpSwitch"
        };

        private readonly Label _registerLabel = new Label
        {
            Text = AppResources.SignUp,
            HorizontalOptions = LayoutOptions.Start, 
        };

        private readonly Label _acceptLabel = new Label
        {
			Text = AppResources.AgbAcception,
            HorizontalOptions = LayoutOptions.Center, 
            IsVisible = false
        };

        private readonly Image _image = new Image 
        {  
            Aspect = Aspect.AspectFit,
            Source = ImageSource.FromResource("App2Night.Data.Image.icon.png")
        };

        private readonly Switch _acceptAgbSwitch = new Switch
        {
            HorizontalOptions = LayoutOptions.Center, 
            IsVisible = false,
            Margin = new Thickness(0, 10),
            AutomationId = "AcceptAgbSwitch"
        };

        private readonly Button _submitButton = new Button
        {
            Text = AppResources.Submit,
            AutomationId = "Submit"
        };

        private readonly Button _useAnonymousButton = new Button
        {
            Text = AppResources.ContinueAnonymous,
            AutomationId = "ContinueAnonym"
        };

        private readonly CustomButton _agbText = new CustomButton
        {
            HorizontalOptions = LayoutOptions.Start,
			IsVisible = false,
			Text = AppResources.AcceptAgb
        };
        #endregion

        Grid _layoutGrid;

        public LoginPage()
        { 
            BackgroundColor = Color.Red;
			OfflineMessage = AppResources.OfflineContent; 

            //Make sure that the page does not merge in to the status bar on iOS.
            if(Device.OS == TargetPlatform.iOS) 
                Padding = new Thickness(0,20,0,0);

			BackgroundColor = Color.White;
            //Bindings

            //Buttons
            _submitButton.SetBinding( Button.CommandProperty, "StartLoginCommand");
            _submitButton.SetBinding( Button.IsEnabledProperty, "CanSubmitForm");

            _useAnonymousButton.SetBinding( Button.CommandProperty, "ContinueAnonymCommand");

            //Entrys
            _usernameEntry.Input.SetBinding(  Entry.TextProperty, "Username");
           _usernameEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidUsername");

            _passwordEntry.Input.SetBinding( Entry.TextProperty, "Password");
       _passwordEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidPassword");

            _emailEntry.Input.SetBinding( Entry.TextProperty, "Email");
            _emailEntry.SetBinding( InputContainer<Entry>.InputValidateProperty, "ValidEmail");

            //Switch
            _acceptAgbSwitch.SetBinding( Switch.IsToggledProperty, "AgbAccepted");
            _signUpSwitch.SetBinding( Switch.IsToggledProperty, "SignUp");


            //Events
            _signUpSwitch.Toggled += SignUpSwitchToggled; 
            _usernameEntry.Input.Completed += UsernameCompleted;
            _passwordEntry.Input.Completed += PasswordCompleted;

            _layoutGrid = new Grid
            {
                RowSpacing = 6,
                ColumnSpacing = 3,
                Padding = new Thickness(20,0),
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},

                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)}, //IconCode

                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},

                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}, //Username
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}, //Password
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}, //E-Mail
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}, //Accept AGB
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}, //Enable SignUp
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}, //Submit Button
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}, //Anonymous Button 

                    new RowDefinition {Height = new GridLength(2, GridUnitType.Star)}
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(0, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(0, GridUnitType.Absolute)}
                },
                Children =
                {
                    {_image, 1, 1 },
                    {_usernameEntry, 1, 3},
                    {_passwordEntry, 1, 4},
                    {_emailEntry, 1, 5}, 
                    {_agbText, 1, 6},  
                    {_acceptAgbSwitch,2, 6 },
                    {_registerLabel, 1, 7},
                    {_signUpSwitch, 2, 7},
                    {_submitButton, 1, 8}, 
                    {_useAnonymousButton, 1, 9} 
                }
            };

            Grid.SetColumnSpan(_image, 2); 
            Grid.SetColumnSpan(_usernameEntry, 2);
            Grid.SetColumnSpan(_passwordEntry, 2);
            Grid.SetColumnSpan(_emailEntry, 2);
            Grid.SetColumnSpan(_submitButton, 2);
            Grid.SetColumnSpan(_useAnonymousButton, 2); 

            Content = _layoutGrid;
        }

        private void PasswordCompleted(object sender, EventArgs eventArgs)
        {
            if (_signUpSwitch.IsToggled)
                _emailEntry.Input.Focus();
        }

        private void UsernameCompleted(object sender, EventArgs eventArgs)
        {
            _passwordEntry.Input.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetHeaderHeight();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            SetHeaderHeight();
        }

        private void SetHeaderHeight()
        {
            if(Height<=0) return;
            var headerHeight = Height/3;
            var imageHeight = headerHeight * (2/3.0);
            var headerSpacing = (headerHeight - imageHeight)/2; 
           
            _layoutGrid.RowDefinitions[0].Height = headerSpacing;
            _layoutGrid.RowDefinitions[1].Height = imageHeight;
            _layoutGrid.RowDefinitions[2].Height = headerSpacing;

        }

        #region Parties  

        /// <summary>
        /// Sets the LoginPage to register the user.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="args"></param>
        private async void SignUpSwitchToggled(object c, EventArgs args)
        {
            // check if the switch is triggerd, then it shows up the email entry
            if (_signUpSwitch.IsToggled)
            {
                await ExpandSignupView();
            }
            else
            {
                await CollapseSignupView();
            }
        }

        private async Task CollapseSignupView()
        {
            var referenceHeight = _emailEntry.Height;
            Animation fadeOutAnimation = new Animation(d =>
            {
                // set value 50, the movement of the animation set the HeightRequest
                _emailEntry.HeightRequest = d * referenceHeight;
                _acceptAgbSwitch.HeightRequest = d * referenceHeight;
                _acceptLabel.HeightRequest = d * referenceHeight;
                _agbText.HeightRequest = d * referenceHeight;
            }, 1, 0);
            fadeOutAnimation.Commit(this, "Move", easing: Easing.CubicIn, length: 250U, finished: (d, b) =>
            {
                _emailEntry.IsVisible = false;
                _acceptLabel.IsVisible = false;
                _acceptAgbSwitch.IsVisible = false;
                _agbText.IsVisible = false;
            });
            await _signUpSwitch.ScaleTo(0.7, easing: Easing.CubicIn, length: 175U);
            await _signUpSwitch.ScaleTo(1, easing: Easing.CubicIn, length: 75U);
        }

        private async Task ExpandSignupView()
        {
            _emailEntry.IsVisible = true;
            _acceptLabel.IsVisible = true;
            _acceptAgbSwitch.IsVisible = true;
            _agbText.IsVisible = true;
            var referenceHeight = _usernameEntry.Height; 
            Animation fadeInAnimation = new Animation(d =>
            {
                // set value 50, the movement of the animation set the HeightRequest
                _emailEntry.HeightRequest = d * referenceHeight;
                _acceptAgbSwitch.HeightRequest = d * referenceHeight;
                _acceptLabel.HeightRequest = d * referenceHeight;
                _agbText.HeightRequest = d * referenceHeight;
            }, 0, 1);
            fadeInAnimation.Commit(this, "StartOpeningAnimation", easing: Easing.BounceOut, length: 1000U);
            _emailEntry.IsVisible = true; 
            await _signUpSwitch.ScaleTo(1.3, easing: Easing.CubicIn);
            await _signUpSwitch.ScaleTo(1, easing: Easing.CubicIn);
        }
        #endregion

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _signUpSwitch.Toggled -= SignUpSwitchToggled;
            _usernameEntry.Input.Completed -= UsernameCompleted;
            _passwordEntry.Input.Completed -= PasswordCompleted;
        } 
    }
}