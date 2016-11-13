using Xamarin.Forms;

namespace App2Night.Service.Interface
{
    public interface IImageFactory
    {
        void CacheImage(ImageSource source, string imageName);

        ImageSource RestoreSource(string name);
    }
}