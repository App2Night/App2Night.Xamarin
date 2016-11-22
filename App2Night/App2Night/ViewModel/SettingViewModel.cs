using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using App2Night.Data.Language;
using App2Night.DependencyService;
using App2Night.Service.Interface;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class SettingViewModel : MvvmNanoViewModel
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