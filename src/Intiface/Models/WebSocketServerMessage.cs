using Buttplug.Server;

namespace Intiface.Models
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
