using System; 
using App2Night.CustomView.View;
using App2Night.Model.Model;
using App2Night.ValueConverter;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    public class QuadraticPartyTemplate : Frame
    {
        //MaskedImage image = new MaskedImage("App2Night.Data.IconCode.default.png")
        //{
        //    Edge = false,
        //    InputTransparent = true,
        //    DrawGradient = true
        //};
        Label _distanceLabel;

        Image image = new Image 
        {
            Aspect = Aspect.AspectFill,
            InputTransparent = true,   
                Source = ImageSource.FromResource("App2Night.Data.IconCode.icon.png"),
                IsOpaque = false
            };
 

        public QuadraticPartyTemplate()
        {
            BackgroundColor = Color.White;
            Padding = 8;
            HasShadow = true; 

            var titleLabel = new Label()
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start
            };
            titleLabel.SetBinding(Label.TextProperty, "Name");

            _distanceLabel = new Label
            { 
                TextColor = Color.White
            };
            _distanceLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter()); 


            var shareIconLabel = new CustomButton
            {
                FontFamily = "FontAwesome",
                Text = "\uf1e0",
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(8, 0)
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
                    //image,
                    new BoxView { Color = Color.Green.MultiplyAlpha(0.9), InputTransparent = true},
                    _distanceLabel,
                    likeButton,
                    {titleLabel, 0, 1},
                    {shareIconLabel, 0, 1}
                }
            };
            Content = mainLayoutGrid;
        } 

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (BindingContext != null)
            {
                var party = (Party) BindingContext;
                party.PropertyChanged += (sender, args) =>
                {
                    if (party.DistanceToParty == -1)
                    {
                        //This is the default falue, distance not measured.
                        _distanceLabel.Text =
                            $"{party.Location.CityName}\n{party.Location.StreetName} {party.Location.HouseNumber}";
                    }
                    else
                    {
                        //Show the distance:
                        var distance = party.DistanceToParty;
                        var unit = string.Empty;
                        if (distance > 1) //Check if distance is above one km
                        {
                            distance = Math.Round(distance, 3);
                            unit = "km";
                        }
                        else
                        {
                            distance = Math.Round(distance*100);
                            unit = "m";
                        }

                        _distanceLabel.Text =
                            $"{distance} {unit}";
                    }
                };
                
            }
        }
    }
}