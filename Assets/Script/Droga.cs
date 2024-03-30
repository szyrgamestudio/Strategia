using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Droga : MonoBehaviour
{
    public bool droga;
    public Sprite[] drogaArt;
    public GameObject kafelek;

    public void updateDroga(int i)
    {
        int x = (int)kafelek.transform.position.x;
        int y = (int)kafelek.transform.position.y;
        int rodzaj = 0;
        droga = true;
        if(Menu.istnieje(x+1,y) && Menu.kafelki[x+1][y].GetComponent<Droga>().droga && Menu.kafelki[x+1][y].GetComponent<Pole>().poziom == kafelek.GetComponent<Pole>().poziom)
        {
            rodzaj += 2;
            if(i==1 )
                Menu.kafelki[x+1][y].GetComponent<Droga>().updateDroga(0);
        }
        if(Menu.istnieje(x,y+1) && Menu.kafelki[x][y+1].GetComponent<Droga>().droga  && Menu.kafelki[x][y+1].GetComponent<Pole>().poziom == kafelek.GetComponent<Pole>().poziom)
        {
            rodzaj += 1;
            if(i==1)
                Menu.kafelki[x][y+1].GetComponent<Droga>().updateDroga(0);
        }
        if(Menu.istnieje(x-1,y) && Menu.kafelki[x-1][y].GetComponent<Droga>().droga && Menu.kafelki[x-1][y].GetComponent<Pole>().poziom == kafelek.GetComponent<Pole>().poziom)
        {
            rodzaj += 8;
            if(i==1 )
                Menu.kafelki[x-1][y].GetComponent<Droga>().updateDroga(0);
        }
        if(Menu.istnieje(x,y-1) && Menu.kafelki[x][y-1].GetComponent<Droga>().droga && Menu.kafelki[x][y-1].GetComponent<Pole>().poziom == kafelek.GetComponent<Pole>().poziom)
        {
            rodzaj += 4;
            if(i==1 )
                Menu.kafelki[x][y-1].GetComponent<Droga>().updateDroga(0);
        }
        kafelek.GetComponent<SpriteRenderer>().sprite = drogaArt[rodzaj];
    }
}
