using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class SwipeView : ContentView
    {
        private Xamarin.Forms.View _topView => _mainGrid.Children.LastOrDefault(); 

        public static BindableProperty ItemSourceProperty =
            BindableProperty.Create(nameof(ItemSource), typeof(IEnumerable<object>), typeof(SwipeView),
                propertyChanged: (bindable, value, newValue) => ((SwipeView)bindable).CollectionSet());

        private void CollectionSet()
        {
            AllCards.Clear();
            _mainGrid.Children.Clear();
            if (ItemSource == null) return; 
            foreach (object o in ItemSource)
            {
                Xamarin.Forms.View card;
                if (_templateType != null)
                {
                    card = (Xamarin.Forms.View) Activator.CreateInstance(_templateType);
                }
                else
                {
                    var rnd = new Random();
                    card = new ContentView
                    { 
                        BackgroundColor =
                            Color.FromRgb(rnd.Next(0, 100)/100.0, rnd.Next(0, 100)/100.0, rnd.Next(0, 100)/100.0),
                        Content = new Label() {Text = "Override this template with SetTemplate."}
                    };
                }
                card.BindingContext = o;
                card.InputTransparent = true;
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
                view.Margin = Width*(1.0/10); 
            }
        }

        public IEnumerable<object> ItemSource
        {
            get { return (IEnumerable<object>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        private Type _templateType;

        public void SetTemplate<TType>(TType t) where TType : Xamarin.Forms.View
        {
            _templateType = t.GetType();
        }

        public List<Xamarin.Forms.View> AllCards = new List<Xamarin.Forms.View>();

        Grid _mainGrid = new Grid
        {
            BackgroundColor = Color.White.MultiplyAlpha(0.0001)
        };

        PanGestureRecognizer _gesture = new PanGestureRecognizer();

        public SwipeView()
        {  
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
