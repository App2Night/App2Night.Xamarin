using App2Night.Model.Model;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.PageModel.SubPages
{
    public class ThirdPartyInfoViewModel : FreshBasePageModel 
    { 
        public License License { get; private set; }
        public Command OpenWebsiteCommand => new Command(OpenWebsite);

        /// <summary>
        /// Open the project website.
        /// </summary>
        private void OpenWebsite()
        {
            Device.BeginInvokeOnMainThread(()=> Device.OpenUri(License.ProjectUri)); 
        }


        public ThirdPartyInfoViewModel(License parameter)
        {
            License = parameter;

        } 
    }
}