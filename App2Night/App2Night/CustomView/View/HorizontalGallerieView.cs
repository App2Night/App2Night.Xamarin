using System;
using System.Linq;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{  
    public class HorizontalGallerieView : GallerieView
    {   
        public int Columns { get; set; } = 2;
        public int Rows { get; set; } = 1;

        protected override void ArrengeElements()
        {
            if (Height < 0 || Width < 0) return;
            if (ContentGrid.Children.Count == 0) return;
            base.ArrengeElements();

            //Calculate the size of an element.
            double elementSize = (Width
                                  - Spacing*2 //Left and right spacing
                                  - Spacing*(Columns - 1)) //Spacing between columns
                                 /Columns;

            int neededRows = Rows;
            int maxRows = (int) Math.Ceiling(ContentGrid.Children.Count()/(double) Columns);
            if (maxRows < neededRows) neededRows = maxRows;

            //Elements in a row
            int elementsPerRow = (int) Math.Ceiling( ContentGrid.Children.Count()/ (double)neededRows);

            for (int i = 0; i < elementsPerRow; i++)
            {
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(elementSize, GridUnitType.Absolute)});
            }
            for (int i = 0; i < neededRows; i++) 
            {
                ContentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(elementSize, GridUnitType.Absolute) }); 
            }
            int tmpRowCounter = 0;
            int tmpColumnCounter = 0;
            foreach (Xamarin.Forms.View view in ContentGrid.Children)
            {
                view.HeightRequest = elementSize;
                view.WidthRequest = elementSize;
                if (tmpColumnCounter == elementsPerRow)
                {
                    tmpRowCounter++;
                    tmpColumnCounter = 0;
                }
                Grid.SetColumn(view, tmpColumnCounter);
                Grid.SetRow(view, tmpRowCounter);
                tmpColumnCounter++;
            } 
        }

        public HorizontalGallerieView()
        {   
            Orientation = ScrollOrientation.Horizontal; 
        }
    }
}