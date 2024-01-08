using UnityEngine;
using Photon.Pun;

public class PlayerCameraSetup : MonoBehaviourPunCallbacks
{
    public Camera player1Camera;
    public Camera player2Camera;

    void Start()
    {
        if (MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView = gameObject.AddComponent<PhotonView>();
                int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;

                if (playerNumber == 1)
                {
                    player1Camera.gameObject.SetActive(true);
                    player2Camera.gameObject.SetActive(false);
                }
                else if (playerNumber == 2)
                {
                    player1Camera.gameObject.SetActive(false);
                    player2Camera.gameObject.SetActive(true);
                }
        }
    }
}
