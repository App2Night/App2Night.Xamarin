using Xamarin.Forms;

namespace App2Night.Helper
{
    public static class ConvertHelper
    {
        public static Color ToXamarinColor(this System.Drawing.Color color)
        {
            return Color.FromRgba(ByteToDouble(color.R), ByteToDouble(color.G), ByteToDouble(color.B), ByteToDouble(color.A));
        }

        static double ByteToDouble(byte bte)
        {
            return bte /256.0;
        }
    }
}