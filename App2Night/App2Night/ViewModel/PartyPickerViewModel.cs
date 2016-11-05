using System.Collections.ObjectModel;
using System.Diagnostics;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class PartyPickerViewModel : MvvmNanoViewModel
    {
        private Party _selectedParty = null;
        public ObservableCollection<Party> Events => MvvmNanoIoC.Resolve<IDataService>().Partys;


        public PartyPickerViewModel()
        {
            Events.CollectionChanged += Events_CollectionChanged;
        }

        private void Events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("CHANGE!!!");
        }

        public Party SelectedParty
        {
            get { return _selectedParty; }
            set
            {
                if (value != null)
                {
                    _selectedParty = value;
                    NavigateTo<PartyViewModel, Party>(value);
                    _selectedParty = null;
                    NotifyPropertyChanged(nameof(SelectedParty));
                }
            }
        }

    }

    public class User
    {
        public string Name { get; set; }
    }
}
