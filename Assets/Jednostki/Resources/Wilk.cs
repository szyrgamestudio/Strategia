using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wilk : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;


    void OnMouseDown()
    {
        if (jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie();

            for (int i = 0; i < jednostka.GetComponent<Jednostka>().zdolnosci; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.Opis.text = teksty[i];
            }
        }
    }
    public int dodatkowyAtak()
    {
        int iloscWilkow = -2;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    int posX = (int)jednostka.transform.position.x + x;
                    int posY = (int)jednostka.transform.position.y + y;

                    if (posX >= 0 && posX < Menu.BoardSizeX && posY >= 0 && posY < Menu.BoardSizeY)
                    {
                        if (Menu.kafelki[posX][posY].GetComponent<Pole>().postac != null && Menu.kafelki[posX][posY].GetComponent<Pole>().postac.GetComponent<Jednostka>().nazwa == "Wilk")
                        {
                            iloscWilkow+=2;
                        }
                    }
                }
            }
            return iloscWilkow;
    }

}
