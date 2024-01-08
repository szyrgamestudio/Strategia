using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kusznik : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    void Start()
    {
    }



    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja)
                    {
                        Przycisk.jednostka[0]=false;
                        jednostka.GetComponent<Jednostka>().akcja = false;
                        float zwienksz = jednostka.GetComponent<Jednostka>().maxdmg - jednostka.GetComponent<Jednostka>().mindmg;
                        jednostka.GetComponent<Buff>().buffZ(1,1,-4,4,zwienksz,0);
                        jednostka.GetComponent<Buff>().buffZ(0,-1,4,4,-zwienksz,0);
                    }
            }

    }
     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            
            
            for(int i = 0 ; i < jednostka.GetComponent<Jednostka>().zdolnosci  ; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
}
