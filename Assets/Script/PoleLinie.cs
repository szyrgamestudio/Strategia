using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoleLinie : MonoBehaviour
{
    public GameObject Nin;
    public GameObject Nout;
    public Sprite skos;
    public Sprite bok;
    public Sprite puste;
    public Sprite Greenskos;
    public Sprite Greenbok;
    public GameObject kafelek;


    void Update()
    {
        // if(!Menu.NIERUSZAC)
        // {
        int kafelekNin = kafelek.GetComponent<Pole>().Nin;

        if (kafelekNin != 0)
        {
            if (kafelekNin % 2 == 0)
            {
                
                if(kafelek.GetComponent<Pole>().CzasDrogi-1 < Pole.pomocnicza[2])
                {
                    Nin.GetComponent<SpriteRenderer>().sprite = Greenbok;
                }
                else{
                    Nin.GetComponent<SpriteRenderer>().sprite = bok;
                }
            }
            else
            {
                if(kafelek.GetComponent<Pole>().CzasDrogi <= Pole.pomocnicza[2])
                {
                    Nin.GetComponent<SpriteRenderer>().sprite = Greenskos;
                }
                else{
                    Nin.GetComponent<SpriteRenderer>().sprite = skos;
                }
            }

            switch (kafelekNin)
            {
                case 1:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case 2:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 3:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 4:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 5:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 6:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 7:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 8:
                    Nin.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
            
        }
        else
        {
            Nin.GetComponent<SpriteRenderer>().sprite = puste;
        }
        int kafelekNout = kafelek.GetComponent<Pole>().Nout;

        if (kafelekNout != 0)
        {
            if (kafelekNout % 2 == 0)
            {
                
                if(kafelek.GetComponent<Pole>().CzasDrogi <= Pole.pomocnicza[2])
                {
                    Nout.GetComponent<SpriteRenderer>().sprite = Greenbok;
                }
                else{
                    Nout.GetComponent<SpriteRenderer>().sprite = bok;
                }
            }
            else
            {
                if(kafelek.GetComponent<Pole>().CzasDrogi < Pole.pomocnicza[2])
                {
                    Nout.GetComponent<SpriteRenderer>().sprite = Greenskos;
                }
                else{
                    Nout.GetComponent<SpriteRenderer>().sprite = skos;
                }
            }

            switch (kafelekNout)
            {
                case 1:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case 2:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 3:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 4:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 5:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 6:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 7:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 8:
                    Nout.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
        }
        else
        {
            Nout.GetComponent<SpriteRenderer>().sprite = puste;
        }
       // }
    }
}
