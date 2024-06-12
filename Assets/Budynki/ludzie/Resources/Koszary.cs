using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(piechur.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>= piechur.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= piechur.GetComponent<Jednostka>().cena;
                            GameObject nowyZbieracz = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("piechur",ref nowyZbieracz);
                            }
                            else
                                nowyZbieracz = Instantiate(piechur, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyZbieracz.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna] * 2;
                            nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update2[druzyna] * 2;
                            nowyZbieracz.transform.position = newPosition;
                            nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                            pole.GetComponent<Pole>().Zajete=true;
                            pole.GetComponent<Pole>().postac=nowyZbieracz;
                        }
                    }
                }
                if(Przycisk.budynek[1]==true)
                {
                    Przycisk.budynek[1]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(lucznik.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>= lucznik.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= lucznik.GetComponent<Jednostka>().cena;
                            GameObject nowyZbieracz = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("lucznik",ref nowyZbieracz);
                            }
                            else
                                nowyZbieracz = Instantiate(lucznik, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyZbieracz.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update4[druzyna] * 2;
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
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(rycerz.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>=rycerz.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=1  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= rycerz.GetComponent<Jednostka>().cena;
                            GameObject nowyZbieracz = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("rycerz",ref nowyZbieracz);
                            }
                            else
                                nowyZbieracz = Instantiate(rycerz, pole.transform.position, Quaternion.identity); 
                            nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna];
                            Vector3 newPosition = nowyZbieracz.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna];
                            nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update2[druzyna] * 2;
                            nowyZbieracz.transform.position = newPosition;
                            nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                            pole.GetComponent<Pole>().Zajete=true;
                            pole.GetComponent<Pole>().postac=nowyZbieracz;
                        }
                    }
                }
                if(Przycisk.budynek[3]==true)
                {
                    Przycisk.budynek[3]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(kusznik.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>=kusznik.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=1  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= kusznik.GetComponent<Jednostka>().cena;
                            GameObject nowyZbieracz = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("kusznik",ref nowyZbieracz);
                            }
                            else
                                nowyZbieracz = Instantiate(kusznik, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyZbieracz.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update4[druzyna] * 2;
                            nowyZbieracz.transform.position = newPosition;
                            nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                            pole.GetComponent<Pole>().Zajete=true;
                            pole.GetComponent<Pole>().postac=nowyZbieracz;
                        }
                    }
                }
                if(Przycisk.budynek[4]==true)
                {
                    Przycisk.budynek[4]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(kawalerzysta.GetComponent<Jednostka>().cena , 0 , 0, true);
                    if(Menu.zloto[Menu.tura]>=kawalerzysta.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=2  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= kawalerzysta.GetComponent<Jednostka>().cena;
                            GameObject nowyZbieracz = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("kawalerzysta",ref nowyZbieracz);
                            }
                            else
                                nowyZbieracz = Instantiate(kawalerzysta, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyZbieracz.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update1[druzyna] * 2;
                            nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update2[druzyna] * 2;
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
            Guzikk.CenaMagic.text = piechur.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = lucznik.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = rycerz.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = kusznik.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[4].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = kawalerzysta.GetComponent<Jednostka>().cena.ToString();

            teksty[2] = "Rycerz to bardzo wytrzymała jednostka";
            teksty[3] = "Kusznik potrafi ustawić się do strzału by później zadać spore obrażenia";
            teksty[4] = "Kawalerzysta to potężna oraz bardzo mobilna jednostka";
          
            
            for(int i = 0 ; i < 5 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       

            if(Menu.ratuszPoziom[druzyna]<2)
            {
                InterfaceBuild.przyciski[4].GetComponent<Image>().sprite = loock;
                teksty[4] = "Wymagany 3 poziom ratusza";

            }
            if(Menu.ratuszPoziom[druzyna]<1)
            {
                InterfaceBuild.przyciski[3].GetComponent<Image>().sprite = loock;
                InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = loock;
                teksty[2] = "Wymagany 2 poziom ratusza";
                teksty[3] = "Wymagany 2 poziom ratusza";
            }
        }
    }
}
