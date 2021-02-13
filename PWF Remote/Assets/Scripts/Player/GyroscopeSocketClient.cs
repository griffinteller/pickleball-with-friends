using System;
using UnityEngine;
using WebSocketSharp;

namespace Player
{
    public class GyroscopeSocketClient : MonoBehaviour
    {
        public Gyroscope gyroscope;
        
        private WebSocket _socket;
        private ConnectionStatus _connectionStatus;
        
        public void Connect()
        {
            if (_connectionStatus != ConnectionStatus.Disconnected)
                return;

            _socket = new WebSocket("ws://10.0.0.37/pwfsocket");
            _socket.Connect();
            _connectionStatus = ConnectionStatus.Connected;
        }

        public void Update()
        {
            if (_connectionStatus != ConnectionStatus.Connected)
                return;

            try
            {
                _socket.Send(QuaternionToBytes(gyroscope.Attitude));
            }
            catch (InvalidOperationException)
            {
                _connectionStatus = ConnectionStatus.Disconnected;
            }
        }

        private static byte[] QuaternionToBytes(Quaternion q) // 2 bytes per float, big endian
        {
            byte[] x = BitConverter.GetBytes(q.x);
            byte[] y = BitConverter.GetBytes(q.y);
            byte[] z = BitConverter.GetBytes(q.z);
            byte[] w = BitConverter.GetBytes(q.w);

            byte[] result = new byte[16];
            x.CopyTo(result, 0);
            y.CopyTo(result, 4);
            z.CopyTo(result, 8);
            w.CopyTo(result, 12);

            return result;
        }
        
        
    }

    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Connected
    }
}
