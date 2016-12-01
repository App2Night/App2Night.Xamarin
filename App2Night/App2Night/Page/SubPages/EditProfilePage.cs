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
            // set entries to stackLayout
            var inputColumns = CreateInputColumns();
            Grid.SetRowSpan(inputColumns, 2);
            Grid.SetColumnSpan(inputColumns, 2);
            var gradientLayer = new BoxView
            {
                Color = Color.White.MultiplyAlpha(0.3)
            };

            Content = new Grid
            {
                RowSpacing = 0,
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(70, GridUnitType.Absolute)}
                },
                Children =
                {
                    inputColumns,
                    {gradientLayer, 0, 1},
                    {_cancelBtn, 0, 1},
                    {_okBtn, 1, 1}
                }
            };

            Grid.SetColumnSpan(gradientLayer, 2);
        }


        private StackLayout CreateInputColumns()
        {
            return new StackLayout
            {
                Children =
                {
                    _nameEntry,
                    _emailEntry,
                    _ageEntry,
                    _genderPicker,
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