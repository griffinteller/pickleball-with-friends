using Photon.Pun;
using TMPro;

namespace Player.Networking
{
    public class JoinRandom : MonoBehaviourPunCallbacks
    {
        public TMP_Text infoText;
        public int players = 2;
        public string matchSceneName;

        public void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (!PhotonNetwork.IsConnectedAndReady)
                PhotonNetwork.ConnectUsingSettings();
            else
                OnConnectedToMaster();
        }

        public override void OnConnectedToMaster()
        {
            infoText.text = "Finding match...";
            PhotonNetwork.NetworkingClient.OpJoinRandomOrCreateRoom(null, null);
        }

        public override void OnJoinedRoom()
        {
            infoText.text = "Waiting for players...";

            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= players)
                PhotonNetwork.LoadLevel(matchSceneName);
        }
    }
}
