using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
	public class EditProfileViewModel : FreshBasePageModel
    {
        public Command MoveToCancelCommand => new Command(async ()=> await MoveToCanecl());

        private async Task MoveToCanecl()
        {
            await CoreMethods.PushPageModel<DashboardPageModel>();
        }
        public Command MoveTOkCommand => new Command(async () => await MoveToOk());

        private async Task MoveToOk()
        {
            await CoreMethods.PushPageModel<DashboardPageModel>();
        }
         
		public Model.Model.User User { get; set; }

        readonly IDataService _dataService;
        readonly IClientService _clientService;

        public EditProfileViewModel(IDataService dataService, IClientService clientService)
		{
			_dataService = dataService;
            _clientService = clientService;
            User = _dataService.User;
		}
    }
}