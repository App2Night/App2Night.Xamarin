using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class PartyTemplate : ViewCell
    {
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create("Name", typeof(string), typeof(PartyTemplate), "");

        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create("Date", typeof(string), typeof(PartyTemplate), "");

        public string Date
        {
            get { return (string) GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create("ImageSource", typeof(string), typeof(PartyTemplate), "");

        public string ImageSource
        {
            get { return (string) GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        BoxView _bottomBoxView = new BoxView
        {
            HeightRequest = 1
        };

        public Color SeperatorColor
        {
            get { return (Color) GetValue(SeperatorColorProperty); }
            set { SetValue(SeperatorColorProperty, value); }
        }

        public static BindableProperty SeperatorColorProperty = BindableProperty.Create(nameof(SeperatorColor),
            typeof(Color), typeof(ListPartyView), Color.Accent,
            propertyChanged: (bindable, value, newValue) => ColorChanged(bindable, (Color) value, (Color) newValue));

        private static void ColorChanged(BindableObject bindable, Color oldValue, Color newValue)
        {
            PartyTemplate thisPartyTemplate = (PartyTemplate) bindable;
            thisPartyTemplate._bottomBoxView.Color = newValue;
        }

        private Image _pictureImage = new Image
        {
            HeightRequest = 50,
            WidthRequest = 50
        };

        private Label _nameLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start
        };

        private Label _dateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start
        };

        public PartyTemplate()
        {
            ColorChanged(this, SeperatorColor, SeperatorColor);
            if (Name != null)
            {
                _nameLabel.Text = Name;
            }
            if (Date != null)
            {
                _dateLabel.Text = Date;
            }
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
                    //{
                    //    new PictureBox
                    //    {
                    //        Name = "pictureBox",
                    //        Size = new Size(100, 50),
                    //        Location = new Point(14, 17),
                    //        Image =
                    //            Image.FromFile(@"C:\Repos\App2Night.Xamarin\App2Night\App2Night\Resource\2492718.jpg"),
                    //        SizeMode = PictureBoxSizeMode.CenterImage
                    //    },
                    //    0, 0
                    //},
                    {
                        _nameLabel,
                        1, 0
                    },
                    {
                        _dateLabel,
                        2, 0
                    },
                }
            };
            View = new StackLayout
            {
                Children =
                {
                    grid,
                    _bottomBoxView
                }
            };
        }
    }
}