using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Biegacz : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool leczenie;
    public int cooldown = 0;

    private bool koniec;

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
                        leczenie = true;
                        OnMouseDown();
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) == 1 && leczenie)
                {
                    leczenie = false;
                    Jednostka.Select2.GetComponent<Jednostka>().szybkosc += 3;
                    cooldown = 2;
                    Menu.usunSelect2();
                    OnMouseDown();
                }
            }
            else
            {
                leczenie = false;
            }
            if(Menu.Next)
            {
                koniec = true;
            }
            if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
            {
                koniec = false;
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
                Guzik.IconMagic.sprite = klepsydra;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
}
