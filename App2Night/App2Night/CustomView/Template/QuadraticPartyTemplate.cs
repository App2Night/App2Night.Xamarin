using System;
using App2Night.CustomView.View;
using App2Night.Helper.ValueConverter;
using App2Night.Service.Interface;
using MvvmNano;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    public class QuadraticPartyTemplate : Frame 
    {
        //MaskedImage image = new MaskedImage("App2Night.Data.Image.default.png")
        //{
        //    Edge = false,
        //    InputTransparent = true,
        //    DrawGradient = true
        //};

        CachedImage image = new CachedImage()
        {
            InputTransparent = true 
        };

        public QuadraticPartyTemplate()
        {
            BackgroundColor = Color.White;
            Padding = 8;
            HasShadow = true;  

            image.SetImage("App2Night.Data.Image.default.png", SourceOrigin.Resource);

            var titleLabel = new Label()
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start
            };
            titleLabel.SetBinding(Label.TextProperty, "Name");

            var distanceLabel = new Label
            {
                Text = "20,4km (Stuttgart)",
                TextColor = Color.White
            };  
            distanceLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());

            var shareIconLabel = new CustomButton
            {
                FontFamily = "FontAwesome",
                Text = "\uf1e0",
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(8,0)
            };
            shareIconLabel.ButtonLabel.FontSize = 30;
            shareIconLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());

            var likeButton = new CustomButton
            {
                Text = "\uf004",
                FontFamily = "FontAwesome",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Margin = 10,
                FontSize = 50
            }; 
            likeButton.ButtonLabel.TextColor = Color.White;
            likeButton.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter()); 

            
            var mainLayoutGrid = new Grid
            {

                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(5, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    image, 
                    distanceLabel,
                    likeButton,
                    {titleLabel, 0, 1},
                    {shareIconLabel, 0, 1}
                }
            };
            Content = mainLayoutGrid; 
        } 
    } 
}