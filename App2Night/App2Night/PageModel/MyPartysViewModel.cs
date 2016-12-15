using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel
{ 
    [ImplementPropertyChanged]
    public class MyPartysViewModel : FreshBasePageModel
    {
        private IDataService _dataService;
        private readonly IAlertService _alertService; 
        public bool IsRefreshing { get; set; } 

        public ObservableCollection<Party> MyParties { get; set; }

        public Command NavigateToHistoryCommand => new Command(async ()=> await OpenHistory());

        private async Task OpenHistory()
        {
            await CoreMethods.PushPageModel<HistoryViewModel>();
        }

        public Command RefreshCommand => new Command(async ()=> await Refresh());

        public bool NoContentAvailable => !_dataService.SelectedPartys.Any(); 

        public MyPartysViewModel(IDataService dataSevice, IAlertService alertService)
        {
            _dataService = dataSevice;
            _dataService.SelectedPartiesUpdated += DataServiceOnSelectedPartiesUpdated;
            _alertService = alertService;
            MyParties = _dataService.SelectedPartys;
        }

        private void DataServiceOnSelectedPartiesUpdated(object sender, EventArgs eventArgs)
        {
            RaisePropertyChanged(nameof(NoContentAvailable));
        }

        public async Task Refresh()
        {
             
            var result = await _dataService.RefreshSelectedParties();
            _alertService.PartyPullFinished(result);
            IsRefreshing = false; 
        }  
    }
}