using MvvmNano.Forms;
using PartyUp.ViewModel.Subpages;
using Xamarin.Forms;

namespace PartyUp.View.Subpages
{
    public class EditProfilePage : MvvmNanoContentPage<EditProfileViewModel>
    {
        public EditProfilePage()
        {
            var titleLabel = new Label
            {
                Text = "Edit Profile",
                FontSize = 20
            };
            var nameLabel = new Label
            {
                Text = "Name",
                HorizontalOptions = LayoutOptions.Start
            };
            var mailLabel = new Label
            {
                Text = "E-Mail",
                HorizontalOptions = LayoutOptions.Start
            };
            var addressLabel = new Label
            {
                Text = "Address",
                HorizontalOptions = LayoutOptions.Start
            };
            var cancelLabel = new Label
            {
                Text = "Cancel"
            };
            var okLabel = new Label
            {
                Text = "Ok"
            };

            var stackPane = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(96, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {titleLabel, 0, 0},
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
                            RowDefinitions = new RowDefinitionCollection
                            {
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                            },
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
                            },
                            Children =
                            {
                                {nameLabel, 0, 0},
                                {addressLabel, 0, 1},
                                {mailLabel, 0, 2},
                            }
                        },
                        0, 2
                    },
                    {
                        new BoxView
                        {
                            HeightRequest = 1,
                            BackgroundColor = Color.Black
                        },
                        0, 3
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
                                {cancelLabel, 0, 0},
                                {okLabel, 1, 0},
                            }
                        },
                        0, 4
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