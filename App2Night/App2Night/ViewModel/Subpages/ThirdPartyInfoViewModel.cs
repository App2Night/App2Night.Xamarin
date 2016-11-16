using System;
using App2Night.Model.Model;
using MvvmNano;
using Xamarin.Forms;

namespace App2Night.ViewModel.Subpages
{
    public class ThirdPartyInfoViewModel : MvvmNanoViewModel<License>
    { 
        public License License { get; private set; }
        public MvvmNanoCommand OpenWebsiteCommand => new MvvmNanoCommand(OpenWebsite);

        /// <summary>
        /// Open the project website.
        /// </summary>
        private void OpenWebsite()
        {
            Device.BeginInvokeOnMainThread(()=> Device.OpenUri(License.ProjectUri)); 
        }

        public override void Initialize(License parameter)
        {
            base.Initialize(parameter);
            License = parameter;
        }
    }
}