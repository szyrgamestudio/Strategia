using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Zniwiarz : MonoBehaviour
{
    public GameObject jednostka;
    public bool koniec;
    void Update()
    {
        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next && Menu.tura == jednostka.GetComponent<Jednostka>().druzyna)//&& (druzyna+1)%(Menu.IloscGraczy+1) == Menu.tura)
        {
            koniec = false;
            if(MenuGlowne.multi)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("updateMulti", RpcTarget.All, Ip.ip, Menu.zloto[jednostka.GetComponent<Jednostka>().druzyna]);
            }
            jednostka.GetComponent<Jednostka>().HP -= Menu.zloto[jednostka.GetComponent<Jednostka>().druzyna];
        }
    }

    [PunRPC]
    public void updateMulti(int ip, int zloto)
    {
        if(ip != Ip.ip)
            jednostka.GetComponent<Jednostka>().HP -= zloto;
    }
}
