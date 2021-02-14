using System;
using UnityEngine;

namespace Player.Mechanics
{
    [RequireComponent(typeof(Rigidbody))]
    public class Paddle : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Vector3 _lastPos;
        private Quaternion _lastRot;

        public GameObject paddleObj;
        public float elasticity = 0.05f;
        
        [Header("Info, editing does nothing")]
        public Vector3 linearVelocity;
        public Vector3 angularVelocity;

        public float PivotRadius
        {
            get => paddleObj.transform.position.z;
            set
            {
                Transform t = paddleObj.transform;
                Vector3 pos = t.localPosition;
                pos.z = value;
                t.localPosition = pos;
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.tag.Equals("Pickleball"))
                return;

            Vector3 paddleVelocity = GetLocalPointVelocity(transform.InverseTransformPoint(other.GetContact(0).point));
            Vector3 collisionNormal = other.GetContact(0).normal;
            Vector3 transferVelocity = Vector3.Project(paddleVelocity, collisionNormal) * elasticity;

            other.rigidbody.velocity = transferVelocity;
        }

        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        public void Start()
        {
            _lastPos = _rigidbody.position;
            _lastRot = _rigidbody.rotation;
        }

        public void FixedUpdate()
        {
            Vector3 nowPos = _rigidbody.position;
            Quaternion nowRot = _rigidbody.rotation;

            linearVelocity = (nowPos - _lastPos) / Time.fixedDeltaTime;

            (Quaternion.Inverse(_lastRot) * nowRot).ToAngleAxis(out float angle, out Vector3 axis);

            angularVelocity = Mathf.Deg2Rad * angle / Time.fixedDeltaTime * axis;

            _lastPos = nowPos;
            _lastRot = nowRot;
        }

        public Vector3 GetLocalPointVelocity(Vector3 localPoint)
        {
            Vector3 perpFromRotAxis = localPoint - Vector3.Project(localPoint, angularVelocity);
            Vector3 rotVelocityComponent = Vector3.Cross(angularVelocity, perpFromRotAxis);
            return linearVelocity + rotVelocityComponent;
        }
    }
}