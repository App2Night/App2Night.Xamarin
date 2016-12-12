using System;
using Acr.UserDialogs;
using App2Night.Data.Language;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using App2Night.Service.Service;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class RateView : PreviewView
    {
        public static double _defaultFontSize = 35;
        #region Views
        InputContainer<Label> _generalRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf005",
            Input = { Text = "General Rating" },
            ValidationVisible = false,
        };
        InputContainer<Label> _priceRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf155",
            Input = { Text = "Price Rating" },
            ValidationVisible = false
        };
        InputContainer<Label> _locationRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf041",
            Input = { Text = "Location Rating" },
            ValidationVisible = false
        };
        InputContainer<Label> _moodRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf0a1",
            Input = { Text = "Mood Rating"},
            ValidationVisible = false
        };
        LikeView _likeGeneral = new LikeView
        {
            FontSize = _defaultFontSize,
            HorizontalOptions = LayoutOptions.End
        };
        LikeView _likePrice = new LikeView
        {
            FontSize = _defaultFontSize,
            HorizontalOptions = LayoutOptions.End,
        };
        LikeView _likeLocation = new LikeView
        {
            FontSize = _defaultFontSize,
            HorizontalOptions = LayoutOptions.End,
        };
        LikeView _likeMood = new LikeView
        {
            FontSize = _defaultFontSize,
            HorizontalOptions = LayoutOptions.End,
        };
        Button _sendRating = new Button
        {
            Text = "Senden",
            HeightRequest = 50
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
            Device.BeginInvokeOnMainThread(async () =>
            {
                using (UserDialogs.Instance.Loading("Rating")) // Resource
                {
                    var party = BindingContext as Party;
                    var result = await FreshIOC.Container.Resolve<IDataService>().RateParty(party.Id, _likeGeneral.LikeState, _likePrice.LikeState, _likeLocation.LikeState, _likeMood.LikeState);
                }
            });
        }

        private Frame CreateView()
        {
            return new Frame
            {
                Padding = 0,
                Margin = 5,
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                    },
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
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