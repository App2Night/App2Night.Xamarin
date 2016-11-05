using App2Night.Helper;
using MvvmNano;
using MvvmNano.Forms;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace App2Night.CustomView.Page
{
    public class ContentPageWithInfo<TViewModel> : MvvmNanoContentPage<TViewModel> where TViewModel : MvvmNanoViewModel
    {
        private static readonly double _size = 50;

        Label _infoLabel = new Label
        {
            Text = "Your device is not connected to the internet.\n" +
                   "App2Night will use cached data if available." 
        };
        BoxView _infoBackgroundBoxView = new BoxView
        {
            Color = System.Drawing.Color.DarkOrange.ToXamarinColor()
        };

        private Grid _grid;
        public ContentPageWithInfo()
        { 
            _grid = new Grid
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
            base.Content = _grid;

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
                        _grid.RowDefinitions[0].Height = new GridLength(d * _size, GridUnitType.Absolute);
                    });
                    animation.Commit(this, "SlideInInfo", easing: Easing.CubicInOut, length: 500U);
                }
                else if (!_oldValue && connected)
                {
                    //Device got connected, slide out info 
                    var animation = new Animation(d =>
                    {
                        _infoLabel.TranslationY = -_size * d;
                        _grid.RowDefinitions[0].Height = new GridLength((1 - d) * _size, GridUnitType.Absolute);
                    }, 0, 1);
                    animation.Commit(this, "SlideInInfo", easing: Easing.CubicInOut, length: 500U);
                }
                _oldValue = connected;
            });
        }


        public override void Dispose()
        {
            //Clean up that handler.
            CrossConnectivity.Current.ConnectivityChanged -= ConnectionChanged;
            base.Dispose();
        }

        /// <summary>
        /// Set the page Content.
        /// </summary>
        public new Xamarin.Forms.View Content
        {
            get { return _grid.Children.Count>2?  _grid.Children[1] : null; }
            set
            {
                if (_grid.Children.Count > 2)
                {
                    _grid.Children.RemoveAt(2);
                }
                _grid.Children.Add(value, 0, 1);
            }
        }
    }
}