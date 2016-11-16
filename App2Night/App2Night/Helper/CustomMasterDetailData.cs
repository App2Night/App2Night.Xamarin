using System;
using MvvmNano.Forms;

namespace App2Night.Helper
{
    public class CustomMasterDetailData : MasterDetailData
    {
        public string IconCode { get; private set; }

        public CustomMasterDetailData(Type viewModelTypeType, string pageTitle, string iconCode) : base(viewModelTypeType, pageTitle)
        {
            IconCode = iconCode;
        }
    }
}