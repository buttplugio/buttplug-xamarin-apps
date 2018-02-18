using System;

namespace Intiface.Android.Utils
{
    public class PlatformCulture
    {
        public string PlatformString { get; }

        public string LanguageCode { get; }

        public string LocaleCode { get; }

        public PlatformCulture(string platformCultureString)
        {
            if (String.IsNullOrEmpty(platformCultureString))
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));

            // .NET expects dash, not underscore
            PlatformString = platformCultureString.Replace("_", "-");

            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if (dashIndex > 0)
            {
                var parts = PlatformString.Split('-');
                LanguageCode = parts[0];
                LocaleCode = parts[1];
            }
            else
            {
                LanguageCode = PlatformString;
                LocaleCode = "";
            }
        }

        public override string ToString()
        {
            return PlatformString;
        }
    }
}