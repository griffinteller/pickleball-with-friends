using UnityEngine;

namespace Player.PhoneConnection
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