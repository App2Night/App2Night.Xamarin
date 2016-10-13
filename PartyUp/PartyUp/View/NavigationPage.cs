using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class NavigationPage : MvvmNanoMasterDetailPage<NavigationViewModel>
    {
        public NavigationPage()
        {
            var label = new Label() { Text = "TEST" };
            BindToViewModel(label, Label.TextProperty, o => o.TestValue);
            MasterContent = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)} 
                },
                Children =
                {
                    label,
                    DetailListView
                }
            };
        } 
    }
}