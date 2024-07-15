using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Charpunnik : MonoBehaviour
{
    public GameObject jednostka;
    public bool odrzut;
    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
        {
            if(Przycisk.jednostka[0] == true && jednostka.GetComponent<Jednostka>().akcja)
            {
                Przycisk.jednostka[0] = false;
                Jednostka.wybieranie = true;
                Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                odrzut = true;
            }

            if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 4 && odrzut)
            {
                odrzut = false;

                Vector3 offset = Jednostka.Select2.transform.position - jednostka.transform.position;
                int x = (int)Jednostka.Select2.transform.position.x;
                int y = (int)Jednostka.Select2.transform.position.y;
                
                if(offset.x > 0 && Menu.istnieje(x-1, y) && Menu.kafelki[x-1][y].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x-1, y);
                }
                else if(offset.x < 0 && Menu.istnieje(x+1, y) && Menu.kafelki[x+1][y].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x+1, y);
                }

                x = (int)Jednostka.Select2.transform.position.x;
                y = (int)Jednostka.Select2.transform.position.y;

                if(offset.y > 0 && Menu.istnieje(x, y-1) && Menu.kafelki[x][y-1].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x, y-1);
                }
                else if(offset.y < 0 && Menu.istnieje(x, y+1) && Menu.kafelki[x][y+1].GetComponent<Pole>().postac == null)
                {
                    MoveJednostka(x, y, x, y+1);
                }

                if(MenuGlowne.multi)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("odrzutMulti", RpcTarget.All, Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, Jednostka.Select2.GetComponent<Jednostka>().druzyna, Jednostka.Select2.transform.position);
                }

                Menu.usunSelect2();
            }
        }
        else
        {
            odrzut = false;
        }
    }

    void MoveJednostka(int oldX, int oldY, int newX, int newY)
    {
        jednostka.GetComponent<Jednostka>().akcja = false;
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().postac = null;
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().Zajete = false; 
        Menu.kafelki[oldX][oldY].GetComponent<Pole>().ZajeteLot = false;
        
        Menu.kafelki[newX][newY].GetComponent<Pole>().postac = Jednostka.Select2;

        if(!Jednostka.Select2.GetComponent<Jednostka>().lata)
            Menu.kafelki[newX][newY].GetComponent<Pole>().Zajete = true;
        else
            Menu.kafelki[newX][newY].GetComponent<Pole>().ZajeteLot = true;

        Jednostka.Select2.transform.position = new Vector3(newX, newY, Jednostka.Select2.transform.position.z);
    }

    [PunRPC]
    void odrzutMulti(int ip, int id, int team, Vector3 pozycja)
    {
        if(ip != Ip.ip)
        {
            Menu.jednostki[team, id].transform.position = pozycja;
        }
    }
}
