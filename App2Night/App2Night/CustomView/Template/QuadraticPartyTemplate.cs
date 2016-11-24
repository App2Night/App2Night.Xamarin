using System;
using App2Night.CustomView.View;
using App2Night.Model.Model;
using App2Night.ValueConverter;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    public class QuadraticPartyTemplate : Frame
    {
        #region Views

        Label _distanceLabel = new Label
        {
            TextColor = Color.White
        };

        Label _titleLabel = new Label()
        {
            FontSize = 20,
            HorizontalTextAlignment = TextAlignment.Start
        };

        CustomButton _likeButton = new CustomButton
        {
            Text = "\uf004",
            FontFamily = "FontAwesome",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            Margin = 10,
            FontSize = 50,
            ButtonLabel = {TextColor = Color.White}
        };

        CustomButton _shareIconLabel = new CustomButton
        {
            FontFamily = "FontAwesome",
            Text = "\uf1e0",
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(8, 0),
            ButtonLabel = { FontSize = 35}
        };

        Image _image = new Image
        {
            Aspect = Aspect.AspectFill,
            InputTransparent = true,
            Source = ImageSource.FromResource("App2Night.Data.IconCode.icon.png"),
            IsOpaque = false
        };

        #endregion
        TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
        private CommitmentState _state = CommitmentState.Rejected;
        public QuadraticPartyTemplate()
        {
            BackgroundColor = Color.White;
            Padding = 8;
            HasShadow = true;

            _titleLabel.SetBinding(Label.TextProperty, "Name");
            _distanceLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());
            _shareIconLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());
            _likeButton.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());

            _tapGestureRecognizer.Tapped += TappedLikeBtn;
            _likeButton.GestureRecognizers.Add(_tapGestureRecognizer);

            Content = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(5, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    //_image,
                    new BoxView {Color = Color.Green.MultiplyAlpha(0.9), InputTransparent = true},
                    _distanceLabel,
                    _likeButton,
                    {new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                           new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star)},
                           new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                        },
                        Children =
                        {
                            {_titleLabel,0,0},
                            {_shareIconLabel,1,0}
                        }
                    },0,1}
                }
            };
        }
        
        private void TappedLikeBtn(object sender, EventArgs e)
        {
            if (_state == CommitmentState.Rejected)
            {
                _likeButton.ButtonLabel.TextColor = Color.Red;
                _state = CommitmentState.Accepted;
            } else if (_state == CommitmentState.Accepted)
            {
                _likeButton.ButtonLabel.TextColor = Color.White;
                _state = CommitmentState.Rejected;
            }
        }

        public enum CommitmentState
        {
            Accepted,
            Noted,
            Rejected
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