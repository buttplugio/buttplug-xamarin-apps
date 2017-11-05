using ReactiveUI;
using Splat;

namespace ButtplugApp.ViewModels
{
    public class LicenseViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "License";

        public IScreen HostScreen { get; }

        public LicenseViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }

    }
}