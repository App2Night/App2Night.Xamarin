using System;
using Android.Graphics;
using App2Night.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabel))]
namespace App2Night.Droid.CustomRenderer
{
    public class CustomLabel : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
            {
                try
                {
                    var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets,
                        e.NewElement.FontFamily + ".ttf");
                    Control.Typeface = font;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Font {e.NewElement.FontFamily}.ttf does not exist."); 
                }
            }
        }
    }
}