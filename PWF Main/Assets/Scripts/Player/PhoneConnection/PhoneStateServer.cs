using UnityEngine;
using WebSocketSharp.Server;

namespace Player.PhoneConnection
{
    public class PhoneStateServer : MonoBehaviour
    {
        public static Quaternion attitudeExposed;
            
        public Quaternion attitude;
        
        private WebSocketServer _server;
        public void Start()
        {
            _server = new WebSocketServer(80);
            _server.AddWebSocketService<PhoneSocketBehavior>("/pwfsocket");
            _server.Start();
        }

        public void Update()
        {
            attitude = attitudeExposed;
        }

        public void OnDisable()
        {
            _server.Stop();
        }
    }
}