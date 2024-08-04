using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class End : MonoBehaviour
{
    public static int maxGraczy;
    public static bool pvp;
    public static bool control;
    public static bool economy;
    public static bool boss;
    public static bool bossPokonany = false;

    public static int tureKontroli;
    public static int punktyKontroli = 0;
    public static int druzynaKontroli = 0;

    public static int poziomRatusza;
    public static Vector3 bossPosition;
    public static int tureDoKonca;
    public static End end;
    public PhotonView photonView;
    void Start()
    {
        end = GetComponent<End>();
    }

    public void updateStats()
    {
        photonView.RPC("updateStatsMulti", RpcTarget.All, pvp, control, economy, boss);
    }

    public static bool czyWygrana()
    {
        if(pvp && czyWygranaPvp())
            return true;
        if(control && czyWygranaControl())
            return true;
        if(economy && czyWygranaEconomy())
            return true;
        if(boss && czyWygranaBoss())
            return true;
        return false;
    }

    static bool czyWygranaPvp()
    {
        List<int> lista = new List<int>();
        for(int i = 0; i < Menu.IloscGraczyStart; i++)
        {
            if(WyburRas.aktywny[i] && !lista.Contains(WyburRas.team[i]))
            {
                lista.Add(WyburRas.team[i]);
            }
        }
        if(lista.Count == 1)
        {
            Ending.wygrany = lista[0] + 1;
        }
        return lista.Count == 1;
    }


    static bool czyWygranaControl()
    {
        // Debug.Log(druzynaKontroli + " " + punktyKontroli);
        if(punktyKontroli >= tureKontroli)
        {
            Ending.wygrany = druzynaKontroli + 1;
            return true;
        }
        return false;
    }
    static bool czyWygranaEconomy()
    {
        for(int i = 0; i < 5; i++)
        {
            if(Menu.ratuszPoziom[i] - 1 >= poziomRatusza)
            {
                
                Ending.wygrany = i + 1;
                return true;
            }
        }
        return false;
    }

    static bool czyWygranaBoss()
    {
        if(bossPokonany)
            return true;
        int j = 4;
        if(SimultanTurns.simultanTurns)
            j /= 4;
        Debug.Log((tureDoKonca * j) + " " + (Menu.nrTury - 1));
        if(tureDoKonca * j < Menu.nrTury - 1 && tureDoKonca != 0)
        {
            Ending.wygrany = 0;
            return true;
        }
        return false;
    }

    [PunRPC]
    public void updateStatsMulti(bool a, bool b, bool c, bool d)
    {
        pvp = a;
        control = b;
        economy = c;
        boss = d;
    }
}
