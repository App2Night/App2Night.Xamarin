using System;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using App2Night.PageModel.SubPages;
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
            SetBindings();
            // set Content with two grids. first one contains all information about the user. last one has a cancel and ok btn.
            var stackLayout = CreateInputColumns();
            var mainScroll = new ScrollView
            {
                Content = stackLayout,
                Orientation = ScrollOrientation.Horizontal
            };
            Content = stackLayout;
        }


        private Grid CreateInputColumns()
        {
            return new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(5, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    {
                        new StackLayout
                        {
                            Children =
                            {
                                _nameEntry,
                                _emailEntry,
                                _ageEntry,
                                _genderPicker,
                                new Grid
                                {
                                    ColumnDefinitions = new ColumnDefinitionCollection
                                    {
                                        new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                                    },
                                    Children =
                                    {
                                        {_streetEntry, 0, 0},
                                        {_numberEntry, 1, 0},
                                    }
                                },
                                new Grid
                                {
                                    ColumnDefinitions = new ColumnDefinitionCollection
                                    {
                                        new ColumnDefinition {Width = new GridLength(2, GridUnitType.Star)},
                                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                                    },
                                    Children =
                                    {
                                        {_addressEntry, 0, 0},
                                        {_zipCodetEntry, 1, 0},
                                    }
                                },
                                new BoxView
                                {
                                    HeightRequest = 1,
                                    BackgroundColor = Color.Black
                                },
                            }
                        },
                        0, 0
                    },
                    {
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
                        },
                        0, 1
                    }
                }
            };
        }

        private void SetBindings()
        {
            _cancelBtn.SetBinding(CustomButton.CommandProperty, "MoveToCancelCommand");

            _okBtn.SetBinding(CustomButton.CommandProperty, "MoveTOkCommand");

            _nameEntry.Input.SetBinding(Entry.TextProperty, "User.Name");

            _emailEntry.Input.SetBinding(Entry.TextProperty, "User.Email");

            _ageEntry.Input.SetBinding(Entry.TextProperty, "User.Age");

            _genderPicker.Input.SetBinding(Picker.SelectedIndexProperty, "User.Gender");
        }
    }
}