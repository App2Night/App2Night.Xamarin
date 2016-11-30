using System.Collections.ObjectModel;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;

namespace App2Night.PageModel
{
    public class HistoryViewModel : FreshBasePageModel
    {
        public ObservableCollection<Party> Parties => FreshIOC.Container.Resolve<IDataService>().PartyHistory; 
    }
}