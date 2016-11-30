using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class PartyPickerPage : CustomContentPage 
    {
        public PartyPickerPage()
        {  
           
            var listView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(PartyTemplate)), 
                RowHeight = 100,
            };

            var swipeView =  
                 new SwipeView();

           
            var changeViewToolbarItem = new ToolbarItem()
            {
                Text = "ToOtherView",
                Command = new Command(() =>
                {
                    if (Content == swipeView)
                        Content = listView;
                    else
                        Content = swipeView;
                })
            };
            ToolbarItems.Add(changeViewToolbarItem);

            swipeView.SetBinding( SwipeView.ItemsSourceProperty, "Parties");
            listView.SetBinding( ListView.ItemsSourceProperty, "Parties");

            Content = listView;
        }
    }
}
