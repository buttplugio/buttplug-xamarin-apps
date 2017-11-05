using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButtplugApp.ViewModels
{
    public class StatusViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Status";

        public IScreen HostScreen { get; }

        public List<string> Addresses { get; set; } = new List<string>();

        public StatusViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            // TODO: Remove this example address
            Addresses.Add("ws://192.168.1.10:12345/buttplug");
        }
    }
}
