using Plugin.Settings.Abstractions;

namespace ButtplugApp
{
    public static class Settings
    {
        public const int WebSocketPort_Default = 12345;
        public const string WebSocketPort_Key = "server_websocket_port";
        
        public const bool RestrictConnections_Default = true;
        public const string RestrictConnections_Key = "server_websocket_restrict_connections";

        public const bool EnableTLS_Default = false;
        public const string EnableTLS_Key = "server_websocket_enable_tls";

        public const bool StartWhenLaunched_Default = true;
        public const string StartWhenLaunched_Key = "server_websocket_autostart";


        #region Extension Methods for ISettings based on Defaults and keys above


        public static int WebSocketPort(this ISettings settings)
            => settings.GetValueOrDefault(WebSocketPort_Key, WebSocketPort_Default);

        public static void WebSocketPort(this ISettings settings, int value)
            => settings.AddOrUpdateValue(WebSocketPort_Key, value);

        public static bool RestrictConnections(this ISettings settings)
            => settings.GetValueOrDefault(RestrictConnections_Key, RestrictConnections_Default);

        public static void RestrictConnections(this ISettings settings, bool value)
            => settings.AddOrUpdateValue(RestrictConnections_Key, value);

        public static bool EnableTLS(this ISettings settings)
            => settings.GetValueOrDefault(EnableTLS_Key, EnableTLS_Default);

        public static void EnableTLS(this ISettings settings, bool value)
            => settings.AddOrUpdateValue(EnableTLS_Key, value);

        public static bool StartWhenLaunched(this ISettings settings)
            => settings.GetValueOrDefault(StartWhenLaunched_Key, StartWhenLaunched_Default);

        public static void StartWhenLaunched(this ISettings settings, bool value)
            => settings.AddOrUpdateValue(StartWhenLaunched_Key, value);

        #endregion
    }
}