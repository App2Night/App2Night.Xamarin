using App2Night.CustomView.View;
using App2Night.Model.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.CustomView.Template
{
    public class SwipeViewTemplate : Grid
    {
        Map _partyLocationmap = new Map
        {
            InputTransparent = true
        };

        Label _titleLabel = new Label
        {
            FontSize = 30,
            Margin = 5
        };

        Label _descriptionLabel = new Label();

        Label _dateLabel = new Label();

        public SwipeViewTemplate()
        {
            _titleLabel.SetBinding(Label.TextProperty, nameof(Party.Name));
            _descriptionLabel.SetBinding(Label.TextProperty, nameof(Party.Description));
            _dateLabel.SetBinding(Label.TextProperty, nameof(Party.Date)); 

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition{Height = new GridLength(1, GridUnitType.Auto)}, //Title
                new RowDefinition{Height = new GridLength(200, GridUnitType.Absolute)}, //Map
                new RowDefinition{Height = new GridLength(1, GridUnitType.Auto)}, //Date
                new RowDefinition{Height = new GridLength(1, GridUnitType.Star)}, //Description 
            };

            Children.Add(_titleLabel); 
            Children.Add(new MapWrapper(_partyLocationmap),0,1);
            Children.Add(_dateLabel,0,2);
            Children.Add (_descriptionLabel , 0, 3);

            foreach (Xamarin.Forms.View child in Children)
            {
                child.InputTransparent = true;
            }
        } 

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                var party = (Party)BindingContext;

                var locationMapSpan =
                    MapSpan.FromCenterAndRadius(new Position(party.Location.Latitude, party.Location.Longitude), Distance.FromKilometers(5)); 
                _partyLocationmap.MoveToRegion(locationMapSpan);
            }
        }
    }
}