using MvvmNano;
using Xamarin.Forms;

namespace PartyUp.ViewModel.Subpages
{
    public class EditPartyViewModel : MvvmNanoViewModel
    {
        public TextCell _NameCell { get; set; }
        public TextCell _EmailCell { get; set; }
        public TextCell _AddressCell { get; set; }
        public TextCell _AgeCell { get; set; }

        public EditPartyViewModel()
        {
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
                    {nameLabel,0,0 },
                    {addressLabel,1,0 },
                    {mailLabel,2,0 },

                    {_NameCell,0,1 }
                }
            };
        }
    }
}