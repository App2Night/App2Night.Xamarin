using System;
using PartyUp.Model.Enum;

namespace App2Night.DependencyService
{
    public class LocationChangeEventArgs : EventArgs
    {
        public LocationChangeEventArgs(bool hasFix, Coordinates location)
        {
            HasFix = hasFix;
            Location = location;
        }

        public Coordinates Location { get; set; }
        public bool HasFix { get; set; }
    }
    public interface IUserLocationService
    {
        event EventHandler<Coordinates> LocationChanged;
        event EventHandler<LocationChangeEventArgs> LocationStatusChanged;
        Coordinates GetUserCoordinates();
    }
}