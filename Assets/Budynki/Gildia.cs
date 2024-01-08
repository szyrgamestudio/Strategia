using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gildia : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;

    public GameObject zbieraczMagi;
    public GameObject magOgnia;
    public GameObject magDruid;
    public GameObject magKaplan;

    public Sprite[] budynki;
    public string[] teksty;
    public Sprite loock;

    void Start()
    {
        druzyna = budynek.GetComponent<Budynek>().druzyna;
    }

    void Update()
    {
        if(budynek == Jednostka.Select)
            {
                pole = budynek.GetComponent<BudynekRuch>().pole;
                if(Przycisk.budynek[0]==true && Menu.zloto[Menu.tura]>=magOgnia.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[0]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= magOgnia.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = Instantiate(magOgnia, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update5[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
                if(Przycisk.budynek[1]==true && Menu.zloto[Menu.tura]>=magDruid.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=2  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[1]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= magDruid.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = Instantiate(magDruid, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update5[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
                if(Przycisk.budynek[2]==true && Menu.zloto[Menu.tura]>=magKaplan.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=2  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[2]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= magKaplan.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = Instantiate(magKaplan, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update5[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
            }
    }
    public void OnMouseDown()
    {
        if(budynek == Jednostka.Select)
        {
            InterfaceBuild.Czyszczenie(); 
            
            PrzyciskInter Guzikk = InterfaceBuild.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = magOgnia.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = magDruid.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = magKaplan.GetComponent<Jednostka>().cena.ToString();

          
            
            for(int i = 0 ; i < 3 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       

            if(Menu.ratuszPoziom[druzyna]<=2)
            {
                InterfaceBuild.przyciski[1].GetComponent<Image>().sprite = loock;
                InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = loock;
            }
        }
    }
}
