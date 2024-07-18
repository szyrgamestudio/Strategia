using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gargulec : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool update;

    void Update()
    {
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        if(Menu.zloto[staty.druzyna] <= 3 && update == false && jednostka.GetComponent<Jednostka>().druzyna != 0)
        {
            update = true;
            jednostka.GetComponent<Jednostka>().Aktualizuj();
            staty.maxdmg = 3;
            staty.mindmg = 3;
        }
        else
        {
            if(update == true)
            {
                update = false;
                jednostka.GetComponent<Jednostka>().Aktualizuj();
                staty.maxdmg = 2;
                staty.mindmg = 1;  
            }
        }
    }
    void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
                        
            for(int i = 0 ; i < 1  ; i++)
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
}
