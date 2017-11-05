using ReactiveUI;
using Splat;

namespace ButtplugApp.ViewModels
{
    public class ServerSettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Settings";

        public IScreen HostScreen { get; }

        public ServerSettingsViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }
    }
}