using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class SwipeView : ContentView
    {
        Queue<object> _nextCards = new Queue<object>();

        private Xamarin.Forms.View TopView => _mainGrid.Children.LastOrDefault(); 

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(SwipeView),
                propertyChanged: (bindable, value, newValue) => ((SwipeView)bindable).CollectionSet());


        public static BindableProperty SwipedOutLeftCommandProperty = BindableProperty.Create(nameof(SwipedOutLeft), typeof(Command<object>), typeof(SwipeView));

        public Command<object> SwipedOutLeft
        {
            get { return (Command<object>)GetValue(SwipedOutLeftCommandProperty); }
            set { SetValue(SwipedOutLeftCommandProperty, value); }
        }


        public static BindableProperty SwipeOutRightCommandProperty = BindableProperty.Create(nameof(SwipeOutRightCommand), typeof(Command<object>), typeof(SwipeView));
        public Command<object> SwipeOutRightCommand
        {
            get { return (Command<object>)GetValue(SwipeOutRightCommandProperty); }
            set { SetValue(SwipeOutRightCommandProperty, value); }
        }


        private void CollectionSet()
        {
            AllCards.Clear();
            _nextCards.Clear();
            _mainGrid.Children.Clear();
            if (ItemsSource == null) return;

            //Add all items to the queue.
            foreach (object o in ItemsSource)
            {
                _nextCards.Enqueue(o);
            }

            //Add the first three queue items as card to the view.
            for (int i = 0; i < 3; i++)
            {
                AddCardFromQueue();
            }
            SetCardSize();
        }

        void AddCardFromQueue()
        {
            if (_nextCards.Count > 0)
            {
                AddCard(_nextCards.Dequeue());
            }
        }

        void AddCard(object o)
        {
            Xamarin.Forms.View card;
            if (_templateType != null)
            {
                card = (Xamarin.Forms.View)Activator.CreateInstance(_templateType);
            }
            else
            {
                card = new ContentView
                {
                    Content = new Label() { Text = "Override this template with SetTemplate." }
                };
            }

            //Give the new card a random background color
            var rnd = new Random();
            var cardBackgroundColor =
                Color.FromRgb(rnd.Next(0, 100) / 100.0, rnd.Next(0, 100) / 100.0, rnd.Next(0, 100) / 100.0);
            cardBackgroundColor.AddLuminosity(50);
            card.BackgroundColor = cardBackgroundColor;

            //Little shake to give a natural feeling 
            card.Rotation = GenerateRandomNumber();
            card.BindingContext = o;
            card.InputTransparent = true;
            card.Margin = RelativeMargin;
            //var cardCount = _mainGrid.Children.Count;
            //if (cardCount == 0)
            //    _mainGrid.Children.Add(card);
            //else
            _mainGrid.Children.Insert(0, card);
        } 

        private int lastRandom = 1337;
        double GenerateRandomNumber()
        {
            var random = new Random(lastRandom);
            lastRandom = random.Next(-750, 750);
            return lastRandom/100.0;
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
                view.Margin = RelativeMargin;
            }
        }

        Thickness RelativeMargin=> Width * (1.0 / 10);

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private Type _templateType;

        public void SetTemplate<TType>( ) where TType : Xamarin.Forms.View
        {
            _templateType = typeof(TType);
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
         

        private void GestureOnPanUpdated(object sender, PanUpdatedEventArgs panUpdatedEventArgs)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var x = panUpdatedEventArgs.TotalX;
                var y = panUpdatedEventArgs.TotalY;
                if (TopView != null)
                {
                    if (x == 0 && y == 0) //reset view to the stack!
                    {
                        Debug.WriteLine("Reset card to stack");
                        _removed = false;
                        if (_lastX != 0 || _lastY != 0)
                            await TopView.TranslateTo(0, 0, 500U, Easing.CubicInOut);
                    }
                    else if (!_removed)
                    { 
                        _lastX = x;
                        _lastY = y;
                        var bound = Width * (5 / 7.0);
                        _removed = true;


                        if (x > bound) //Move right out of the picture
                        {
                            await TopView.TranslateTo(Width, y);
                            MovedOutRight();
                            _mainGrid.Children.Remove(TopView);

                            AddCardFromQueue();

                        }
                        else if (x < -bound) //Move left out of the picture
                        {
                            await TopView.TranslateTo(-Width, y);
                            MovedOutLeft();
                            _mainGrid.Children.Remove(TopView);

                            AddCardFromQueue();

                        }
                        else
                        {
                            await TopView.TranslateTo(x, y, 10U);
                            _removed = false;

                        }

                    }
                }
            });
        }

        void MovedOutLeft()
        {
            SwipedOutLeft?.Execute(TopView.BindingContext);
        }

        void MovedOutRight()
        { 
            SwipeOutRightCommand?.Execute(TopView.BindingContext);
        } 
    }
}
