using System.Collections.Generic;
using App2Night.CustomView;
using App2Night.ViewModel;
using MvvmNano.Forms;
using PartyUp.Model.Model;

namespace App2Night.View
{
    public class EventPickerPage : MvvmNanoContentPage<EventPickerViewModel>
    {
        public EventPickerPage()
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

            var swipeView =  
                 new SwipeView();

           
            var dummiParties = new List<Party>();
            for (int i = 0; i < 12; i++)
            {
                dummiParties.Add(new Party() {Name = "PartyNr: " + (i+1)});
            }
            var gallerieView = new VerticalGallerieView
            {
                Columns = 2,
                Template = typeof(QuadraticPartyTemplate),
                ItemSource = dummiParties,
                
            };
            BindToViewModel(swipeView, SwipeView.ItemSourceProperty, vm => vm.Events);
            Content = gallerieView;
        }
    }
}
