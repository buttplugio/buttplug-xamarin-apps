using Plugin.Settings.Abstractions;
using ReactiveUI;
using Splat;
using System;

using ButtplugApp;

namespace ButtplugApp.ViewModels
{
    public class ServerSettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Settings _settings;

        public string UrlPathSegment => "Settings";

        public IScreen HostScreen { get; }

        private int _webSocketPort;
        public int WebSocketPort {
            get => _webSocketPort = _settings.WebSocketPort;
            set {
                if (_webSocketPort == value)
                    return;
                _settings.WebSocketPort = value;
                this.RaisePropertyChanged(nameof(WebSocketPort));

            }
        }

        private bool _restrictConnections;
        public bool RestrictConnections {
            get => _restrictConnections = _settings.RestrictConnections;
            set
            {
                if (_restrictConnections == value)
                    return;
                _settings.RestrictConnections = value;
                this.RaisePropertyChanged(nameof(RestrictConnections));

            }
        }

        private bool _enableTLS;
        public bool EnableTLS
        {
            get => _enableTLS = _settings.EnableTLS;
            set
            {
                if (_enableTLS == value)
                    return;
                _settings.EnableTLS = value;
                this.RaisePropertyChanged(nameof(EnableTLS));

            }
        }

        private bool _startWhenLaunched;
        public bool StartWhenLaunched
        {
            get => _startWhenLaunched = _settings.StartWhenLaunched;
            set
            {
                if (_startWhenLaunched == value)
                    return;
                _settings.StartWhenLaunched = value;
                this.RaisePropertyChanged(nameof(StartWhenLaunched));

            }
        }

        public string Status { get; }

        public string Logs { get; }

        /// <summary>
        /// Clear the log output.
        /// </summary>
        public ReactiveCommand ClearLogs;

        /// <summary>
        /// Update the WebSocket Server if it's running.
        /// </summary>
        public ReactiveCommand Update;

        public ServerSettingsViewModel(IScreen hostScreen = null, ISettings settingsSource = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            if (settingsSource != null)
            {
                _settings = settingsSource as Settings
                    ?? new Settings(settingsSource);
            }
            else
            {
                _settings = (App.Current as App)?.Settings
                    ?? throw new ArgumentNullException($"Could not locate an appropiate {nameof(settingsSource)} default value.");
            }

            ClearLogs = ReactiveCommand.Create(() => { });
        }


    }
}