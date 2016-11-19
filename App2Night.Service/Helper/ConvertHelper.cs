using Xamarin.Forms;

namespace App2Night.Service.Helper
{
    public static class ConvertHelper
    {
        public static Color ToXamarinColor(this System.Drawing.Color color)
        {
            return Color.FromRgba(ByteToDouble(color.R), ByteToDouble(color.G), ByteToDouble(color.B), ByteToDouble(color.A));
        }

        public static System.Drawing.Color ToSystemDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(DoubleToInt(color.A), DoubleToInt(color.R), DoubleToInt(color.G), DoubleToInt(color.B));
        }

        static double ByteToDouble(byte bte)
        {
            return bte /256.0;
        }

        static int DoubleToInt(double d)
        {
            return (int) (d*255);
        }
    }
}