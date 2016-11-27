using App2Night.CustomView.View;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page
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
            BindToViewModel(_nameLabel, Label.TextProperty, vm => vm.Party.Name);
            BindToViewModel(_descriptionLabel, Label.TextProperty, vm => vm.Party.Description);
            BindToViewModel(_dateLabel, Label.TextProperty, vm => vm.Party.Date);
            BindToViewModel(_partyTypeLabel, Label.TextProperty, vm => vm.Party.PartyType);
            BindToViewModel(_creationDateLabel, Label.TextProperty, vm => vm.Party.CreationDateTime);
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