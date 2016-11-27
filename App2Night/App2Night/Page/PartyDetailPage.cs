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
            IconCode = "\uf1ae"
        };
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            IconCode = "\uf040",
            HeightRequest = 100 
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            IconCode = "\uf073" 
        };
        InputContainer<Label> _creationDateLabel = new InputContainer<Label>
        {
            IconCode = "\uf017" 
        };
        InputContainer<Label> _MusicGenreLabel = new InputContainer<Label>
        {
            IconCode = "\uf001" 
        };
        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc" 
        };
        MapWrapper _partyLocation = new MapWrapper(new Map());
        #endregion
        

        public PartyDetailPage()
        {
            _nameLabel.SetBinding(Label.TextProperty, "Party.Name");
            _descriptionLabel.SetBinding(Label.TextProperty, "Party.Description");
           _dateLabel.SetBinding(Label.TextProperty, "Party.Date");
            //.SetBinding();
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