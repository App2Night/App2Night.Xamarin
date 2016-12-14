using System;
using App2Night.Data.Language;
using App2Night.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class PartyRatingView : Grid
    {
        public event EventHandler<Party> RatePressedEvent;

        private static int _defaultFontSize = 20;
        private static int _defaultIconSize = 50; 

        public static BindableProperty PartyProperty = BindableProperty.Create(nameof(Party), typeof(Party), typeof(PartyRatingView),
            propertyChanged:((bindable, value, newValue) => ((PartyRatingView)bindable).PartyUpdated())); 

        public Party Party
        {
            get { return (Party)GetValue(PartyProperty); }
            set { SetValue(PartyProperty, value); }
        } 

        public static BindableProperty RateCommandProperty = BindableProperty.Create(nameof(RateCommand), typeof(Command<Party>), typeof(PartyRatingView));
        public Command<Party> RateCommand
        {
            get { return (Command<Party>)GetValue(RateCommandProperty); }
            set { SetValue(RateCommandProperty, value); }
        }


        public static BindableProperty RatingVisibleProperty = BindableProperty.Create(nameof(RatingVisible), typeof(bool), typeof(PartyRatingView), false,
            propertyChanged: (bindable, value, newValue) => ((PartyRatingView)bindable).RatingVisibilityUpdated());


        public bool RatingVisible
        {
            get { return (bool)GetValue(RatingVisibleProperty); }
            set { SetValue(RatingVisibleProperty, value); }
        }


        #region views

        Label _generalRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = _defaultFontSize,
        };

        Label _priceRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = _defaultFontSize,
        };

        Label _locationRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = _defaultFontSize,
        };

        Label _moodRateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = _defaultFontSize,
        };

        CustomButton _rateButton = new CustomButton
        {
            Text = AppResources.Rate,
            ButtonLabel =
            {
                FontSize = 25,
                FontFamily = "FontAwesome",
            },
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        #endregion

        public PartyRatingView()
        {
            _rateButton.ButtonTapped += RateButtonTapped; 
            CreateLayout();
        } 
        private void RatingVisibilityUpdated()
        {
            _rateButton.IsVisible = RatingVisible;
        }

        private void RateButtonTapped(object sender, EventArgs eventArgs)
        {
            RateCommand?.Execute(Party);
            RatePressedEvent?.Invoke(this, Party);
        }

        private void CreateLayout()
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Absolute)}, 
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Absolute)}, 
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Absolute)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
            };
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)} 
            };

            // Location Rating 
            Children.Add(new Label
            {
                Text = "\uf041",
                FontFamily = "FontAwesome",
                TextColor = Color.Gray.MultiplyAlpha(0.3),
                FontSize = _defaultIconSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
            });
            Children.Add(_locationRateLabel, 0, 1);

            //Price rating
            Children.Add(new Label
            {
                Text = "\uf155",
                FontFamily = "FontAwesome",
                TextColor = Color.Gray.MultiplyAlpha(0.3),
                FontSize = _defaultIconSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
            }, 2, 0);
            Children.Add(_priceRateLabel, 2, 1);

            //Moot rating
            Children.Add(new Label
            {
                Text = "\uf0a1",
                FontFamily = "FontAwesome",
                TextColor = Color.Gray.MultiplyAlpha(0.3),
                FontSize = _defaultIconSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
            }, 4, 0);
            Children.Add(_moodRateLabel, 4, 1);

            //General rating
            Children.Add(new Label
            {
                Text = "\uf005",
                FontFamily = "FontAwesome",
                TextColor = Color.Gray.MultiplyAlpha(0.3),
                FontSize = _defaultIconSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
            }, 6, 0);

            //Add spacings
            for (int i = 0; i < 3; i++)
            {
                var column = i*2 + 1;
                var spacer = new BoxView {Color = Color.Accent};
                Children.Add(spacer, column, 0);
            }

            Children.Add(_generalRateLabel, 6, 1);

            Children.Add(_rateButton, 0, 2);

            //Send button
            SetColumnSpan(_rateButton, 7);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if(width>0)
                RowDefinitions[0].Height = (width - 3)/4;
        }

        private void PartyUpdated()
        {
            if (Party == null) return;

            _generalRateLabel.Text = FormatPercentage(Party.GeneralAvg);
            _moodRateLabel.Text = FormatPercentage(Party.MoodAvg);
            _priceRateLabel.Text = FormatPercentage(Party.PriceAvg);
            _locationRateLabel.Text = FormatPercentage(Party.LocationAvg); 
        }

        private string FormatPercentage(double d)
        {
            return (d*100) + "%";
        }
    }
}