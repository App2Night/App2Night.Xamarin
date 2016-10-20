using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PartyUp.CustomView
{
    public class GallerieView : CustomScrollView
    { 
        public static BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(IEnumerable<object>), typeof(GallerieView),
            propertyChanged:(bindable, value, newValue) => ((GallerieView)bindable).GenerateElemets());
        public IEnumerable<object> ItemSource
        {
            get { return (IEnumerable<object>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public double MaxElementSize { get; set; } = 100;

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
            if (ItemSource == null) return;
            _contentGrid.Children.Clear();
            foreach (object o in ItemSource)
            { 
                _contentGrid.Children.Add(new BoxView() { Color = Color.Gray.MultiplyAlpha(0.2) });
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
            }         }

        void ArrengeElements()
        {
            if (Height <= 0) return;
            if (_contentGrid.Children.Count == 0) return;

            _contentGrid.Padding = Spacing;
            _contentGrid.RowDefinitions.Clear();
            _contentGrid.ColumnDefinitions.Clear();

            int rows = 0;
            double rowsRaw = (Height - Spacing*2 ) /(MaxElementSize);
            rows = (int) (rowsRaw + 0.5);
            double actualElementSize = (Height - Spacing*2 - (Spacing * (rows - 1))) / rows;

            for (int i = 0; i < rows; i++)
            {
                _contentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < ItemSource.Count() / rows; i++)
            {
                _contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(actualElementSize, GridUnitType.Absolute) });
            }
            int columnCount = 0;
            int row = 0;
            foreach (Xamarin.Forms.View o in _contentGrid.Children)
            {
                if (_contentGrid.ColumnDefinitions.Count < columnCount)
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