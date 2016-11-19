using System;
using App2Night.Service.Helper;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace App2Night.CustomView.View
{
    public class InputContainer<T> :Grid where T : Xamarin.Forms.View, new()
    {

        public static BindableProperty InputValidateProperty = BindableProperty.Create(nameof(InputValidate), typeof(object), typeof(InputContainer<T>), propertyChanged: delegate(BindableObject bindable, object value, object newValue) { ((InputContainer<T>)bindable).ChangeValidationItem((bool)newValue); });

        private bool _validationVisible;

        public bool ValidationVisible
        {
            get { return _validationVisible; }
            set
            {
                _validationVisible = value;
                _validationIconLabel.IsVisible = value;
                ColumnDefinitions[2].Width = new GridLength(value ? 1 : 0, GridUnitType.Star);
            }
        }

        private void ChangeValidationItem(bool newValue)
        {
            if (newValue)
            {
                _validationIconLabel.Text = "\uf00c";
                _validationIconLabel.TextColor = Color.LawnGreen.ToXamarinColor();
                _validationIconLabel.FontSize = 25;
            }
            else
            {
                _validationIconLabel.Text = "\uf111";
                _validationIconLabel.TextColor = Color.DarkOrange.ToXamarinColor();
                _validationIconLabel.FontSize = 10; 
            }
        }

        public bool InputValidate
        {
            get { return (bool)GetValue(InputValidateProperty); }
            set { SetValue(InputValidateProperty, value); }
        }



        public T Input{ get; } = new T { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End};
        public string Image
        {
            get { return _imageLabel.Text; }
            set { _imageLabel.Text = value; }
        }

        private Label _validationIconLabel = new Label
        {
            FontFamily = "FontAwesome",
            FontSize = 25,
            Opacity = 0.7
        };

        public double FontSize { get { return _imageLabel.FontSize; } set { _imageLabel.FontSize = value; } }

        Label _imageLabel = new Label
        {
            FontFamily = "FontAwesome",
            FontSize = 25
        };


        public InputContainer()
        {
            ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(10, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(15, GridUnitType.Absolute)},

            };
            Children.Add(_imageLabel, 0, 0);
            Children.Add(Input, 1, 0);
            Children.Add(_validationIconLabel, 2, 0);

            _validationIconLabel.IsVisible = ValidationVisible;
        }

        public static explicit operator InputContainer<T>(Type v)
        {
            throw new NotImplementedException();
        }
    }
}