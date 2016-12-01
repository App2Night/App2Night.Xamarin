using System;
using System.Collections.ObjectModel;
using System.Linq;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;

namespace App2Night.PageModel
{
    public class HistoryViewModel : FreshBasePageModel
    {
        private readonly IDataService _dataService;

        public bool PartyAvailable { get; private set; }

        public ObservableCollection<Party> Parties => FreshIOC.Container.Resolve<IDataService>().PartyHistory;

        public HistoryViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.HistoryPartisUpdated += HistoryUpdated;
        }

        private void HistoryUpdated(object sender, EventArgs eventArgs)
        {
            PartyAvailable = _dataService.PartyHistory.Any();
        } 
    }
}