using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Wampir : MonoBehaviour
{
    public float lifeSteal = 0.5f;
    public Sprite[] budynki;
    public string[] teksty;
    public GameObject jednostka;
    public bool ignoruj;

    void OnMouseDown()
    {
        if(jednostka == Jednostka.Select && !ignoruj)
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
