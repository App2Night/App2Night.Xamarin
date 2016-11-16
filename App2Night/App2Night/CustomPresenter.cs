using App2Night.View;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night
{
    public class CustomPresenter : MvvmNanoFormsPresenter
    {
        public CustomPresenter(MvvmNanoApplication application) : base(application)
        {
        }

        protected override void OpenPage(Page page)
        { 
            if (page is LoginPage)
            {
                Device.BeginInvokeOnMainThread(async () =>
                    await CurrentPage.Navigation.PushModalAsync(page)); 
                return;
            } 
            base.OpenPage(page);
        }
    }
}