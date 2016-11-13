using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public enum MaskType
    {
        Round, RoundFlatBottom, RoundCorners
    }

    public class MaskedImage : ImageFromPortable
    {
        public MaskType MaskType { get; set; } = MaskType.RoundCorners;
        public bool Edge { get; set; } = true;
        public bool DrawGradient { get; set; } = false;
        public double EdgeSize { get; set; } = 10;
        public Color EdgeColor { get; set; } = Color.Accent;

        private int _lastKnownWidth;
        private int _lastKnownHeight;


        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e); 
            int width = e.Info.Width;
            int height = e.Info.Height;
            _lastKnownWidth = width;
            _lastKnownHeight = height;

            //Rectangle representing the view
            SKRect rect = new SKRect(0, 0, width, height);
            SKPath path = new SKPath();
            if (MaskType == MaskType.RoundFlatBottom)
            {
                //Points generated with inkscape
                path.MoveTo(GetCorrectX(130.148150), GetCorrectY(945));
                FixedCubicTo(path, 57.098614, 841.030040, 0, 698.921590, 0, 554.575240);
                FixedCubicTo(path, 0, 219.793510, 238.396740, -51.600685, 528.728170, -51.600707);
                FixedCubicTo(path, 819.059620, -51.600707, 1054.419800, 219.793510, 1054.419800, 554.575240);
                FixedCubicTo(path, 1054.419800, 712.181670, 1022.872800, 836.647530, 937.369210, 945);
            }
            else if (MaskType == MaskType.Round)
            {
                path.AddCircle((float)(width / 2.0), (float)(height / 2.0),
                    (float)((width > height ? height : width) / 2.0));
            }
            else if (MaskType == MaskType.RoundCorners)
            {
                path.AddRoundedRect(new SKRect(0, 0, width, height), 10, 10);
            }

            e.Surface.Canvas.ClipPath(path);
            base.OnPaintSurface(e);
            if (DrawGradient)
            {
                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    using (var shader = SKShader.CreateLinearGradient(
                        new SKPoint(0,  height),
                        new SKPoint(0, 0),
                        new[] { Color.Black.MultiplyAlpha(0.1).ToSKColor(), Color.Black.MultiplyAlpha(0.5).ToSKColor() },
                        null,
                        SKShaderTileMode.Clamp))
                    {
                        paint.Shader = shader;
                        e.Surface.Canvas.DrawPaint(paint);
                    }
                }
            }

            if (Edge)
            {
                SKPaint edgePaint = new SKPaint();
                edgePaint.IsAntialias = true;
                edgePaint.Style = SKPaintStyle.Stroke;
                edgePaint.Color = EdgeColor.ToSKColor();
                edgePaint.StrokeWidth = (float)EdgeSize;
                e.Surface.Canvas.DrawPath(path, edgePaint);
            }
        }

        public MaskedImage(string sourcePath) : base(sourcePath)
        {
            _lastKnownHeight = 0; 
        }

        /// <summary>
        /// Normalizes all vector points to the width and height of the view.
        /// Vector points were generated with inkscape.
        /// </summary> 
        void FixedCubicTo(SKPath path, double x1, double y1, double x2, double y2, double x3, double y3)
        {
            path.CubicTo(GetCorrectX(x1), GetCorrectY(y1), GetCorrectX(x2), GetCorrectY(y2), GetCorrectX(x3),
                GetCorrectY(y3));
        }

        /// <summary>
        /// Normalizes the x vector points to the width of the view.
        /// </summary> 
        float GetCorrectX(double x)
        {
            var xMulti = 1054.419800;
            return (float) (_lastKnownWidth*(x/xMulti));
        }

        /// <summary>
        /// Normalizes the y vector points to the height of the view.
        /// </summary> 
        float GetCorrectY(double y)
        {
            var yOffset = 52;
            var yMulti = 945 + yOffset;
            return (float) (_lastKnownHeight*((y + yOffset)/yMulti));
        }
    }
}
