using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Player.Networking
{
    public class JoinRandom : MonoBehaviourPunCallbacks
    {
        public TMP_Text infoText;
        public byte players = 2;
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
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            infoText.text = "Waiting for players...";
            OnPlayerEnteredRoom(PhotonNetwork.LocalPlayer);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            RoomOptions options = new RoomOptions
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = players
            };
            PhotonNetwork.CreateRoom(null, options);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= players)
                PhotonNetwork.LoadLevel(matchSceneName);
        }
    }
}
