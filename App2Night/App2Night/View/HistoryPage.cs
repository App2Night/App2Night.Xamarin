using App2Night.CustomView;
using App2Night.ViewModel;
using MvvmNano.Forms;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.View
{
    public class HistoryPage : MvvmNanoContentPage<HistoryViewModel>
    {
        public HistoryPage()
        {
            var stackLayout = new ListView()
            {
                ItemTemplate = new DataTemplate(() => {
                    var nativeCell = new PartyTemplate();
                    nativeCell.SetBinding(PartyTemplate.NameProperty, "Name");
                    nativeCell.SetBinding(PartyTemplate.DateProperty, "Date");
                    nativeCell.SetBinding(PartyTemplate.ImageSourceProperty, "ImageSource");

                    return nativeCell;
                }),
                ItemsSource = new Party[]
                {
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                }
            };
            Title = "History";
            Content = new ScrollView
            {
                Content = stackLayout,
                Orientation = ScrollOrientation.Vertical
            };
        }
    }
}
