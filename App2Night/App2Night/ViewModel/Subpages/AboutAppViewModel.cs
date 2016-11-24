using System;
using System.Diagnostics;
using MvvmNano;
using Xamarin.Forms;

namespace App2Night.ViewModel.Subpages
{
    public class AboutAppViewModel : MvvmNanoViewModel
    {
        public MvvmNanoCommand OpenProjectSiteCommand => new MvvmNanoCommand(OpenProjectOnGithub); 

        private void OpenProjectOnGithub()
        {
            Device.BeginInvokeOnMainThread(() => Device.OpenUri(new Uri("https://github.com/App2Night/App2Night.Xamarin")));
        }
    }
}