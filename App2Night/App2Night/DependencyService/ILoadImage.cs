using System;
using Xamarin.Forms;

namespace App2Night
{
	public interface ILoadImage
	{
		/// <summary>
		/// Returns an image from local device.
		/// </summary>
		void LoadImage();
		/// <summary>
		/// Occurs when image selected.
		/// </summary>
		event EventHandler<ImageSourceEventArgs> ImageSelected;
	}
	/// <summary>
	/// Image source event arguments.
	/// </summary>
	public class ImageSourceEventArgs : EventArgs
	{
		public ImageSourceEventArgs(ImageSource imageSource)
		{
			if (imageSource == null) throw new ArgumentNullException("imageSource");

			ImageSource = imageSource;
		}

		public ImageSource ImageSource { get; private set; }
	}
}
