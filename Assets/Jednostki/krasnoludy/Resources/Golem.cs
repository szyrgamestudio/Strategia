using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Golem : MonoBehaviour
{
    public float DMG;
    public void zadaj(GameObject target)
    {
        target.GetComponent<Jednostka>().HP -= DMG;
        if(MenuGlowne.multi)
        {
            PhotonView photonView = target.GetComponent<PhotonView>();
            photonView.RPC("zaatakowanieMulti", RpcTarget.All,Ip.ip, DMG);
        }
    } 
}
