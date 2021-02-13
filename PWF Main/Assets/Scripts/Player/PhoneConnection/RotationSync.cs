using System;
using UnityEngine;

namespace Player
{
    public class RotationSync : MonoBehaviour
    {
        public PhoneStateServer server;

        public void Update()
        {
            transform.rotation = server.attitude;
        }
    }
}