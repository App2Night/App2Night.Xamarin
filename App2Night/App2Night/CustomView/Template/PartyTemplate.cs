using System.Collections.Generic;
using App2Night.CustomView.View;
using App2Night.Model.Language;
using App2Night.Model.Model;
using App2Night.ValueConverter;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyTemplate : ViewCell
    {
        private Image _pictureImage = new Image
        { 
            Aspect = Aspect.AspectFill,
            HeightRequest = 140,
            WidthRequest = 140
        };

        private Label _nameLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 20
        };

        private ContentView _content = new ContentView();

        protected Xamarin.Forms.View Content
        {
            get { return _content.Content; }
            set { _content.Content = value; }
        }

        private CommitmentStateView _commitmentView = new CommitmentStateView
        {
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 38,
            Padding = new Thickness(0,10,10,0),
            Scale = 1.15
        };

        private Grid _grid;

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Parent" && Parent != null)
            {
                var parentListViewRowHeight = ((ListView) Parent).RowHeight;
                if (parentListViewRowHeight > 0)
                {
                    _grid.ColumnDefinitions[0].Width = ((ListView) Parent).RowHeight * 0.8;
                }
            }
        }

        public PartyTemplate()
        {
			if (Device.OS != TargetPlatform.Android)
           		_pictureImage.SetBinding(Image.SourceProperty, nameof(Party.ImageSource));
            _nameLabel.SetBinding(Label.TextProperty, nameof(Party.Name));

            _commitmentView.SetBinding(CommitmentStateView.CommitmentStateProperty, nameof(Party.CommitmentState));
            _commitmentView.SetBinding(CommitmentStateView.CommitmentStatePendingProperty,
                nameof(Party.CommitmentStatePending));

            _commitmentView.SetBinding(CommitmentStateView.HostedByUserProperty, nameof(Party.HostedByUser));

            _grid = new Grid
            { 
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(140, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {_pictureImage, 0, 0},
                    {_nameLabel, 1, 0},
                    {_commitmentView, 3, 0}
                }
            };

            int rowCount = 0;
            foreach (KeyValuePair<string, Xamarin.Forms.View> nameView in FillContent())
            {
                rowCount++;
                _grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
                var nameLabel = new Label {Text = nameView.Key, HorizontalOptions = LayoutOptions.Start};
                _grid.Children.Add(nameLabel, 1, rowCount);
                _grid.Children.Add(nameView.Value, 2, rowCount);
                Grid.SetColumnSpan(nameView.Value, 2);
            }

            rowCount++;
            Label adressLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Start
            };
            adressLabel.SetBinding(Label.TextProperty, nameof(Party.AdressFormatted));

            _grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)});
            _grid.Children.Add(adressLabel, 1, rowCount);

            Grid.SetColumnSpan(adressLabel, 3);

            Grid.SetColumnSpan(_nameLabel, 2); 
            Grid.SetRowSpan(_pictureImage, rowCount + 1);

            View = new Frame
            {
              HasShadow = Device.OS == TargetPlatform.Android,
              Content  = _grid,
              Padding = 0,
              Margin = 5
            };
        }

        public virtual Dictionary<string, Xamarin.Forms.View> FillContent()
        {
            var dic = new Dictionary<string, Xamarin.Forms.View>();

            Label dateLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center
            };
            dateLabel.SetBinding(Label.TextProperty, nameof(Party.Date), stringFormat: "{0:dd MM yyyy}");
            dic.Add(AppResources.DateString, dateLabel); 
            Label musicGenreLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center
            };
            musicGenreLabel.SetBinding(Label.TextProperty, nameof(Party.MusicGenre));
            dic.Add(AppResources.Genre, musicGenreLabel);  

            return dic;
        }
    }
}