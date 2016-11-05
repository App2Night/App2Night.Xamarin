using App2Night.CustomView.View;
using App2Night.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyTemplate : ViewCell
    {
        private RoundImage _pictureImage = new RoundImage("App2Night.Data.Image.default.png")
        {
            VerticalOptions = LayoutOptions.Center
        };

        private Label _nameLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 20
        };

        private ContentView _content = new ContentView();

        protected Xamarin.Forms.View Content
        {
            get { return _content.Content; }
            set { _content.Content = value; }
        }


        private Grid _grid;
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Parent" && Parent !=null )
            {
                _grid.ColumnDefinitions[0].Width = ((ListView)Parent).RowHeight * 0.8;
            }

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var test = BindingContext;
        }

        public PartyTemplate()
        {
            _nameLabel.SetBinding(Label.TextProperty, nameof(Party.Name));
            _grid= new Grid
            {
                Padding = new Thickness(5),
                VerticalOptions = LayoutOptions.Center,
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(150, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
               
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {_pictureImage,0, 0},
                    {_nameLabel,1, 0},
                    {_content,1,1}
                }
            };
            Grid.SetRowSpan(_pictureImage,2);

            View = _grid;
        }
    }
}