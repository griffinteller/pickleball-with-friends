using UnityEngine;
using WebSocketSharp;

namespace Player
{
    public class GyroscopeSocketClient : MonoBehaviour
    {
        private WebSocket _socket;
        private ConnectionStatus _connectionStatus;
        
        public void Connect()
        {
            if (_connectionStatus != ConnectionStatus.Disconnected)
                return;

            _socket = new WebSocket("ws://10.0.0.37/pwfsocket");
            _socket.Connect();
            _socket.Send("Hello!");
        }
    }

    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Connected
    }
}
