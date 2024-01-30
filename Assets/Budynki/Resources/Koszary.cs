using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Koszary : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;

    public GameObject lucznik;
    public GameObject piechur;
    public GameObject rycerz;
    public GameObject kusznik;
    public GameObject kawalerzysta;

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
                if(Przycisk.budynek[0]==true && Menu.zloto[Menu.tura]>= piechur.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[0]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= piechur.GetComponent<Jednostka>().cena;
                        GameObject nowyLucznik = Instantiate(piechur, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyLucznik.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyLucznik.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna];
                        nowyLucznik.GetComponent<Jednostka>().atak += Kuznia.update2[druzyna];
                        nowyLucznik.transform.position = newPosition;
                        nowyLucznik.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyLucznik;
                    }
                }
                if(Przycisk.budynek[1]==true && Menu.zloto[Menu.tura]>= lucznik.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[1]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= lucznik.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = Instantiate(lucznik, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update4[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
                if(Przycisk.budynek[2]==true && Menu.zloto[Menu.tura]>=rycerz.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=2  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[2]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= rycerz.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = Instantiate(rycerz, pole.transform.position, Quaternion.identity); 
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna];
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna];
                        nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update2[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
                if(Przycisk.budynek[3]==true && Menu.zloto[Menu.tura]>=kusznik.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=2  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[3]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= kusznik.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = Instantiate(kusznik, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update4[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
                if(Przycisk.budynek[4]==true && Menu.zloto[Menu.tura]>=kawalerzysta.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=3  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[4]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= kawalerzysta.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = Instantiate(kawalerzysta, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna];
                        nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update2[druzyna];
                        nowyZbieracz.GetComponent<Jednostka>().szybkosc += Kuznia.update3[druzyna];
                        nowyZbieracz.GetComponent<Jednostka>().maxszybkosc += Kuznia.update3[druzyna];
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
            Guzikk.CenaMagic.text = piechur.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = lucznik.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = rycerz.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = kusznik.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[4].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = kawalerzysta.GetComponent<Jednostka>().cena.ToString();
          
            
            for(int i = 0 ; i < 5 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       

            if(Menu.ratuszPoziom[druzyna]<3)
            {
                InterfaceBuild.przyciski[4].GetComponent<Image>().sprite = loock;
            }
            if(Menu.ratuszPoziom[druzyna]<2)
            {
                InterfaceBuild.przyciski[3].GetComponent<Image>().sprite = loock;
                InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = loock;
            }
        }
    }
}
