using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ArcyDruid : MonoBehaviour
{
    public GameObject jednostka;
    public Sprite[] ciala;
    public bool zmieniony;

    public Sprite[] budynki;
    public string[] teksty;
    public PhotonView photonView;

    float HPdruida;

    public int zdolnosci = 3;
    
    void Update()
    {
        if(jednostka.GetComponent<Heros>().levelUp)
        {
            jednostka.GetComponent<Heros>().levelUp=false;
            levelUp(jednostka.GetComponent<Heros>().level);
        }
        if(jednostka == Jednostka.Select)
        {
            if(Przycisk.jednostka[0]==true && zmieniony )
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
                Menu.magia[Menu.tura]-=3;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 1 , Ip.ip);
                wilk();
                OnMouseDown();
            }
            if(Przycisk.jednostka[1]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=3)
            {
                Przycisk.jednostka[1]=false;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 2 , Ip.ip);
                Menu.magia[Menu.tura]-=3;
                pająk();
                OnMouseDown();
            }
            if(Przycisk.jednostka[2]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=1)
            {
                Przycisk.jednostka[2]=false;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 3 , Ip.ip);
                Menu.magia[Menu.tura]-=1;
                szczur();
                OnMouseDown();
            }
            if(Przycisk.jednostka[3]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=3)
            {
                Przycisk.jednostka[3]=false;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 4 , Ip.ip);
                Menu.magia[Menu.tura]-=3;
                jaskolka();
                OnMouseDown();
            }
            if(Przycisk.jednostka[4]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=5)
            {
                Przycisk.jednostka[4]=false;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 5 , Ip.ip);
                Menu.magia[Menu.tura]-=5;
                niedzwiedz();
                OnMouseDown();
            }
            if(Przycisk.jednostka[5]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=5)
            {
                Przycisk.jednostka[5]=false;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 6 , Ip.ip);
                Menu.magia[Menu.tura]-=5;
                gryf();
                OnMouseDown();
            }
            if(Przycisk.jednostka[6]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=7)
            {
                Przycisk.jednostka[6]=false;
                if(MenuGlowne.multi)
                    photonView.RPC("multiPrzemiana", RpcTarget.Others, 7 , Ip.ip);
                Menu.magia[Menu.tura]-=7;
                duchLasu();
                OnMouseDown();
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
                case 2: pająk(); break;
                case 3: szczur(); break;
                case 4: jaskolka(); break;
                case 5: niedzwiedz(); break;
                case 6: gryf(); break;
                case 7: duchLasu(); break;

            }
    }

    void niedzwiedz()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[3];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.akcja = false;
        HPdruida = staty.HP;
        staty.HP = 8 + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 8 + jednostka.GetComponent<Heros>().level * 2;
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
        staty.HP = 1 + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 1 + jednostka.GetComponent<Heros>().level * 2;
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
        staty.HP = 6 + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 6 + jednostka.GetComponent<Heros>().level * 2;
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

    void szczur()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[4];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        HPdruida = staty.HP;
        staty.HP = 1 + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 1 + jednostka.GetComponent<Heros>().level * 2;
        staty.atak = 1;
        staty.obrona = 1;
        staty.akcja = false;
        staty.mindmg = 1;
        staty.maxdmg = 2;
        staty.zasieg = 1;
        staty.zdolnosci = 1;
        staty.maxszybkosc = 6;
        staty.szybkosc = 6;
        staty.nazwa = "Szczur";
        staty.lata = false;
        zmieniony = true;
    } 
    void pająk()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[5];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        HPdruida = staty.HP;
        staty.HP = 1 + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 1 + jednostka.GetComponent<Heros>().level * 2;
        staty.atak = 2;
        staty.obrona = 2;
        staty.akcja = false;
        staty.mindmg = 1;
        staty.maxdmg = 3;
        staty.zasieg = 1;
        staty.zdolnosci = 1;
        staty.maxszybkosc = 8;
        staty.szybkosc = 8;
        staty.nazwa = "Wielki Pająk";
        staty.lata = false;
        zmieniony = true;
    } 
    void duchLasu()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[7];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        HPdruida = staty.HP;
        staty.HP = 7 + + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 7 + jednostka.GetComponent<Heros>().level * 2;
        staty.atak = 3;
        staty.obrona = 5;
        staty.akcja = false;
        staty.mindmg = 2;
        staty.maxdmg = 4;
        staty.zasieg = 1;
        staty.zdolnosci = 1;
        staty.maxszybkosc = 8;
        staty.szybkosc = 8;
        staty.nazwa = "Duch Lasu";
        staty.lata = false;
        zmieniony = true;
    } 
    void gryf()
    {
        budynki[0] = ciala[0];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[6];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        HPdruida = staty.HP;
        staty.HP = 6 + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 6 + jednostka.GetComponent<Heros>().level * 2;
        staty.atak = 3;
        staty.obrona = 3;
        staty.akcja = false;
        staty.mindmg = 2;
        staty.maxdmg = 3;
        staty.zasieg = 1;
        staty.zdolnosci = 1;
        staty.maxszybkosc = 8;
        staty.szybkosc = 8;
        staty.nazwa = "Gryf";
        staty.lata = true;
        zmieniony = true;
    } 


    void zmianaCofka()
    {
        budynki[0] = ciala[1];
        jednostka.GetComponent<SpriteRenderer>().sprite = ciala[0];
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.HP = HPdruida + jednostka.GetComponent<Heros>().level * 2;
        staty.maxHP = 5 + jednostka.GetComponent<Heros>().level * 2;
        staty.atak = 2;
        staty.obrona = 2;
        staty.mindmg = 2;
        staty.maxdmg = 2;
        staty.zasieg = 3;
        staty.zdolnosci = zdolnosci;
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
            Guzikk.CenaMagic.text = "1"; 
            Guzikk = InterfaceUnit.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "3"; 
            Guzikk = InterfaceUnit.przyciski[4].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "5"; 
            Guzikk = InterfaceUnit.przyciski[5].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "5";
            Guzikk = InterfaceUnit.przyciski[6].GetComponent<PrzyciskInter>();
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
     private void levelUp(int level)
    {
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.HP += 2;
        staty.maxHP += 2;   
        switch (level){
            case 2:
                staty.zdolnosci += 2;
                zdolnosci += 2;
                break;
            case 3:
                staty.zdolnosci += 1;
                zdolnosci += 1;
                break;
            case 4:
                staty.zdolnosci += 1;
                zdolnosci += 1;
                break;
            case 5:
                staty.atak += 2;
                staty.obrona += 1;
                staty.maxdmg += 2;
                staty.maxszybkosc += 2;
                break;
        }
    }


}
