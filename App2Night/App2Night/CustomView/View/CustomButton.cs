using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class CustomButton : ContentView
    {
        public event EventHandler ButtonTapped;

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(CommandProperty),
            typeof(Command), typeof(EnhancedContainer));


        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "IsEnabled")
                IsEnabledChanged();
        }

        private double _defaultOpacity;
        private bool _wasDisabled;

        void IsEnabledChanged()
        {
            if (IsEnabled)
            {
                Enabled();
            }
            else
            {
                ButtonLabel.Opacity = 0.6;
                _wasDisabled = true;
            }
        }

        private void Enabled()
        {
            if (_wasDisabled)
                ButtonLabel.Opacity = _defaultOpacity;
            else
                _defaultOpacity = ButtonLabel.Opacity;
            _wasDisabled = false;
        }

        public Command Command
        {
            get { return (Command) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public Label ButtonLabel { get; } = new Label();

        public string Text
        {
            get { return ButtonLabel.Text; }

            set { ButtonLabel.Text = value; }
        }

        public double FontSize
        {
            get { return ButtonLabel.FontSize; }
            set { ButtonLabel.FontSize = value; }
        }

        public string FontFamily
        {
            get { return ButtonLabel.FontFamily; }
            set { ButtonLabel.FontFamily = value; }
        }

        private TapGestureRecognizer gestureRecognizer;
        public CustomButton()
        {
            gestureRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Tapped();
                })
            };
            GestureRecognizers.Add(gestureRecognizer);
            Content = ButtonLabel;

            _defaultOpacity = ButtonLabel.Opacity;
        }

        private async Task Tapped()
        {
            if ( Command != null)
            {
                await Animation();
                Command.Execute(null);
            }
            OnButtonTapped();
        }

        private async Task Animation()
        {
            var animation = new Animation(d => { ButtonLabel.Scale = d; }, 1, 1.4);
            var nextAnimation = new Animation(d => { ButtonLabel.Scale = d; }, 1.4, 1);
            animation.Commit(this, "Scale", length: 100U , finished: delegate { nextAnimation.Commit(this, "Descale"); });
        }

        protected void OnButtonTapped()
        {
            ButtonTapped?.Invoke(this, EventArgs.Empty);
        }
    }
}