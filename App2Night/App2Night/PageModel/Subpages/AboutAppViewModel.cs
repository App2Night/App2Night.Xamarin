using System;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.PageModel.SubPages
{
    public class AboutAppViewModel : FreshBasePageModel
    {
        public Command OpenProjectSiteCommand => new Command(OpenProjectOnGithub); 

        private void OpenProjectOnGithub()
        {
            Device.BeginInvokeOnMainThread(() => Device.OpenUri(new Uri("https://github.com/App2Night/App2Night.Xamarin")));
        }
    }
}