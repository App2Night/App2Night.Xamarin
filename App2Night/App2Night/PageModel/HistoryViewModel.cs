using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    [ImplementPropertyChanged]
    public class HistoryViewModel : FreshBasePageModel
    {
        private readonly IDataService _dataService;

        public bool PartyAvailable { get; private set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<Party> Parties => FreshIOC.Container.Resolve<IDataService>().PartyHistory;

        public Command RefreshCommand => new Command(async  ()=> await Refresh());

        private async Task Refresh()
        {
            await _dataService.RefreshPartyHistory();
            IsRefreshing = false;
        }

        public HistoryViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.HistoryPartisUpdated += HistoryUpdated;
            CheckIfPartyIsAvailable();
        }

        private void HistoryUpdated(object sender, EventArgs eventArgs)
        {
            CheckIfPartyIsAvailable();
        }

        private void CheckIfPartyIsAvailable()
        {
            PartyAvailable = _dataService.PartyHistory.Any();
        }
    }
}