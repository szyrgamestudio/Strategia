using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nPoszukiwacz : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    void Update()
    {
        if(Jednostka.Select == jednostka)
        {
            int j = Menu.kafelki[(int)transform.position.x][(int)transform.position.y].GetComponent<Pole>().zloto;
            int i3 = 0;
            int k3 = 0;
            int p3 = 0;
            if(j>9)
                j=9;
            InterfaceUnit.przyciski[0].GetComponent<Image>().sprite = budynki[j];
            for(int i = -1; i<=1;i++)
                for(int i2 = -1; i2<=1;i2++)
                {
                    if(Mathf.Abs(i2) + Mathf.Abs(i) < 2 && (int)transform.position.x + i >= 0 && (int)transform.position.x + i <= Menu.BoardSizeX-1 && (int)transform.position.y + i2 >= 0 && (int)transform.position.y + i2 <= Menu.BoardSizeY-1)
                        i3 += Menu.kafelki[(int)transform.position.x + i][(int)transform.position.y + i2].GetComponent<Pole>().zloto;
                }
            if(i3>9)
                i3 =9;
            InterfaceUnit.przyciski[1].GetComponent<Image>().sprite = budynki[i3];
            for(int k = -2; k<=2;k++)
                for(int k2 = -2; k2<=2;k2++)
                {
                    if(Mathf.Abs(k2) + Mathf.Abs(k) < 3 && (int)transform.position.x + k >= 0 && (int)transform.position.x + k <= Menu.BoardSizeX -1 && (int)transform.position.y + k2 >= 0 && (int)transform.position.y + k2 <= Menu.BoardSizeY -1)
                        k3 += Menu.kafelki[(int)transform.position.x + k][(int)transform.position.y + k2].GetComponent<Pole>().zloto;
                }
            if(k3>9)
                k3 =9;
            InterfaceUnit.przyciski[2].GetComponent<Image>().sprite = budynki[k3];
            for(int p = -3; p<=3;p++)
                for(int p2 = -3; p2<=3;p2++)
                {
                    if(Mathf.Abs(p2) + Mathf.Abs(p) < 4 && (int)transform.position.x + p >= 0 && (int)transform.position.x + p <= Menu.BoardSizeX-1 && (int)transform.position.y + p2 >= 0 && (int)transform.position.y + p2 <= Menu.BoardSizeY-1)
                        p3 += Menu.kafelki[(int)transform.position.x + p][(int)transform.position.y + p2].GetComponent<Pole>().zloto;
                }
            if(p3>9)
                p3 =9;
            InterfaceUnit.przyciski[3].GetComponent<Image>().sprite = budynki[p3];
        }
    }

     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            jednostka.GetComponent<Jednostka>().OnMouseDown();
            if(jednostka.GetComponent<Jednostka>().zdolnosci > 4)
                jednostka.GetComponent<Jednostka>().zdolnosci = 4;
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
