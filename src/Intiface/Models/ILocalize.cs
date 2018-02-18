using System.Globalization;

namespace Intiface.Models
{
    public interface ILocalise
    {
        void SetLocale(CultureInfo cultureInfo);
        CultureInfo GetCurrentCultureInfo();
    }
}