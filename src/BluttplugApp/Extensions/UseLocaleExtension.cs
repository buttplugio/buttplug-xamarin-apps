using ButtplugApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ButtplugApp.Extensions
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty(nameof(Text))]
    public class UseLocaleExtension : IMarkupExtension
    {
        private readonly CultureInfo _cultureInfo;
        private static readonly Lazy<ResourceManager> _resourceManager = new Lazy<ResourceManager>(() => Properties.Resource.ResourceManager);

        public string Text { get; set; }

        public UseLocaleExtension()
        {
            _cultureInfo = (App.Current as App).CurrentCulture;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var translation = _resourceManager.Value.GetString(Text, _cultureInfo);

            if (translation == null)
            {
                Debug.WriteLine($"Key '{Text}' was not found in resources '{_resourceManager.Value.BaseName}' for culture '{_cultureInfo.Name}'.");
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
            }
            return translation;
        }
    }
}