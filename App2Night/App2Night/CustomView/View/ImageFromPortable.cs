using System;
using System.IO;
using System.Reflection;
using App2Night.CustomView.Template;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace App2Night.CustomView.View
{
    public class ImageFromPortable : SKCanvasView
    {
        private string _imagePath;

        public ImageFromPortable(string imagePath)
        {
            ImagePath = imagePath; 
        }

        SKBitmap bitmap;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                CreateBitmap();
            }
        }

        void CreateBitmap()
        {
            var assembly = typeof(QuadraticPartyTemplate).GetTypeInfo().Assembly; 
            Stream stream = assembly.GetManifestResourceStream(_imagePath);
            using (var s = new SKManagedStream(stream))
            { 
                using (var codec = SKCodec.Create(s))
                {
                    var info = codec.Info;
                    bitmap = new SKBitmap(info.Width, info.Height, info.ColorType /*SKImageInfo.PlatformColorType*/, info.IsOpaque ? SKAlphaType.Opaque : SKAlphaType.Premul);

                    IntPtr length;
                    var result = codec.GetPixels(bitmap.Info, bitmap.GetPixels(out length));
                    if (!(result == SKCodecResult.Success || result == SKCodecResult.IncompleteInput)) 
                    { 
                        //throw new ArgumentException("Unable to load bitmap from provided data");
                    }
                }
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            if(bitmap!=null)
                e.Surface.Canvas.DrawBitmap(bitmap, new SKRect(0, 0, e.Info.Width, e.Info.Height));
        }
    }
}