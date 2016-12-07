using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Service.Helper;
using FreshMvvm;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;
using static System.Drawing.Color;

namespace App2Night.CustomView.Page
{
    public class  CustomContentPage : FreshBaseContentPage
    {

        public static BindableProperty ShowNoContentWarningProperty = BindableProperty.Create(nameof(ShowNoContentWarning), 
            typeof(bool), 
            typeof(CustomContentPage), 
            false,
            propertyChanged: (bindable, value, newValue) =>
            {
                ((CustomContentPage)bindable).ContentAvailabilityChanged();
            });
        public bool ShowNoContentWarning
        {
            get { return (bool)GetValue(ShowNoContentWarningProperty); }
            set { SetValue(ShowNoContentWarningProperty, value); }
        } 
    
        private static readonly double _size = 50;
        private static Color _infoBackgroundColor = Khaki.ToXamarinColor();
        private static Color _infoTextColor = DimGray.ToXamarinColor();

        public string OfflineMessage
        {
            get { return _infoLabel.Text; }
            set { _infoLabel.Text = value; }
        }
        
        public string NoContentWarningMessage
        {
            get { return _noContentWarningLabel.Text; }
            set {  _noContentWarningLabel.Text = value; } 
        }

        

        BoxView _referenceView;

        public uint OpeningAnimationLength { get; set; } = 500;
        public uint ClosingAnimationLength { get; set; } = 500;
        public uint SwitchingAnimationLength { get; set; } = 500;

        /// <summary>
        /// Animations for the preview view gets handled here.
        /// </summary> 
        private bool _isPreviewVisible;
        private int _selectedPreviewContainer;
        private PreviewView _preview;

        private Label _noContentWarningLabel = new Label
        {
            Text = "No content availably",
            IsVisible = false
        };

        Label _infoLabel = new Label
        {
            Text = "Your device is not connected to the internet.\n" +
                   "App2Night will use cached data if available.",
            TextColor = _infoTextColor
        };
        BoxView _infoBackgroundBoxView = new BoxView
        {
            Color = _infoBackgroundColor
        }; 
         
        private Grid _mainLayout;

        public  CustomContentPage()
        {
            _mainLayout = new Grid
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(CrossConnectivity.Current.IsConnected ? 0 : _size, GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(4, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(5, GridUnitType.Star)}
                },
                Children =
                {
                    
                     _infoBackgroundBoxView,
                     _infoLabel,
                     _noContentWarningLabel,
                }
            };  

            Grid.SetRowSpan(_noContentWarningLabel,3);

            base.Content = _mainLayout;

            //Set start connectivity value.
            _oldValue = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += ConnectionChanged; 
        }

        /// <summary>
        /// Set the page Content.
        /// </summary>
        public new Xamarin.Forms.View Content
        {
            get { return _mainLayout.Children.Count > 3 ? _mainLayout.Children[2] : null; }
            set
            {
                _mainLayout.Children.Remove(_noContentWarningLabel);

                if (_mainLayout.Children.Count > 2)
                {
                    _mainLayout.Children.RemoveAt(2);
                } 

                _mainLayout.Children.Add(value, 0, 1);
                Grid.SetRowSpan(value, 2);

                _mainLayout.Children.Add(_noContentWarningLabel);
            }
        }

        public void ContentAvailabilityChanged()
        {
            _noContentWarningLabel.IsVisible = ShowNoContentWarning;

        }

        private bool _oldValue;

        private void ConnectionChanged(object sender, ConnectivityChangedEventArgs connectivityChangedEventArgs)
        {
            var connected = connectivityChangedEventArgs.IsConnected;
            Device.BeginInvokeOnMainThread(() =>
            { 
                if (_oldValue && !connected)
                    //Device lost connection, slide in info
                    ShowInformationAnimation();
                else if (!_oldValue && connected) 
                    //Device got connected, slide out info 
                    HideInformationAnimation(); 
                _oldValue = connected;
            });
        }

        private void HideInformationAnimation()
        {
            var animation = new Animation(d =>
            {
                _infoLabel.TranslationY = -_size * d;
                _mainLayout.RowDefinitions[0].Height = new GridLength((1 - d) * _size, GridUnitType.Absolute);
            }, 0, 1);
            animation.Commit(this, "SlideInInfo", easing: Easing.CubicInOut, length: 500U);
        }

