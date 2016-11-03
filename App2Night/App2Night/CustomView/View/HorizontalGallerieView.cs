using System;
using System.Collections.Generic;
using System.Linq;
using MvvmNano;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class HorizontalGallerieView : AbstractGallerieView
    {  
        public double ElementSize { get; set; } = 100;
        public int MaxRows { get; set; } = 1;
        public bool FitRows { get; set; } = false;

        protected override void ArrengeElements()
        {
            if (Height <= 0) return;
            if (ContentGrid.Children.Count == 0) return;
            base.ArrengeElements(); 

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
        }

        private void CreateGridColumns(int rows, double actualElementSize)
        {
            var columns = (int)Math.Ceiling(ItemSource.Count() / (double)rows);
            for (int i = 0; i < columns; i++)
            {
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(actualElementSize, GridUnitType.Absolute)
                });
            }
        }

        void CreateGridRows(int rows)
        {
            for (int i = 0; i < rows; i++)
            {
                ContentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
        }

        void PositionElementsInGrid()
        {
            int columnCount = 0;
            int row = 0; 
            foreach (Xamarin.Forms.View o in ContentGrid.Children)
            {
                if (ContentGrid.ColumnDefinitions.Count <= columnCount)
                {
                    columnCount = 0;
                    row++;
                }
                Grid.SetRow(o, row);
                Grid.SetColumn(o, columnCount);
                columnCount++;
            }
        }

        public HorizontalGallerieView()
        {   
            Orientation = ScrollOrientation.Horizontal; 
        }
    }
}