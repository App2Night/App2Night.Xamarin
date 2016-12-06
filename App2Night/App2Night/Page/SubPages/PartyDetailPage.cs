using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page.SubPages
{
    public class PartyDetailPage : FreshBaseContentPage
    {
        private static int _defaultFontSize = 16;
        #region Views
        InputContainer<Label> _descriptionLabel = new InputContainer<Label>
        {
            IconCode = "\uf040",
            HeightRequest = 100,
            Input = { HorizontalOptions = LayoutOptions.Center, FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Start},
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };
        InputContainer<Label> _dateLabel = new InputContainer<Label>
        {
            IconCode = "\uf073",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Center, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _startDateTimeLabel = new InputContainer<Label>
        {
            IconCode = "\uf017",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Center, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _MusicGenreLabel = new InputContainer<Label>
        {
            IconCode = "\uf001",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Center, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _partyTypeLabel = new InputContainer<Label>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Center, FontSize = _defaultFontSize },
            ValidationVisible = false
        };
        InputContainer<Label> _addressLabel = new InputContainer<Label>
        {
            IconCode = "\uf279",
            HorizontalOptions = LayoutOptions.Start,
            Input = { HorizontalOptions = LayoutOptions.Center, FontSize = _defaultFontSize },
            ValidationVisible = false
        };

        ToolbarItem _editToolbarItem = new ToolbarItem
        {
            Text = "Edit",
            
        };

        readonly Map _partyLocation = new Map()
        {
            HeightRequest = 200,
            IsShowingUser = true,
        };
        private Position _partyPosition;

        #endregion
        #region BindablePinProperty 
        public static BindableProperty MapPinsProperty = BindableProperty.Create(nameof(MapPins),
            typeof(Pin),
            typeof(DashboardPage),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                {
                    ((PartyDetailPage)bindable).MapPinsSet((Pin)newValue);
                }
            });

        public void MapPinsSet(Pin pin)
        {
            _partyLocation.Pins.Add(pin);
        }
        public Pin MapPins
        {
            get { return (Pin)GetValue(MapPinsProperty); }
            set { SetValue(MapPinsProperty, value); }
        }
        #endregion
        public static readonly BindableProperty ToolbarItemsVisibleProperty =
        BindableProperty.Create<PartyDetailPage, bool>(ctrl => ctrl.ToolbarItemsVisible,
        defaultValue: false,
        defaultBindingMode: BindingMode.OneWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            if (!oldValue && newValue)
            {
                // add your ToolBarItem(s)
                var page = (PartyDetailPage)bindable;
                page.ToolbarItems.Add(page._editToolbarItem);
            }
            else if (oldValue && !newValue)
            {
                var page = (PartyDetailPage)bindable;
                page.ToolbarItems.Clear();
            }
        });

        public bool ToolbarItemsVisible
        {
            get { return (bool)GetValue(ToolbarItemsVisibleProperty); }
            set { SetValue(ToolbarItemsVisibleProperty, value); }
        }
        public PartyDetailPage()
        {
            SetBindings();
            Device.BeginInvokeOnMainThread(async () => await InitializeMapCoordinates());
            
            this.SetBinding(ToolbarItemsVisibleProperty, nameof(PartyDetailViewModel.IsMyParty));
            Content = new StackLayout
            {
                Children =
                {
                    _partyLocation,
                    _descriptionLabel,
                    _dateLabel,
                    _startDateTimeLabel,
                    _MusicGenreLabel,
                    _partyTypeLabel
                }
            };
        }
        private async Task InitializeMapCoordinates()
        {
            var coordinates = await CrossGeolocator.Current.GetPositionAsync();
            if (coordinates != null)
            {
                MoveMapToCoordinates(coordinates);
            }
        }
        private void MoveMapToCoordinates(Plugin.Geolocator.Abstractions.Position coordinates)
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(coordinates.Latitude, coordinates.Longitude), Distance.FromKilometers(2));
            _partyLocation.MoveToRegion(mapSpan);
        }
        private void SetBindings()
        {
            this.SetBinding(TitleProperty, "Party.Name");
            _descriptionLabel.Input.SetBinding(Label.TextProperty, "Party.Description");
            _dateLabel.Input.SetBinding(Label.TextProperty, "Party.Date", stringFormat: AppResources.Date);
            _MusicGenreLabel.Input.SetBinding(Label.TextProperty, "Party.MusicGenre");
            _partyTypeLabel.Input.SetBinding(Label.TextProperty, "Party.PartyType");
            _startDateTimeLabel.Input.SetBinding(Label.TextProperty, "Party.CreationDateTime",
                stringFormat: AppResources.DateTime);
            this.SetBinding(DashboardPage.MapPinsProperty, nameof(PartyDetailViewModel.MapPins));
        }
    }
}