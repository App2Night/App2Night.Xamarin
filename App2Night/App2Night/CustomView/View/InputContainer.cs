using System;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class InputContainer<T> :Grid where T : Xamarin.Forms.View, new()
    {
        public T Input{ get; } = new T { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End};
        public string Image
        {
            get { return _ImageLabel.Text; }
            set { _ImageLabel.Text = value; }
        }

        public double FontSize { get { return _ImageLabel.FontSize; } set { _ImageLabel.FontSize = value; } }

        Label _ImageLabel = new Label
        {
            FontFamily = "FontAwesome",
            FontSize = 25
        };

        public InputContainer()
        {
            ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition {Width = new GridLength(10, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(90, GridUnitType.Star)},
            };
            Children.Add(_ImageLabel, 0, 0);
            Children.Add(Input, 1, 0);

        }

        public static explicit operator InputContainer<T>(Type v)
        {
            throw new NotImplementedException();
        }
    }
}