using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ButtplugApp.ViewModels;
using System.Reactive.Disposables;

namespace ButtplugApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationView : ReactiveContentPage<NavigationViewModel>
    {
        public NavigationView()
        {
            InitializeComponent();

            MenuItemsListView.ItemSelected += OnMenuItemItemSelected;

            this.WhenActivated(disposables =>
            {
                ViewModel.SelectedItem = null;

                this.OneWayBind(ViewModel, vm => vm.NavigationItems, v => v.MenuItemsListView.ItemsSource)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedItem, v => v.MenuItemsListView.SelectedItem)
                    .DisposeWith(disposables);
            });
        }

        private void OnMenuItemItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}