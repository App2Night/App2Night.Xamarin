﻿using System;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class LikeView : ContentView
    {
        CustomButton _likeButton = new CustomButton
        {
            Text = "\uf087",
            HorizontalOptions = LayoutOptions.Center,
            ButtonLabel = { FontFamily = "FontAwesome" }
        };
        CustomButton _dislikeButton = new CustomButton
        {
            Text = "\uf088",
            HorizontalOptions = LayoutOptions.Center,
            ButtonLabel = { FontFamily = "FontAwesome"},
        };

        //public double FontSize { get; set; }

        //public static BindableProperty LikeStateProperty = BindableProperty.Create(nameof(LikeState), typeof(int), typeof(LikeView),
        //    propertyChanged: (bindable, value, newValue) =>
        //    {
        //        ((LikeView)bindable).LikeState = (int)newValue;
        //    });

        public int LikeState { get; set; } = 0;
        public LikeView()
        {
            //_likeButton.FontSize = FontSize;
            //_dislikeButton.FontSize = FontSize;
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
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {_dislikeButton,0,0 },
                    {_likeButton,1,0 }
                }
            };
        }


    }
}