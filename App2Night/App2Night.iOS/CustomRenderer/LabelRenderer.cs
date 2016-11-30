using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using LabelRenderer = App2Night.iOS.CustomRenderer.LabelRenderer;

[assembly: ExportRenderer(typeof(Label), typeof(LabelRenderer))] 
namespace App2Night.iOS.CustomRenderer
{
    public class LabelRenderer : Xamarin.Forms.Platform.iOS.LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
            {
                try
                {
                    var test = Element.FontSize;
                    var test2 = e.NewElement.FontSize;
                    Control.Font = UIFont.FromName(  e.NewElement.FontFamily  , (nfloat) Element.FontSize);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    Console.WriteLine($"Font {e.NewElement.FontFamily}.ttf does not exist.");
                }
            }
        }
    }
}