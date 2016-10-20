using System;
using MvvmNano;
using Xamarin.Forms;

namespace PartyUp.CustomView
{
    public class EnhancedContainer : Grid
    {

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(CommandProperty), typeof(MvvmNanoCommand), typeof(EnhancedContainer));
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

        public Xamarin.Forms.View Content
        {
            get { return Children[4]; }
            set
            {
                if (Children[4] != null)
                    Children.Remove(Children[4]);

                Children.Add(value, 0, 3);
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

        private Label _nameLabel = new Label() {HorizontalOptions = LayoutOptions.Start};

        private Label _btnLabel = new Label() {HorizontalOptions = LayoutOptions.End};

        private BoxView _topBoxView = new BoxView() {IsVisible = false};
        private BoxView _middleBoxView = new BoxView();
        private BoxView _bottomBoxView = new BoxView();
        private Xamarin.Forms.View _content;


        TapGestureRecognizer _gesture = new TapGestureRecognizer();

        public EnhancedContainer()
        {
            ColorChanged(this, SeperatorColor, SeperatorColor);
            _gesture.Tapped += GestureOnTapped;
            _btnLabel.GestureRecognizers.Add(_gesture);

            RowSpacing = 0;

            RowDefinitions = new RowDefinitionCollection
            { 
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(50, GridUnitType.Absolute)},
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
            _gesture.Tapped += GestureOnTapped;

        }

        private void GestureOnTapped(object sender, EventArgs eventArgs)
        {
            if(Command!=null)
                Command.Execute(null);
        }
    }
}