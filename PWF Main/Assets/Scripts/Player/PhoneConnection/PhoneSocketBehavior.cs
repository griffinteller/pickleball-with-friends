using System;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Player.PhoneConnection
{
    public class PhoneSocketBehavior : WebSocketBehavior
    {

        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log(e.RawData.Length);
            PhoneStateServer.attitudeExposed = ByteArrayToQuaternion(e.RawData);
        }

        private static Quaternion ByteArrayToQuaternion(byte[] data)
        {
            Quaternion q = new Quaternion // out of order to fix orientation
            {
                x = BitConverter.ToSingle(data, 0),
                z = BitConverter.ToSingle(data, 4),
                y = BitConverter.ToSingle(data, 8),
                w = BitConverter.ToSingle(data, 12)
            };

            return Quaternion.Inverse(q);
        }
        
    }
}