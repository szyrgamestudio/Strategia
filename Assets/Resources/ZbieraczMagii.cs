using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZbieraczMagii : MonoBehaviour
{
    private bool koniec;
    public GameObject jednostka;

    void Update()
    {
        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
        {
            koniec = false;
            KoniecTury();
        }
    }

    private void KoniecTury()
    {
        switch(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().magia)
        {
            case 2: Menu.magia[Menu.tura]+=1 ; jednostka.GetComponent<Jednostka>().ShowDMG(1, new Color(1.0f, 0.0f, 1.0f, 1.0f)); DodawanieStat.magia += 1; break;
            case 3: Menu.magia[Menu.tura]+=3 ; jednostka.GetComponent<Jednostka>().ShowDMG(3, new Color(1.0f, 0.0f, 1.0f, 1.0f)); DodawanieStat.magia += 3;break; 
        }
        
    }
    
}
