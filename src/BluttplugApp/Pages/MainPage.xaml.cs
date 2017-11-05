using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ButtplugApp.ViewModels;
using System.Reactive.Disposables;

namespace ButtplugApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ReactiveMasterDetailPage<MainPageViewModel>
    {
        public MainPage()
        {
            InitializeComponent();
            IsPresented = false;

            ViewModel = MainPageViewModel.Default;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm, v => v.Navigation.ViewModel)
                    .DisposeWith(disposables);
            });         
        }
    }
}