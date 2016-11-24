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
        Image _image = new Image();
        #endregion
        
        /// <summary>
        /// Shows party in fullscreen with all details.
        /// </summary>
        public PartyDetailPage()
        {
            // Set Bindings to get set Label's with the information of the party
            BindToViewModel(_descriptionLabel, Label.TextProperty, vm => vm.Party.Description);
            BindToViewModel(_dateLabel, Label.TextProperty, vm => vm.Party.Date);
            BindToViewModel(_creationDateLabel, Label.TextProperty, vm => vm.Party.CreationDateTime);
            BindToViewModel(_MusicGenreLabel, Label.TextProperty, vm => vm.Party.MusicGenre.ToString());
            BindToViewModel(_partyTypeLabel, Label.TextProperty, vm => vm.Party.PartyType.ToString());
            BindToViewModel(this, TitleProperty, vm => vm.Party.Name);
            // add grid with all views
            var information = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}, // Image of the Party
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}, // Information
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}, // map
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}, // participant
                },
                Children =
                {
                    {_image, 0, 0},
                    {
                        new StackLayout
                        {
                            Children =
                            {
                                _descriptionLabel,
                                _MusicGenreLabel,
                                _partyTypeLabel,
                                _dateLabel,
                                _creationDateLabel
                            }
                        },
                        1, 0
                    },
                    {_partyLocation, 2, 0},

                }
            };
            Content = new ScrollView
            {
                Content = information
            };
        }
    }
}