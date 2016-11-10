using System;
using Xamarin.Forms;

namespace App2Night
{
	public interface ILoadImage
	{
			/// <summary>
			/// Returns an image from local device.
			/// </summary>
			Image ILoadImage();
	}
}
