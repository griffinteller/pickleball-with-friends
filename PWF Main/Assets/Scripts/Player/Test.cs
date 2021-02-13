using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using WebSocketSharp.Server;

namespace Player
{
    public class Test : MonoBehaviour
    {
        private WebSocketServer _server;
        public void Start()
        {
            _server = new WebSocketServer(80);
            _server.AddWebSocketService<WebAppSocketServer>("/pwfsocket");
            _server.Start();
        }

        public void OnDisable()
        {
            _server.Stop();
        }
    }
}