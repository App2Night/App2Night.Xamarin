using App2Night.Service.Interface;
using FreshMvvm;

namespace App2Night.PageModel
{
    public class SettingViewModel : FreshBasePageModel
    {
        private readonly IStorageService _storageService;
        private int _selectedRange;


        public SettingViewModel(IStorageService storageService)
        {
            _storageService = storageService; 
        }

        public int SelectedRange
        {
            get { return _storageService.Storage.FilterRadius; }
            set
            {
                _storageService.Storage.FilterRadius = value;
                //TODO save range after x amount of time.
            }
        }

        public bool GpsEnabled
        {
            get { return _storageService.Storage.UseGps; }
            set
            {
                _storageService.Storage.UseGps = value;
                _storageService.SaveStorage();
                //TODO show a manuel position entry view if usegps = false 
            }
        }
    }
}