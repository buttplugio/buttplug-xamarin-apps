using System.Globalization;

namespace ButtplugApp.Models
{
    public interface ILocalise
    {
        void SetLocale(CultureInfo cultureInfo);
        CultureInfo GetCurrentCultureInfo();
    }
}