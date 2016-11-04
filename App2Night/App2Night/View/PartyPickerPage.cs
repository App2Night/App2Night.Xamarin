using System.Collections.Generic;
using App2Night.CustomView;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using MvvmNano.Forms;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.View
{
    public class PartyPickerPage : MvvmNanoContentPage<PartyPickerViewModel>
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

            BindToViewModel(swipeView, SwipeView.ItemsSourceProperty, vm => vm.Events);
            BindToViewModel(listView, ListView.ItemsSourceProperty, vm => vm.Events);

            Content = listView;
        }
    }
}
