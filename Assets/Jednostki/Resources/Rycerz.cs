using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Rycerz : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool odrzut;

    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja)
                    {
                        Przycisk.jednostka[0]=false;
                        jednostka.GetComponent<Jednostka>().obrona += 3;
                        jednostka.GetComponent<Jednostka>().akcja = false;
                        jednostka.GetComponent<Buff>().buffP(0,0,0,0,3f);
                    }
                if(Przycisk.jednostka[1]==true && jednostka.GetComponent<Jednostka>().akcja)
                    {
                        Przycisk.jednostka[1]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        odrzut = true;
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) == 1 && odrzut)
                {
                    odrzut = false;
                    Jednostka.Select2.GetComponent<Jednostka>().zaatakowanie(jednostka);

                    Vector3 offset = Jednostka.Select2.transform.position - jednostka.transform.position;


                        if(!Menu.kafelki[(int)(Jednostka.Select2.transform.position.x + offset.x)][(int)(Jednostka.Select2.transform.position.y + offset.y)].GetComponent<Pole>().Zajete)
                        {
                            Menu.kafelki[(int)(Jednostka.Select2.transform.position.x + offset.x)][(int)(Jednostka.Select2.transform.position.y + offset.y)].GetComponent<Pole>().Zajete=true;
                            Menu.kafelki[(int)(Jednostka.Select2.transform.position.x + offset.x)][(int)(Jednostka.Select2.transform.position.y + offset.y)].GetComponent<Pole>().postac =
                            Menu.kafelki[(int)(Jednostka.Select2.transform.position.x)][(int)(Jednostka.Select2.transform.position.y)].GetComponent<Pole>().postac;
                            Menu.kafelki[(int)(Jednostka.Select2.transform.position.x)][(int)(Jednostka.Select2.transform.position.y)].GetComponent<Pole>().postac = null;
                            Menu.kafelki[(int)(Jednostka.Select2.transform.position.x)][(int)(Jednostka.Select2.transform.position.y)].GetComponent<Pole>().Zajete = false;
                            Jednostka.Select2.transform.position = new Vector3(Jednostka.Select2.transform.position.x + offset.x, Jednostka.Select2.transform.position.y + offset.y, Jednostka.Select2.transform.position.z);
                        }
                        else
                        {
                            GameObject drugi = Menu.kafelki[(int)(Jednostka.Select2.transform.position.x + offset.x)][(int)(Jednostka.Select2.transform.position.y + offset.y)].GetComponent<Pole>().postac;
                            Debug.Log(drugi.name);
                            Jednostka Atakujacy = drugi.GetComponent<Jednostka>();
                            jednostka.GetComponent<Jednostka>().akcja = true;
                            jednostka.GetComponent<Jednostka>().zasieg += 1;
                            if(Atakujacy != null)
                            {
                                Atakujacy.zaatakowanie(jednostka);
                            }
                            else
                            {
                                drugi.GetComponent<Budynek>().zaatakowanie();
                            }
                            jednostka.GetComponent<Jednostka>().akcja = false;
                            jednostka.GetComponent<Jednostka>().zasieg -= 1;
                        }   
                    Menu.usunSelect2();
                }
            }
            else
            {
                odrzut = false;
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
        }
    }
}
