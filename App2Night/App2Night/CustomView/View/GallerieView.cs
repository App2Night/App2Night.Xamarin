using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            //Make sure to dispose old views
            DisposeOldElements();

            //Check if an ItemSource exists
            if (ItemSource == null) return; 

            foreach (object o in ItemSource)
            {
                //Check if element already exists
                var exists = false;
                foreach (Xamarin.Forms.View child in ContentGrid.Children)
                {
                    if (child.BindingContext == o)
                    {
                        exists = true;
                        break;
                    }
                } 
                if(exists) continue; //Skip this element if it is already part of the List.
                Xamarin.Forms.View view;
                if (Template == null)
                    //Add a default element if no template is set.
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
        /// Removes all views that are not longer part of the ItemSource to avoid duplicated views.
        /// </summary>
        void DisposeOldElements()
        {
            List<Xamarin.Forms.View> removableItems;

            if (ItemSource == null)
            {
                //Remove all items if the new ItemSource is empty.
                removableItems = ContentGrid.Children.ToList();
                Debug.WriteLine("Remove all items from gallerie.");
            } 
            else
                //Only remove items that are no longer part of the new ItemSource
                removableItems = ContentGrid.Children.Where(o => !ItemSource.Contains(o.BindingContext)).ToList();

            //Remove items and dispose the handler
            while (removableItems.Any())
            {
                var view = removableItems.First();
                ((TapGestureRecognizer)view.GestureRecognizers[0]).Tapped -= GestureRekognizerOnTapped;
                ContentGrid.Children.Remove(view);
                removableItems.RemoveAt(0);
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

        private int lastResizeWidth = 0;
        private double tmpResizeWidth = 0;
        private bool newAllocation = false;
        protected async override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            tmpResizeWidth = width;
            await Task.Delay(50); 
            if(tmpResizeWidth!=width) return;
            if (Math.Abs(lastResizeWidth - (int)width) > 3)
            {
                lastResizeWidth = (int)width;
                ArrengeElements();
            }
        }

        /// <summary>
        /// Set the columns/rows and size for all elements.
        /// </summary>
        protected virtual void ArrengeElements()
        { 
            //Clear the grid
            ContentGrid.Padding = Spacing;
            ContentGrid.RowDefinitions.Clear();
            ContentGrid.ColumnDefinitions.Clear();
        }
    }
}