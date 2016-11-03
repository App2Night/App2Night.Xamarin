using System;
using System.IO;
using System.Reflection;
using App2Night.CustomView.View;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class QuadraticPartyTemplate : Grid
    {
        public QuadraticPartyTemplate()
        {
            SKCanvasView dummiImage = new ImageFromPortable("App2Night.Data.partydummi.jpg");
              

            var titleLabel = new Label()
            {
                FontSize = 20
            };
            titleLabel.SetBinding(Label.TextProperty, "Name");

            var distanceLabel = new Label
            {
                Text = "In 20,4 Km (Stuttgart)"
            };

            var descriptionLabel = new Label
            {
                Text = "Die super duper Party in Studi, kommt ran, es gibt alkoholische Getränke!!!!"
            };

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = new GridLength(30, GridUnitType.Star)}, 
                new RowDefinition {Height = new GridLength(4, GridUnitType.Star)},
                new RowDefinition {Height = new GridLength(50, GridUnitType.Star)},
                new RowDefinition {Height = new GridLength(20, GridUnitType.Star)}

            }; 
             
            Children.Add(dummiImage);
            SetRowSpan(dummiImage, 4);
            Children.Add(titleLabel);
            Children.Add(descriptionLabel, 0, 2); 
            Children.Add(distanceLabel, 0, 3); 
            var headerOverlayView = new BoxView
            {
                Color = Color.White.MultiplyAlpha(0.2)
            };

            var bottomOverlayView = new BoxView
            {
                Color = Color.White.MultiplyAlpha(0.2)
            };

            Children.Add(headerOverlayView); 
            Children.Add(bottomOverlayView, 0, 2);
            SetRowSpan(bottomOverlayView, 2);

        }
    }
}