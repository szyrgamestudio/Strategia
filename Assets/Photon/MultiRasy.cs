using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiRasy : MonoBehaviourPunCallbacks
{
    
    public GameObject[] graczDostepny;
    public  static int playerCount;
    public int dd;
    void Start()
    {
        // Sprawdź, czy jesteśmy połączeni z Master Server
        if (PhotonNetwork.IsConnected)
        {
            // Pobierz aktualną liczbę graczy w pokoju
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if(Ip.ip==0)
                Ip.ip = playerCount;
        }
    }

    // Ta funkcja zostanie wywołana, gdy gracz dołączy do pokoju
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Aktualizuj liczbę graczy po dołączeniu nowego gracza
        UpdatePlayerCount();
    }

    // Ta funkcja zostanie wywołana, gdy gracz opuści pokój
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // Aktualizuj liczbę graczy po opuszczeniu pokoju przez innego gracza
        UpdatePlayerCount();
    }

    // Funkcja do aktualizacji i wyświetlania liczby graczy
    void UpdatePlayerCount()
    {
        // Upewnij się, że jesteśmy nadal połączeni z Master Server
        if (PhotonNetwork.IsConnected)
        {
            // Pobierz aktualną liczbę graczy w pokoju
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            // Wyświetl informację o liczbie graczy w konsoli
            Debug.Log("Liczba graczy w pokoju: " + playerCount);
            Debug.Log("ocb");

            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("checkVersion", RpcTarget.All, Ip.version);
        }
    }

    [PunRPC]
    public void checkVersion(string version)
    {
        Debug.Log(version + " version" + Ip.version + " " + Ip.ip);
        if(Ip.ip != 1 && version != Ip.version)
        {
            Debug.Log("dis");
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0); 
        }
    }

    void Update()
    {
        if (MenuGlowne.multi)
        {
            for(int i = 0; i<4;i++)
            {
                if(PhotonNetwork.CurrentRoom.PlayerCount >= i+1)
                    graczDostepny[i].SetActive(true);
                else
                    graczDostepny[i].SetActive(false);
            }
        }
    }

}
