using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PartyUp.CustomView
{
    public class SwipeView : ContentView
    {
        private Xamarin.Forms.View _topView => _mainGrid.Children.LastOrDefault();

        private Xamarin.Forms.View itemTemplate;
    


        public static BindableProperty ItemSourceProperty =
            BindableProperty.Create(nameof(ItemSource), typeof(IEnumerable<object>), typeof(SwipeView),
                propertyChanged: (bindable, value, newValue) => ((SwipeView)bindable).CollectionSet((IEnumerable<object>)value, (IEnumerable<object>)newValue));

        private void CollectionSet(IEnumerable<object> oldValue, IEnumerable<object> newValue)
        {
            AllCards.Clear();
            _mainGrid.Children.Clear();
            if (newValue == null) return; 
            var rnd = new Random();
            foreach (object o in newValue)
            {  
                var card = new BoxView() //Replace this grid with a fancy card design
                {
                    Color = Color.FromRgb(rnd.Next(0,100)/100.0, rnd.Next(0, 100) / 100.0, rnd.Next(0, 100) / 100.0)
                    ,InputTransparent = true,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                _mainGrid.Children.Add(card);
            }
            SetCardSize();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            SetCardSize();
        }

        void SetCardSize()
        {
            foreach (Xamarin.Forms.View view in _mainGrid.Children)
            {
                view.WidthRequest = Width*(5.0/7);
                view.HeightRequest = Height * (5.0 / 7); 
            }
        }

        public IEnumerable<object> ItemSource
        {
            get { return (IEnumerable<object>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public Xamarin.Forms.View ItemTemplate
        {
            get
            {
                return itemTemplate;
            }

            set
            {
                itemTemplate = value;
            }
        }

        public List<Xamarin.Forms.View> AllCards = new List<Xamarin.Forms.View>();

        Grid _mainGrid = new Grid
        {
            BackgroundColor = Color.White.MultiplyAlpha(0.0001)
        };

        PanGestureRecognizer _gesture = new PanGestureRecognizer();

        public SwipeView()
        {
            CollectionSet(null, ItemSource);
            Content = _mainGrid;    
            _gesture.PanUpdated += GestureOnPanUpdated;      
            _mainGrid.GestureRecognizers.Add(_gesture);
        }


        double _lastX = 0;
        double _lastY = 0;
        private bool _removed = false;
        private async void GestureOnPanUpdated(object sender, PanUpdatedEventArgs panUpdatedEventArgs)
        {
            var x = panUpdatedEventArgs.TotalX;
            var y = panUpdatedEventArgs.TotalY;
            if (_topView != null)
            {
                if (x == 0 && y == 0) //reset view!
                {
                    Debug.WriteLine("Reset");
                    _removed = false;
                    if (_lastX != 0 || _lastY != 0)
                        await _topView.TranslateTo(0, 0, 500U, Easing.CubicInOut);
                }
                else if(!_removed)
                {
                    Debug.WriteLine("Pan: " + x+ " " + y);

                    _lastX = x;
                    _lastY = y; 
                    var bound = Width*(5/7.0);
                    _removed = true;
                   

                    if (x > bound) //Move right out of the picture
                    {
                        await _topView.TranslateTo(Width, y);
                        _mainGrid.Children.Remove(_topView);
                    }
                    else if (x < -bound) //Move left out of the picture
                    {
                        await _topView.TranslateTo(-Width, y);
                        _mainGrid.Children.Remove(_topView); 
                    }
                    else
                    {
                        await _topView.TranslateTo(x, y, 10U);
                        _removed = false;

                    }

                } 
            }   
        } 
    }
}
