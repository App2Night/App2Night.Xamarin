﻿using System;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class EditProfilePage : FreshBaseContentPage
    {
        #region Nodes

        readonly InputContainer<Entry> _nameEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.Username},
            IconCode = "\uf128",
            ValidationVisible = true
        };

        readonly InputContainer<Entry> _emailEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.EmailAdress},
            IconCode = "\uf003",
            ValidationVisible = true
        };

        readonly InputContainer<Entry> _addressEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.Address},
            IconCode = "\uf1ae",
            ValidationVisible = true
        };

        readonly InputContainer<Entry> _ageEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.Age, Keyboard = Keyboard.Numeric},
            IconCode = "\uf1ae",
            ValidationVisible = true
        };

        readonly InputContainer<EnumBindablePicker<Gender>> _genderPicker =
            new InputContainer<EnumBindablePicker<Gender>> {IconCode = "\uf183", ValidationVisible = true};

        // TODO handle IconCode
        Image _image = new Image {BackgroundColor = Color.Gray, HeightRequest = 300};

        private readonly CustomButton _cancelBtn = new CustomButton
        {
            Text = "\uf00d",
            ButtonLabel = {FontFamily = "FontAwesome", FontSize = 50},
        };

        private readonly CustomButton _okBtn = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel = {FontFamily = "FontAwesome", FontSize = 50},
        };

        private InputContainer<Entry> _cityNameEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.Cityname},
            IconCode = "\uf279",
            ValidationVisible = true

        };

        private InputContainer<Entry> _streetEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.StrName},
            ValidationVisible = true,

            IconCode = "\uf0f3"
        };

        private InputContainer<Entry> _numberEntry = new InputContainer<Entry>
        {
            Input = {Keyboard = Keyboard.Numeric, Placeholder = AppResources.HNumber},
            ValidationVisible = true

        };

        private InputContainer<Entry> _locationEntry = new InputContainer<Entry>
        {
            Input = {Placeholder = AppResources.Location},
            IconCode = "\uf015",
            ValidationVisible = true,

            Margin = new Thickness(5, 0)
        };

        InputContainer<Entry> _zipCodetEntry = new InputContainer<Entry>
        {
            Input = {Keyboard = Keyboard.Numeric, Placeholder = AppResources.Zipcode},
            ValidationVisible = true

        };

        #endregion

        /// <summary>
        /// Page where Name, Email, Address etc. can be changed.
        /// </summary>
        public EditProfilePage()
        {
            // set title and add Command for ViewModel
            Title = AppResources.EditProfile;
            // bind to view model
            _cancelBtn.SetBinding(CustomButton.CommandProperty, "MoveToCancelCommand");
            _okBtn.SetBinding(CustomButton.CommandProperty, "MoveTOkCommand");

            _nameEntry.Input.SetBinding(Entry.TextProperty, "User.Name");

            _emailEntry.Input.SetBinding(Entry.TextProperty, "User.Email");

            _ageEntry.Input.SetBinding(Entry.TextProperty, "User.Age");

            _genderPicker.Input.SetBinding(EnumBindablePicker<Gender>.SelectedItemProperty, "User.Gender");

            // set event handler
            _okBtn.ButtonTapped += OnOkBtnTapped;
            // set Content with two grids. first one contains all information about the user. last one has a cancel and ok btn.
            StackLayout stackLayout = new StackLayout()
            {
                Children =
                {
                    _nameEntry,
                    _emailEntry,
                    _ageEntry,
                    _genderPicker,
                    _streetEntry,
                    _numberEntry,
                    _cityNameEntry,
                    _zipCodetEntry,
                    new BoxView
                    {
                        HeightRequest = 1,
                        BackgroundColor = Color.Black
                    },
                    new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                        },
                        Children =
                        {
                            {_cancelBtn, 0, 0},
                            {_okBtn, 1, 0},
                        }
                    }
                }
            };
            var mainScroll = new ScrollView
            {
                Content = stackLayout,
                Orientation = ScrollOrientation.Horizontal
                //{

                //    new Grid
                //    {
                //        RowDefinitions = new RowDefinitionCollection
                //        {
                //            new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                //            new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                //        },
                //        Children =
                //        {
                //            {_image, 0, 0},
                //            {stackLayout, 1, 0}
                //        }
                //    }

                //},
                
            };
            Content = stackLayout;
        }

        private void OnOkBtnTapped(object sender, EventArgs e)
        {
            if (_okBtn.Command != null)
            {
                var animation = new Animation(d => { _okBtn.Scale = d; }, 1, 1.6);
                var nextAnimation = new Animation(d => { _okBtn.Scale = d; }, 1.6, 1);
                animation.Commit(this, "Scale", finished: delegate { nextAnimation.Commit(this, "Descale"); });
                _okBtn.Command.Execute(null);
            }
        }

        private void OnCancelBtnTapped(object sender, EventArgs e)
        {
            if (_cancelBtn.Command != null)
            {
                var animation = new Animation(d => { _cancelBtn.Scale = d; }, 1, 1.6);
                var nextAnimation = new Animation(d => { _cancelBtn.Scale = d; }, 1.6, 1);
                animation.Commit(this, "Scale", finished: delegate { nextAnimation.Commit(this, "Descale"); });
                _cancelBtn.Command.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _okBtn.ButtonTapped -= OnOkBtnTapped;
        }
    }
}