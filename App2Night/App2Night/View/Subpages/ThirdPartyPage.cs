using App2Night.CustomView.Page;
using App2Night.ViewModel.Subpages;
using MvvmNano;
using Xamarin.Forms;

namespace App2Night.View.Subpages
{
    public class ThirdPartyPage : ContentPageWithInfo<ThirdPartyViewModel>
    {
        public ThirdPartyPage()
        {
            //Set up page
            SetViewModel(MvvmNanoIoC.Resolve<ThirdPartyViewModel>());
            Title = "Third Party";

            var label = new Label();
            BindToViewModel(label, Label.TextProperty, o => o.TestString);
            Content = label;
        }
    }
}