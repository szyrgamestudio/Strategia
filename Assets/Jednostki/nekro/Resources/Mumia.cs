using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mumia : MonoBehaviour
{
    public GameObject jednostka;
    public bool koniec;
    void Update()
    {
        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next && Menu.tura == jednostka.GetComponent<Jednostka>().druzyna)//&& (druzyna+1)%(Menu.IloscGraczy+1) == Menu.tura)
        {
            koniec = false;
            jednostka.GetComponent<Jednostka>().HP = jednostka.GetComponent<Jednostka>().maxHP;
            jednostka.GetComponent<Jednostka>().Aktualizuj();
        }
    }
    
}
