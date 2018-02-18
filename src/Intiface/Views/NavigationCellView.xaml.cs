using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Intiface.ViewModels;

namespace Intiface.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavigationCellView : ReactiveViewCell<NavigationEntry>
	{
        // Since this ViewCell is recycled, care is needed when disposing everything
        protected readonly CompositeDisposable SubscriptionDisposables = new CompositeDisposable();

        public NavigationCellView ()
		{
			InitializeComponent();

            this.WhenActivated(_ =>
            {
                this.OneWayBind(ViewModel, vm => vm.Title, v => v.Title.Text)
                    .DisposeWith(SubscriptionDisposables);
            });
		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            SubscriptionDisposables.Clear();
        }
    }
}