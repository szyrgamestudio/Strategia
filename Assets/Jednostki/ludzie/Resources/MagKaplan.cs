using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MagKaplan : MonoBehaviour
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
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=6)
                    {
                        jednostka.GetComponent<Ramki>().showRamka2();
                        Przycisk.jednostka[0]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        ignis = true;
                    }
                if(Przycisk.jednostka[1]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=4)
                    {
                        jednostka.GetComponent<Ramki>().showRamka2();
                        Przycisk.jednostka[1]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        fireBall = true;
                    }
                if(Przycisk.jednostka[2]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=8)
                    {
                        jednostka.GetComponent<Ramki>().showRamka2();
                        Przycisk.jednostka[2]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        podpalenie = true;
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && ignis)
                {
                    Menu.magia[Menu.tura]-=6;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    ignis = false;
                    Jednostka.Select2.GetComponent<Buff>().buffP(1,0,0,0,3);
                    Jednostka.Select2.GetComponent<Jednostka>().obrona += 3;   
                    Menu.usunSelect2();
                }
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && fireBall)
                {
                    Menu.magia[Menu.tura]-=4;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    fireBall = false;
                    float zwienksz = Jednostka.Select2.GetComponent<Jednostka>().maxdmg - Jednostka.Select2.GetComponent<Jednostka>().mindmg;
                    Jednostka.Select2.GetComponent<Jednostka>().mindmg += zwienksz;
                    Jednostka.Select2.GetComponent<Buff>().buffZ(0,0,0,0,zwienksz,0);
                    Menu.usunSelect2();
                }
                 if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && podpalenie)
                {
                    Menu.magia[Menu.tura]-=8;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    podpalenie = false;
                    float pomocMulti = Jednostka.Select2.GetComponent<Jednostka>().HP - Jednostka.Select2.GetComponent<Jednostka>().maxHP * (float)1.1;
                    Jednostka.Select2.GetComponent<Jednostka>().HP = Jednostka.Select2.GetComponent<Jednostka>().maxHP * (float)1.1;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, pomocMulti,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                    }
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
            Guzikk.CenaMagic.text = "6"; 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "4"; 
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "8"; 
            
            
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
