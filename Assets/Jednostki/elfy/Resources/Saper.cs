using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saper : MonoBehaviour
{
 public GameObject jednostka;

    public int cooldown = 0;

    private bool koniec;

    public Sprite klepsydra;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && cooldown == 0)
                    {
                        Przycisk.jednostka[0]=false;
                        //Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<PoleFind>().pulapka = jednostka.GetComponent<Jednostka>().druzyna;
                        Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<PoleFind>().rodzaj = 9;
                        Debug.Log(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].name);
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
}
