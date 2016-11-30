using System; 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2Night.CustomView.View
{
    public class EnhancedContainer : Grid
    { 
        public static BindableProperty NoContentWarnignVisibleProperty = BindableProperty.Create(nameof(ContentWarningVisible), typeof(bool), typeof(EnhancedContainer), false, propertyChanged: NoContentWarningChanged );

        private static void NoContentWarningChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = (EnhancedContainer)bindable;
            bool visible = (bool)newValue && thisView.NoContentWarningVisible;
            thisView.SetNoContentViewVisiblity(visible);
        }

        void SetNoContentViewVisiblity(bool visible)
        {
            _noContentView.IsVisible = visible;
            if (_content != null)
                _content.IsVisible = !visible;
        }

        public bool ContentWarningVisible
        {
            get { return (bool)GetValue(NoContentWarnignVisibleProperty); }
            set { SetValue(NoContentWarnignVisibleProperty, value); }
        } 

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(CommandProperty), typeof(Command), typeof(EnhancedContainer),
            propertyChanged: CommandAssigned);

        private static void CommandAssigned(BindableObject bindable, object oldValue, object newValue)
        {
            ((EnhancedContainer) bindable)._moreBtn.IsVisible = newValue != null;
        }

        public Command Command
        {
            get { return (Command)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        public static BindableProperty SeperatorColorProperty = BindableProperty.Create(nameof(SeperatorColor),
            typeof(Color), typeof(EnhancedContainer), (Color)Application.Current.Resources["DarkPrimaryColor"],
            propertyChanged: (bindable, value, newValue) => ColorChanged(bindable, (Color) value, (Color) newValue));

        private static void ColorChanged(BindableObject bindable, Color oldValue, Color newValue)
        {
            EnhancedContainer thisEnhancedContainer = (EnhancedContainer) bindable;
            thisEnhancedContainer._topBoxView.Color = newValue;
            thisEnhancedContainer._middleBoxView.Color = newValue;
            thisEnhancedContainer._bottomBoxView.Color = newValue;
        }

        StackLayout _noContentView = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(0,20)
        };
        Label _noContentText = new Label {Text = "No data loaded." }; 

        public Color SeperatorColor
        {
            get { return (Color) GetValue(SeperatorColorProperty); }
            set { SetValue(SeperatorColorProperty, value); }
        }

        private Xamarin.Forms.View _content;

        public Xamarin.Forms.View Content
        {
            get { return _content; }
            set
            {
                if (_content!= null)
                    Children.Remove(_content); 
                _content = value;
                if (_content != null)
                {
                    Children.Add(_content, 0, 3); 
                }
                   
            }
        }

        public bool NoContentWarningVisible
        {
            get { return _noContentWarningVisible; }
            set
            {
                _noContentWarningVisible = value;
                SetNoContentViewVisiblity(value); 
            }
        }


        public static BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(EnhancedContainer),
            propertyChanged: NameChanged);

        private static void NameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as EnhancedContainer;
            thisView._nameLabel.Text = (string) newValue;
        }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        } 

        public string ButtonText
        {
            get { return _moreBtn.ButtonLabel.Text; }
            set
            {
                
                    _moreBtn.ButtonLabel.FontFamily = "FontAwesome";
                _moreBtn.ButtonLabel.Text = value;
            }
        }

        Label _nameLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.Start,
            FontSize = 23,
            Margin = new Thickness(4, 10),
            Style = (Style)Application.Current.Resources["AccentLabel"]
        };

        CustomButton _moreBtn = new CustomButton 
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 50,
            IsVisible = false
        };
        BoxView _topBoxView = new BoxView();
        BoxView _middleBoxView = new BoxView();
        BoxView _bottomBoxView = new BoxView();

        public int HeaderHeight
        {
            get { return _headerHeight; }
            set
            {
                _headerHeight = value;
                RowDefinitions[1].Height = new GridLength(value, GridUnitType.Absolute);
            }
        }

        public string ContentMissingText
        {
            get { return _noContentText.Text; }
            set { _noContentText.Text = value; }
        }

        public bool TopSpacerVisible
        {
            get { return _topBoxView.IsVisible; }
            set
            { 
                _topBoxView.IsVisible = value;
                RowDefinitions[0].Height = 0;
            }
        }

        public double SpacerSize
        {
            get { return _spacerSize; }
            set
            {
                _spacerSize = value;
                RowDefinitions[0].Height = TopSpacerVisible ?  value : 0;
                RowDefinitions[2].Height = value;
                RowDefinitions[4].Height = value;
            }
        }

        private double _spacerSize = 1;

        private int _headerHeight = 40;
        private bool _noContentWarningVisible = true;  

        public EnhancedContainer()
        {
            var noContentViewContainer = new Grid
            { 
                Children = {_noContentView}
            };
            _moreBtn.ButtonLabel.Style = (Style) Application.Current.Resources["AccentLabel"];
            _topBoxView.IsVisible = false;
            _noContentView.Children.Add(new Label
            {
                FontFamily = "FontAwesome",
                Text = "\uf11a",
                FontSize = 100,
                FontAttributes = FontAttributes.Bold
            });
            _moreBtn.ButtonTapped += (sender, args) => Command?.Execute(null);
            SetNoContentViewVisiblity(ContentWarningVisible);
            _moreBtn.ButtonLabel.FontSize = 25; 

            if (string.IsNullOrEmpty(ButtonText))
                ButtonText = "\uf061";
            ColorChanged(this, SeperatorColor, SeperatorColor);  
            RowSpacing = 0; 
            RowDefinitions = new RowDefinitionCollection
            { 
                new RowDefinition {Height = new GridLength(TopSpacerVisible ? _spacerSize : 0, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(_spacerSize, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(_spacerSize, GridUnitType.Absolute)},
            };

            Children.Add(_topBoxView, 0, 0);
            Children.Add(new BoxView() {Color = (Color)Application.Current.Resources["PrimaryColor"] }, 0, 1);
            Children.Add(_nameLabel, 0, 1);
            Children.Add(_moreBtn, 0, 1);
            Children.Add(_middleBoxView, 0, 2);
            Children.Add(noContentViewContainer, 0, 3); 
            Children.Add(_bottomBoxView, 0, 4);

            SpacerSize = _spacerSize; 
        }
    }
}