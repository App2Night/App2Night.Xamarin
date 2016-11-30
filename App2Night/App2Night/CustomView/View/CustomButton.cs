﻿using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class CustomButton : ContentView
    {
        public Label ButtonLabel { get; } = new Label();

        public event EventHandler ButtonTapped;

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(CommandProperty), typeof(Command), typeof(EnhancedContainer));


        public new static BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled),
            typeof(bool), 
            typeof(CustomButton),
            true,
            propertyChanged: (bindable, value, newValue)=>
            {
                ((CustomButton)bindable).IsEnabledChanged();
            });

        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

#region IsEnabled handling
        private double _defaultOpacity; 

        void IsEnabledChanged()
        {
            if (IsEnabled) 
                Enabled(); 
            else 
                Disabled(); 
        }

        private void Disabled()
        {
            //Add the disabled opacity
            ButtonLabel.IsEnabled = false;
            ButtonLabel.Opacity = 0.6; 
        }

        private void Enabled()
        {
            //Restore the original opacity
            ButtonLabel.Opacity = _defaultOpacity;
            ButtonLabel.IsEnabled = true;
        }
#endregion

        public Command Command
        {
            get { return (Command)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        } 

        

        public string Text
        {
            get
            {
                return ButtonLabel.Text;
            }

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

        TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
        public CustomButton()
        {
            
            gestureRecognizer.Command = new Command(Tapped);
            GestureRecognizers.Add(gestureRecognizer);
            Content = ButtonLabel;

            _defaultOpacity = ButtonLabel.Opacity;
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