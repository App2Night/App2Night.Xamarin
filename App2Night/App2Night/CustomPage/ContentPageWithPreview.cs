using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App2Night.CustomView;
using MvvmNano;
using MvvmNano.Forms;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomPage
{
    public class ContentPageWithPreview<TViewModel> : MvvmNanoContentPage<TViewModel> where TViewModel : MvvmNanoViewModel
    {
        private readonly ContentView _previewContainer1 = new ContentView
        {
            IsVisible = false
        };

        private readonly ContentView _previewContainer2 = new ContentView
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

        public ContentView _content = new ContentView();

        public new Xamarin.Forms.View  Content
        {
            get { return _content.Content; }
            set { _content.Content = value; }
        }

        public ContentPageWithPreview()
        {
            base.Content = new Grid
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                },
                Children =
                {
                    _content,
                    {_previewContainer1, 0,1 },
                    {_previewContainer2, 0,1 } 
                }
            };
            Grid.SetRowSpan(_content, 2);
        }



        /// <summary>
        /// Gets fired if a partie item gets tapped on.
        /// Will display the party info view.
        /// </summary> 
        public async void PreviewItemSelected<TItemType, TPreviewType>(TItemType sender, object [] parameter) where TPreviewType : PreviewView
        {
            var p = new List<object> {sender};
            p.AddRange(parameter);
           
            _preview = (TPreviewType) Activator.CreateInstance(typeof(TPreviewType),p.ToArray());
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
            _preview.StartClosingAnimataion(ClosingAnimationLength);
            await currentContainerRef.TranslateTo(0, _preview.HeightRequest, ClosingAnimationLength, Easing.CubicInOut);
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
            newView.StartOpeningAnimation(OpeningAnimationLength);
            await _previewContainer1.TranslateTo(0, 0, OpeningAnimationLength, Easing.CubicInOut);
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
                nextContainerRef.TranslationX = -Width * (1 - d);
                currentContainerRef.TranslationX = Width * d;
            });
            rotateAnimation.Commit(this, "RotateAnimation", length:SwitchingAnimationLength, easing: Easing.CubicInOut, finished: (d, b) =>
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