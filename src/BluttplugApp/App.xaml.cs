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

        private Settings _settings = null;
        public Settings Settings => _settings ?? (_settings = new Settings(CrossSettings.Current));

        public App()
        {
            InitializeComponent();

            var service = Locator.CurrentMutable;

            service.RegisterConstant<IScreen>(this);
            service.Register<IViewFor<AboutViewModel>>(() => new About());
            service.Register<IViewFor<LicenseViewModel>>(() => new License());
            service.Register<IViewFor<DeviceListViewModel>>(() => new DeviceListView());
            service.Register<IViewFor<StatusViewModel>>(() => new StatusView());
            service.Register<IViewFor<ServerSettingsViewModel>>(() => new ServerSettingsView());

            Router = new RoutingState();
            Router.NavigateAndReset
                  .Execute(new StatusViewModel())
                  .Subscribe();

            MainPage = new MainPage();
        }
    }
}