using System;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ButtplugApp.Pages;
using ButtplugApp.ViewModels;
using ButtplugApp.Views;
using Plugin.Settings.Abstractions;
using Plugin.Settings;

namespace ButtplugApp
{
    public partial class App : Application, IScreen
    {
        public RoutingState Router { get; protected set; }

        public ISettings Settings { get => CrossSettings.Current; }

        public App()
        {
            InitializeComponent();

            var service = Locator.CurrentMutable;

            service.RegisterConstant<IScreen>(this);
            service.Register<IViewFor<DeviceListViewModel>>(() => new DeviceListView());
            service.Register<IViewFor<ServerSettingsViewModel>>(() => new ServerSettingsView());

            Router = new RoutingState();
            Router.NavigateAndReset
                  .Execute(new DeviceListViewModel())
                  .Subscribe();

            MainPage = new MainPage();
        }
    }
}