using System.Globalization;

namespace App2Night.DependencyService
{
    public interface ICultureService
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}