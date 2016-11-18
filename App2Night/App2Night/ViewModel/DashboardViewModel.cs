using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using App2Night.ViewModel.Subpages;
using MvvmNano; 

namespace App2Night.ViewModel
{ 
    public class DashboardViewModel : MvvmNanoViewModel
    {
        public bool InterestingPartieAvailable { get; set; }
        public bool PartyHistoryAvailable { get; set; }
        public bool SelectedpartiesAvailable { get; set; } 


        public ObservableCollection<Party> InterestingPartiesForUser  => MvvmNanoIoC.Resolve<IDataService>().InterestingPartys;
        public ObservableCollection<Party> PartyHistory => MvvmNanoIoC.Resolve<IDataService>().PartyHistory;
        public ObservableCollection<Party> Selectedparties => MvvmNanoIoC.Resolve<IDataService>().SelectedPartys;

		public MvvmNanoCommand MoveToUserEditCommand => new MvvmNanoCommand(() => NavigateTo<EditProfileViewModel>());
        public MvvmNanoCommand MoveToMyPartiesCommand => new MvvmNanoCommand(() => NavigateTo<MyPartysViewModel>());
        public MvvmNanoCommand MoveToHistoryCommand => new MvvmNanoCommand(() => NavigateTo<HistoryViewModel>());
        public MvvmNanoCommand MoveToPartyPicker => new MvvmNanoCommand(() => NavigateTo<PartyPickerViewModel>());

		IDataService _service;


		public DashboardViewModel(IDataService service)
        {
            
			_service = service;
			_service.PartiesUpdated += OnPartiesUpdated;
			_User = _service.User;

			SetAvailabilitys();
        } 

        private void OnPartiesUpdated(object sender, EventArgs eventArgs)
        {
            SetAvailabilitys();
        }

		private Model.Model.User _User;

		public Model.Model.User User
		{
			get { return _User; }
			set
			{
				_User = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("IsFormValid");
			}
		}

        public override void Initialize()
        {
            base.Initialize();
            SetAvailabilitys();
        }

        void SetAvailabilitys()
        {
            InterestingPartieAvailable = InterestingPartiesForUser.Any();
            PartyHistoryAvailable = PartyHistory.Any();
            SelectedpartiesAvailable = Selectedparties.Any(); 
        }
    }
}