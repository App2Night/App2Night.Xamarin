using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Model;
using App2Night.PageModel;
using App2Night.Service.Helper;
using App2Night.ValueConverter;
using FreshMvvm;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Position = Xamarin.Forms.Maps.Position;

namespace App2Night.Page
{ 
    public class DashboardPage : CustomContentPage 
    {

        public static BindableProperty MapPinsProperty = BindableProperty.Create(nameof(MapPins), 
            typeof(IList<Pin>), 
            typeof(DashboardPage),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                {
                    ((DashboardPage) bindable).MapPinsSet((IList<Pin>) newValue);
                } 
            });
        public IList<Pin> MapPins
        {
            get { return (IList<Pin>)GetValue(MapPinsProperty); }
            set { SetValue(MapPinsProperty, value); }
        }
        
        #region Views
        readonly EnhancedContainer _interestingPartieContainer = new EnhancedContainer
        {
            Name = AppResources.IntParty,
            ContentMissingText = AppResources.ContIntParty
        };

        readonly EnhancedContainer _myPartiesContainer = new EnhancedContainer
        {
            Name = AppResources.MyParty,
            ContentMissingText = AppResources.ContMyParty
        };

        readonly EnhancedContainer _historyContainer = new EnhancedContainer
        {
            Name = AppResources.History,
            ContentMissingText = AppResources.ContHistory
        }; 

        readonly Image _profilePicture = new Image
        {
            HeightRequest = 100,
            WidthRequest = 100,
            Margin = new Thickness(10),
            Source = ImageSource.FromResource("App2Night.Data.IconCode.icon.png")
        };

        Map _headerMap = new Map()
        {
            HeightRequest = 200,
            IsShowingUser = true,
        }; 

        readonly Label _usernameLabel = new Label();
        readonly Label _emaiLabel = new Label();

        readonly HorizontalGallerieView _historyGallerieView = new HorizontalGallerieView
        {
            Columns = 2,
            Rows = 1,
            Template = typeof(QuadraticPartyTemplate)
        };

        readonly HorizontalGallerieView _interestingPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        };

        readonly HorizontalGallerieView _myPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        };
        #endregion

        public DashboardPage()
        {
            AddReloadToolbarIcon();

            this.SetBinding(DashboardPage.MapPinsProperty, nameof(DashboardPageModel.MapPins));
             
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await CoordinateHelper.HasGeolocationAccess())
                {
                        CrossGeolocator.Current.PositionChanged += PositionChanged;
                        await InitializeMapCoordinates();
                }
            }); 

            InitializeNearPartyView();
            InitializeMyPartyView();
            InitializeHistoryPartyView();

            //Main layout
            var mainLayout = new Grid()
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    new MapWrapper(_headerMap),
                    {_myPartiesContainer, 0, 2},
                    {_interestingPartieContainer, 0, 3},
                    {_historyContainer, 0, 4}
                }
            };

            var mainScroll = new ScrollView
            {
                Content = mainLayout,
                Orientation = ScrollOrientation.Vertical
            };
            Content = mainScroll;
        }

        void AddReloadToolbarIcon()
        {
            var reloadToolbarIcon = new ToolbarItem
            {
                Text = "Reload"
            };
            ToolbarItems.Add(reloadToolbarIcon);
            reloadToolbarIcon.SetBinding(MenuItem.CommandProperty, "ReloadCommand");
        }

        private async Task InitializeMapCoordinates()
        {
           
            var coordinates = await CoordinateHelper.GetCoordinates(true);
            if (coordinates != null)
            {
                MoveMapToCoordinates(coordinates); 
            }
        }

        private void MapPinsSet(IList<Pin> pins)
        {
            _headerMap.Pins.Clear();

            foreach (Pin pin in pins)
            {
                _headerMap.Pins.Add(pin);
            }
        }

        private void PositionChanged(object sender, PositionEventArgs positionEventArgs)
        {
            var coordinates = positionEventArgs.Position;
            MoveMapToCoordinates(new Coordinates((float) coordinates.Longitude, (float) coordinates.Latitude));
        }

        private void MoveMapToCoordinates(Coordinates coordinates)
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(coordinates.Latitude, coordinates.Longitude), Distance.FromKilometers(2));
            _headerMap.MoveToRegion(mapSpan);
        }

        private void InitializeHistoryPartyView()
        {
            _historyContainer.Content = _historyGallerieView;
            _historyContainer.SetBinding(EnhancedContainer.CommandProperty, "MoveToHistoryCommand");
            _historyGallerieView.ElementTapped += PartieSelected;
            _historyGallerieView.SetBinding(GallerieView.ItemSourceProperty, "PartyHistory");
            _historyContainer.SetBinding(EnhancedContainer.NoContentWarnignVisibleProperty,
                "PartyHistoryAvailable", converter: new InvertBooleanConverter());
        }

        private void InitializeMyPartyView()
        {
            _myPartiesContainer.Content = _myPartieGallerie;
            _myPartieGallerie.ElementTapped += PartieSelected;
            _myPartiesContainer.SetBinding(EnhancedContainer.CommandProperty, "MoveToMyPartiesCommand");
            _myPartieGallerie.SetBinding(GallerieView.ItemSourceProperty, "Selectedparties");
            _myPartiesContainer.SetBinding(EnhancedContainer.NoContentWarnignVisibleProperty,
                "SelectedpartiesAvailable", converter: new InvertBooleanConverter());
        }

        private void InitializeNearPartyView()
        {
            _interestingPartieContainer.Content = _interestingPartieGallerie;
            _interestingPartieGallerie.ElementTapped += PartieSelected;
            _interestingPartieGallerie.SetBinding(GallerieView.ItemSourceProperty, "InterestingPartiesForUser");
            _interestingPartieContainer.SetBinding(EnhancedContainer.CommandProperty, "MoveToPartyPicker");
            _interestingPartieContainer.SetBinding(EnhancedContainer.NoContentWarnignVisibleProperty,
               "InterestingPartieAvailable", converter: new InvertBooleanConverter());
        }

        /// <summary>
        /// Opens seleted party with <see cref="PartyPreviewView"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        private void PartieSelected(object sender, object o)
        {
            PreviewItemSelected<Party, PartyPreviewView>((Party) o, new object[] {Height, Width});
        }

        private int _lastColumns = 1;

        /// <summary>
        /// Allocates size for <see cref="EnhancedContainer"/>. 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //Handle big screens
            int columns = (int) Math.Ceiling((Width - 200)/300); //Available columns
            if (_lastColumns != columns)
            {
                _lastColumns = columns;
                _historyGallerieView.Columns = columns;
                _interestingPartieGallerie.Columns = columns;
                _myPartieGallerie.Columns = columns;
            }

            _profilePicture.HeightRequest = Width/2 - 20;
            _profilePicture.WidthRequest = Width/2 - 20;
        }
        /// <summary>
        /// Removes <see cref="EventHandler"/> of <see cref="GallerieView"/>'s
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //_interestingPartieGallerie.ElementTapped -= PartieSelected;
            //_myPartieGallerie.ElementTapped -= PartieSelected;
            //_historyGallerieView.ElementTapped -= PartieSelected;
        }
    }
}
