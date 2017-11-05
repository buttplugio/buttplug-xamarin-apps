using System.ComponentModel;
using ReactiveUI;
using Splat;

namespace ButtplugApp.ViewModels
{
    public class DeviceListViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Devices";

        public IScreen HostScreen { get; }

        public DeviceListViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();


        }
    }
}