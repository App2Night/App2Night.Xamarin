using MvvmNano.Forms;
using PartyUp.ViewModel.Subpages;
using Xamarin.Forms;

namespace PartyUp.View.Subpages
{
    public class EditProfilePage : MvvmNanoContentPage<EditProfileViewModel>
    {
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

        Entry _nameEntry = new Entry();

        Entry _emailEntry = new Entry();

        Entry _addressEntry = new Entry();

        Button cancelBtn = new Button
        {
            Text = "Cancel"
        };

        Button okBtn = new Button
        {
            Text = "Ok"
        };

        public EditProfilePage()
        {
            Title = "Edit Profile";
            BindToViewModel(cancelBtn, Button.CommandProperty, vm => vm.MoveToCancelCommand);
            BindToViewModel(okBtn, Button.CommandProperty, vm => vm.MoveTOkCommand);
            var stackPane = new Grid()
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
                                        HorizontalOptions = LayoutOptions.Start
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
                                {_nameEntry, 1, 0},
                                {_emailEntry, 1, 1},
                                {_addressEntry, 1, 2},
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
                                {cancelBtn, 0, 0},
                                {okBtn, 1, 0},
                            }
                        },
                        0, 2
                    }
                }
            };
            Content = new ContentView()
            {
                Content = stackPane,
                BackgroundColor = Color.White
            };
        }
    }
}