using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbazynca : MonoBehaviour
{
    public GameObject jednostka;
    bool akcja1;
    bool akcja2;

    bool koniec;
    // Update is called once per frame
    void Update()
    {
        if(jednostka.GetComponent<Heros>().levelUp)
        {
            jednostka.GetComponent<Heros>().levelUp=false;
            levelUp(jednostka.GetComponent<Heros>().level);
        }
        if(!jednostka.GetComponent<Jednostka>().akcja && akcja1)
        {
            jednostka.GetComponent<Jednostka>().akcja = true;
            akcja1 = false;
        }
        if(!akcja1 && akcja2)
        {
            akcja1 = true;
            akcja2 = false;
        }
        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
        {
            koniec = false;
            Jednostka staty = jednostka.GetComponent<Jednostka>();
            if(staty.zdolnosci >= 1)
                akcja1 = true;
            if(staty.zdolnosci >= 2)
                akcja2 = true;
        }
    }
    private void levelUp(int level)
    {
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.HP += 2;
        staty.maxHP += 2;   
        switch (level){
            case 2:
                staty.zdolnosci += 1;
                break;
            case 3:
                staty.szybkosc += 4;
                staty.maxszybkosc += 4;
                staty.atak += 1;
                break;
            case 4:
                staty.atak += 1;
                staty.obrona += 1;
                staty.mindmg += 1;
                staty.maxdmg += 1;
                break;
            case 5:
                staty.zdolnosci += 1;
                staty.atak += 1;
                break;
        }
    }
}
