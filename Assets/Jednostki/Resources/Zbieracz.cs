using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zbieracz : MonoBehaviour
{
    private bool koniec;
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

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
        if(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().las == true)
        {
            for(int i=0;i<10;i++)
            {
                if(Menu.bazy[Menu.tura,i]==null)
                    break;
                if(Walka.odleglosc(jednostka,Menu.bazy[Menu.tura,i])<=3 || sprawdzenieTartaka())
                {
                    Menu.drewno[Menu.tura]++;
                    if(sprawdzenieTartaka())
                    {
                        Menu.drewno[Menu.tura]++;
                        DodawanieStat.drewno += 2;
                        jednostka.GetComponent<Jednostka>().ShowDMG(2, new Color(0.6f, 0.4f, 0.2f, 1.0f));
                    }
                    else
                    {
                        jednostka.GetComponent<Jednostka>().ShowDMG(1, new Color(0.6f, 0.4f, 0.2f, 1.0f));
                        DodawanieStat.drewno += 1;
                    }
                        

                           // if(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().las)
                    break;
                }
            }
        }
    }
    private bool sprawdzenieTartaka()
    {
        for(int n = ((int)jednostka.transform.position.x) - 1 ; n <= ((int)jednostka.transform.position.x)+1 ; n++)
            for(int m=((int)jednostka.transform.position.y) - 1 ; m<=((int)jednostka.transform.position.y)+1 ; m++)
            {
                if(Menu.istnieje(n,m) && Menu.kafelki[n][m].GetComponent<Pole>().postac != null)
                {
                    GameObject postac = Menu.kafelki[n][m].GetComponent<Pole>().postac;
                    Tartak tartak = postac.GetComponent<Tartak>();
                    if(tartak != null && 
                    postac.GetComponent<Budynek>().punktyBudowy >= postac.GetComponent<Budynek>().punktyBudowyMax)
                    {
                        return true;
                    }
                }
            }
        return false;
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
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
    
}
