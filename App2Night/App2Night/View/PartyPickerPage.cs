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
            //var eventListView = new ListPartyView(ListViewCachingStrategy.RecycleElement);
            //BindToViewModel(eventListView, ListPartyView.ItemsSourceProperty, vm => vm.Events);
            //BindToViewModel(eventListView, ListPartyView.SelectedItemProperty, vm => vm.SelectedParty);

            //eventListView.ItemTemplate  = new DataTemplate(() =>
            //{
            //    var nameCell = new TextCell();
            //    nameCell.SetBinding(TextCell.TextProperty, "Name"); 
            //    return nameCell;
            //}); 



            //Content = eventListView;
            var dummiParties = new List<Party>();
            for (int i = 0; i < 12; i++)
            {
                dummiParties.Add(new Party() {Name = "PartyNr: " + (i+1)});
            }
           
            var listView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(PartyTemplate)),
                ItemsSource = dummiParties,
                RowHeight = 100,
            };

            var swipeView =  
                 new SwipeView();

           
            
            BindToViewModel(swipeView, SwipeView.ItemSourceProperty, vm => vm.Events);
            Content = listView;
        }
    }
}
