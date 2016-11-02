using System;
using MvvmNano;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class CustomButton : ContentView
    {
        public event EventHandler ButtonTapped;

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(CommandProperty), typeof(MvvmNanoCommand), typeof(EnhancedContainer));
          
       
        public MvvmNanoCommand Command
        {
            get { return (MvvmNanoCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public Label ButtonLabel { get; } = new Label();

        public CustomButton()
        {
            TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Command = new Command(Tapped);
            GestureRecognizers.Add(gestureRecognizer);
            Content = ButtonLabel;
        }

        private void Tapped()
        {
            OnButtonTapped();
            if (Command != null)
            { 
                Command.Execute(null);
            }
        }

        protected void OnButtonTapped()
        {
            ButtonTapped?.Invoke(this, EventArgs.Empty);
        }
    }
}