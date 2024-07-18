using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tancerz : MonoBehaviour
{
    public GameObject jednostka;
    bool akcja1;

    bool koniec;
    void Update()
    {
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
            Jednostka staty = jednostka.GetComponent<Jednostka>();
            akcja1 = true;
        }
    }
}
