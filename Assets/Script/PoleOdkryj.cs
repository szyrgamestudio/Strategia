using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PoleOdkryj : MonoBehaviour
{
    public GameObject kafelek;
    public GameObject dark;

    public static bool mgla = true;
    // Start is called before the first frame update
    void Start()
    {
        if(!MenuGlowne.multi || !mgla)
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
        //Debug.Log(WyburRas.team[Ip.ip-1] + " --- " + soj);
        if(WyburRas.team[Ip.ip-1] == soj)
            Destroy(dark);
    }
}
