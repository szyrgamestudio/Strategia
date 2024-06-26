using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SimultanTurns : MonoBehaviourPun
{
    public static bool simultanTurns = false;
    public static bool ready;
    public static int readyCount = 0;
    private static PhotonView photonView;
    public Image wait;


    void Update()
    {
        if(MenuGlowne.multi)
        {
            wait.enabled = ready;

            if(Input.GetKeyDown(KeyCode.BackQuote)) // Tylda znajduje się na klawiszu BackQuote
            {
                Debug.Log(readyCount + " + " + Menu.IloscGraczy);
            }
            if (readyCount == Menu.IloscGraczy && readyCount != 0)
            {
                Debug.Log(readyCount + " + " + Menu.IloscGraczy + " " + ready);
                readyCount = 0;
                photonView.RPC("NextTurn", RpcTarget.All);
                Menu.menu.NextTurn();
            }
        }
    }
    public void Start()
    {
        if(MenuGlowne.multi && simultanTurns){
            photonView = GetComponent<PhotonView>(); // Get component after the instance is created
            Menu.tura = Ip.ip;
            Menu.zloto[1] = 15;
            Menu.drewno[1] = 15;
            Menu.zloto[2] = 15;
            Menu.drewno[2] = 15;
            Menu.zloto[3] = 15;
            Menu.drewno[3] = 15;
            Menu.zloto[4] = 15;
            Menu.drewno[4] = 15;
        }
    }

    public static void EndTurn()
    {
        ready = true;
        photonView.RPC("UpdateCount", RpcTarget.All, readyCount);
       
    }
    public static void playerTurn()
    {
        Debug.Log("Ip: " + Ip.ip);
        photonView.RPC("playerUpdate", RpcTarget.All);
    }

    [PunRPC]
    void UpdateCount(int x)
    {
        x++;
        readyCount = x;
    }

    [PunRPC]
    void NextTurn()
    {
        // Assuming Menu.tura is static or accessible from here
        Menu.tura = 0;
    }
    [PunRPC]
    void playerUpdate()
    {
        if(WyburRas.aktywny[Ip.ip-1])
        {
        Menu.tura = Ip.ip;
        readyCount = 0;
        ready = false;
        Menu.NIERUSZAC = false;
        }
        else
        Menu.NIERUSZAC = true;
    }
}

