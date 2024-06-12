using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Plac : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;

    public GameObject jaskolka;
    public GameObject wilk;
    public GameObject gryf;

    public Sprite[] budynki;
    public string[] teksty;
    public Sprite loock;

    void Start()
    {
        druzyna = budynek.GetComponent<Budynek>().druzyna;
    }
    public void jednostkaMulti(string nazwa, ref GameObject nowyZbieracz)
    {
        nowyZbieracz = PhotonNetwork.Instantiate(nazwa, new Vector3(0, 0, 1), Quaternion.identity);
        nowyZbieracz.transform.position = pole.transform.position;
        nowyZbieracz.GetComponent<Jednostka>().druzyna = budynek.GetComponent<Budynek>().druzyna;
        nowyZbieracz.GetComponent<Jednostka>().sojusz = budynek.GetComponent<Budynek>().sojusz;
        nowyZbieracz.transform.position = new Vector3(nowyZbieracz.transform.position.x, nowyZbieracz.transform.position.y, -2f);
        nowyZbieracz.GetComponent<Jednostka>().Aktualizuj();
        nowyZbieracz.GetComponent<Jednostka>().AktualizujPol();    
    }
    void Update()
    {
        if(budynek == Jednostka.Select)
            {
                pole = budynek.GetComponent<BudynekRuch>().pole;
                if(Przycisk.budynek[0]==true)
                {
                    Przycisk.budynek[0]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(jaskolka.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>= jaskolka.GetComponent<Jednostka>().cena && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= jaskolka.GetComponent<Jednostka>().cena;
                            GameObject nowyLucznik = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("jaskolka",ref nowyLucznik);
                            }
                            else
                                nowyLucznik = Instantiate(jaskolka, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyLucznik.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyLucznik.GetComponent<Jednostka>().obrona += Kuznia.update4[druzyna] * 2;
                            nowyLucznik.GetComponent<Jednostka>().atak += Kuznia.update2[druzyna] * 2;
                            nowyLucznik.GetComponent<Jednostka>().szybkosc += Kuznia.update3[druzyna] * 2;
                            nowyLucznik.GetComponent<Jednostka>().maxszybkosc += Kuznia.update3[druzyna] * 2;
                            nowyLucznik.transform.position = newPosition;
                            nowyLucznik.GetComponent<Jednostka>().druzyna = druzyna;
                            pole.GetComponent<Pole>().Zajete=true;
                            pole.GetComponent<Pole>().postac=nowyLucznik;
                        }
                    }
                }
                if(Przycisk.budynek[1]==true)
                {
                    Przycisk.budynek[1]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(wilk.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>=wilk.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= wilk.GetComponent<Jednostka>().cena;
                            GameObject nowyZbieracz = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("wilk",ref nowyZbieracz);
                            }
                            else
                                nowyZbieracz = Instantiate(wilk, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyZbieracz.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyZbieracz.GetComponent<Jednostka>().szybkosc += Kuznia.update3[druzyna] * 2;
                            nowyZbieracz.GetComponent<Jednostka>().maxszybkosc += Kuznia.update3[druzyna] * 2;
                            nowyZbieracz.transform.position = newPosition;
                            nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                            pole.GetComponent<Pole>().Zajete=true;
                            pole.GetComponent<Pole>().postac=nowyZbieracz;
                        }
                    }
                }
                if(Przycisk.budynek[2]==true)
                {
                    Przycisk.budynek[2]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(gryf.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>=gryf.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=1  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= gryf.GetComponent<Jednostka>().cena;
                            GameObject nowyZbieracz = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("gryf",ref nowyZbieracz);
                            }
                            else
                                nowyZbieracz = Instantiate(gryf, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyZbieracz.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update4[druzyna] * 2;
                            nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update5[druzyna] * 2;
                            nowyZbieracz.GetComponent<Jednostka>().szybkosc += Kuznia.update3[druzyna] * 2;
                            nowyZbieracz.GetComponent<Jednostka>().maxszybkosc += Kuznia.update3[druzyna] * 2;
                            nowyZbieracz.transform.position = newPosition;
                            nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                            pole.GetComponent<Pole>().Zajete=true;
                            pole.GetComponent<Pole>().postac=nowyZbieracz;
                        }
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
            Guzikk.CenaMagic.text = jaskolka.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = wilk.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = gryf.GetComponent<Jednostka>().cena.ToString();
            teksty[2] = "Gryf to potężna jednostka latająca";

            for(int i = 0 ; i < 3 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       

            if(Menu.ratuszPoziom[druzyna]<1)
            {
                teksty[2] = "Wymagany 2 poziom ratusza";
                InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = loock;
            }
        }
    }
}
