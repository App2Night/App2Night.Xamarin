using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using Xamarin.Forms;

namespace App2Night.View
{
    public class CreatePartyPage : ContentPageWithInfo<CreatePartyViewModel>
    {
        private Entry _entryName = new Entry();
        private Entry _descriptionEntry = new Entry();
        private readonly DatePicker _datePicker = new DatePicker
        {
            MinimumDate = System.DateTime.Now,
            MaximumDate = System.DateTime.Now.AddMonths(12)
        };
        private Picker _musicGenreSwitch = new Picker();
        private CustomButton _cancelButton = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel ={FontFamily = "FontAwesome", FontSize = 50},
        };
        private CustomButton _acceptButton = new CustomButton
        {
            Text = "\uf00d",
            ButtonLabel = { FontFamily = "FontAwesome",FontSize = 50},
        };
        private Grid _grid;
        public CreatePartyPage()
        {
            Title = "Create Party";
            _grid = new Grid
            {
                Padding = new Thickness(10),
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {
                        new Label
                    {
                        Text = "Name",
                        HorizontalOptions = LayoutOptions.Start
                    },0,0 },
                    {
                        new Label
                    {
                        Text = "Description",
                        HorizontalOptions = LayoutOptions.Start
                    },0,1 },
                    {
                        new Label
                    {
                        Text = "Date",
                        HorizontalOptions = LayoutOptions.Start
                    },0,2 },
                    {
                        new Label
                    {
                        Text = "Music Genre",
                        HorizontalOptions = LayoutOptions.Start
                    },0,3 },

                    {_entryName,1,0 },
                    {_descriptionEntry,1,1 },
                    {_datePicker,1,2 },
                    {_musicGenreSwitch,1,3 },

                    {_cancelButton,0,4 },
                    {_acceptButton,1,4 }
                }
            };
            Content = _grid;
        }
        //TODO handle start animation
        private void Animation(Grid grid)
        {
            var animation = new Animation(d =>
            {
                grid.HeightRequest = d * Height;
            }, 0, 1);
            animation.Commit(this, "startAnimation", length: 1000U, easing: Easing.BounceIn);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Animation(_grid);

        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}