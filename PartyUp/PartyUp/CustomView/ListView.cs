using Xamarin.Forms;

namespace PartyUp.CustomView
{
    public class ListView : Grid
    {

        public ListView()
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = new GridLength(1,GridUnitType.Auto )},
                new ColumnDefinition {Width = new GridLength(1,GridUnitType.Auto )},
                new ColumnDefinition {Width = new GridLength(1,GridUnitType.Auto )}
            };
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
            };
            Children.Add(new BoxView
            {
                HeightRequest = 1
            },0,0);
            Children.Add(new BoxView
            {
                HeightRequest = 1
            },2,0);
            Children.Add(new Label
            {
                Text = "Bla"
            },1,1);
            
        }
    }
}