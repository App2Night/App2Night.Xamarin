using App2Night.Page.SubPages;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public sealed class AboutTabbedPage : TabbedPage
    {
        public AboutTabbedPage()  
        {
            var aboutAppPage =FreshPageModelResolver.ResolvePageModel<AboutAppViewModel>();
            var thirdPartyPage = FreshPageModelResolver.ResolvePageModel<ThirdPartyViewModel>();

            Children.Add(aboutAppPage);
            Children.Add(thirdPartyPage);
             
            SelectedItem = Children[0];
        }
    }
}