using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jaskolka : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    void Start()
    {
    }



    void Update()
    { 
        if (jednostka == Jednostka.Select)
        {
            if (Przycisk.jednostka[0] == true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura] >= 3 && !Menu.bazy[Menu.tura, 0].GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete)
            {
                Przycisk.jednostka[0] = false;
                transform.position = Menu.bazy[Menu.tura, 0].GetComponent<BudynekRuch>().pole.transform.position;
                Vector3 newPosition = transform.position;
                newPosition.z = -2f;
                transform.position = newPosition;
                Menu.magia[Menu.tura] -= 3;
                jednostka.GetComponent<Jednostka>().akcja = false;
            }
        }


    }
     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 

            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "3"; 
            
            for(int i = 0 ; i < jednostka.GetComponent<Jednostka>().zdolnosci  ; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            } 
            InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>().IconMagic.enabled = false;      
        }
    }
}
