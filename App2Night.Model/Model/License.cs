using System;

namespace App2Night.Model.Model
{
    public class License
    {
        public string ProjectName { get; set; }
        public string LicenseName { get; set; }
        public Uri ProjectUri { get; set; }
        public string LicenseFileName { get; set; }
        
        public string LicenseText { get; set; }
        public string Description { get; set; }
    }
}