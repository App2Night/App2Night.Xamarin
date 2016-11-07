
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using App2Night.UWP.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabel))]

namespace App2Night.UWP.Renderer
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
                    Control.FontFamily = new FontFamily($"/Assets/Fonts/{e.NewElement.FontFamily}.ttf#{e.NewElement.FontFamily}");
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Font {e.NewElement.FontFamily}.ttf does not exist.");
                }
            }
        }
    }
}