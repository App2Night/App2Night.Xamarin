using System;
using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Model.Model;
using App2Night.PageModel;
using App2Night.ValueConverter;
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
                IsPullToRefreshEnabled = true
            };

            var swipeView =  
                 new SwipeView();
            swipeView.SetTemplate<SwipeViewTemplate>();

           
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
            swipeView.SetBinding(SwipeView.SwipedOutLeftCommandProperty, "NotePartyCommand");
            swipeView.SetBinding(SwipeView.SwipeOutRightCommandProperty, "AcceptPartyCommand");


            //List view bindings
            listView.SetBinding( ListView.ItemsSourceProperty, "Parties");
            listView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
            listView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing"); 
            listView.ItemSelected += ListViewOnItemSelected;

            this.SetBinding(CustomContentPage.ShowNoContentWarningProperty, nameof(PartyPickerViewModel.NearPartyAvailable),
                converter: new InvertBooleanConverter());

            Content = listView;
        }

        private void ListViewOnItemSelected(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        {
            var listView = (ListView) sender;

            if (listView.SelectedItem != null)
            {
                PreviewItemSelected<Party, PartyPreviewView>((Party)listView.SelectedItem, new object[] {Height, Width});
                listView.SelectedItem = null; 
            }
        }
    }
}
