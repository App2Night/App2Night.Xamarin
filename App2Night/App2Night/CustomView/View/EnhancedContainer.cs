using System; 
using MvvmNano;
using Xamarin.Forms;

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

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(CommandProperty), typeof(MvvmNanoCommand), typeof(EnhancedContainer),
            propertyChanged: CommandAssigned);

        private static void CommandAssigned(BindableObject bindable, object oldValue, object newValue)
        {
            ((EnhancedContainer) bindable)._moreBtn.IsVisible = newValue != null;
        }

        public MvvmNanoCommand Command
        {
            get { return (MvvmNanoCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        public static BindableProperty SeperatorColorProperty = BindableProperty.Create(nameof(SeperatorColor),
            typeof(Color), typeof(EnhancedContainer), Color.Accent,
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
        Label _noContentText = new Label {Text = "No data loaded."}; 

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

        public string Name
        {
            get { return _nameLabel.Text; }
            set { _nameLabel.Text = value; }
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
            FontSize = 20,
            Margin = 4
        };

        CustomButton _moreBtn = new CustomButton 
        {
            HorizontalOptions = LayoutOptions.End, 
            WidthRequest = 50,
            IsVisible = false
        };
        BoxView _topBoxView = new BoxView() {IsVisible = false};
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

        private int _headerHeight = 40;
        private bool _noContentWarningVisible = true;


        public EnhancedContainer()
        {
            var noContentViewContainer = new Grid
            { 
                Children = {_noContentView}
            };
            _noContentView.Children.Add(new Label
            {
                FontFamily = "FontAwesome",
                Text = "\uf11a",
                FontSize = 100,
                FontAttributes = FontAttributes.Bold
            });
            _noContentView.Children.Add(_noContentText);
            SetNoContentViewVisiblity(ContentWarningVisible); 
            _moreBtn.ButtonTapped += MoreBtnOnButtonTapped;
            _moreBtn.ButtonLabel.FontSize = 18;

            TapGestureRecognizer noContentViewGesture = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    MoreBtnOnButtonTapped(null, EventArgs.Empty);
                })
            };
            noContentViewContainer.GestureRecognizers.Add(noContentViewGesture);

            if (string.IsNullOrEmpty(ButtonText))
                ButtonText = "\uf061";
            ColorChanged(this, SeperatorColor, SeperatorColor); 

           RowSpacing = 0;

            RowDefinitions = new RowDefinitionCollection
            { 
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(HeaderHeight, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
            };

            Children.Add(_topBoxView, 0, 0);
            Children.Add(_middleBoxView, 0, 2);
            Children.Add(_bottomBoxView, 0, 4);
            Children.Add(_nameLabel, 0, 1);
            Children.Add(_moreBtn, 0, 1);
            Children.Add(noContentViewContainer, 0, 3);
        } 

        private void MoreBtnOnButtonTapped(object sender, EventArgs eventArgs)
        {
            if (Command != null)
            {
                var animation = new Animation(d =>
                {
                    _moreBtn.Scale = d;
                },1, 1.6);
                var nextAnimation = new Animation(d =>
                {
                    _moreBtn.Scale = d;
                },1.6, 1);
                animation.Commit(this, "Scale", finished: delegate
                {
                    nextAnimation.Commit(this, "Descale");
                });
                Command.Execute(null);
            }
        }

        protected override void OnRemoved(Xamarin.Forms.View view)
        {
            base.OnRemoved(view);
            _moreBtn.ButtonTapped -= MoreBtnOnButtonTapped;

        } 
    }
}