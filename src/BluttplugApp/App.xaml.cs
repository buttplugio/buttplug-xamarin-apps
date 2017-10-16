using System;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ButtplugApp.Pages;
using ButtplugApp.ViewsModels;
using ButtplugApp.Views;

namespace ButtplugApp
{
    public partial class App : Application, IScreen
    {
        public RoutingState Router { get; protected set; }

        public App()
        {
            InitializeComponent();

            var service = Locator.CurrentMutable;

            service.RegisterConstant<IScreen>(this);
            service.Register<IViewFor<DeviceListViewModel>>(() => new DeviceListView());

            Router = new RoutingState();
            Router.NavigateAndReset
                  .Execute(new DeviceListViewModel())
                  .Subscribe();

            MainPage = new MainPage();
        }
    }
}