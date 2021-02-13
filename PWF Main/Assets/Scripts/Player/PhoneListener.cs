using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

namespace Player
{
    public class PhoneListener : MonoBehaviour
    {

        public ConnectionState ConnectionState { get; private set; } = ConnectionState.Disconnected;
        public IPEndPoint PlayerEndPoint { get; private set; }
        public Vector3 Eulers { get; private set; }

        private const int BytesPerMessage = 6; // 2 bytes per number

        private TcpListener _server;
        private TcpClient _clientConnection;
        private NetworkStream _stream;
        private Task<TcpClient> _connectionTask;

        public void Start()
        {
            // TODO: verify that we are connected to the internet
            
            _server = new TcpListener(IPAddress.Any, 5010);
            _server.Start();
            _connectionTask = _server.AcceptTcpClientAsync();
            ConnectionState = ConnectionState.Connecting;
        }

        public void Update()
        {
            if (ConnectionState == ConnectionState.Connecting)
            {
                if (!_connectionTask.IsCompleted)
                    return;
                
                OnConnected();
            }

            UpdateDataMostRecent();
            print(Eulers);
        }

        private void OnConnected()
        {
            ConnectionState = ConnectionState.Connected;
            _clientConnection = _connectionTask.Result;
            _stream = _clientConnection.GetStream();
            
            print("Phone Connected!");
        }

        private void UpdateDataMostRecent()
        {
            byte[] buffer = new byte[BytesPerMessage];

            while (_stream.Read(buffer, 0, BytesPerMessage) != 0) { }

            Eulers = new Vector3(
                (buffer[0] << 8 + buffer[1]) / 10f, // adjust based on how webapp gives eulers
                (buffer[2] << 8 + buffer[3]) / 10f,
                (buffer[4] << 8 + buffer[5]) / 10f);
        }

        public void OnDisable()
        {
            _stream.Close();
            _clientConnection.Close();
            _server.Stop();
        }
    }

    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected
    }
}
