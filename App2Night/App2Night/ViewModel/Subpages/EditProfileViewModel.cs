using MvvmNano;
using App2Night.Model.Model;
using App2Night.Service.Interface;

namespace App2Night.ViewModel.Subpages
{
	public class EditProfileViewModel : MvvmNanoViewModel
    {
        public MvvmNanoCommand MoveToCancelCommand => new MvvmNanoCommand(MoveToCanecl);

        private void MoveToCanecl()
        {
            NavigateTo<DashboardViewModel>();
        }
        public MvvmNanoCommand MoveTOkCommand => new MvvmNanoCommand(MoveToOk);

        private void MoveToOk()
        {
            NavigateTo<DashboardViewModel>();
        }

		private Model.Model.User _user;

		public Model.Model.User User 
		{ 
			get { return _user; }
			set
			{
				_user = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("IsFormValid");
			}
		}

		IDataService _dataService;

		public EditProfileViewModel(IDataService dataService)
		{
			_dataService = dataService;
			_user = _dataService.User;
		}
	}
}