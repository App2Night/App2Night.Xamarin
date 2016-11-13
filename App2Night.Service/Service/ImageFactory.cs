using System.Collections.Generic;
using System.Linq;
using App2Night.Service.Interface;
using Xamarin.Forms;

namespace App2Night.Service.Service
{
    public class ImageFactory : IImageFactory
    {
        List<ImageStore> _images = new List<ImageStore>();

        public void CacheImage(ImageSource source, string imageName)
        {
            _images.Add(new ImageStore(imageName, source));
        }

        public ImageSource RestoreSource(string name)
        {
            var result = _images.FirstOrDefault(o => o.Name == name)?.Source;
            return result;
        }
    }

    public class ImageStore
    {
        public string Name { get; }
        public ImageSource Source { get; }

        public ImageStore(string name, ImageSource source)
        {
            Name = name;
            Source = source;
        }
    }
}