using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoombie : Jednostka
{
    public Sprite[] budynki;
    public string[] teksty;

    public override void umieranie()
    {
        if(zdolnosci == 1)
        {
            zdolnosci = 0;
            HP = 1;
        }
        else
        {
            base.umieranie();
        }
    }

    void OnMouseDown()
    {
        base.OnMouseDown();
        if(jednostka == Jednostka.Select)
        {
            Debug.Log("w mordzie");
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
        Debug.Log("w lalal");
    }
}
