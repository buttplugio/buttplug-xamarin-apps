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

using ButtplugApp.ViewModels;

namespace ButtplugApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatusView : ReactiveContentPage<StatusViewModel>
	{
		public StatusView ()
		{
			InitializeComponent ();

            ListeningAddresses.ItemSelected += OnAddressSelected;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Addresses, v => v.ListeningAddresses.ItemsSource)
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