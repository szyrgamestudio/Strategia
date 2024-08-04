using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LucznikSzef : MonoBehaviour
{
    public GameObject jednostka;
    bool akcja1;

    public Sprite[] budynki;
    public string[] teksty;

    int cooldown = 0;
    bool koniec;
    bool buff;

    bool buff2;
    int cooldown2 = 0;

    public Sprite klepsydra;
    public Texture2D customCursorBudowa;


    void Update()
    {
        if(jednostka == Jednostka.Select)
            {
        if(Przycisk.jednostka[0]==true && cooldown == 0)
        {
            Przycisk.jednostka[0]=false;
            Jednostka.wybieranie = true;
            Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
            buff = true;
        } 
        if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && buff && 
                Jednostka.Select2.GetComponent<Jednostka>().sojusz == jednostka.GetComponent<Jednostka>().sojusz && Jednostka.Select2.GetComponent<Jednostka>().zasieg > 2 && Jednostka.Select2 != jednostka)
                {
                    buff = false;
                    Jednostka.Select2.GetComponent<Jednostka>().atak += 3;
                    cooldown = 3;
                    Jednostka.Select2.GetComponent<Buff>().buffP(1, 0, 0,3, 0);
                    Menu.usunSelect2();
                    OnMouseDown();
                }
        if(Przycisk.jednostka[1]==true && cooldown2 == 0)
        {
            Przycisk.jednostka[1]=false;
            Jednostka.wybieranie = true;
            Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
            buff2 = true;
        } 
        if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) == 1 && buff2 && 
                Jednostka.Select2.GetComponent<Jednostka>().sojusz == jednostka.GetComponent<Jednostka>().sojusz && Jednostka.Select2.GetComponent<Jednostka>().zasieg == 1)
                {
                    buff2 = false;
                    Jednostka.Select2.GetComponent<Jednostka>().zasieg += 2;
                    cooldown2 = 3;
                    Jednostka.Select2.GetComponent<Buff>().buffZ(1, 2, 0,0, 0, 0);
                    Menu.usunSelect2();
                    OnMouseDown();
                }
        
        }
        else
        {
            buff = false;
        }
         if(jednostka.GetComponent<Heros>().levelUp)
        {
            jednostka.GetComponent<Heros>().levelUp=false;
            levelUp(jednostka.GetComponent<Heros>().level);
        }
        if(!jednostka.GetComponent<Jednostka>().akcja && akcja1)
        {
            jednostka.GetComponent<Jednostka>().akcja = true;
            akcja1 = false;
        }
        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
        {
            koniec = false;
            if(cooldown > 0)
                cooldown --;
            if(cooldown2 > 0)
                cooldown2 --;
            if(jednostka.GetComponent<Heros>().level >= 4)
                akcja1 = true;
        }
    }

    void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();

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
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            if(cooldown>0)
            {
                Guzikk.CenaMagic.text = cooldown2.ToString(); 
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
                //Guzik.IconMagic.enabled = true;
                if(i < 2)
                {
                    Guzik.IconMagic.enabled = true;
                    Guzik.IconMagic.sprite = klepsydra;
                    Guzik.Opis.text = teksty[i];  
                }
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
                staty.maxdmg += 2;
                staty.maxszybkosc += 2;
                break;
            case 3:
                staty.zdolnosci += 1;
                break;
            case 4:
                staty.zdolnosci += 1;
                akcja1 = true;
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
