using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Disposables;
using System.Threading.Tasks;

using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Intiface.ViewModels;

namespace Intiface.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatusView : ReactiveContentPage<StatusViewModel>
	{
		public StatusView ()
		{
            InitializeComponent();

            ListeningAddresses.ItemSelected += OnAddressSelected;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                        vm => vm.StartStopText,
                        v => v.StartStop.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, 
                        vm => vm.StatusText, 
                        v => v.Status.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Addresses, v => v.ListeningAddresses.ItemsSource)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel, 
                        vm => vm.StartStopCommand, 
                        v => v.StartStop, 
                        nameof(StartStop.Clicked))
                    .DisposeWith(disposables);

            });
		}

        private void OnAddressSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Xamarin.Forms doesn't have a nice way of making items non-selectable. This is their offical workaround
            (sender as ListView).SelectedItem = null;
        }
    }
}