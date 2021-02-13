using System;
using Microsoft.Win32;
using Photon.Pun;
using Player.Networking;
using UnityEngine;

namespace Player.Mechanics
{
    public class Pickleball : MonoBehaviourPun
    {
        public float clickForce = 0.05f;
        public Vector3 clickDirection = new Vector3(0, 0.5f, 0.5f);
        
        private GameDirector _gameDirector;
        private Rigidbody _rigidbody;
        private int _ownerNum = 1;
        
        public void Start()
        {
            _gameDirector = FindObjectOfType<GameDirector>();
            _rigidbody = GetComponent<Rigidbody>();
            clickDirection.Normalize();
        }

        public void FixedUpdate()
        {
            _ownerNum = photonView.IsMine ? _gameDirector.thisPlayerNum : 3 - _gameDirector.thisPlayerNum;
            
            if (!photonView.IsMine)
                return;

            int correctOwnerNum = transform.position.z > _gameDirector.pickleballOwnershipTransferZ ? 2 : 1;

            if (correctOwnerNum == _ownerNum)
                return;
            
            int newOwnerActorNumber = _gameDirector.ActorNumberByPlayerNum[correctOwnerNum];
            photonView.TransferOwnership(PhotonNetwork.CurrentRoom.GetPlayer(newOwnerActorNumber));
        }

        public void Update()
        {
            if (!photonView.IsMine)
                return;
            
            KeyCheck();
        }

        private void KeyCheck()
        {
            if (Input.GetKeyUp(KeyCode.Space))
                _rigidbody.AddForce(clickDirection * clickForce, ForceMode.Impulse);
        }
    }
}
