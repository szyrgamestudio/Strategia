using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Medyk : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool leczenie;

    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja)
                    {
                        Przycisk.jednostka[0]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        leczenie = true;
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) == 1 && leczenie)
                {
                    leczenie = false;
                    Jednostka.Select2.GetComponent<Jednostka>().HP += 3;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, -3,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                    }
                    Jednostka.Select2.GetComponent<Jednostka>().ShowDMG(3f,new Color(0.0f, 1.0f, 0.0f, 1.0f));
                    Menu.usunSelect2();
                }
            }
            else
            {
                leczenie = false;
            }
    }

    [PunRPC]
    public void dmg(int ip, int id, int dmg, int team)
    {
        if(ip != Ip.ip)
        {
            GameObject Oponenet = Menu.jednostki[team,id];
            Debug.Log(Oponenet.name);
            Oponenet.GetComponent<Jednostka>().HP -= dmg;
        }
    }

     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            
            
            for(int i = 0 ; i < jednostka.GetComponent<Jednostka>().zdolnosci  ; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
}
