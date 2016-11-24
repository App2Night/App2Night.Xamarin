using App2Night.CustomView.View;
using App2Night.ViewModel.Subpages;
using MvvmNano.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.View.Subpages
{
    public class PartyDetailPage : MvvmNanoContentPage<PartyDetailViewModel>
    {
        #region Views
        InputContainer<Label> _nameLabel = new InputContainer<Label>
        {
            FontSize = 35,
            Image = "\uf1ae"
        };
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            Image = "\uf040",
            HeightRequest = 100,
            FontSize = 35,
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            Image = "\uf073",
            FontSize = 35,
        };
        InputContainer<Label> _creationDateLabel = new InputContainer<Label>
        {
            Image = "\uf017",
            FontSize = 35,
        };
        InputContainer<Label> _MusicGenreLabel = new InputContainer<Label>
        {
            Image = "\uf001",
            FontSize = 35,
        };
        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            Image = "\uf0fc",
            FontSize = 35,
        };
        MapWrapper _partyLocation = new MapWrapper(new Map());
        #endregion
        

        public PartyDetailPage()
        {
            BindToViewModel(_nameLabel, Label.TextProperty, vm => vm.Party.Name);
            BindToViewModel(_descriptionLabel, Label.TextProperty, vm => vm.Party.Description);
            BindToViewModel(_dateLabel, Label.TextProperty, vm => vm.Party.Date);
            //BindToViewModel();
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