using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Kaplanran : MonoBehaviour
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
                if(Przycisk.jednostka[0]==true)
                    {
                        Przycisk.jednostka[0]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        leczenie = true;
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) == 1 && leczenie && 
                Jednostka.Select2.GetComponent<Jednostka>().sojusz == jednostka.GetComponent<Jednostka>().sojusz)
                {
                    leczenie = false;
                    float rozniaca = Jednostka.Select2.GetComponent<Jednostka>().maxHP - Jednostka.Select2.GetComponent<Jednostka>().HP;
                    if(rozniaca < Menu.magia[Menu.tura] / 2)
                    {
                        Jednostka.Select2.GetComponent<Jednostka>().HP += rozniaca;
                        if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, -rozniaca,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                        }
                        Jednostka.Select2.GetComponent<Jednostka>().ShowDMG(rozniaca,new Color(0.0f, 1.0f, 0.0f, 1.0f));
                        Menu.magia[Menu.tura] -= (int)rozniaca /2;
                    }
                    else
                    {
                        Jednostka.Select2.GetComponent<Jednostka>().HP += (float)Menu.magia[Menu.tura] * 2;
                        if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, -(float)Menu.magia[Menu.tura] * 2,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                        }
                        Jednostka.Select2.GetComponent<Jednostka>().ShowDMG((float)Menu.magia[Menu.tura]*2,new Color(0.0f, 1.0f, 0.0f, 1.0f));
                        Menu.magia[Menu.tura] = 0;
                    }

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
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.IconMagic.enabled = true;

            
            for(int i = 0 ; i < jednostka.GetComponent<Jednostka>().zdolnosci  ; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
}
