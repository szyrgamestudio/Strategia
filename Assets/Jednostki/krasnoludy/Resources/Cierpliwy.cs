using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cierpliwy : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public int cooldown;
    public bool koniec;
    public Sprite klepsydra;

    // Update is called once per frame
    void Update()
    {
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && cooldown == 0 && jednostka.GetComponent<Jednostka>().akcja)
                    {
                        Przycisk.jednostka[0]=false;
                        Menu.zloto[jednostka.GetComponent<Jednostka>().druzyna]++;
                        jednostka.GetComponent<Jednostka>().ShowDMG(1, new Color(1.0f, 1.0f, 0.0f, 1.0f));
                        cooldown = 3;
                        jednostka.GetComponent<Jednostka>().akcja = false;
                        OnMouseDown();
                    }
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
                Guzik.IconMagic.enabled = true;
                Guzik.IconMagic.sprite = klepsydra;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
}
