using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kawalerzysta : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool leczenie;

    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja)
                    {
                        Przycisk.jednostka[0]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        leczenie = true;
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= jednostka.GetComponent<Jednostka>().szybkosc / 2 && leczenie)
                {
                    leczenie = false;
                    bool bieg = true;
                    float wzrostSily = 0;
                    Vector3 offset = Jednostka.Select2.transform.position - jednostka.transform.position;
                    Vector3 pozycja = jednostka.transform.position;
                    if(offset.x == 0)
                    {
                        if(offset.y>0)
                        {
                            for(float i = 1; i < offset.y; i++)
                            {
                                if(Menu.kafelki[(int)pozycja.x][(int)(pozycja.y + i)].GetComponent<Pole>().Zajete)
                                {
                                    bieg = false;
                                }
                                wzrostSily++;
                            }
                            if(bieg)
                            {
                                StartCoroutine(Rozpend(1f, wzrostSily));

                            }
                        }else{
                            for(float i = -1; i > offset.y; i--)
                            {
                                if(Menu.kafelki[(int)pozycja.x][(int)(pozycja.y + i)].GetComponent<Pole>().Zajete)
                                {
                                    bieg = false;
                                }
                                wzrostSily++;
                            }
                            if(bieg)
                            {
                                StartCoroutine(Rozpend(-1f, wzrostSily));    
                            }
                        }
                    }
                    if( offset.y == 0)
                    {
                    if(offset.x>0)
                        {
                            for(float i = 1; i < offset.x; i++)
                            {
                                if(Menu.kafelki[(int)(pozycja.x + i)][(int)(pozycja.y)].GetComponent<Pole>().Zajete)
                                {
                                    bieg = false;
                                }
                                wzrostSily++;
                            }
                            if(bieg)
                            {
                                StartCoroutine(RozpendBok(1f, wzrostSily));

                            }
                        }else{
                            for(float i = -1; i > offset.x; i--)
                            {
                                if(Menu.kafelki[(int)(pozycja.x + i)][(int)(pozycja.y)].GetComponent<Pole>().Zajete)
                                {
                                    bieg = false;
                                }
                                wzrostSily++;
                            }
                            if(bieg)
                            {
                                StartCoroutine(RozpendBok(-1f, wzrostSily));    
                            }
                        }
                    }
                    if(!bieg)
                        Menu.usunSelect2();
                }
            }
            else
            {
                leczenie = false;
            }
    }

    IEnumerator Rozpend(float x, float wzrostSily)
    {
        Vector3 pozycja = jednostka.transform.position;
        Menu.kafelki[(int)pozycja.x][(int)(pozycja.y + x * wzrostSily)].GetComponent<Pole>().OnMouseDown();
        yield return new WaitForSeconds(0.1f);
        Menu.kafelki[(int)pozycja.x][(int)(pozycja.y + x * wzrostSily)].GetComponent<Pole>().OnMouseDown();
        yield return new WaitForSeconds(0.5f * wzrostSily);
        jednostka.GetComponent<Jednostka>().atak += wzrostSily;
        Jednostka.Select2.GetComponent<Jednostka>().zaatakowanie();
        jednostka.GetComponent<Jednostka>().atak -= wzrostSily;
        Menu.usunSelect2();
    }
        IEnumerator RozpendBok(float x, float wzrostSily)
    {
        Vector3 pozycja = jednostka.transform.position;
        Menu.kafelki[(int)(pozycja.x + x * wzrostSily)][(int)(pozycja.y )].GetComponent<Pole>().OnMouseDown();
        yield return new WaitForSeconds(0.1f);
        Menu.kafelki[(int)(pozycja.x + x * wzrostSily)][(int)(pozycja.y )].GetComponent<Pole>().OnMouseDown();
        jednostka.GetComponent<Jednostka>().zasieg += 4;
        yield return new WaitForSeconds(0.45f * wzrostSily);
        jednostka.GetComponent<Jednostka>().atak += wzrostSily;
        Jednostka.Select2.GetComponent<Jednostka>().zaatakowanie();
        jednostka.GetComponent<Jednostka>().atak -= wzrostSily;
        jednostka.GetComponent<Jednostka>().zasieg -= 4;
        Menu.usunSelect2();
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
