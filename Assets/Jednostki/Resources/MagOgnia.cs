using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MagOgnia : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool ignis;
    public bool fireBall;
    public bool podpalenie;

    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=4)
                    {
                        jednostka.GetComponent<Ramki>().showRamka2();
                        Przycisk.jednostka[0]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        ignis = true;
                    }
                if(Przycisk.jednostka[1]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=7)
                    {
                        jednostka.GetComponent<Ramki>().showRamka2();
                        Przycisk.jednostka[1]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        fireBall = true;
                    }
                if(Przycisk.jednostka[2]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=4)
                    {
                        jednostka.GetComponent<Ramki>().showRamka2();
                        Przycisk.jednostka[2]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        podpalenie = true;
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && ignis)
                {
                    ignis = false;
                    Menu.magia[Menu.tura]-=4;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    Jednostka.Select2.GetComponent<Jednostka>().HP -= 4;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, 4,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                    }
                    Jednostka.Select2.GetComponent<Jednostka>().ShowDMG(4f,new Color(1.0f, 0.0f, 0.0f, 1.0f));
                    Menu.usunSelect2();
                }
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && fireBall)
                {
                    fireBall = false;
                    Menu.magia[Menu.tura]-=7;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    for(int i = -1; i < 2 ; i++)
                        for(int j = -1; j < 2 ; j++)
                            if(Menu.kafelki[(int)Jednostka.Select2.transform.position.x + i][(int)Jednostka.Select2.transform.position.y + j].GetComponent<Pole>().postac != null)
                            {
                                GameObject postka = Menu.kafelki[(int)Jednostka.Select2.transform.position.x + i][(int)Jednostka.Select2.transform.position.y + j].GetComponent<Pole>().postac;
                                postka.GetComponent<Jednostka>().HP -= 3;
                                if(MenuGlowne.multi)
                                {
                                    PhotonView photonView = GetComponent<PhotonView>();
                                    photonView.RPC("dmg", RpcTarget.All,Ip.ip, postka.GetComponent<Jednostka>().nr_jednostki, 3,postka.GetComponent<Jednostka>().druzyna);
                                }
                                postka.GetComponent<Jednostka>().ShowDMG(3f,new Color(1.0f, 0.0f, 0.0f, 1.0f));
                            }
                    Menu.usunSelect2();
                }
                 if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && podpalenie)
                {
                    podpalenie = false;
                    Menu.magia[Menu.tura]-=4;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("podpalenieMulti", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                    }
                    Jednostka.Select2.GetComponent<Jednostka>().szybkosc += 3;
                    Jednostka.Select2.GetComponent<Buff>().buffP(0,2f,0,0,0);
                    Jednostka.Select2.GetComponent<Buff>().buffP(1,2f,0,0,0);
                    Jednostka.Select2.GetComponent<Buff>().buffP(2,2f,0,0,0);
                    Menu.usunSelect2();
                }
                    if (Input.GetMouseButtonDown(1))
                {
                    jednostka.GetComponent<Ramki>().Start();
                }
            }
            else
            {
                ignis = false;
                fireBall = false;
            }
    }

    [PunRPC]
    public void podpalenieMulti(int ip, int id, int team)
    {
        if(ip != Ip.ip)
        {
            GameObject Oponenet = Menu.jednostki[team,id];
            Oponenet.GetComponent<Jednostka>().szybkosc += 3;
            Oponenet.GetComponent<Buff>().buffP(0,2f,0,0,0);
            Oponenet.GetComponent<Buff>().buffP(1,2f,0,0,0);
            Oponenet.GetComponent<Buff>().buffP(2,2f,0,0,0);
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
            Guzikk.CenaMagic.text = "4"; 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "7"; 
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "4"; 
            
            
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
