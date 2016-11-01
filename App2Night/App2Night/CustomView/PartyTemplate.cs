using System;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class PartyTemplate : ViewCell
    {
        BoxView _bottomBoxView = new BoxView
        {
            HeightRequest = 1
        };

        //private Image _pictureImage = new Image
        //{
        //    HeightRequest = 50,
        //    WidthRequest = 50
        //};

        private Label _nameLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start
        };

        private Label _dateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start
        };

        public PartyTemplate()
        {
            _nameLabel.SetBinding(Label.TextProperty, nameof(Party.Name));
            _dateLabel.SetBinding(Label.TextProperty, nameof(Party.Date));
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(20, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(60, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(20, GridUnitType.Star)}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    //{
                    //    _pictureImage,
                    //    0, 0
                    //},
                    {
                        _nameLabel,
                        1, 0
                    },
                    {
                        _dateLabel,
                        2, 0
                    },
                }
            };
            View = new StackLayout
            {
                Children =
                {
                    grid,
                    _bottomBoxView
                }
            };
        }
    }
}