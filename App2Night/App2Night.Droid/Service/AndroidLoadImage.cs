using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidLoadImage))]
namespace App2Night.Droid
{
	public class AndroidLoadImage : ILoadImage
	{
		public Image ILoadImage()
		{
			throw new NotImplementedException();
		}
	}
}
