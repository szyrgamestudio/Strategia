using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Zongler : MonoBehaviour
{
    public GameObject jednostka;
    public bool ignis;

    public Texture2D customCursorBudowa;

    void Update()
    {
    if(jednostka == Jednostka.Select)
        {
            if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja)
                {
                    jednostka.GetComponent<Ramki>().showRamka3();
                    Przycisk.jednostka[0]=false;
                    Jednostka.wybieranie = true;
                    Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                    ignis = true;
                }
            if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && ignis)
            {
                ignis = false;
                jednostka.GetComponent<Jednostka>().akcja = false;

                List<GameObject> lista = przeszukanie(Jednostka.Select2);
                foreach(GameObject ludek in lista)
                {
                    ludek.GetComponent<Jednostka>().HP -= 2.5f;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("dmg", RpcTarget.All,Ip.ip, ludek.GetComponent<Jednostka>().nr_jednostki, 2.5f ,ludek.GetComponent<Jednostka>().druzyna);
                    }
                    ludek.GetComponent<Jednostka>().ShowDMG(4f,new Color(1.0f, 0.0f, 0.0f, 1.0f));
                    Menu.usunSelect2();
                }
            }
        }
    }

    private  List<GameObject> przeszukanie(GameObject NPC)
    {
        List<GameObject> lista = new List<GameObject>();
         GameObject help;
        int odleglosc = 2;
        for (int j = -odleglosc; j <= odleglosc; j++)
        {
            for (int i = -odleglosc; i <= odleglosc; i++)
            {
                int newX = (int)(NPC.transform.position.x + i);
                int newY = (int)(NPC.transform.position.y + j);

                if (Mathf.Abs(i) + Mathf.Abs(j) < odleglosc &&
                    newX >= 0 && newX <= (Menu.BoardSizeX - 1) &&
                    newY >= 0 && newY <= (Menu.BoardSizeY - 1))
                {
                    var pole = Menu.kafelki[newX][newY].GetComponent<Pole>();

                    if (pole.postac != null)
                    {
                        help = pole.postac;
                        Jednostka czek = help.GetComponent<Jednostka>();

                        if (czek != null  && czek.HP > 0.1 && !lista.Contains(help))
                        {
                            lista.Add(help);
                        }
                    }
                }
            }
        }
        Debug.Log(lista.Count);
        return lista;
    }

    [PunRPC]
    public void dmg(int ip, int id, float dmg, int team)
    {
        if(ip != Ip.ip)
        {
            GameObject Oponenet = Menu.jednostki[team,id];
            Debug.Log(Oponenet.name);
            Oponenet.GetComponent<Jednostka>().HP -= dmg;
        }
    }
}
