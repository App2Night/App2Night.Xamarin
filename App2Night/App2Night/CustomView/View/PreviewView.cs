﻿using System;
using App2Night.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class PreviewView : ContentView
    {
        #region views  
        static Style _labelStyle = new Style(typeof(Label))
        {
            BaseResourceKey = "AccentLabel",
            Setters =
            {
                new Setter
                {
                    Property = Label.FontSizeProperty,
                    Value = 23
                }
            }
        };

        protected readonly object Item;

        CustomButton _closeButton = new CustomButton()
        {
            HorizontalOptions = LayoutOptions.End,
            WidthRequest = 50,
            Text = "\uf078",
            VerticalOptions = LayoutOptions.Center,
            ButtonLabel = {FontFamily = "FontAwesome"}
        };

        CustomButton _moreButton = new CustomButton()
        {
            HorizontalOptions = LayoutOptions.End,
            WidthRequest = 50,
            Text = "\uf061",
            VerticalOptions = LayoutOptions.Center,
            ButtonLabel = {FontFamily = "FontAwesome"}
        };

        Label _titleLabel = new Label
        {
            Style = _labelStyle,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.Center,
            LineBreakMode = LineBreakMode.TailTruncation
        };

        private BoxView _middleBoxView = new BoxView
        {
            Color = Color.Accent
        };

        private BoxView _topBoxView = new BoxView
        {
            Color = (Color) Application.Current.Resources["PrimaryTextColor"]
        };

        public new Xamarin.Forms.View Content
        {
            get { return _content.Content; }
            set { _content.Content = value; }
        }

        readonly ContentView _content = new ContentView();

        public BindableProperty VisibleMoreBtnProperty = BindableProperty.Create(nameof(IsMoreBtnVisible), typeof(bool),
            typeof(PreviewView), true, BindingMode.Default);

        public bool IsMoreBtnVisible
        {
            get { return (bool) GetValue(VisibleMoreBtnProperty); }
            set
            {
                SetValue(VisibleMoreBtnProperty, value);
                _moreButton.IsVisible = value;
                _moreButton.ButtonLabel.IsEnabled = value;
            }
        }
        #endregion

        public PreviewView(string title, object item)
        {
            BindingContext = item;
            Item = item;
            _titleLabel.Text = title;
            _moreButton.ButtonLabel.Style = _labelStyle;
            _moreButton.OnTabAnimation = new Animation(d => { _moreButton.TranslationX = d * 100; }, 0, 1);
            _moreButton.AnimationCallback = (d, b) =>
            {
                _moreButton.TranslationX = 0;
            };

            _closeButton.ButtonLabel.Style = _labelStyle;
            _closeButton.OnTabAnimation = null;
            // add events
            _closeButton.ButtonTapped += CloseButtonOnButtonTapped; 
            _moreButton.ButtonTapped += (sender, args) => MoreEvent?.Invoke(null, EventArgs.Empty);
            Grid mainGrid = CreateInputRows();
            base.Content = mainGrid;
        }

        private Grid CreateInputRows()
        {
            var mainGrid = new Grid
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)}, // header information
                    new RowDefinition {Height = new GridLength(50, GridUnitType.Absolute)}, // content
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                },
                Children =
                {
                    new BoxView {Color = (Color) Application.Current.Resources["DarkPrimaryColor"]},
                    {_topBoxView, 0, 1},
                    // add grid to handle length of party name
                    {
                        new Grid
                        {
                            ColumnSpacing = 2,
                            ColumnDefinitions =
                            {
                                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                                new ColumnDefinition {Width = new GridLength(4, GridUnitType.Star)},
                                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                            },
                            Children =
                            {
                                {_closeButton, 0, 0},
                                {_titleLabel, 1, 0},
                                {_moreButton, 2, 0}
                            }
                        },
                        0, 1
                    },
                    {_middleBoxView, 0, 2},
                    {_content, 0, 3}
                }
            };
            return mainGrid;
        }

        #region Events

        /// <summary>
        /// Starts animation that rotate the <code>_closeButton</code>.
        /// </summary>
        /// <param name="length"></param>
        public void StartOpeningAnimation(uint length = 500U)
        {
            _titleLabel.SetBinding(Label.TextProperty, "Name");

            _closeButton.ButtonLabel.RotationX = 180;
            Animation openingAnimation = new Animation(d => { _closeButton.ButtonLabel.RotationX = 180*d; }, 1, 0);
            openingAnimation.Commit(this, "StartOpeningAnimation", length: length);
        }

        /// <summary>
        /// Starts animation that rotate the <code>_closeButton</code>.
        /// </summary>
        /// <param name="length"><see cref="uint"/>of the length of the animation.</param>
        public void StartClosingAnimataion(uint length = 500U)
        {
            _closeButton.ButtonLabel.RotationX = 0;
            Animation openingAnimation = new Animation(d => { _closeButton.ButtonLabel.RotationX = 180*d; }, 0, 1);
            openingAnimation.Commit(this, "StartOpeningAnimation", length: length);
        } 

        private void CloseButtonOnButtonTapped(object sender, EventArgs eventArgs)
        {
            CloseView();
        }

        public event EventHandler CloseViewEvent;

        public void CloseView()
        {
            CloseViewEvent?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler MoreEvent;
        #endregion
    }

    /// <summary>
    /// Enum to compare state of animation.
    /// </summary>
    public enum AnimationType
    {
        Opening,
        Closing
    }
}