using ReactiveUI;
using Splat;

namespace ButtplugApp.ViewModels
{
    public class AboutViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "About";

        public IScreen HostScreen { get; }

        public AboutViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }
    }
}