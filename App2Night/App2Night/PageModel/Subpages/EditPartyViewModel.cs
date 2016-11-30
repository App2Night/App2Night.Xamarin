using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
    public class EditPartyViewModel : FreshBasePageModel
    {
        public Model.Model.User User => FreshIOC.Container.Resolve<IDataService>().User;
    }
}