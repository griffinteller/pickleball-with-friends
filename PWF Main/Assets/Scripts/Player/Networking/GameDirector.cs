using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Player.Mechanics;
using TMPro;
using UnityEngine;

namespace Player.Networking
{
    public enum GameState
    {
        Loading,
        PhonesConnecting,
        Serving,
        Point,
        WaitingForRespawn
    }
    public class GameDirector : MonoBehaviour, IOnEventCallback
    {
        private GameState _gameState;
        private bool _masterClient;
        private PlayerMovement _thisPlayerMovement;

        public int thisPlayerNum;
        public float pickleballOwnershipTransferZ = 0;
        public Vector3 playerOneStartingPos;
        public Vector3 playerTwoStartingPos;
        public float respawnDelay;
        public TMP_Text messageText;
        public int serverNum = 1;
        public Vector3 playerLocalServingPos;
        public float tossMomentum;
        
        public Dictionary<int, int> ActorNumberByPlayerNum = new Dictionary<int, int>();

        public void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void Start()
        {
            _masterClient = PhotonNetwork.IsMasterClient;
            thisPlayerNum = _masterClient ? 1 : 2;
            ActorNumberByPlayerNum[thisPlayerNum] = PhotonNetwork.LocalPlayer.ActorNumber;
            ActorNumberByPlayerNum[thisPlayerNum == 1 ? 2 : 1] = PhotonNetwork.PlayerListOthers[0].ActorNumber;

            Hashtable properties = new Hashtable
            {
                [PlayerProps.InMatch] = true
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

        public void Update()
        {
            switch (_gameState)
            {
                case GameState.Loading:
                
                    LoadingUpdate();
                    break;
                
                case GameState.PhonesConnecting:

                    PhonesConnectingUpdate();
                    break;

                case GameState.Serving:
                    
                    ServingUpdate();
                    break;
                
                case GameState.Point:
                    break;
                
                case GameState.WaitingForRespawn:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ServingUpdate()
        {
            if (thisPlayerNum != serverNum)
                return;

            if (Input.GetKeyUp(KeyCode.Space))
                Serve();
        }

        private void Serve()
        {
            GameObject pickleball = PhotonNetwork.Instantiate(
                "Pickleball",
                _thisPlayerMovement.transform.TransformPoint(playerLocalServingPos),
                Quaternion.identity);
            
            pickleball.GetComponent<Rigidbody>().AddForce(Vector3.up * tossMomentum);
        }

        private void PhonesConnectingUpdate()
        {
            if (!_masterClient)
                return;

            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                if (!player.CustomProperties.ContainsKey(PlayerProps.PhoneConnected) 
                    || !(bool) player.CustomProperties[PlayerProps.PhoneConnected])
                    return;

            _gameState = GameState.Serving;
            RaiseEventOptions options = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            };
            PhotonNetwork.RaiseEvent(
                (byte) PunEventCode.PhonesConnected,
                null, options,
                SendOptions.SendReliable);
        }

        public void LoadingUpdate()
        {
            if (!_masterClient)
                return;

            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                if (!player.CustomProperties.ContainsKey("inMatch") || !(bool) player.CustomProperties["inMatch"])
                    return;

            _gameState = GameState.PhonesConnecting;
            RaiseEventOptions options = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            };
            PhotonNetwork.RaiseEvent(
                (byte) PunEventCode.LoadedScene,
                null, options,
                SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case (byte) PunEventCode.LoadedScene:

                    OnLoaded();
                    break;
                
                case (byte) PunEventCode.PhonesConnected:

                    OnPhonesConnected();
                    break;

                default:
                    break;
            }
        }

        private void OnPhonesConnected()
        {
            Debug.Log("Phones Connected");
            _gameState = GameState.Serving;
        }

        private void OnLoaded()
        {
            Debug.Log("Loaded into match");
            messageText.text = "Waiting for phone connections...";
            _gameState = GameState.PhonesConnecting;

            Vector3 startingPos = thisPlayerNum == 1 ? playerOneStartingPos : playerTwoStartingPos;
            GameObject player = PhotonNetwork.Instantiate("Player", startingPos, Quaternion.identity);
            _thisPlayerMovement = player.GetComponent<PlayerMovement>();
            _thisPlayerMovement.GoToServingPos();
        }
    }
}