using System;
using App2Night.Model.Model;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms.Maps;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
    public class MyPartyDetailViewModel : FreshBasePageModel
    {
        public Party Party { get; set; }

        private Pin _mapPin;
        public Pin MapPins
        {
            get
            {
                return new Pin
                {
                    Position = new Position(Party.Location.Latitude, Party.Location.Longitude),
                    Label = Party.Name
                };
            }
            private set
            {
                _mapPin = new Pin
                {
                    Position = new Position(Party.Location.Latitude, Party.Location.Longitude),
                    Label = Party.Name
                };
            }
        }

        [AlsoNotifyFor(nameof(ValidName))]
        public string Name { get; set; }
        public bool ValidName => Name.Length > 3;

        public string Description { get; set; }

        [AlsoNotifyFor(nameof(ValidDate))]
        public DateTime Date { get; set; }
        public bool ValidDate => Date > DateTime.Now;

        [AlsoNotifyFor(nameof(ValidTime))]
        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public bool ValidTime => Time > DateTime.Now;

        public override void Init(object initData)
        {
            base.Init(initData);
            Party = (Party)initData;
            _time = Party.Date;
        }
    }
}