using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using App2Night.CustomView.Template;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

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
                if (_imagePath.EndsWith(".svg")) return FileType.Svg;
                if (_imagePath.EndsWith(".png") || _imagePath.EndsWith(".jpg")) return FileType.Image;
                return FileType.Image;
            }
        }

        private string _imagePath;

        public ImageFromPortable(string imagePath)
        {
            svg.ThrowOnUnsupportedElement = true;
            ImagePath = imagePath;
        }

        SKBitmap bitmap;
        private SKImage image;
        SKSvg svg = new SKSvg(200);

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                CreateBitmap();
            }
        }

        public ImageFromPortable()
        {
            this.PaintSurface += (sender, args) => { };

            this.MeasureInvalidated += (sender, args) => { };
        }

        void CreateBitmap()
        {
            if (string.IsNullOrEmpty(ImagePath)) return;

            Stopwatch streamBitmapWatch = new Stopwatch();
            streamBitmapWatch.Start(); 
            try
            {
                //var assembly = typeof (QuadraticPartyTemplate).GetTypeInfo().Assembly;
                //Stream stream = assembly.GetManifestResourceStream(_imagePath); 

                //if (FileType == FileType.Image)
                //{
                //    using (var s = new SKManagedStream(stream))
                //    {
                //        using (var codec = SKCodec.Create(s))
                //        {
                //            var info = codec.Info;

                //            bitmap = new SKBitmap(info.Width, info.Height, info.ColorType
                //                /*SKImageInfo.PlatformColorType*/,
                //                info.IsOpaque ? SKAlphaType.Opaque : SKAlphaType.Premul);

                //            IntPtr length;
                //            var result = codec.GetPixels(bitmap.Info, bitmap.GetPixels(out length));
                //            if (!(result == SKCodecResult.Success || result == SKCodecResult.IncompleteInput))
                //            {
                //                throw new ArgumentException("Unable to load bitmap from provided data");
                //            }
                //        }
                //    }
                //}
                //else 
                //{ 
                //    //TODO Add svg support
                //} 
                InvalidateSurface();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                Debug.WriteLine("INFO: Encoding Bitmap took: " + streamBitmapWatch.ElapsedMilliseconds);
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e); 
            Stopwatch showingBitmap = new Stopwatch();
            showingBitmap.Start();
            base.OnPaintSurface(e);

            var width = e.Info.Width;
            var height = e.Info.Height;
            

            if (FileType == FileType.Image)
            {
                if (bitmap != null)
                {
                    float multiplyer = 0;
                    float xOffset = 0;
                    float yOffset = 0;
                    if (width > height)
                    {
                        multiplyer = (float)width / bitmap.Width;
                    }
                    else
                    {
                        multiplyer = (float)height / bitmap.Height;
                    }

                    var cHeight = bitmap.Height * multiplyer;
                    var cWidth = bitmap.Width * multiplyer;

                    xOffset = (width - bitmap.Width) / 2f;
                    yOffset = (height - bitmap.Height) / 2f;
                    if (xOffset < 0) xOffset = 0;
                    if (yOffset < 0) yOffset = 0;
                    e.Surface.Canvas.DrawBitmap(bitmap, new SKRect(0, 0, cWidth, cHeight),
                        new SKPaint() {IsAntialias = true});
                }
            }
            else 
            {
                try
                {
                    //var assembly = typeof(QuadraticPartyTemplate).GetTypeInfo().Assembly;
                    //Stream stream = assembly.GetManifestResourceStream(_imagePath);
                    //var image = new SKSvg();
                    //image.ThrowOnUnsupportedElement = true; 
                    //image.Load(stream);
                    //float canvasMin = Math.Min(e.Info.Width, e.Info.Width); 
                    //float svgMax = Math.Max(image.Picture.Bounds.Width, image.Picture.Bounds.Height);
                    //float scale = canvasMin / svgMax; 
                    //var matrix = SKMatrix.MakeScale(scale, scale); 
                    ////matrix = SKMatrix.MakeSkew(width, height); 
                    //e.Surface.Canvas.DrawPicture(image.Picture, ref matrix);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
            }
            Debug.WriteLine($"INFO: Showing {FileType} took: { showingBitmap.ElapsedMilliseconds} ms.");

        }
    }
}