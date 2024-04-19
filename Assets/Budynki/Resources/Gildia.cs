using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
                if(Przycisk.budynek[0]==true && Menu.zloto[Menu.tura]>=magOgnia.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[0]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= magOgnia.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = null;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti("magOgnia",ref nowyZbieracz);
                        }
                        else
                            nowyZbieracz = Instantiate(magOgnia, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update5[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
                if(Przycisk.budynek[1]==true && Menu.zloto[Menu.tura]>=magDruid.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=1  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[1]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= magDruid.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = null;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti("magDruid",ref nowyZbieracz);
                        }
                        else
                            nowyZbieracz = Instantiate(magDruid, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update5[druzyna];
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
                if(Przycisk.budynek[2]==true && Menu.zloto[Menu.tura]>=magKaplan.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=1  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                    Przycisk.budynek[2]=false;
                    if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= magKaplan.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = null;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti("magKaplan",ref nowyZbieracz);
                        }
                        else
                            nowyZbieracz = Instantiate(magKaplan, pole.transform.position, Quaternion.identity); 
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

            if(Menu.ratuszPoziom[druzyna]<=1)
            {
                InterfaceBuild.przyciski[1].GetComponent<Image>().sprite = loock;
                InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = loock;
            }
        }
    }
}
