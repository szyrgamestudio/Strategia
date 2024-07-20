using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Saper : MonoBehaviour
{
 public GameObject jednostka;

    public int cooldown = 0;

    private bool koniec;
    public Sprite[] budynki;
    public string[] teksty;
    

    public Sprite klepsydra;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && cooldown == 0)
                    {
                        Przycisk.jednostka[0]=false;
                        Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<PoleFind>().pulapka = jednostka.GetComponent<Jednostka>().druzyna;
                        Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<PoleFind>().rodzaj = 9;
                        Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<PoleFind>().Start();
                        Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<PoleFind>().updateMultiWywolaj(9, jednostka.GetComponent<Jednostka>().druzyna);
                        Debug.Log(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].name);
                        cooldown = 3;
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
                Guzik.IconMagic.sprite = klepsydra;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
}
