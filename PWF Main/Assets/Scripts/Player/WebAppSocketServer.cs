using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Player
{
    public class WebAppSocketServer : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log(e.Data);
        }
    }
}