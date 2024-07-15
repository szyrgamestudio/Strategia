using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Muzyka : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] ST;
    public AudioClip NPC;
    public int[] progresja = new int[4];
    public int[] tury1 = new int[4];
    public int[] tury2 = new int[4];

    public static Muzyka muzyka;

   public void Start()
   {
        if(muzyka == null)
            muzyka = GetComponent<Muzyka>();
        int j = 16;
        if(SimultanTurns.simultanTurns)
            j /= 4;
        for(int i = 0; i < 4; i++)
        {
            if(progresja[i] == 1 && tury1[i] + j < Menu.nrTury)
            {
                progresja[i] = 0;

            }
            if(progresja[i] == 2 && tury2[i] + j < Menu.nrTury)
            {
                progresja[i] = 1;
                tury1[i] = Menu.nrTury;
            }
        }
        if(MenuGlowne.multi && Ip.ip == 1)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("update", RpcTarget.All);
        }
        if(Menu.tura == 0)
        {
            source.clip = NPC;
            source.Play();
        }
        else
        {
            if(SimultanTurns.simultanTurns)
            {
                source.clip = ST[progresja[Ip.ip-1] + 3 * WyburRas.rasa[Ip.ip-1]];
                source.Play();
            }
            else
            {
                source.clip = ST[progresja[Menu.tura-1] + 3 * WyburRas.rasa[Menu.tura-1]];
                source.Play();
            }
        
        }
   }

    [PunRPC]
    void update()
    {
        if(Ip.ip != 1)
            Start();
    }
}
