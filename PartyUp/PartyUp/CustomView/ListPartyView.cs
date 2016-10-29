using MvvmNano;
using Xamarin.Forms;

namespace PartyUp.CustomView
{
    public class ListPartyView : StackLayout
    {
        BoxView _bottomBoxView = new BoxView
        {
            HeightRequest = 1
        };
        public Color SeperatorColor
        {
            get { return (Color)GetValue(SeperatorColorProperty); }
            set { SetValue(SeperatorColorProperty, value); }
        }
        public static BindableProperty SeperatorColorProperty = BindableProperty.Create(nameof(SeperatorColor),
            typeof(Color), typeof(ListPartyView), Color.Accent,
            propertyChanged: (bindable, value, newValue) => ColorChanged(bindable, (Color)value, (Color)newValue));

        private static void ColorChanged(BindableObject bindable, Color oldValue, Color newValue)
        {
            ListPartyView thisListPartyView = (ListPartyView)bindable;
            thisListPartyView._bottomBoxView.Color = newValue;
        }

        public ListPartyView()
        {
            ColorChanged(this, SeperatorColor, SeperatorColor);
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(20, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(60, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(20, GridUnitType.Star)}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {
                        new Label
                        {
                            Text = "Image"
                        },
                        0, 0
                    },
                    {
                        new Label
                        {
                            Text = "Name"
                        },
                        1, 0
                    },
                    {
                        new Label
                        {
                            Text = "Date",
                            HorizontalOptions = LayoutOptions.End
                        },
                        2, 0
                    },
                }
            };
            Children.Add(new BoxView
            {
                HeightRequest = 1
            });
            Children.Add(grid);
            Children.Add(_bottomBoxView);
        }

    }
}