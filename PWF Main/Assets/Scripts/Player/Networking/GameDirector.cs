using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Player.Networking
{
    public class GameDirector : MonoBehaviour, IOnEventCallback
    {
        private bool _loaded;
        private bool _masterClient;

        public int playerNum;

        public void Start()
        {
            _masterClient = PhotonNetwork.IsMasterClient;
            playerNum = _masterClient ? 1 : 2;

            Hashtable settings = new Hashtable
            {
                {"inMatch", true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(settings);
        }

        public void Update()
        {
            if (!_loaded)
            {
                if (!_masterClient)
                    return;

                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                    if (!player.CustomProperties.ContainsKey("inMatch"))
                        return;

                RaiseEventOptions options = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All
                };
                PhotonNetwork.RaiseEvent(
                    (byte) PunEventCode.LoadedScene,
                    null, options,
                    SendOptions.SendReliable);
            }
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
            _loaded = true;
        }
    }
}