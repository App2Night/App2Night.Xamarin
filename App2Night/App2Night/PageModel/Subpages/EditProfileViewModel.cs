using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Model.Enum;
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
            await _dataService.UpdateUser();
            await CoreMethods.PushPageModel<DashboardPageModel>();

        }

        [AlsoNotifyFor(nameof(ValidName))]
        public string Name { get; set; }
        public bool ValidName => Name.Length > 3;

        [AlsoNotifyFor(nameof(ValidName))]
        public string Email { get; set; }
        public bool ValidEmail => Service.Helper.ValidateHelper.EmailIsValid(Email);


        public Model.Model.User User { get; set; }

        readonly IDataService _dataService;
        readonly IClientService _clientService;

        public EditProfileViewModel(IDataService dataService, IClientService clientService)
        {
            _dataService = dataService;
            _clientService = clientService;
            User = _dataService.User;
            SetAttributes();
        }

        private void SetAttributes()
        {
            Name = User.Name;
            Email = User.Email;
        }
    }
}