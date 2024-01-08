using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviourPun
{
    public static int CurrentMapID = 1; // Ustaw domyślny identyfikator mapy

    public void ChangeMap(int newMapID)
    {
        CurrentMapID = newMapID;

        // Wczytaj nową scenę z odpowiednią mapą
        PhotonNetwork.LoadLevel("Mapa" + newMapID.ToString());
    }
}
