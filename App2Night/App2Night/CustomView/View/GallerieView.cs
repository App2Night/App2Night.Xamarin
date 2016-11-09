using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MvvmNano;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class GallerieView : CustomScrollView
    {
        private double _spacing = 8;
        protected Grid ContentGrid = new Grid(); 

        public double Spacing
        {
            get { return _spacing; }
            set
            {
                _spacing = value;
                ContentGrid.RowSpacing = Spacing;
                ContentGrid.ColumnSpacing = Spacing;
            }
        }

        public static BindableProperty ElementTappedCommandProperty = BindableProperty.Create(nameof(ElementTappedCommand), typeof(MvvmNanoCommand), typeof(HorizontalGallerieView));
        public MvvmNanoCommand ElementTappedCommand
        {
            get { return (MvvmNanoCommand)GetValue(ElementTappedCommandProperty); }
            set { SetValue(ElementTappedCommandProperty, value); }
        }

        public event EventHandler<object> ElementTapped;


        public static BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(IEnumerable<object>), typeof(HorizontalGallerieView),
            propertyChanged: (bindable, value, newValue) =>
            ((GallerieView)bindable).CollectionSet((IEnumerable<object>) value, (IEnumerable<object>) newValue));
        public IEnumerable<object> ItemSource
        {
            get { return (IEnumerable<object>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        void CollectionSet(IEnumerable<object> oldCollection, IEnumerable<object> newCollection)
        {
            if (oldCollection != null)
            {
                //Check if old collection is observable and remove handler if it is.
                bool isOldObservableCollection = oldCollection.GetType().GetGenericTypeDefinition() ==
                                                 typeof (ObservableCollection<>);
                if (isOldObservableCollection)
                {
                    ((INotifyCollectionChanged)oldCollection).CollectionChanged -= OnCollectionChanged;
                }
            }

            if (newCollection != null)
            {
                //Check if new collection is observable and add handler if it is.
                bool isNewObservableCollection = newCollection.GetType().GetGenericTypeDefinition() ==
                                                 typeof(ObservableCollection<>);
                if (isNewObservableCollection)
                {
                    ((INotifyCollectionChanged)newCollection).CollectionChanged += OnCollectionChanged;
                }
                GenerateElemets();
            } 
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            GenerateElemets();
        }

        public GallerieView()
        {
            GenerateElemets();
            Content = ContentGrid;
        }

        public Type Template { get; set; } 

        void GenerateElemets()
        {
            //Make sure to dispose old handler
            DisposeHandler();
            if (ItemSource == null) return;
            ContentGrid.Children.Clear();
            foreach (object o in ItemSource)
            {
                Xamarin.Forms.View view;
                if (Template == null)
                    view = new BoxView() {Color = Color.Gray.MultiplyAlpha(0.5)};
                else
                    view = (Xamarin.Forms.View) Activator.CreateInstance(Template);
                view.BindingContext = o;
                ContentGrid.Children.Add(view);
                var gestureRekognizer = new TapGestureRecognizer();
                gestureRekognizer.Tapped += GestureRekognizerOnTapped;
                view.GestureRecognizers.Add(gestureRekognizer);
            }
            ArrengeElements();
        }

        /// <summary>
        /// Removes GestureRekognizerOnTapped handlers from all items.
        /// </summary>
        void DisposeHandler()
        {
            foreach (Xamarin.Forms.View view in ContentGrid.Children)
            {
                ((TapGestureRecognizer)view.GestureRecognizers[0]).Tapped -= GestureRekognizerOnTapped;
            }
        }

        /// <summary>
        /// Gets triggered if an element gets tapped.
        /// </summary> 
        private void GestureRekognizerOnTapped(object sender, EventArgs eventArgs)
        {
            var contextObject = ((Xamarin.Forms.View)sender).BindingContext;
            ElementTappedCommand?.Execute(contextObject);
            var handler = ElementTapped;
            handler?.Invoke(this, contextObject);
        }

        private int oldWidth = 0;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (oldWidth != (int)width)
            {
                oldWidth = (int)width;
                ArrengeElements();
            }
        }

        protected virtual void ArrengeElements()
        {

            
            //Clear the grid
            ContentGrid.Padding = Spacing;
            ContentGrid.RowDefinitions.Clear();
            ContentGrid.ColumnDefinitions.Clear();
        }
    }
}