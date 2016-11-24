using System;
using App2Night.Service.Helper;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace App2Night.CustomView.View
{
    public class InputContainer<T> :Grid where T : Xamarin.Forms.View, new()
    { 
        public static BindableProperty InputValidateProperty = BindableProperty.Create(nameof(InputValidate), typeof(object), typeof(InputContainer<T>), propertyChanged: delegate(BindableObject bindable, object value, object newValue) { ((InputContainer<T>)bindable).ChangeValidationItem((bool)newValue); });

        private bool _validationVisible = true;

        public bool ValidationVisible
        {
            get { return _validationVisible; }
            set
            {
                _validationVisible = value;
                _validationIconLabel.IsVisible = value;
                //ColumnDefinitions[2].Width = new GridLength(value ? 25 : 0, GridUnitType.Absolute);
            }
        }

        private void ChangeValidationItem(bool newValue)
        {
            if (newValue)
            {
                _validationIconLabel.Text = "\uf00c";
                _validationIconLabel.TextColor = Color.LawnGreen.ToXamarinColor();
                _validationIconLabel.FontSize = 25;
                InvalidateLayout();
            }
            else
            {
                _validationIconLabel.Text = "\uf111";
                _validationIconLabel.TextColor = Color.DarkOrange.ToXamarinColor();
                _validationIconLabel.FontSize = 10;
                InvalidateLayout();
            }
        }

        public bool InputValidate
        {
            get { return (bool)GetValue(InputValidateProperty); }
            set { SetValue(InputValidateProperty, value); }
        }



        public T Input{ get; } = new T { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End};


        public string IconCode
        {
            get { return _imageLabel.Text; }
            set
            {
                _imageLabel.Text = value; 
                if(!string.IsNullOrEmpty(value))
                    ColumnDefinitions[0].Width = new GridLength(30, GridUnitType.Absolute);
            }
        }

        private Label _validationIconLabel = new Label
        {
            FontFamily = "FontAwesome", 
            Opacity = 0.7,
            Margin = new Thickness(3)
        }; 

        Label _imageLabel = new Label
        {
            FontFamily = "FontAwesome",
            FontSize = 25,
            Margin = new Thickness(3)
        };


        public InputContainer()
        {
            ColumnSpacing = 0;
            ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition {Width = new GridLength(0, GridUnitType.Absolute)},
                new ColumnDefinition {Width = new GridLength(10, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(50, GridUnitType.Absolute)},

            };
            Children.Add(_imageLabel, 0, 0);
            Children.Add(Input, 1, 0);
            Children.Add(_validationIconLabel, 2, 0);

            ValidationVisible = _validationVisible; 
        }

        public static explicit operator InputContainer<T>(Type v)
        {
            throw new NotImplementedException();
        }
    }
}