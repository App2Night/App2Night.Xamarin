using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmNano.Forms;
using PartyUp.CustomView;
using PartyUp.Model.Model;
using PartyUp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PartyUp.View
{
    public class DashboardPage : MvvmNanoContentPage<DashboardViewModel>
    {
        Map _headerMap = new Map()
        { 
        };

        RoundView profilePicture = new RoundView()
        {
            //TODO Replace with actuel profile picture
            Content = new BoxView() { Color = Color.Red },
            BackgroundColor = Color.White
        };

        private readonly ContentView _previewContainer1 = new ContentView
        {
            IsVisible = false
        };

        private readonly ContentView _previewContainer2 = new ContentView
        {
            IsVisible = false
        }; 

        private GallerieView historyGallerieView = new GallerieView
        { 
            MaxRows = 3,
            ElementSize = 100, 
        };

        private GallerieView interestingPartieGallerie = new GallerieView
        {
            ElementSize = 150
        };

        private GallerieView myPartieGallerie = new GallerieView
        { 
            ElementSize = 150,
            ItemSource = new List<string>()
            {
                "Test1",
                "Test2",
                "Test3",
                "Test4",
                "Test5",
                "Test6",
                "Test7"
            }
        };

        public DashboardPage()
        {
            //Dummy elements
            var dummys = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                dummys.Add("Dummy " + (i+1));  
            }
            historyGallerieView.ItemSource = dummys;

            //User info view
            EnhancedContainer _userInfoContainer = new EnhancedContainer
            {
                Name = "User",
                ButtonText = "Edit"
            };
            BindToViewModel(_userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);


            //Interesting partie view
            var interestingPartieContainer = new EnhancedContainer
            {
                Name = "Events near you",
                ButtonText = "More",
                Content = interestingPartieGallerie
            };
            BindToViewModel(interestingPartieContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToMyPartiesCommand);
            BindToViewModel(interestingPartieGallerie, GallerieView.ItemSourceProperty, vm => vm.InterestingPartiesForUser);
            BindToViewModel(interestingPartieContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToPartyPicker);

            //Users parties view
            var myPartiesContainer = new EnhancedContainer
            {
                Name = "MyEvents", 
                Content = myPartieGallerie
            };
            myPartieGallerie.ElementTapped += PartieSelected;
           

            //Partie history
            var historyContainer = new EnhancedContainer
            {
                Name = "History",
                ButtonText = "More",
                Content = historyGallerieView
            };
            BindToViewModel(historyContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToHistoryCommand);

            //Header
            var profilePictureHeight = 200;
            var mapWrapper = new MapWrapper(_headerMap);

            var headerContainer = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(0, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(profilePictureHeight, GridUnitType.Absolute)},
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(profilePictureHeight*1.2, GridUnitType.Absolute)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    {profilePicture, 1, 1},
                    mapWrapper
                }
            };
            Grid.SetColumnSpan(mapWrapper, 3);
            Grid.SetRowSpan(mapWrapper, 2);


            //Main layout
            var mainLayout = new StackLayout()
            {
                Spacing = 0,
                Children =
                {
                    headerContainer,
                    new BoxView
                    {
                        Color = Color.Black,
                        HeightRequest = 1
                    },
                    _userInfoContainer,
                    myPartiesContainer,
                    interestingPartieContainer,
                    historyContainer
                }
            };

            var mainScroll = new ScrollView
            {
                Content = mainLayout,
                Orientation = ScrollOrientation.Vertical
            };
            Grid.SetRowSpan(mainScroll, 2); 

            Content = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                },
                Children =
                {
                    mainScroll,
                    {_previewContainer1, 0, 1},
                    {_previewContainer2, 0, 1}
                }
            };
        }

        /// <summary>
        /// Animations for the preview view gets handled here.
        /// </summary>
        #region preview animation
        private bool _isPreviewVisible;
        private int _selectedPreviewContainer;
        private PreviewView _preview;

        /// <summary>
        /// Gets fired if a partie item gets tapped on.
        /// Will display the party info view.
        /// </summary> 
        private async void PartieSelected(object sender, object o)
        {
            var party = (Party) o;
            _preview = new PartyPreviewView(party, Height, Width);
            _preview.CloseViewEvent += ClosePreviewEvent;
            await ShowPreview(_preview);
        }

        private async void ClosePreviewEvent(object sender, EventArgs eventArgs)
        {
            _preview.CloseViewEvent -= ClosePreviewEvent;
            await ClosePreview();
        }

        private async Task ClosePreview()
        {
            var currentContainerRef = _selectedPreviewContainer == 0 ? _previewContainer1 : _previewContainer2;
            await currentContainerRef.TranslateTo(0, _preview.HeightRequest, easing: Easing.CubicInOut);
            _isPreviewVisible = false;
            _previewContainer1.IsVisible = false;
            _previewContainer2.IsVisible = false;
            _previewContainer1.TranslationY = 0;
            _previewContainer2.TranslationY = 0;
        }

        private async Task ShowPreview(PreviewView view)
        {
            if (_isPreviewVisible)
                ChangeToPreview(view);
            else
                await OpenPreview(view);
        }

        private async Task OpenPreview(PreviewView newView)
        {
            _selectedPreviewContainer = 0;
            _previewContainer1.Content = newView;
            _previewContainer1.TranslationY = newView.HeightRequest;
            _previewContainer1.IsVisible = true;
            await _previewContainer1.TranslateTo(0, 0, easing: Easing.CubicInOut);
            _isPreviewVisible = true;
            _preview = newView;
        }

        private void ChangeToPreview(PreviewView newView)
        {
            var nextContainerRef = _selectedPreviewContainer == 0 ? _previewContainer2 : _previewContainer1;
            var currentContainerRef = _selectedPreviewContainer == 0 ? _previewContainer1 : _previewContainer2;

            nextContainerRef.Content = newView;
            nextContainerRef.TranslationX = -Width;
            nextContainerRef.IsVisible = true;
            var rotateAnimation = new Animation(d =>
            {
                nextContainerRef.TranslationX = -Width*(1 - d);
                currentContainerRef.TranslationX = Width*d;
            });
            rotateAnimation.Commit(this, "RotateAnimation", easing: Easing.CubicInOut, finished: (d, b) =>
            {
                currentContainerRef.Content = null;
                currentContainerRef.IsVisible = false;
                currentContainerRef.TranslationX = 0;
                _selectedPreviewContainer = _selectedPreviewContainer == 0 ? 1 : 0;
                _preview = newView;
            });
        }
    }
    #endregion
}
