using Android.Graphics;  
using PartyUp.CustomView;
using PartyUp.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundView), typeof(RoundViewRenderer))]
namespace PartyUp.Droid.CustomRenderer
{
    public class RoundViewRenderer : ViewRenderer<RoundView, ViewRenderer>
    {  
        public override void Draw(Canvas canvas)
        { 
            var rect = new Rect();
            var paint = new Paint()
            {
                Color = Element.BackgroundColor.ToAndroid(),
                AntiAlias = true,
            };

            GetDrawingRect(rect);

            var radius = (float)(rect.Width() / Width * 90);
            Path path = new Path();
            path.MoveTo((float) (rect.Width() * 1/3.0), 0);
            path.LineTo((float)(rect.Width() * 2/ 3.0), 0);

            canvas.DrawRoundRect(new RectF(rect), Width/2, Height/2, paint);
        }
    }
}