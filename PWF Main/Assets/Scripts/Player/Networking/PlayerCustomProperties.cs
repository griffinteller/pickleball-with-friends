using ExitGames.Client.Photon;
using Photon.Pun;

namespace Player.Networking
{
    public class PlayerCustomProperties
    {
        public const string InMatch = "inMatch";

        public static Hashtable DefaultTable => new Hashtable
        {
            {InMatch, false}
        };

        public static void SetDefaultTable()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(DefaultTable);
        }
    }
}