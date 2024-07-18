using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Krematorium : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;

    // public GameObject lucznik;
    // public GameObject piechur;
    // public GameObject rycerz;
    // public GameObject kusznik;
    // public GameObject kawalerzysta;
 
    public GameObject[] jednostki;
    public int[] wymagany;
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
                    respJednostki(jednostki[0], 0);
                }
                if(Przycisk.budynek[1]==true)
                {
                    respJednostki(jednostki[1], 1);
                }
                if(Przycisk.budynek[2]==true)
                {
                    respJednostki(jednostki[2], 2);
                }
                if(Przycisk.budynek[3]==true)
                {
                    respJednostki(jednostki[3], 3);
                }
                if(Przycisk.budynek[4]==true)
                {
                   respJednostki(jednostki[4], 4);
                }
            }
    }

    public void respJednostki(GameObject obj, int i)
    {
        Przycisk.budynek[i]=false;
        Interface.interfaceStatic.GetComponent<Interface>().Brak(obj.GetComponent<Jednostka>().cena , obj.GetComponent<Jednostka>().drewno , 0, true);
        if(Menu.zloto[Menu.tura]>=obj.GetComponent<Jednostka>().cena && Menu.drewno[Menu.tura]>=obj.GetComponent<Jednostka>().drewno && Menu.ratuszPoziom[druzyna]>=wymagany[i]  && (Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna] || obj.GetComponent<Szczur>()))
        {  
            if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
            {
                Menu.zloto[Menu.tura] -= obj.GetComponent<Jednostka>().cena;
                Menu.drewno[Menu.tura] -= obj.GetComponent<Jednostka>().drewno;
                GameObject nowyZbieracz = null;
                if(MenuGlowne.multi)
                {
                    jednostkaMulti(obj.name,ref nowyZbieracz);
                }
                else
                    nowyZbieracz = Instantiate(obj, pole.transform.position, Quaternion.identity); 
                Vector3 newPosition = nowyZbieracz.transform.position;
                newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                nowyZbieracz.transform.position = newPosition;
                nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                if(obj.GetComponent<Jednostka>().lata)
                    pole.GetComponent<Pole>().ZajeteLot=true;
                else
                    pole.GetComponent<Pole>().Zajete=true;
                pole.GetComponent<Pole>().postac=nowyZbieracz;

                /////////////ZWIĘKSZANIE STATYSTYK////////////////////////////////

                Jednostka staty = nowyZbieracz.GetComponent<Jednostka>();
                staty.obrona += Kbiblioteka.update1[druzyna] * 2;
                Debug.Log("jeden " + staty.nazwa);
                switch(staty.nazwa)
                {
                    case "Szczur" : staty.maxdmg += Biblioteka.update1[druzyna]; break;
                    case "Zoombie" : staty.maxHP += Biblioteka.update2[druzyna] * 2; staty.HP += Biblioteka.update2[druzyna] * 2; staty.obrona += Biblioteka.update5[druzyna] * 2; break;
                    case "Mumia" : staty.maxHP += Biblioteka.update2[druzyna] * 2; staty.HP += Biblioteka.update2[druzyna] * 2; staty.obrona += Biblioteka.update5[druzyna] * 2; break;
                    case "Wampir" : staty.maxHP += Biblioteka.update2[druzyna] * 2; staty.HP += Biblioteka.update2[druzyna] * 2; staty.atak += Biblioteka.update4[druzyna] * 2; break;
                    case "Marty Łucznik" : staty.atak += Biblioteka.update3[druzyna]; staty.obrona += Biblioteka.update3[druzyna]; break;
                    case "Lisz" : staty.atak += Biblioteka.update3[druzyna]; staty.obrona += Biblioteka.update3[druzyna]; staty.obrona += Biblioteka.update5[druzyna] * 2; break;
                    case "Martwy Wojak" : staty.atak += Biblioteka.update3[druzyna]; staty.obrona += Biblioteka.update3[druzyna]; break;
                    case "Wielki Pająk" : staty.atak += Biblioteka.update4[druzyna] * 2; break;
                    case "Gargulec" : staty.atak += Biblioteka.update4[druzyna] * 2; break;
                    case "Żniwiarz" : staty.obrona += Biblioteka.update5[druzyna] * 2; break;
                    ///////////////////////////////
                    case "Kamikaze" : staty.atak += Kbiblioteka.update2[druzyna] * 2; break;
                    case "Żongler dynamitu" : staty.atak += Kbiblioteka.update2[druzyna] * 2; break;
                    case "Anty-Budynkowa-Maszyna" : staty.atak += Kbiblioteka.update2[druzyna] * 2; break;
                    case "Kwatermistrz" : staty.atak += Kbiblioteka.update2[druzyna] * 2; break;
                    case "Golem" : staty.HP += Kbiblioteka.update3[druzyna] * 2; staty.maxHP += Kbiblioteka.update3[druzyna] * 2; break;
                    case "Wielki Golem" : staty.HP += Kbiblioteka.update3[druzyna] * 2; staty.maxHP += Kbiblioteka.update3[druzyna] * 2; nowyZbieracz.GetComponent<Golem>().DMG += Kbiblioteka.update4[druzyna];break;
                    case "Strzelec" : staty.atak += Kbiblioteka.update5[druzyna] * 2; break;
                    case "Charpunnik" : staty.atak += Kbiblioteka.update5[druzyna] * 2; break;
                    case "Tarczownik" : staty.atak += Kbiblioteka.update5[druzyna] * 2; break;
                    case "Cierpliwy" : staty.atak += Kbiblioteka.update5[druzyna] * 2; break;
                }
            }
        }
    }

    public void OnMouseDown()
    {
        if(budynek == Jednostka.Select)
        {
            InterfaceBuild.Czyszczenie(); 
            
            for(int i = 0 ; i < budynek.GetComponent<Budynek>().zdolnosci ; i++)
            {
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                if(jednostki[i].GetComponent<Jednostka>().drewno == 0)
                {
                    Guzik.CenaMagic.text = jednostki[i].GetComponent<Jednostka>().cena.ToString();
                    Guzik.IconMagic.enabled = true;
                }
                else
                {
                    Guzik.CenaZloto.text = jednostki[i].GetComponent<Jednostka>().cena.ToString();
                    Guzik.CenaDrewno.text = jednostki[i].GetComponent<Jednostka>().drewno.ToString();
                    Guzik.IconZloto.enabled = true;
                    Guzik.IconDrewno.enabled = true;
                }
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                
                
                Guzik.Opis.text = teksty[i];  
            }
            

            for(int i = 0 ; i < budynek.GetComponent<Budynek>().zdolnosci ; i++)
            {
                if(wymagany[i] > Menu.ratuszPoziom[druzyna])
                {
                    InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = loock;
                    switch(wymagany[i])
                    {
                        case 1:  teksty[i] = "Wymagany 2 poziom ratusza"; break;
                        case 2:  teksty[i] = "Wymagany 3 poziom ratusza"; break;
                        case 3:  teksty[i] = "Wymagany 4 poziom ratusza"; break;
                    }
                }
            }
        }
    }
}
