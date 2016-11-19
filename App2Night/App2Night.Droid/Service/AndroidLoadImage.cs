using System;
using Android.App;
using Android.Content;
using App2Night.DependencyService;
using App2Night.Droid.Service;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidLoadImage))]
namespace App2Night.Droid.Service
{
	public class AndroidLoadImage : Java.Lang.Object, ILoadImage
	{
		public event EventHandler<ImageSourceEventArgs> ImageSelected;

		public void LoadImage()
		{
			MainActivity androidContext = (MainActivity)Forms.Context;

			Intent imageIntent = new Intent();
			imageIntent.SetType("image/*");
			imageIntent.SetAction(Intent.ActionGetContent);

			androidContext.ConfigureActivityResultCallback(ImageChooserCallback);
			androidContext.StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 0);
		}

		private void ImageChooserCallback(int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok)
			{
				if (ImageSelected != null)
				{
					Android.Net.Uri uri = data.Data;
					if (ImageSelected != null)
					{
						ImageSource imageSource = ImageSource.FromStream(() => Forms.Context.ContentResolver.OpenInputStream(uri));
						ImageSelected.Invoke(this, new ImageSourceEventArgs(imageSource));
					}
				}
			}
		}
	}
}
