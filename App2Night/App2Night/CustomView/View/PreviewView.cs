using System;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class PreviewView : ContentView
    {
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
            HorizontalOptions = LayoutOptions.Start,
            WidthRequest = 50,
            Text = "\uf078",
            VerticalOptions = LayoutOptions.Center 
        };
        CustomButton _moreButton = new CustomButton()
        {
            HorizontalOptions = LayoutOptions.End,
            WidthRequest = 50,
            Text = "\uf061",
            VerticalOptions = LayoutOptions.Center
        };

        Label _titleLabel = new Label
        {
            Style = _labelStyle
        };

        public new Xamarin.Forms.View Content
        {
            get { return _content.Content; }
            set
            {
                _content.Content = value;
            }
        }

        readonly ContentView _content = new ContentView();

        public void StartOpeningAnimation(uint length = 500U)
        {
            _titleLabel.SetBinding(Label.TextProperty, "Name");
             

            _closeButton.ButtonLabel.Rotation = 180;
            Animation openingAnimation = new Animation(d =>
            {
                _closeButton.ButtonLabel.Rotation = 180*d;
            },1,0);
            openingAnimation.Commit(this, "StartOpeningAnimation", length: length);
        }

        public void StartClosingAnimataion(uint length = 500U)
        {
            _closeButton.ButtonLabel.Rotation = 0;
            Animation openingAnimation = new Animation(d =>
            {
                _closeButton.ButtonLabel.Rotation = 180 * d;
            }, 0, 1);
            openingAnimation.Commit(this, "StartOpeningAnimation", length: length);
        }

        public PreviewView(string title, object item)
        {
            BindingContext = item;
            _moreButton.ButtonLabel.FontFamily = "FontAwesome";
            _closeButton.ButtonLabel.FontFamily = "FontAwesome";
            _moreButton.ButtonLabel.Style = _labelStyle;
            _closeButton.ButtonLabel.Style = _labelStyle;

            Item = item;
            _closeButton.ButtonTapped += CloseButtonOnButtonTapped;
            _moreButton.ButtonTapped += MoreButtonOnTapped;
            _titleLabel.Text = title;
            var mainGrid = new Grid
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)}, 
                    new RowDefinition {Height = new GridLength(50, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)}, 
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                },
                Children =
                {
                    new BoxView { Color = (Color) Application.Current.Resources["DarkPrimaryColor"]},
                    {new BoxView {  Color = (Color) Application.Current.Resources["PrimaryTextColor"]},0,1 },
                    {_closeButton,0,1 },
                    { _titleLabel,0,1 },
                    {_moreButton,0,1 },
                    { new BoxView { Color = Color.Accent}, 0,2},
                    {_content, 0, 3 }
                }
            };
            base.Content = mainGrid;
        }

        private void MoreButtonOnTapped(object sender, EventArgs e)
        {
            Animation moveAnimation = new Animation(d =>
            {
                _moreButton.TranslationX = d*100;
            },0,1);
            moveAnimation.Commit(this,"MoveMoreButton", length:1000U, finished: (d, b) =>_moreButton.TranslationX = 0);
        }

        private void CloseButtonOnButtonTapped(object sender, EventArgs eventArgs)
        {
            CloseView();
        }

        public event EventHandler CloseViewEvent;

        public void CloseView()
        {
            if(CloseViewEvent!=null)
                CloseViewEvent(this, EventArgs.Empty);
        }

        public event EventHandler<object> MoreEvent;

        public void More()
        {
            if (MoreEvent != null)
                MoreEvent(this, EventArgs.Empty);
        }
    }

    public enum AnimationType
    {
        Opening, Closing
    }
}