using MvvmNano;

namespace App2Night.ViewModel.Subpages
{
	public class EditProfileViewModel : MvvmNanoViewModel<User>
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

		//private User _User;

		//public User User 
		//{ 
		//	get { return _User; } 
		//	set { _User = value; } 
		//}

		//public override void Initialize(User pUser)
		//{
		//	base.Initialize(pUser);
		//	User = pUser;
		//}
    }
}