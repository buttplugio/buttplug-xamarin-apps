using Buttplug.Server;

namespace ButtplugApp.Models
{
    public enum ServerCommand
    {
        Start,
        Stop,
    }

    public class ServerCommandMessage
    {
        public ServerCommand Command { get; set; }
    }
}
