using System;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Player
{
    public class PhoneSocketServer : WebSocketBehavior
    {

        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log(e.RawData.Length);
            PhoneStateServer.attitudeExposed = ByteArrayToQuaternion(e.RawData);
        }

        private static Quaternion ByteArrayToQuaternion(byte[] data)
        {
            Quaternion q = new Quaternion
            {
                x = BitConverter.ToSingle(data, 0),
                y = BitConverter.ToSingle(data, 4),
                z = BitConverter.ToSingle(data, 8),
                w = BitConverter.ToSingle(data, 12)
            };

            return q;
        }
        
    }
}