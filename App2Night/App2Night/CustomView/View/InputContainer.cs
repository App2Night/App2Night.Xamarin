using System;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class InputContainer<T> :Grid where T : Xamarin.Forms.View, new()
    {
        public T Input{ get; } = new T { VerticalOptions = LayoutOptions.Fill };
        public string Image
        {
            get { return _ImageLabel.Text; }
            set { _ImageLabel.Text = value; }
        }

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
    }
}