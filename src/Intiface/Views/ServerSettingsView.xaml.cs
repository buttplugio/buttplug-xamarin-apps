using ReactiveUI.XamForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reactive.Disposables;
using Intiface.ViewModels;

namespace Intiface.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServerSettingsView : ReactiveContentPage<ServerSettingsViewModel>
    {
        public ServerSettingsView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.WebSocketPort, v => v.Port.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.RestrictConnections, v => v.LocalhostOnly.IsToggled)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.EnableTLS, v => v.EnableTLS.IsToggled)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.StartWhenLaunched, v => v.AutoStartServer.IsToggled)
                    .DisposeWith(disposables);
            });
        }
    }
}