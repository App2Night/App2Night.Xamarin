using System;
using Acr.UserDialogs;
using App2Night.Model.Language;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class RateView : PreviewView
    {
        public static double _defaultIconSize = 35;
        public static double _defaultFontSize = 18;
        #region Views
        InputContainer<Label> _generalRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf005",
            Input = { Text = "General Rating", FontSize = _defaultFontSize },
            ValidationVisible = false,
        };
        InputContainer<Label> _priceRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf155",
            Input = { Text = "Price Rating", FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _locationRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf041",
            Input = { Text = "Location Rating", FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _moodRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf0a1",
            Input = { Text = "Mood Rating", FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        LikeView _likeGeneral = new LikeView
        {
            FontSize = _defaultIconSize,
            HorizontalOptions = LayoutOptions.End
        };
        LikeView _likePrice = new LikeView
        {
            FontSize = _defaultIconSize,
            HorizontalOptions = LayoutOptions.End,
        };
        LikeView _likeLocation = new LikeView
        {
            FontSize = _defaultIconSize,
            HorizontalOptions = LayoutOptions.End,
        };
        LikeView _likeMood = new LikeView
        {
            FontSize = _defaultIconSize,
            HorizontalOptions = LayoutOptions.End,
        };
        Button _sendRating = new Button
        {
            Text = "Senden",
            HeightRequest = 50,
            VerticalOptions = LayoutOptions.End
        };

        #endregion
        public RateView(Party party, double parentHeight, double parentWidth) : base(AppResources.Rate + party.Name, party)
        {
            // Sets BackgroundColor white, otherwise view is visible
            BackgroundColor = Color.White;
            HeightRequest = parentWidth * 2 / 3.0;
            Frame frame = CreateView();
            frame.HeightRequest = 4*(parentWidth/5);
            _sendRating.Clicked += SendRating;
            IsMoreBtnVisible = false;
            Grid.SetColumnSpan(_sendRating, 2);
            Content = frame;
        }

        private void SendRating(object sender, EventArgs e)
        {
            if (_likeGeneral.LikeState != 0 || _likeLocation.LikeState != 0 || _likePrice.LikeState != 0 ||
                _likeMood.LikeState != 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    using (UserDialogs.Instance.Loading(AppResources.Rating))  
                    {
                        var party = BindingContext as Party;
                        var result =
                            await
                                FreshIOC.Container.Resolve<IDataService>()
                                    .RateParty(party.Id, _likeGeneral.LikeState, _likePrice.LikeState,
                                        _likeLocation.LikeState, _likeMood.LikeState);
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.Alert(new AlertConfig().Message = "You have to like minium one aspect of the party");
                });
            }
        }

        private Frame CreateView()
        {
            return new Frame
            {
                Padding = 5,
                Margin = 5,
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                    },
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    },
                    Children =
                    {
                        {_generalRatingLabel, 0, 0},
                        {_likeGeneral, 1, 0},
                        {_priceRatingLabel, 0, 1},
                        {_likePrice, 1, 1},
                        {_locationRatingLabel, 0, 2},
                        {_likeLocation, 1, 2},
                        {_moodRatingLabel, 0, 3},
                        {_likeMood, 1, 3},
                        {_sendRating,0,4 }
                    }
                }
            };
        }
    }
}