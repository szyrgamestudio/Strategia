using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DuchLasu : Jednostka
{
    public static bool duch;

    public Sprite[] budynki;
    public string[] teksty;

    public static int cenaD = 2;
    public static int drewnoD = 2;

    public override void umieranie()
    {
        duch = false;
        cenaD *= 2;
        drewnoD *= 2;
        Debug.Log("DuchLasu");
        base.umieranie();
    }

    void OnMouseDown()
    {
        base.OnMouseDown();
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
                        
            for(int i = 0 ; i < zdolnosci  ; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.IconMagic.enabled = false;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
     [PunRPC]
    void ZaktualizujStatystykiRPC(int druzyna, int sojusz, float HP, float maxHP, float atak, float obrona, int zasieg, int maxszybkosc,
                                   int szybkosc, float mindmg, float maxdmg, int zdolnosci, bool zbieracz, bool lata, int cena,
                                   string nazwa, bool akcja, int nr_jednostki, bool koniec, int ip)
    {
        if(ip != Ip.ip)
        {
        this.druzyna = druzyna;
        this.sojusz = sojusz;
        this.HP = HP;
        this.maxHP = maxHP;
        this.atak = atak;
        this.obrona = obrona;
        this.zasieg = zasieg;
        this.maxszybkosc = maxszybkosc;
        this.szybkosc = szybkosc;
        this.mindmg = mindmg;
        this.maxdmg = maxdmg;
        this.zdolnosci = zdolnosci;
        this.zbieracz = zbieracz;
        this.lata = lata;
        this.cena = cena;
        this.nazwa = nazwa;
        this.akcja = akcja;
        this.nr_jednostki = nr_jednostki;
        this.koniec = koniec;
        poczatek = true;
            switch(druzyna)
            {
                case 0: obramowka.color = new Color(0.0f, 0.0f, 0.0f); break;
                case 1: obramowka.color = new Color(1.0f, 0.0f, 0.0f); break;
                case 2: obramowka.color = new Color(0.0f, 1.0f, 0.0f); break;
                case 3: obramowka.color = new Color(0.0f, 0.0f, 1.0f); break;
                case 4: obramowka.color = new Color(1.0f, 1.0f, 0.0f); break;
            }
        }
    }
}
