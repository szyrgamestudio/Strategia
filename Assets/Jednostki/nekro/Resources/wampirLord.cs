using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class wampirLord : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public int cooldown;
    public bool koniec;

    public bool bat;
    public Sprite batArt;
    public Sprite lord;

    void Start()
    {
        jednostka.GetComponent<Wampir>().ignoruj = true;
    }

    void Update()
    { 
        if(jednostka.GetComponent<Heros>().levelUp)
        {
            jednostka.GetComponent<Heros>().levelUp=false;
            levelUp(jednostka.GetComponent<Heros>().level);
        }
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[1]==true && cooldown == 0 && jednostka.GetComponent<Jednostka>().akcja)
                    {
                        Przycisk.jednostka[1]=false;
                        cooldown = 4;
                        Jednostka staty = jednostka.GetComponent<Jednostka>();
                        staty.szybkosc = 2 * staty.maxszybkosc;
                        staty.akcja = false;
                        staty.lata = true;
                        Debug.Log("gang");
                        bat = true;
                        jednostka.GetComponent<SpriteRenderer>().sprite = batArt;
                    }
            }


            if(Menu.Next)
            {
                koniec = true;
            }
            if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
            {
                koniec = false;
                if(bat)
                {
                    Jednostka staty = jednostka.GetComponent<Jednostka>();
                    staty.lata = false;
                    jednostka.GetComponent<SpriteRenderer>().sprite = lord;
                    bat = false;
                }
                if(cooldown != 0)
                    cooldown--;
            }

    }


     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.IconMagic.enabled = true;
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();

            if(cooldown>0)
            {
                Guzikk.CenaMagic.text = cooldown.ToString(); 
                Guzikk.IconMagic.enabled = true;
            }
            else
            {
               Guzikk.CenaMagic.text = "";
               Guzikk.IconMagic.enabled = false;
            }

            
            for(int i = 0 ; i < jednostka.GetComponent<Jednostka>().zdolnosci  ; i++)
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
    private void levelUp(int level)
    {
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.HP += 2;
        staty.maxHP += 2;   
        switch (level){
            case 2:
                staty.atak += 1;
                staty.obrona += 1;
                staty.mindmg += 1;
                staty.maxdmg += 1;
                staty.maxszybkosc += 1;
                jednostka.GetComponent<Wampir>().lifeSteal += 0.1f;
                break;
            case 3:
                staty.zdolnosci += 1;
                staty.maxszybkosc += 1;
                jednostka.GetComponent<Wampir>().lifeSteal += 0.1f;
                break;
            case 4:
                staty.atak += 1;
                staty.obrona += 1;
                staty.maxdmg += 1;
                jednostka.GetComponent<Wampir>().lifeSteal += 0.1f;
                break;
            case 5:
                staty.HP += 2;
                staty.maxHP += 2; 
                jednostka.GetComponent<Wampir>().lifeSteal = 1.1f;
                break;
        }
    }
}
