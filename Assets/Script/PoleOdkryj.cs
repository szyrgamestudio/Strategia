using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PoleOdkryj : MonoBehaviour
{
    public GameObject kafelek;
    public GameObject dark;

    public static bool mgla = true;

    void Start()
    {
        if(!MenuGlowne.multi || !mgla )
            Destroy(dark);
    }

    void Update()
    {
        if(MenuGlowne.multi && !WyburRas.aktywny[Ip.ip-1])//Ip.ip > Menu.IloscGraczyStart && Menu.IloscGraczyStart != 0)
            Destroy(dark);
    }

    public void remove()
    {
        if(MenuGlowne.multi)
        {
            PhotonView photonView = kafelek.GetComponent<PhotonView>();
            photonView.RPC("usunMulti", RpcTarget.All, WyburRas.team[Ip.ip-1]);
        }
        
        Destroy(dark);
    }
    [PunRPC]
    public void usunMulti(int soj)
    {
        if(WyburRas.team[Ip.ip-1] == soj)
            Destroy(dark);
    }
}
