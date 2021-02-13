using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using WebSocketSharp.Server;

namespace Player
{
    public class PhoneStateServer : MonoBehaviour
    {
        public static Quaternion attitudeExposed;
            
        public Vector3 attitudeEulers;
        
        private WebSocketServer _server;
        public void Start()
        {
            _server = new WebSocketServer(80);
            _server.AddWebSocketService<PhoneSocketServer>("/pwfsocket");
            _server.Start();
        }

        public void Update()
        {
            attitudeEulers = attitudeExposed.eulerAngles;
        }

        public void OnDisable()
        {
            _server.Stop();
        }
    }
}