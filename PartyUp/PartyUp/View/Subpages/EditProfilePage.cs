using MvvmNano.Forms;
using PartyUp.ViewModel.Subpages;
using Xamarin.Forms;

namespace PartyUp.View.Subpages
{
    public class EditProfilePage : MvvmNanoContentPage<EditProfileViewModel>
    {
        public EditProfilePage()
        {
            var nameCell = new TextCell();
            var nameLabel = new Label
            {
                Text = "Name",
            };
            var mailLabel = new Label
            {
                Text = "E-Mail"
            };
            var addressLabel = new Label
            {
                Text = "Address"
            };

            var grid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(50, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(50, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(50, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(50, GridUnitType.Absolute)}
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
            };
            Content = new ContentView()
            {
                Content = grid,
                BackgroundColor = Color.White
            };
        }

    }
}