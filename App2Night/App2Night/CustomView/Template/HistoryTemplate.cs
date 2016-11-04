using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    /// <summary>
    /// Template to list the parties.
    /// </summary>
    public class HistoryTemplate : PartyTemplate
    {
        private Label _musicGenreLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center
        };

        private Label _dateLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center
        };
        public HistoryTemplate()
        {
            // Set Bindings for ListView
            _dateLabel.SetBinding(Label.TextProperty, nameof(Party.Date), stringFormat: "{0:dd MM yyyy}");
            _musicGenreLabel.SetBinding(Label.TextProperty, nameof(Party.MusicGenre));
            // Set Content
            Content = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions = 
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) }
                },
                Children =
                {
                    {new Label {Text = "Genre:"},0,0 },
                    {new Label {Text = "Datum:"},0,1 },

                    {_musicGenreLabel,1,0 },
                    {_dateLabel,1,1 }
                }
            };
        }
    }
}