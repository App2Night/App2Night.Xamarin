using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Service.Helper;
using FreshMvvm;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace App2Night.CustomView.Page
{
    public class  stomContentPage : FreshBaseContentPage
    {
        private static readonly double _size = 50;

        private ContentView _previewContainer1 = new ContentView
        {
            IsVisible = false
        };

        private ContentView _previewContainer2 = new ContentView
        {
            IsVisible = false
        };

        public uint OpeningAnimationLength { get; set; } = 500;
        public uint ClosingAnimationLength { get; set; } = 500;
        public uint SwitchingAnimationLength { get; set; } = 500;

        /// <summary>
        /// Animations for the preview view gets handled here.
        /// </summary> 
        private bool _isPreviewVisible;
        private int _selectedPreviewContainer;
        private PreviewView _preview;

        Label _infoLabel = new Label
        {
            Text = "Your device is not connected to the internet.\n" +
                   "App2Night will use cached data if available.",
            TextColor = Color.White
        };
        BoxView _infoBackgroundBoxView = new BoxView
        {
            Color = System.Drawing.Color.DarkOrange.ToXamarinColor()
        }; 

        private Grid _contentGrid;
        private Grid _previewGrid;

        public  stomContentPage()
        {
            _contentGrid = new Grid
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(CrossConnectivity.Current.IsConnected ? 0 : _size, GridUnitType.Absolute)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    
                     _infoBackgroundBoxView,
                     _infoLabel 
                }
            };

            _previewGrid = new Grid
            {
                Children =
                {
                    _contentGrid,
                   _previewContainer1,
                   _previewContainer2
                }
            }; 

            base.Content = _previewGrid;

            //Set start connectivity value.
            _oldValue = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += ConnectionChanged; 
        }

        private bool _oldValue;

        private void ConnectionChanged(object sender, ConnectivityChangedEventArgs connectivityChangedEventArgs)
        {
            var connected = connectivityChangedEventArgs.IsConnected;
            Device.BeginInvokeOnMainThread(() =>
            { 
                if (_oldValue && !connected)
                {
                    //Device lost connection, slide in info
                    var animation = new Animation(d => 
                    {
                        _infoLabel.TranslationY = -_size + _size * d;
                        _contentGrid.RowDefinitions[0].Height = new GridLength(d * _size, GridUnitType.Absolute);
                    });
                    animation.Commit(this, "SlideInInfo", easing: Easing.CubicInOut, length: 500U);
                }
                else if (!_oldValue && connected)
                {
                    //Device got connected, slide out info 
                    var animation = new Animation(d =>
                    {
                        _infoLabel.TranslationY = -_size * d;
                        _contentGrid.RowDefinitions[0].Height = new GridLength((1 - d) * _size, GridUnitType.Absolute);
                    }, 0, 1);
                    animation.Commit(this, "SlideInInfo", easing: Easing.CubicInOut, length: 500U);
                }
                _oldValue = connected;
            });
        }

        protected override void OnDisappearing()
        {
            CrossConnectivity.Current.ConnectivityChanged -= ConnectionChanged;

            base.OnDisappearing();
        } 

        /// <summary>
        /// Set the page Content.
        /// </summary>
        public new Xamarin.Forms.View Content
        {
            get { return _contentGrid.Children.Count>2?  _contentGrid.Children[1] : null; }
            set
            {
                if (_contentGrid.Children.Count > 2)
                {
                    _contentGrid.Children.RemoveAt(2);
                }
                _contentGrid.Children.Add(value, 0, 1); 
            }
        }

        /// <summary>
        /// Gets fired if a partie item gets tapped on.
        /// Will display the party info view.
        /// </summary> 
        public async void PreviewItemSelected<TItemType, TPreviewType>(TItemType sender, object[] parameter) where TPreviewType : PreviewView
        {
            var p = new List<object> { sender };
            p.AddRange(parameter);

            _preview = (TPreviewType)Activator.CreateInstance(typeof(TPreviewType), p.ToArray());
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
            //Search for the current container
            var currentContainerRef = _selectedPreviewContainer == 0 ? _previewContainer1 : _previewContainer2;
            //Start closing animation in view 
            _preview.StartClosingAnimataion(ClosingAnimationLength);
            //Move container out of the view
            await currentContainerRef.TranslateTo(0, _preview.HeightRequest, ClosingAnimationLength, Easing.CubicInOut);
            //Preview is not longer visible
            _isPreviewVisible = false;
            //Hide container
            _previewContainer1.HeightRequest = 0;
            _previewContainer1.IsVisible = false;
            _preview = null;
            _previewContainer1.Content = null;
            _previewContainer1.TranslationY = 0;
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
            //Select previewContainer1 as final container
            _previewContainer1.Content = newView;
            //Move container to bottom
            _previewContainer1.TranslationY = newView.HeightRequest;
            //Make container visible
            _previewContainer1.IsVisible = true;
            //Start animations in view
            newView.StartOpeningAnimation(OpeningAnimationLength);
            //Move container up
            await _previewContainer1.TranslateTo(0, 0, OpeningAnimationLength, Easing.CubicInOut);
            //Preview is visible now
            _isPreviewVisible = true;
            //Set the preview to the new view.
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
                nextContainerRef.TranslationX = -Width * (1 - d);
                currentContainerRef.TranslationX = Width * d;
            });
            rotateAnimation.Commit(this, "RotateAnimation", length: SwitchingAnimationLength, easing: Easing.CubicInOut, finished: (d, b) =>
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