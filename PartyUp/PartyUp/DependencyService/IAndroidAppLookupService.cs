namespace PartyUp.DependencyService
{
    public interface IAndroidAppLookupService
    {
        /// <summary>
        /// Checks wether a name is installed on the device.
        /// </summary>
        bool DoesAppExist(string packageName);
    }
}