using System;
using System.Collections.Generic;
using System.Linq;
using MvvmNano;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class GallerieView : CustomScrollView
    { 

        public static BindableProperty ElementTappedCommandProperty = BindableProperty.Create(nameof(ElementTappedCommand), typeof(MvvmNanoCommand), typeof(GallerieView));
        public MvvmNanoCommand ElementTappedCommand
        {
            get { return (MvvmNanoCommand)GetValue(ElementTappedCommandProperty); }
            set { SetValue(ElementTappedCommandProperty, value); }
        }

        public event EventHandler<object> ElementTapped;


        public static BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(IEnumerable<object>), typeof(GallerieView),
            propertyChanged:(bindable, value, newValue) => ((GallerieView)bindable).GenerateElemets());
        public IEnumerable<object> ItemSource
        {
            get { return (IEnumerable<object>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public double ElementSize { get; set; } = 100;
        public int MaxRows { get; set; } = 1;
        public bool FitRows { get; set; } = false;

        public double Spacing
        {
            get { return _spacing; }
            set
            {
                _spacing = value;
                _contentGrid.RowSpacing = Spacing;
                _contentGrid.ColumnSpacing = Spacing;
            }
        }
         
        private Grid _contentGrid = new Grid();
        private double _spacing = 5; 

        void GenerateElemets()
        {
            //Make sure to dispose old handler
            DisposeHandler(); 
            if (ItemSource == null) return;
            _contentGrid.Children.Clear();
            foreach (object o in ItemSource)
            {
                var view = new BoxView() {Color = Color.Gray.MultiplyAlpha(0.5)};
                view.BindingContext = new Party
                {
                    Name = "TESTPARTY!",
                    Date = DateTime.Today.AddDays(19)
                };
                _contentGrid.Children.Add(view);
                var gestureRekognizer = new TapGestureRecognizer();
                gestureRekognizer.Tapped += GestureRekognizerOnTapped;
                view.GestureRecognizers.Add(gestureRekognizer);
            }
        }

        /// <summary>
        /// Gets triggered if an element gets tapped.
        /// </summary> 
        private void GestureRekognizerOnTapped(object sender, EventArgs eventArgs)
        {
            var contextObject = ((Xamarin.Forms.View) sender).BindingContext;
            ElementTappedCommand?.Execute(contextObject);
            var handler = ElementTapped;
            handler?.Invoke(this, contextObject);
        }

        /// <summary>
        /// Removes GestureRekognizerOnTapped handlers from all items.
        /// </summary>
        void DisposeHandler()
        {
            foreach (Xamarin.Forms.View view in _contentGrid.Children)
            {
                ((TapGestureRecognizer) view.GestureRecognizers[0]).Tapped -= GestureRekognizerOnTapped;
            }
        } 

        private int oldWidth = 0;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (oldWidth != (int) width)
            {
                oldWidth = (int)width; 
                ArrengeElements(); 
            }
        }

        void ArrengeElements()
        {
            if (Height <= 0) return;
            if (_contentGrid.Children.Count == 0) return;

            //Clear the grid
            _contentGrid.Padding = Spacing;
            _contentGrid.RowDefinitions.Clear();
            _contentGrid.ColumnDefinitions.Clear();

            //Gallerie view can take a given ElementSize or calculate 
            //the actualElementSize based on a maxElementSize and the Height ob the view.
            int rows = MaxRows;
            double actualElementSize = ElementSize;
            if (!FitRows)
            {
                //Calculate how many rows are needed
                int visibleElementsPerRow = (int)Math.Ceiling(Width / actualElementSize);
                int neededRows = 0;
                int tmpElementCount = ItemSource.Count();
                for (int i = 0; i < rows; i++)
                {
                    if (tmpElementCount > visibleElementsPerRow)
                    {
                        neededRows++;
                        tmpElementCount -= visibleElementsPerRow;
                    }
                    else
                        break;
                } 
                rows = neededRows == 0 ? 1 : neededRows;
                HeightRequest = (rows - 1) * Spacing + Spacing * 2 + rows * ElementSize;
            }
            else
            {
                double rowsRaw = (Height - Spacing * 2) / (ElementSize);
                rows = (int)Math.Ceiling(rowsRaw);
                actualElementSize = (Height - Spacing * 2 - (Spacing * (rows - 1))) / rows;
            }
            if (rows == 0) return;

            //Create the rows
            CreateGridRows(rows);

            //Create the columns
            CreateGridColumns(rows, actualElementSize);

            //Position the elements
            PositionElementsInGrid();

            //var totalElementWidth = actualElementSize*ItemSource.Count() + Spacing*2 + Spacing*(ItemSource.Count() - 1);
            //IsEnabled = totalElementWidth > Width;
        }

        private void CreateGridColumns(int rows, double actualElementSize)
        {
            var columns = (int)Math.Ceiling(ItemSource.Count() / (double)rows);
            for (int i = 0; i < columns; i++)
            {
                _contentGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(actualElementSize, GridUnitType.Absolute)
                });
            }
        }

        void CreateGridRows(int rows)
        {
            for (int i = 0; i < rows; i++)
            {
                _contentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
        }

        void PositionElementsInGrid()
        {
            int columnCount = 0;
            int row = 0; 
            foreach (Xamarin.Forms.View o in _contentGrid.Children)
            {
                if (_contentGrid.ColumnDefinitions.Count <= columnCount)
                {
                    columnCount = 0;
                    row++;
                }
                Grid.SetRow(o, row);
                Grid.SetColumn(o, columnCount);
                columnCount++;
            }
        }

        public GallerieView()
        {  
            GenerateElemets();
            Orientation = ScrollOrientation.Horizontal;
            Content = _contentGrid;
        }
    }
}