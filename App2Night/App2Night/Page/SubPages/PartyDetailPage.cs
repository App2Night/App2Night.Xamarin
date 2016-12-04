using App2Night.CustomView.View;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page.SubPages
{
    public class PartyDetailPage : FreshBaseContentPage
    {
        #region Views
        InputContainer<Label> _nameLabel = new InputContainer<Label>
        { 
            IconCode = "\uf1ae",
            HorizontalOptions = LayoutOptions.Start
        };
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            IconCode = "\uf040",
            HeightRequest = 100,
            HorizontalOptions = LayoutOptions.Start
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            IconCode = "\uf073",
            HorizontalOptions = LayoutOptions.Start
        };
        InputContainer<Label> _creationDateLabel = new InputContainer<Label>
        {
            IconCode = "\uf017",
            HorizontalOptions = LayoutOptions.Start
        };
        InputContainer<Label> _MusicGenreLabel = new InputContainer<Label>
        {
            IconCode = "\uf001",
            HorizontalOptions = LayoutOptions.Start
        };
        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start
        };
        MapWrapper _partyLocation = new MapWrapper(new Map());
        #endregion
        

        public PartyDetailPage()
        {
            this.SetBinding(TitleProperty, "Party.Name");
            _nameLabel.Input.SetBinding(Label.TextProperty, "Party.Name");
            _descriptionLabel.Input.SetBinding(Label.TextProperty, "Party.Description");
            _dateLabel.Input.SetBinding(Label.TextProperty, "Party.Date");
            _partyTypeLabel.Input.SetBinding(Label.TextProperty, "Party.PartyType");
            _creationDateLabel.Input.SetBinding(Label.TextProperty, "Party.CreationDateTime");
            // TODO Bind coodinates to Map
            //BindToViewModel(_partyLocation.Map, Map.NavigationProperty, vm => vm.Party.Coordinates);
            Content = new StackLayout
            {
                Children =
                {
                    _nameLabel,
                    _descriptionLabel,
                    _dateLabel,
                    _creationDateLabel,
                    _MusicGenreLabel,
                    _partyTypeLabel
                }
            };
        }
    }
}