using System;
using MvvmNano;
using Xamarin.Forms;

namespace PartyUp.CustomView
{
    public class EnhancedContainer : Grid
    {

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(CommandProperty), typeof(MvvmNanoCommand), typeof(EnhancedContainer),
            propertyChanged: CommandAssigned);

        private static void CommandAssigned(BindableObject bindable, object oldValue, object newValue)
        {
            ((EnhancedContainer) bindable)._btnLabel.IsVisible = newValue != null;
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
                Children.Add(_content, 0, 3);
            }
        }

        public string Name
        {
            get { return _nameLabel.Text; }
            set { _nameLabel.Text = value; }
        }

        public string ButtonText
        {
            get { return _btnLabel.Text; }
            set { _btnLabel.Text = value; }
        }

        Label _nameLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.Start,
            FontSize = 20
        };

        Label _btnLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(8,3),
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

        TapGestureRecognizer _gesture = new TapGestureRecognizer();
        private int _headerHeight = 40;

        public EnhancedContainer()
        {
            ColorChanged(this, SeperatorColor, SeperatorColor);
            _gesture.Tapped += GestureOnTapped;
            _btnLabel.GestureRecognizers.Add(_gesture);

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
            Children.Add(_btnLabel, 0, 1);
        }

        protected override void OnRemoved(Xamarin.Forms.View view)
        {
            base.OnRemoved(view);
            _gesture.Tapped -= GestureOnTapped;

        }

        private async void GestureOnTapped(object sender, EventArgs eventArgs)
        {
            if (Command != null)
            {
                await _btnLabel.ScaleTo(1.6, 25U);
                await _btnLabel.ScaleTo(1, 300U,  Easing.BounceIn);

                Command.Execute(null); 
            }
        }
    }
}