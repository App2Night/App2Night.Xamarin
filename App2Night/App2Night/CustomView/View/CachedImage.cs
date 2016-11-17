using System;
using System.Diagnostics;
using System.IO;
using App2Night.Service.Interface;
using MvvmNano;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class CachedImage : Image
    { 
        private readonly IImageFactory _imageFactory; 
        public void SetImage(string imagePath, SourceOrigin origin = SourceOrigin.File)
        {
            ImageSource source = RestoreImage(imagePath);
            if (source == null) 
            {
                switch (origin)
                {
                    case SourceOrigin.Resource:
                        source = ImageSource.FromResource(imagePath);
                        break; 
                    case SourceOrigin.Uri:
                        source = ImageSource.FromUri(new Uri(imagePath));
                        break;
                    case SourceOrigin.File:
                        source = ImageSource.FromFile(imagePath);
                        break; 
                }
                if(source!=null)
                    _imageFactory.CacheImage(source, imagePath);
            }   
            if(Device.OS != TargetPlatform.Windows)
                Source = source;
        }

        ImageSource RestoreImage(string name)
        {
            var source = _imageFactory.RestoreSource(name);
            Debug.WriteLine($"File {name} {(source != null ? "" : "not ")}cached.");
            return source; 
        }

        public void SetImage(string imagePath, Func<Stream> stream = null)
        {
            ImageSource source = RestoreImage(imagePath);
            if (source == null && stream!=null)
            {
                source = ImageSource.FromStream(stream);
                if (source != null)
                    _imageFactory.CacheImage(source, imagePath);
            }
        }

        public CachedImage()
        {
            _imageFactory = MvvmNanoIoC.Resolve<IImageFactory>(); 
        }
    }

    public enum SourceOrigin
    {
        Resource,
        Stream,
        Uri,
        File
    }
}