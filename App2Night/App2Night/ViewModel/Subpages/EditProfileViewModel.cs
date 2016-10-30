using MvvmNano;

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
    }
}