using System;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System.Globalization;
using Intiface.Models;
using Intiface.Pages;
using Intiface.Properties;
using Intiface.ViewModels;
using Intiface.Views;

namespace Intiface
{
    public partial class App : Application, IScreen
    {
        public RoutingState Router { get; protected set; }

        private Settings _settings = null;

        public Settings Settings => _settings ?? (_settings = new Settings(CrossSettings.Current));

        private CultureInfo _currentCulture = null;

        public CultureInfo CurrentCulture => _currentCulture ?? (_currentCulture = GetCurrentCulture());

        public App()
        {
            InitializeComponent();

            // Set the current culture to our resource
            Resource.Culture = GetCurrentCulture();

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

        protected override void OnStart()
        {
            base.OnStart();

            if (Settings.StartWhenLaunched)
                MessagingCenter.Send(new ServerCommandMessage { Command = ServerCommand.Start }, nameof(ServerCommandMessage));
        }

        private CultureInfo GetCurrentCulture()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                return DependencyService.Get<ILocalise>().GetCurrentCultureInfo();

            return CultureInfo.CurrentCulture;
        }
    }
}