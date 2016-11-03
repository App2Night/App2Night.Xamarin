using System;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class PartyTemplate : ViewCell
    {
        private RoundImage _pictureImage = new RoundImage("App2Night.Data.partydummi.jpg");

        private Label _musicGenreLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
        };

        private Label _nameLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start,
            FontSize = 16
        };

        private Label _dateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start
        };

        public PartyTemplate()
        {
            _nameLabel.SetBinding(Label.TextProperty, nameof(Party.Name));
            _dateLabel.SetBinding(Label.TextProperty, nameof(Party.Date), stringFormat: "{0:dd MM yyyy}");
            _musicGenreLabel.SetBinding(Label.TextProperty, nameof(Party.MusicGenre));
            View = new Grid
            {
                Padding = new Thickness(5),
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(20, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(30, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(10, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(30, GridUnitType.Star)}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                },
                Children =
                {
                    {
                        _pictureImage,
                        0, 0
                    },
                    {
                        _nameLabel,
                        1, 0
                    },
                    {
                        _musicGenreLabel, 2, 0
                    },
                    {
                        _dateLabel,
                        3, 0
                    },
                }
            };
        }
    }
}