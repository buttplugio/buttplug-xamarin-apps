using ReactiveUI.XamForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ButtplugApp.ViewModels;

namespace ButtplugApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServerSettingsView : ReactiveContentPage<ServerSettingsViewModel>
    {
        public ServerSettingsView()
        {
            InitializeComponent();
        }
    }
}