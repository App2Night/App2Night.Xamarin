using App2Night.CustomView.View;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    public class QuadraticPartyTemplate : Grid
    {
        public QuadraticPartyTemplate()
        {
            SKCanvasView image = new ImageFromPortable("App2Night.Data.Image.default.png"); 

            var titleLabel = new Label()
            {
                FontSize = 20,
                TextColor = Color.Black
            };
            titleLabel.SetBinding(Label.TextProperty, "Name");

            var distanceLabel = new Label
            {
                Text = "20,4km (Stuttgart)",
                TextColor = Color.Black
            }; 

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
            };
            var shadow = new BoxView() {Color = Color.White
                //FromHex("#ffedb3")
                .MultiplyAlpha(0.6)};
            
            Children.Add(image);
            Children.Add(shadow);
            Children.Add(titleLabel);
            Children.Add(distanceLabel, 0, 1); 
            SetRowSpan(image, 2);
            SetRowSpan(shadow, 2);
        }
    }
}