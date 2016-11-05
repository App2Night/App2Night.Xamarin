using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using App2Night.CustomView.Template;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace App2Night.CustomView.View
{
    public enum FileType
    {
        Svg,
        Image
    }

    public class ImageFromPortable : SKCanvasView
    {
        private FileType FileType
        {
            get
            {
                if(_imagePath.EndsWith(".svg")) return FileType.Svg;
                if(_imagePath.EndsWith(".png")|| _imagePath.EndsWith(".jpg")) return FileType.Image;
                return FileType.Image;
            }
        }
        private string _imagePath;

        public ImageFromPortable(string imagePath)
        { 
            ImagePath = imagePath;
        }

        SKBitmap bitmap;
        private SKImage image;

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
            if (string.IsNullOrEmpty(ImagePath)) return;
            Task.Run(() => 
            {
                Stopwatch streamBitmapWatch = new Stopwatch();
                streamBitmapWatch.Start();
                var assembly = typeof (QuadraticPartyTemplate).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(_imagePath);


                using (var s = new SKManagedStream(stream))
                {
                    if (FileType == FileType.Image)
                    {
                        using (var codec = SKCodec.Create(s))
                        {
                            var info = codec.Info;

                            bitmap = new SKBitmap(info.Width, info.Height, info.ColorType
                                /*SKImageInfo.PlatformColorType*/,
                                info.IsOpaque ? SKAlphaType.Opaque : SKAlphaType.Premul);

                            IntPtr length;
                            var result = codec.GetPixels(bitmap.Info, bitmap.GetPixels(out length));
                            if (!(result == SKCodecResult.Success || result == SKCodecResult.IncompleteInput))
                            {
                                //throw new ArgumentException("Unable to load bitmap from provided data");
                            }
                        }
                    }
                    else
                    {
                        //TODO Add svg support
                    }
                }
                Debug.WriteLine("INFO: Encoding Bitmap took: " + streamBitmapWatch.ElapsedMilliseconds);
                InvalidateSurface();
            }); 
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            Stopwatch showingBitmap = new Stopwatch();
            showingBitmap.Start();
            base.OnPaintSurface(e);
            if (FileType == FileType.Image)
            {
                if (bitmap != null)
                    e.Surface.Canvas.DrawBitmap(bitmap, new SKRect(0, 0, e.Info.Width, e.Info.Height));
                Debug.WriteLine("INFO: Showing Bitmap took: " + showingBitmap.ElapsedMilliseconds);

            } 
            else if(image!=null)
            {
                //TODO Add svg support
            }
        }
    }
}