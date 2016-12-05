using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page.SubPages
{
    public class PartyDetailPage : FreshBaseContentPage
    {
        #region Views
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            IconCode = "\uf040",
            HeightRequest = 100,
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            IconCode = "\uf073",
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _creationDateLabel = new InputContainer<Label>
        {
            IconCode = "\uf017",
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _MusicGenreLabel = new InputContainer<Label>
        {
            IconCode = "\uf001",
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        MapWrapper _partyLocation = new MapWrapper(new Map());
        private Position _partyPosition = new Position();

        #endregion


        public PartyDetailPage()
        {
            this.SetBinding(TitleProperty,  "Party.Name");
            _descriptionLabel.Input.SetBinding( Label.TextProperty, "Party.Description");
            _dateLabel.Input.SetBinding( Label.TextProperty, "Party.Date", stringFormat: AppResources.Date);
            _MusicGenreLabel.Input.SetBinding(Label.TextProperty, "Party.MusicGenre");
            _partyTypeLabel.Input.SetBinding( Label.TextProperty, "Party.PartyType");
            _creationDateLabel.Input.SetBinding(Label.TextProperty, "Party.CreationDateTime",
                stringFormat: AppResources.Date);
            Content = new StackLayout
            {
                Children =
                {
                    _partyLocation,
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