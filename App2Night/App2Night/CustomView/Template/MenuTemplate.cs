using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    public class MenuTemplate: ViewCell 
    {
        public MenuTemplate()
        {
            var titleLabel = new Label
            { 
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 18
            };
            titleLabel.SetBinding(Label.TextProperty, "Title");

            var iconLabel = new Label
            {
                FontFamily = "FontAwesome",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 26
            };
            iconLabel.SetBinding(Label.TextProperty, "IconCode");

            View = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute)},
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                    },
                Children =
                    {
                        iconLabel,
                        { titleLabel, 1, 0 }
                    }
            };
        }
    }
}
