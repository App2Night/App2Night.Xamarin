using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Service.Interface;
using App2Night.ViewModel.Subpages;
using MvvmNano;
using PartyUp.Model.Model;
using PartyUp.Service.Interface;

namespace App2Night.ViewModel
{
    public class DashboardViewModel : MvvmNanoViewModel
    {
        public override void Initialize()
        {
            base.Initialize();
            Task.Run(async () => await StartupSync());
        }

        private async Task StartupSync()
        {
            UserDialogs.Instance.ShowLoading("Starting session.");
            var result = await MvvmNanoIoC.Resolve<IDataService>().RequestToken("test", "test");
            UserDialogs.Instance.Toast(
            new ToastConfig("Token request finished " + (result.Success ? "" : "un") + "successfull.")
            {
                BackgroundColor = result.Success ? Color.LawnGreen : Color.LightCoral
            });
            UserDialogs.Instance.Loading("Loading partys.");
            await MvvmNanoIoC.Resolve<IDataService>().RefreshPartys();
            new ToastConfig("Loading parties finished " + (result.Success ? "" : "un") + "successfull.")
            {
                BackgroundColor = result.Success ? Color.LawnGreen : Color.LightCoral
            });
        }

        public ObservableCollection<Party> InterestingPartiesForUser => MvvmNanoIoC.Resolve<IDataService>().Partys;
        public ObservableCollection<Party> PartyHistory => MvvmNanoIoC.Resolve<IDataService>().Partys; 
        public ObservableCollection<Party> NextPartiesForUser => MvvmNanoIoC.Resolve<IDataService>().Partys;


        public MvvmNanoCommand MoveToUserEditCommand => new MvvmNanoCommand(()=> NavigateTo<EditProfileViewModel>());
        public MvvmNanoCommand MoveToMyPartiesCommand => new MvvmNanoCommand(()=> NavigateTo<MyPartysViewModel>()); 
        public MvvmNanoCommand MoveToHistoryCommand => new MvvmNanoCommand(() => NavigateTo<HistoryViewModel>());
        public MvvmNanoCommand MoveToPartyPicker => new MvvmNanoCommand(() => NavigateTo<EventPickerViewModel>());
    }
}