using System;
using System.Collections.Generic;
using System.Text;

using ReactiveUI;
using Xamarin.Forms;

namespace ButtplugApp.ViewModels
{
    public class NavigationEntry : ReactiveObject
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public ReactiveCommand Command { get; set; }
    }
}
