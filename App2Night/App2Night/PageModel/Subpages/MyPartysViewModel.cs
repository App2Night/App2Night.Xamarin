using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace App2Night.PageModel.SubPages
{
    public class MyPartysViewModel : FreshBasePageModel
    {
        private IDataService _dataService;
        private Party selectedParty;
        private bool _isRefreshing;
        
        public ObservableCollection<Party> MyParties { get; set; }

        public Command<Party> RefreshCommand => new Command<Party>(Refresh);

        public bool NoContentAvailable => !_dataService.SelectedPartys.Any();
        
        public Party SelectedParty
        {
            get
            {
                return selectedParty;
            }

            set
            {
                selectedParty = value;
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return IsRefreshing;
            }

            set
            {
                IsRefreshing = value;
            }
        }

        public MyPartysViewModel(IDataService dataSevice)
        {
            _dataService = dataSevice; 
            MyParties = _dataService.SelectedPartys;
        }

        public void Refresh(object o)
        {
           
        }

    }
}