        private void ShowInformationAnimation()
        {
            var animation = new Animation(d =>
            {
                _infoLabel.TranslationY = -_size + _size * d;
                _mainLayout.RowDefinitions[0].Height = new GridLength(d * _size, GridUnitType.Absolute);
            });
            animation.Commit(this, "SlideInInfo", easing: Easing.CubicInOut, length: 500U);
        }

        protected override void OnDisappearing()
        {
            CrossConnectivity.Current.ConnectivityChanged -= ConnectionChanged; 
            base.OnDisappearing();
        }

        private object _lastSender;

        /// <summary>
        /// Opens a preview for the object.
        /// </summary> 
        /// <typeparam name="TPreviewType">A derivation <see cref="PreviewView"/> that will show the object.</typeparam>
        /// <param name="sender">Object that will be shown in the preview.</param>
        /// <param name="parameter">Parameter for the <see cref="TPreviewType"/>.</param>
        public void PreviewItemSelected<TItemType, TPreviewType>(TItemType sender, object[] parameter) where TPreviewType : PreviewView
        {
            //Check if the new object is already displayed and close the view if it is already displayed
            if (_lastSender != null && _lastSender == (object)sender)
            {
                Device.BeginInvokeOnMainThread(async () => await ClosePreview()); 
                return;
            }
            _lastSender = sender;

            var p = new List<object> { sender };
            p.AddRange(parameter);

            //Create an instance of the PreviewView
            var nextView = (TPreviewType)Activator.CreateInstance(typeof(TPreviewType), p.ToArray());
            nextView.CloseViewEvent += ClosePreviewEvent;
            ShowPreview(nextView);
        }

        private void ClosePreviewEvent(object sender, EventArgs eventArgs)
        { 
            Device.BeginInvokeOnMainThread(async ()=>  await ClosePreview() );
        } 

        private void ShowPreview(PreviewView view)
        {
            if (_isPreviewVisible)
                ChangeToPreview(view);
            else
                OpenPreview(view);
        }

        private async Task ClosePreview()
        {
            if (_isPreviewVisible)
            {
                _preview.CloseViewEvent -= ClosePreviewEvent;
                //Preview is not longer visible
                _isPreviewVisible = false;
                //Start closing animation in view 
                _preview.StartClosingAnimataion(ClosingAnimationLength);
                //Move container out of the view
                await _preview.TranslateTo(0, _preview.HeightRequest, ClosingAnimationLength, Easing.CubicInOut);

                //Hide container
                //currentContainerRef.HeightRequest = 0;
                _mainLayout.Children.RemoveAt(4);
                _preview = null;
                _lastSender = null;
            } 
        }

        private void OpenPreview(PreviewView newView)
        {
            var height = Height / 9.0 * 5;

            _preview = newView;
            _mainLayout.Children.Add(_preview, 0, 2);
            _preview.HeightRequest = height;

            //Move container to bottom
            _preview.TranslationY = height; 

            //Make container visible
            _preview.IsVisible = true;  
            
            //Start animations in view
            _preview.StartOpeningAnimation(OpeningAnimationLength); 
            
            //Move container up 
            var animation = new Animation(d =>
            {
                _preview.TranslationY = d*height;
                ForceLayout();
            },1,0);
            animation.Commit(_preview, "OpeningAnimation", easing: Easing.CubicInOut);  

            _isPreviewVisible = true;
            //Set the preview to the new view. 
        }

        private void ChangeToPreview(PreviewView newView)
        {
            var nextPreview = newView;
            nextPreview.IsVisible = false;
            _mainLayout.Children.Add(nextPreview, 0, 2);
            nextPreview.TranslationX = -Width; 
            nextPreview.IsVisible = true;
            
            var animation = new Animation(d =>
            {
                _preview.TranslationX = Width * d;
                nextPreview.TranslationX = -Width*(1 - d); 
            });
            animation.Commit(this, "swipeAnimation", length: SwitchingAnimationLength, easing: Easing.CubicInOut,
                finished: (d, b) =>
                {
                    _mainLayout.Children.Remove(_preview);
                    _preview = newView;
                }); 
        }
    }
}