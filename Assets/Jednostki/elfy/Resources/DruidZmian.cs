using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DruidZmian : MonoBehaviour
{
    public GameObject jednostka;
    public Sprite[] ciala;
    public bool zmieniony;

    public Sprite[] budynki;
    public string[] teksty;

    public int cooldown;
    float HPdruida;
    public PhotonView photonView;
    
    void Update()
    {
        if(jednostka == Jednostka.Select)
        {
            if(Przycisk.jednostka[0]==true  &&  zmieniony )
            {
                Przycisk.jednostka[0]=false;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 0 , Ip.ip);
                zmianaCofka();
                OnMouseDown();
            }
            if(Przycisk.jednostka[0]==true  && jednostka.GetComponent<Jednostka>().akcja && !zmieniony && Menu.magia[Menu.tura]>=3)
            {
                Przycisk.jednostka[0]=false;
                Menu.magia[Menu.tura] -= 3;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 1 , Ip.ip);
                wilk();
                OnMouseDown();
            }
            if(Przycisk.jednostka[1]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=3)
            {
                Przycisk.jednostka[1]=false;
                Menu.magia[Menu.tura] -= 3;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 2 , Ip.ip);
                jaskolka();
                OnMouseDown();
            }
            if(Przycisk.jednostka[2]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=6)
            {
                Przycisk.jednostka[2]=false;
                Menu.magia[Menu.tura] -= 6;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 3 , Ip.ip);
                niedzwiedz();
                OnMouseDown();
            }
        }
    }
    void niedzwiedz()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[3];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.akcja = false;
        HPdruida = staty.HP;
        staty.HP = 8;
        staty.maxHP = 8;
        staty.atak = 4;
        staty.obrona = 4;
        staty.mindmg = 3;
        staty.maxdmg = 4;
        staty.zasieg = 1;
        staty.zdolnosci = 1;
        staty.maxszybkosc = 6;
        staty.szybkosc = 6;
        staty.nazwa = "Niedźwiedź";
        staty.lata = true;
        zmieniony = true;
    } 
    void jaskolka()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[2];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        HPdruida = staty.HP;
        staty.HP = 1;
        staty.maxHP = 1;
        staty.atak = 1;
        staty.akcja = false;
        staty.obrona = 1;
        staty.mindmg = 1;
        staty.maxdmg = 3;
        staty.zasieg = 1;
        
        staty.maxszybkosc = 6;
        staty.szybkosc = 6;
        staty.nazwa = "Jaskółka";
        staty.lata = true;
        zmieniony = true;
    } 
    void wilk()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[1];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        HPdruida = staty.HP;
        staty.HP = 6;
        staty.maxHP = 6;
        staty.atak = 1;
        staty.obrona = 2;
        staty.akcja = false;
        staty.mindmg = 1;
        staty.maxdmg = 3;
        staty.zasieg = 1;
        staty.zdolnosci = 1;
        staty.maxszybkosc = 8;
        staty.szybkosc = 8;
        staty.nazwa = "Wilk";
        staty.lata = false;
        zmieniony = true;
    } 

    void zmianaCofka()
    {
        budynki[0] = ciala[1];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[0];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.HP = HPdruida;
        staty.maxHP = 5;
        staty.atak = 2;
        staty.obrona = 2;
        staty.mindmg = 2;
        staty.maxdmg = 2;
        staty.zasieg = 3;
        staty.zdolnosci = 3;
        staty.maxszybkosc = 6;
        staty.nazwa = "Zmiennokształtny druid";
        staty.szybkosc = 6;
        staty.lata = false;
        zmieniony = false;
    } 
     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            if(zmieniony)
                Guzikk.CenaMagic.text = "0"; 
            else
                Guzikk.CenaMagic.text = "3"; 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "3"; 
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "6"; 
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

        [PunRPC]
    public void multiPrzemiana(int nr, int ip)
    {
        if(Ip.ip != ip)
            switch(nr)
            {
                case 0: zmianaCofka(); break;
                case 1: wilk(); break;
                case 2: jaskolka(); break;
                case 3: niedzwiedz(); break;
            }
    }


}
