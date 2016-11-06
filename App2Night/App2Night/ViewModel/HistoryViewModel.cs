using System.Collections.ObjectModel;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class HistoryViewModel : MvvmNanoViewModel
    {
        public ObservableCollection<Party> Parties => MvvmNanoIoC.Resolve<IDataService>().PartyHistory; 
    }
}