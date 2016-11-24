using Xamarin.Forms;

namespace App2Night.Service.Helper
{

    /// <summary>
    /// Helps converting from .Net to Xamarin types and back.
    /// </summary>
    public static class ConvertHelper
    {

        /*
         * R,G,B,A in System.Drawing.Color have a range form 0-255, in Xamarin.Forms from 0-1. 
         */

        /// <summary>
        /// Converts a System.Drawing color to a Xamarin.Forms color.
        /// </summary>
        /// <param name="color">System.Drawing.Color</param>
        /// <returns>Xamarin.Forms.Color</returns>
        public static Color ToXamarinColor(this System.Drawing.Color color)
        {
            //R,G,B,A in System.Drawing.Color have a range form 0-255, in Xamarin.Forms from 0-1
            //Convert all R,G,B,A values.
            return Color.FromRgba(ToXamarinFormsArgb(color.R), ToXamarinFormsArgb(color.G), ToXamarinFormsArgb(color.B), ToXamarinFormsArgb(color.A));
        }

        /// <summary>
        /// Converts a Xamarin.Forms color to a System.Drawing color.
        /// </summary>
        /// <param name="color">Xamarin.Forms.Color</param>
        /// <returns>System.Drawing.Color</returns>
        public static System.Drawing.Color ToSystemDrawingColor(this Color color)
        {
            //Convert all R,G,B,A values.
            return System.Drawing.Color.FromArgb(ToSystemDrawingByte(color.A), ToSystemDrawingByte(color.R), ToSystemDrawingByte(color.G), ToSystemDrawingByte(color.B));
        }
          
        /// <summary>
        /// Converts an ARGB value between 0 and 255 to a  double value of 0 to 1 to fit the Xamarin.Forms.Color ARGB scheme.
        /// </summary> 
        static double ToXamarinFormsArgb(byte @byte)
        {
            return @byte /255.0;
        }

        /// <summary>
        /// Converts an ARGB value between 0 and 1 to a byte value of 0 to 255 to fit the System.Drawing.Color scheme.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        static int ToSystemDrawingByte(double d)
        {
            return (int) (d*255);
        }
    }
}