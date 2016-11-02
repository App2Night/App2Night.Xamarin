using System;
using System.Threading.Tasks;
using App2Night.CustomView;
using App2Night.ViewModel;
using MvvmNano.Forms;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.View
{
    public class HistoryPage : MvvmNanoContentPage<HistoryViewModel>
    {
        private readonly ContentView _previewContainer1 = new ContentView
        {
            IsVisible = false
        };

        private readonly ContentView _previewContainer2 = new ContentView
        {
            IsVisible = false
        };
        public HistoryPage()
        {
            Title = "History";
            var listView = new ListView()
            {
                RowHeight = 100,
                ItemTemplate = new DataTemplate(typeof(PartyTemplate)),
                ItemsSource = new Party[]
                {
                    new Party
                    {
                        Name = "DH goes Party",
                        MusicGenre = MusicGenre.Rock,
                        Date = DateTime.Now.AddDays(30)
                    },
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                }
            };
            listView.ItemTapped += PartieSelected;
            var mainScroll = new ScrollView
            {
                Content = listView,
                Orientation = ScrollOrientation.Vertical
            };
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
            var listView = (ListView)sender;
            var party = (listView.SelectedItem as Party);
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
                nextContainerRef.TranslationX = -Width * (1 - d);
                currentContainerRef.TranslationX = Width * d;
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
