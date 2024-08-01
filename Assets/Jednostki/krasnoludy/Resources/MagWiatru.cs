using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class MagWiatru : MonoBehaviour
{
   public GameObject jednostka;
    public bool odrzut;
    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
        {
            if(Przycisk.jednostka[0] == true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=4)
            {
                Przycisk.jednostka[0] = false;
                Menu.magia[Menu.tura]-=4;
                List<GameObject> lista = new List<GameObject>();
                przeszukanie(0,jednostka,lista);
                foreach(GameObject ludek in lista)
                {
                    Debug.Log(ludek.name);
                    Vector3 offset = ludek.transform.position - jednostka.transform.position;
                    int x = (int)ludek.transform.position.x;
                    int y = (int)ludek.transform.position.y;
                    
                    if(offset.x < 0 && Menu.istnieje(x-1, y) && Menu.kafelki[x-1][y].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x-1, y, ludek);
                    }
                    else if(offset.x > 0 && Menu.istnieje(x+1, y) && Menu.kafelki[x+1][y].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x+1, y, ludek);
                    }

                    x = (int)ludek.transform.position.x;
                    y = (int)ludek.transform.position.y;

                    if(offset.y < 0 && Menu.istnieje(x, y-1) && Menu.kafelki[x][y-1].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x, y-1, ludek);
                    }
                    else if(offset.y < 0 && Menu.istnieje(x, y+1) && Menu.kafelki[x][y+1].GetComponent<Pole>().postac == null)
                    {
                        MoveJednostka(x, y, x, y+1, ludek);
                    }

                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("odrzutMulti", RpcTarget.All, Ip.ip, ludek.GetComponent<Jednostka>().nr_jednostki, ludek.GetComponent<Jednostka>().druzyna, ludek.transform.position);
                    }
                    ludek.GetComponent<Jednostka>().HP --;
                    if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, ludek.GetComponent<Jednostka>().nr_jednostki, 1f ,ludek.GetComponent<Jednostka>().druzyna);
                        }

                    Menu.usunSelect2();
                }
            }
            
        }
        else
        {
            odrzut = false;
        }
    }

    [PunRPC]
    public void dmg(int ip, int id, int dmg, int team)
    {
        if(ip != Ip.ip)
        {
            GameObject Oponenet = Menu.jednostki[team,id];
            Oponenet.GetComponent<Jednostka>().HP -= dmg;
        }
    }

    private List<GameObject> przeszukanie(int odleglosc, GameObject NPC, List<GameObject> lista)
    {
       // List<GameObject> lista = new List<GameObject>();
        GameObject help;

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

                        if (czek != null  && czek.HP > 0.1 && !lista.Contains(help) && !help.GetComponent<Wieza>())
                        {
                            lista.Add(help);
                        }
                    }
                }
            }
        }


        if (odleglosc <= 3)
        {
            lista = przeszukanie(odleglosc + 1, NPC, lista);
        }

        return lista;
    }

    void MoveJednostka(int oldX, int oldY, int newX, int newY, GameObject ludek)
    {
        jednostka.GetComponent<Jednostka>().akcja = false;
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().postac = null;
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().Zajete = false; 
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().ZajeteLot = false;
        
        Menu.kafelki[newX][newY].GetComponent<Pole>().postac = ludek;

        if(!ludek.GetComponent<Jednostka>().lata)
            Menu.kafelki[newX][newY].GetComponent<Pole>().Zajete = true;
        else
            Menu.kafelki[newX][newY].GetComponent<Pole>().ZajeteLot = true;

        ludek.transform.position = new Vector3(newX, newY, ludek.transform.position.z);
    }

    [PunRPC]
    void odrzutMulti(int ip, int id, int team, Vector3 pozycja)
    {
        if(ip != Ip.ip)
        {
            MoveJednostka((int)Menu.jednostki[team, id].transform.position.x,(int)Menu.jednostki[team, id].transform.position.y,(int)pozycja.x,(int)pozycja.y,Menu.jednostki[team, id]);
            
            Menu.jednostki[team, id].transform.position = pozycja;
        }
    }
}
