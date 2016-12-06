using App2Night.Model.Model;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms.Maps;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
    public class PartyDetailViewModel : FreshBasePageModel
    {
        public Party Party { get; set; }

        public bool IsMyParty => Party.HostedByUser;

        private Pin _mapPin;

        public Pin MapPins
        {
            get { return new Pin
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

        public override void Init(object initData)
        {
            base.Init(initData);
            Party = (Party) initData;
        }
    }
}