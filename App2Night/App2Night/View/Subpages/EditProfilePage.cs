using System;
using App2Night.CustomView.View;
using App2Night.CustomViews;
using App2Night.Model.Enum;
using App2Night.ViewModel.Subpages;
using MvvmNano;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View.Subpages
{
    public class EditProfilePage : MvvmNanoContentPage<EditProfileViewModel>
    {
        // TODO Set Text
        public string NameText
        {
            get { return _nameEntry.Text; }
            set { _nameEntry.Text = value; }
        }

        public string EmailText
        {
            get { return _emailEntry.Text; }
            set { _emailEntry.Text = value; }
        }

        public string AddressText
        {
            get { return _addressEntry.Text; }
            set { _addressEntry.Text = value; }
        }

        // TODO Set Style of Entry
        readonly Entry _nameEntry = new Entry();

        readonly Entry _emailEntry = new Entry();

        readonly Entry _addressEntry = new Entry();

        readonly EnumBindablePicker<Gender> _genderPicker = new EnumBindablePicker<Gender>();

        private readonly CustomButton _cancelBtn = new CustomButton
        {
            Text = "\uf00d",
            ButtonLabel = { FontFamily = "FontAwesome", FontSize = 50 },
        };

        private readonly CustomButton _okBtn = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel = { FontFamily = "FontAwesome", FontSize = 50 },
        };

        /// <summary>
        /// Page where Name, Email, Address etc. can be changed.
        /// </summary>
        public EditProfilePage()
        {
            // set title and add Command for ViewModel
            Title = "Edit Profile";
            BindToViewModel(_cancelBtn, EditPartyPage.ContentProperty, vm => vm.MoveToCancelCommand);
            BindToViewModel(_okBtn, CustomButton.CommandProperty, vm => vm.MoveTOkCommand);
            _okBtn.ButtonTapped += OnOkBtnTapped;
            // add eventHandler to change gender of user
            _genderPicker.SelectedIndexChanged += SelectGender;
            // set Content with two grids. first one contains all information about the user. last one has a cancel and ok btn.
            Content = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(96, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {
                        new Grid
                        {
                            Padding = new Thickness(10),
                            RowDefinitions = new RowDefinitionCollection
                            {
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                            },
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                                new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)}
                            },
                            Children =
                            {
                                {
                                    new Label
                                    {
                                        Text = "Name",
                                        HorizontalOptions = LayoutOptions.Start,
                                    },
                                    0, 0
                                },

                                {
                                    new Label
                                    {
                                        Text = "E-Mail",
                                        HorizontalOptions = LayoutOptions.Start
                                    },
                                    0, 1
                                },
                                {
                                    new Label
                                    {
                                        Text = "Address",
                                        HorizontalOptions = LayoutOptions.Start
                                    },
                                    0, 2
                                },
                                {
                                    new Label
                                    {
                                        Text = "Gender",
                                        HorizontalOptions = LayoutOptions.Start
                                    },
                                    0, 3
                                },
                                {_nameEntry, 1, 0},
                                {_emailEntry, 1, 1},
                                {_addressEntry, 1, 2},
                                {_genderPicker, 1, 3}
                            }
                        },
                        0, 0
                    },
                    {
                        new BoxView
                        {
                            HeightRequest = 1,
                            BackgroundColor = Color.Black
                        },
                        0, 1
                    },
                    {
                        new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                                new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)}
                            },
                            Children =
                            {
                                {_cancelBtn, 0, 0},
                                {_okBtn, 1, 0},
                            }
                        },
                        0, 2
                    }
                }
            };
        }

        private void OnOkBtnTapped(object sender, EventArgs e)
        {
            // TODO handle tap
        }

        private void SelectGender(object o, EventArgs e)
        {
            if (_genderPicker.SelectedIndex == 0)
            {
            }
            else if (_genderPicker.SelectedIndex == 1)
            {
            }
            else
            {
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _okBtn.ButtonTapped -= OnOkBtnTapped;
        }
    }
}