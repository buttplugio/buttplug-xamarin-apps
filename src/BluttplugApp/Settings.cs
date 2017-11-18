using System;
using Plugin.Settings.Abstractions;

namespace ButtplugApp
{
    public class Settings : ISettings
    {
        public const int WebSocketPort_Default = 12345;
        public const string WebSocketPort_Key = "server_websocket_port";
        
        public const bool RestrictConnections_Default = true;
        public const string RestrictConnections_Key = "server_websocket_restrict_connections";

        public const bool EnableTLS_Default = false;
        public const string EnableTLS_Key = "server_websocket_enable_tls";

        public const bool StartWhenLaunched_Default = true;
        public const string StartWhenLaunched_Key = "server_websocket_autostart";

        public Settings(ISettings settings)
        {
            _settings = settings;
        }

        private ISettings _settings;

        public int WebSocketPort
        {
            get => _settings.GetValueOrDefault(WebSocketPort_Key, WebSocketPort_Default);
            set => _settings.AddOrUpdateValue(WebSocketPort_Key, value);
        }

        public bool RestrictConnections
        {
            get => _settings.GetValueOrDefault(RestrictConnections_Key, RestrictConnections_Default);
            set => _settings.AddOrUpdateValue(RestrictConnections_Key, value);
        }

        public bool EnableTLS
        {
            get => _settings.GetValueOrDefault(EnableTLS_Key, EnableTLS_Default);
            set => _settings.AddOrUpdateValue(EnableTLS_Key, value);
        }

        public bool StartWhenLaunched
        {
            get => _settings.GetValueOrDefault(StartWhenLaunched_Key, StartWhenLaunched_Default);
            set => _settings.AddOrUpdateValue(StartWhenLaunched_Key, value);
        }

        public decimal GetValueOrDefault(string key, decimal defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public bool GetValueOrDefault(string key, bool defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public long GetValueOrDefault(string key, long defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public string GetValueOrDefault(string key, string defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public int GetValueOrDefault(string key, int defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public float GetValueOrDefault(string key, float defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public DateTime GetValueOrDefault(string key, DateTime defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public Guid GetValueOrDefault(string key, Guid defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public double GetValueOrDefault(string key, double defaultValue, string fileName = null)
            => _settings.GetValueOrDefault(key, defaultValue, fileName);

        public bool AddOrUpdateValue(string key, decimal value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, bool value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, long value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, string value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, int value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, float value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, DateTime value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, Guid value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public bool AddOrUpdateValue(string key, double value, string fileName = null)
            => _settings.AddOrUpdateValue(key, value, fileName);

        public void Remove(string key, string fileName = null)
            => _settings.Remove(key, fileName);

        public void Clear(string fileName = null)
            => _settings.Clear(fileName);

        public bool Contains(string key, string fileName = null)
            => _settings.Contains(key, fileName);

        public bool OpenAppSettings()
            => _settings.OpenAppSettings();
    }
}