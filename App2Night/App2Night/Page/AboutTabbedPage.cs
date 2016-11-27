using App2Night.Page.SubPages;
using App2Night.PageModel.SubPages;
using FreshMvvm;

namespace App2Night.Page
{
    public sealed class AboutTabbedPage : FreshTabbedNavigationContainer
    {
        public AboutTabbedPage() : base("AboutApp")
        { 
            AddTab<AboutAppViewModel>("About this App", null);
            AddTab<ThirdPartyViewModel>("ThirdParty", null);  
        }
    }
}