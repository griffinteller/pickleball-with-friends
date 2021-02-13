using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Player.Networking
{
    public enum GameState
    {
        Loading,
        PhonesConnecting,
        WaitingForServe,
        Serving,
        Point,
        WaitingForRespawn
    }
    public class GameDirector : MonoBehaviour, IOnEventCallback
    {
        private GameState _gameState;
        private bool _masterClient;

        public int thisPlayerNum;
        public float pickleballOwnershipTransferZ = 0;
        public Vector3 pickleballStartingPos;
        public float respawnDelay;
        public TMP_Text messageText;
        
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
                [PlayerCustomProperties.InMatch] = true
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
                    break;
                
                case GameState.WaitingForServe:
                    break;
                
                case GameState.Serving:
                    break;
                
                case GameState.Point:
                    break;
                
                case GameState.WaitingForRespawn:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void LoadingUpdate()
        {
            if (!_masterClient)
                return;

            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                if (!player.CustomProperties.ContainsKey("inMatch"))
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

                default:
                    break;
            }
        }

        private void OnLoaded()
        {
            Debug.Log("Loaded into match");
            _gameState = GameState.PhonesConnecting;

            if (!_masterClient)
                return;

            PhotonNetwork.Instantiate("Pickleball", pickleballStartingPos, Quaternion.identity);
        }
    }
}