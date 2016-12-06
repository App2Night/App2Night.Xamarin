using System;
using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Enum;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.Page.SubPages
{
    public class MyPartyDetailPage : FreshBaseContentPage
    {
        private static int _defaultFontSize = 16;

        #region Views

        InputContainer<Entry> _descriptionLabel = new InputContainer<Entry>
        {
            IconCode = "\uf040",
            HeightRequest = 100,
            Input =
            {
                HorizontalOptions = LayoutOptions.Center,
                FontSize = _defaultFontSize,
                VerticalOptions = LayoutOptions.Start
            },
            HorizontalOptions = LayoutOptions.Start,
            ValidationVisible = false
        };

        InputContainer<DatePicker> _dateLabel = new InputContainer<DatePicker>
        {
            IconCode = "\uf073",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Center},
            ValidationVisible = false
        };

        InputContainer<TimePicker> _startDateTimeLabel = new InputContainer<TimePicker>
        {
            IconCode = "\uf017",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Center},
            ValidationVisible = false
        };

        InputContainer<EnumBindablePicker<MusicGenre>> _MusicGenreLabel = new InputContainer
            <EnumBindablePicker<MusicGenre>>
        {
            IconCode = "\uf001",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Center},
            ValidationVisible = false
        };

        InputContainer<EnumBindablePicker<PartyType>> _partyTypeLabel = new InputContainer<EnumBindablePicker<PartyType>>
        {
            IconCode = "\uf0fc",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Center },
            ValidationVisible = false
        };

        InputContainer<Entry> _addressLabel = new InputContainer<Entry>
        {
            IconCode = "\uf279",
            HorizontalOptions = LayoutOptions.Start,
            Input = {HorizontalOptions = LayoutOptions.Center, FontSize = _defaultFontSize},
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
                    ((PartyDetailPage) bindable).MapPinsSet((Pin) newValue);
                }
            });

        private void MapPinsSet(Pin pin)
        {
            _partyLocation.Pins.Add(pin);
        }

        public Pin MapPins
        {
            get { return (Pin) GetValue(MapPinsProperty); }
            set { SetValue(MapPinsProperty, value); }
        }

        #endregion

        public MyPartyDetailPage()
        {
            SetBindings();
            Device.BeginInvokeOnMainThread(async () => await InitializeMapCoordinates());
            ToolbarItems.Add(_editToolbarItem);
            _editToolbarItem.Clicked += SetEditEnable;

            Content = new ScrollView
            {
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
                }
            };
        }

        private void SetEditEnable(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(coordinates.Latitude, coordinates.Longitude),
                Distance.FromKilometers(2));
            _partyLocation.MoveToRegion(mapSpan);
        }

        private void SetBindings()
        {
            this.SetBinding(TitleProperty, "Party.Name");
            _descriptionLabel.Input.SetBinding(Entry.TextProperty, "Party.Description");
            _dateLabel.Input.SetBinding(DatePicker.DateProperty, "Party.Date");
            _MusicGenreLabel.Input.SetBinding(Picker.SelectedIndexProperty, "Party.MusicGenre");
            _partyTypeLabel.Input.SetBinding(Picker.SelectedIndexProperty, "Party.PartyType");
            _startDateTimeLabel.Input.SetBinding(TimePicker.TimeProperty, "Party.CreationDateTime");
            this.SetBinding(DashboardPage.MapPinsProperty, nameof(PartyDetailViewModel.MapPins));
        }
    }
}