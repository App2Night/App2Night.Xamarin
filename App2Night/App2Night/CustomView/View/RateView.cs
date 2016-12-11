using App2Night.Data.Language;
using App2Night.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class RateView : PreviewView
    {
        public static double _defaultFontSize = 35.0;
        #region Views
        InputContainer<Label> _generalRatingLabel = new InputContainer<Label>
        {
            IconCode = "\uf007",
            Input = { Text = "General Rating" },
            ValidationVisible = false
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
            //FontSize = _defaultFontSize
        };
        LikeView _likePrice = new LikeView
        {
            //FontSize = _defaultFontSize
        };
        LikeView _likeLocation = new LikeView
        {
            //FontSize = _defaultFontSize
        };
        LikeView _likeMood = new LikeView
        {
            //FontSize = _defaultFontSize
        };
        #endregion
        public RateView(Party party, double parentHeight, double parentWidth) : base(AppResources.Rate + party.Name, party)
        {
            // Sets BackgroundColor white, otherwise view is visible
            BackgroundColor = Color.White;
            HeightRequest = parentWidth * 2 / 3.0;
            Frame frame = CreateView();
            frame.HeightRequest = 4*(parentWidth/5);
            Content = frame;
        }

        private Frame CreateView()
        {
            return new Frame
            {
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
                    },
                    RowDefinitions = new RowDefinitionCollection
                    {
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
                        {_likeMood, 1, 3}
                    }
                }
            };
        }
    }
}