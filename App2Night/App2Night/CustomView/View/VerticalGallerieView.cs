using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class VerticalGallerieView : GallerieView
    {
        public int Columns { get; set; } = 3;

        public VerticalGallerieView()
        {
            Orientation = ScrollOrientation.Vertical;
        }

        protected override void ArrengeElements()
        {
            if (Height <= 0) return;
            if (ContentGrid.Children.Count == 0) return;
            base.ArrengeElements();
            double elementWidth = Width/Columns - Spacing*2 - Spacing*(Columns - 1);
            int tmpColumnCounter = 0;
            int tmpRowCounter = 0;

            for (int i = 0; i < Columns; i++)
            {
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(elementWidth, GridUnitType.Absolute) });
            }
             
            foreach (Xamarin.Forms.View view in ContentGrid.Children)
            {
                if (tmpColumnCounter + 1 > Columns)
                {
                    tmpRowCounter++;
                    tmpColumnCounter = 0;
                }
                Grid.SetColumn(view, tmpColumnCounter);
                Grid.SetRow(view, tmpRowCounter);
                tmpColumnCounter++;
            } 

            for (int index = 0; index < tmpRowCounter; index++)
            {
                ContentGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(elementWidth, GridUnitType.Absolute)});
            }
            var t = ContentGrid.Height;
            ContentGrid.HeightRequest = (tmpRowCounter+1)*elementWidth + Spacing*(tmpRowCounter - 1) + 2*Spacing;
            HeightRequest = ContentGrid.HeightRequest;
            if (ContentGrid.HeightRequest > Height && ContentGrid.HeightRequest > 0)
            {
                InvalidateLayout(); 
            }
        }
    }
}