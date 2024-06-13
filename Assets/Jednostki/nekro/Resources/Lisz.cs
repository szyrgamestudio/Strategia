using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Lisz : MonoBehaviour
{

    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true)
                    {
                        Przycisk.jednostka[0]=false;
                        Interface.interfaceStatic.GetComponent<Interface>().Brak(0 , 0 , 3, false);
                        if(Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] >= 3)
                        {
                            Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] -= 3;
                            int tx = (int)jednostka.transform.position.x ;
                            int ty = (int)jednostka.transform.position.y ;
                            for(int x = -3; x <= 3; x++)
                                for(int y = -3; y <= 3; y++)
                                {
                                    int a = x;
                                    int b = y;
                                    if(Math.Abs(a) + Math.Abs(b) < 4)
                                    {
                                        if(Menu.istnieje(x+tx,y+ty) && Menu.kafelki[x+tx][y+ty].GetComponent<Pole>().postac != null && Menu.kafelki[x+tx][y+ty].GetComponent<Pole>().postac.GetComponent<Skeleton>())
                                        {
                                            Jednostka staty = Menu.kafelki[x+tx][y+ty].GetComponent<Pole>().postac.GetComponent<Jednostka>();
                                            GameObject szkielet = Menu.kafelki[x+tx][y+ty].GetComponent<Pole>().postac;
                                            staty.atak++;
                                            staty.obrona++;
                                            staty.maxszybkosc++;
                                            staty.szybkosc++;
                                            staty.mindmg++;
                                            staty.maxdmg++;
                                            szkielet.GetComponent<Buff>().buffP(0,0,0,1f,1f);
                                            szkielet.GetComponent<Buff>().buffZ(0, 0, 1,1, 1, 1);
                                            staty.Aktualizuj();
                                        }
                                    }
                                }
                        }
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
            
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.IconMagic.enabled = true;
            Guzikk.CenaMagic.text = "3"; 
        }
    }
}


