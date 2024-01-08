using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviourPun
{
    private MapManager mapManager;

    void Start()
    {
        mapManager = GetComponent<MapManager>();
    }

    void Update()
    {
        // Sprawdź, czy jesteśmy w trybie multiplayer
        if (PhotonNetwork.IsConnected)
        {
            // Sprawdź, czy jest komenda przeniesienia do mapy o ID 2
            if (Input.GetKeyDown(KeyCode.M))
            {
                int newMapID = 1;
                photonView.RPC("ChangeMapRPC", RpcTarget.All, newMapID);
            }
        }
    }

    [PunRPC]
    void ChangeMapRPC(int newMapID)
    {
        // Wywołaj funkcję zmiany mapy na wszystkich klientach
        mapManager.ChangeMap(newMapID);
    }
}
