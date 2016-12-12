using System;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class LikeView : ContentView
    {
        CustomButton _likeButton = new CustomButton
        {
            Text = "\uf087",
            HorizontalOptions = LayoutOptions.Center,
            ButtonLabel = {FontFamily = "FontAwesome"}
        };

        CustomButton _dislikeButton = new CustomButton
        {
            Text = "\uf088",
            HorizontalOptions = LayoutOptions.Center,
            ButtonLabel = {FontFamily = "FontAwesome"},
        };

        private double _fontSize = 16;
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                _likeButton.ButtonLabel.FontSize = _fontSize;
                _dislikeButton.ButtonLabel.FontSize = _fontSize;
            }
        }

        public static BindableProperty LikeStateProperty = BindableProperty.Create(nameof(LikeState), typeof(int),
            typeof(LikeView), 0,
            propertyChanged: (bindable, value, newValue) => { ((LikeView) bindable).LikeState = (int) newValue; });

        public int LikeState
        {
            get { return (int) GetValue(LikeStateProperty); }
            set { SetValue(LikeStateProperty, value); }
        }

        public LikeView()
        {
            _likeButton.ButtonTapped += TapLikeBtn;
            _dislikeButton.ButtonTapped += TapDislikeBtn;
            Grid grid = CreateInputColumns();
            Content = grid;
        }

        private void TapDislikeBtn(object sender, EventArgs e)
        {
            if (_dislikeButton.ButtonLabel.TextColor == Color.Red)
            {
                _dislikeButton.ButtonLabel.TextColor = Color.Gray;
                LikeState = 0;
            }
            else
            {
                _dislikeButton.ButtonLabel.TextColor = Color.Red;
                _likeButton.ButtonLabel.TextColor = Color.Gray;
                LikeState = -1;
            }
        }

        private void TapLikeBtn(object sender, EventArgs e)
        {
            if (_likeButton.ButtonLabel.TextColor == Color.Lime)
            {
                _likeButton.ButtonLabel.TextColor = Color.Gray;
                LikeState = 0;
            }
            else
            {
                _likeButton.ButtonLabel.TextColor = Color.Lime;
                _dislikeButton.ButtonLabel.TextColor = Color.Gray;
                LikeState = 1;
            }
        }

        private Grid CreateInputColumns()
        {
            return new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                },
                Children =
                {
                    {_dislikeButton, 0, 0},
                    {_likeButton, 1, 0}
                }
            };
        }
    }
}