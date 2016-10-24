using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MvvmNano.Forms;
using PartyUp.CustomView;
using PartyUp.Model.Model;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class DashboardPage : MvvmNanoContentPage<DashboardViewModel>
    {
        private EnhancedContainer _userInfoContainer = new EnhancedContainer
        {
            Name = "User",
            ButtonText = "Edit",
            Content = new GallerieView
            {
                //HeightRequest = 100,
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
            }
        };

        readonly ContentView _previewContainer1 = new ContentView
        { 
            IsVisible = false
        };

        readonly ContentView _previewContainer2 = new ContentView
        {
            IsVisible = false
        };

        readonly EnhancedContainer _myPartiesContainer = new EnhancedContainer
        {
            Name = "MyEvents",
            ButtonText = "More" 
        }; 

        private EnhancedContainer _historyContainer = new EnhancedContainer
        {
            Name = "History",
            ButtonText = "More",
            Content = new GallerieView
            { 
                Rows = 3,
                ElementSize = 100,
                ItemSource = new List<string>()
                {
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7",
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7",
                    "Test1",
                    "Test2",
                    "Test3",
                    "Test4",
                    "Test5",
                    "Test6",
                    "Test7"
                }
            }
        };

        GallerieView partieGallerie = new GallerieView
        {
            //HeightRequest = 200,
            ElementSize = 200,
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
            _myPartiesContainer.Content = partieGallerie;
            partieGallerie.ElementTapped += PartieSelected;

            BindToViewModel(_userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);
            BindToViewModel(_myPartiesContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToMyPartiesCommand);
            BindToViewModel(_historyContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToHistoryCommand);

            BackgroundColor = Color.White; 

            var profilePictureHeight = 200;

            //TODO Replace with actual map
            var mapView = new BoxView
            {
                Color = Color.Green,
                IsVisible = false
            };

            var profilePicture = new RoundView()
            {
                //TODO Replace with actuel profile picture
                Content = new BoxView() { Color = Color.Red },
                BackgroundColor = Color.White
            };

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
                    {profilePicture, 1,1 },
                    mapView
                }
            };
            Grid.SetColumnSpan(mapView,3);
            Grid.SetRowSpan(mapView,2); 

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
                    _myPartiesContainer,
                    _historyContainer
                }
            };

            var mainScroll = new ScrollView
            {
                Content = mainLayout,
                Orientation = ScrollOrientation.Vertical
            };
            Grid.SetRowSpan(mainScroll,2);

            

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
                    {_previewContainer1,0,1 },
                    {_previewContainer2,0,1 }
                }
            };
        }


        private bool _isPreviewVisible;
        private int _selectedPreviewContainer = 0;
        PreviewView _preview;

        /// <summary>
        /// Gets fired if a partie item gets tapped on.
        /// Will display the party info view.
        /// </summary> 
        private async void PartieSelected(object sender, object o)
        {
            var party = (Party)o; 
            _preview = new PartyPreviewView(party, Height, Width);
            _preview.CloseViewEvent += ClosePreviewEvent;
            await ShowPreview(_preview);
        }

        private async void ClosePreviewEvent(object sender, EventArgs eventArgs)
        {
            _preview.CloseViewEvent -= ClosePreviewEvent;
            await ClosePreview();
        }

        async Task ClosePreview()
        {
            var currentContainerRef = _selectedPreviewContainer == 0 ? _previewContainer1 : _previewContainer2;
            await currentContainerRef.TranslateTo(0, _preview.HeightRequest, easing: Easing.CubicInOut);
            _isPreviewVisible = false;
            _previewContainer1.IsVisible = false;
            _previewContainer2.IsVisible = false;
            _previewContainer1.TranslationY = 0;
            _previewContainer2.TranslationY = 0; 
        }

        async Task ShowPreview(PreviewView view)
        {
            if (_isPreviewVisible)
                ChangeToPreview(view);
            else
                await OpenPreview(view);
        }

        async Task OpenPreview(PreviewView newView)
        {
            _selectedPreviewContainer = 0;
            _previewContainer1.Content = newView; 
            _previewContainer1.TranslationY = newView.HeightRequest;
            _previewContainer1.IsVisible = true;
            await _previewContainer1.TranslateTo(0, 0, easing: Easing.CubicInOut);
            _isPreviewVisible = true;
            _preview = newView;
        }

        void ChangeToPreview(PreviewView newView)
        {
            var nextContainerRef = _selectedPreviewContainer == 0 ? _previewContainer2 : _previewContainer1;
            var currentContainerRef = _selectedPreviewContainer == 0 ? _previewContainer1 : _previewContainer2; 

            nextContainerRef.Content = newView;
            nextContainerRef.TranslationX = -Width;
            nextContainerRef.IsVisible = true; 
            var rotateAnimation = new Animation(d =>
            {
                nextContainerRef.TranslationX = -Width* (1-d);
                currentContainerRef.TranslationX = Width * d; 
            }, 0, 1);
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
}
