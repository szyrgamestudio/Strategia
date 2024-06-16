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
        Interface.interfaceStatic.GetComponent<Interface>().Brak(obj.GetComponent<Jednostka>().cena , 0 , 0, true);
        if(Menu.zloto[Menu.tura]>=obj.GetComponent<Jednostka>().cena && Menu.ratuszPoziom[druzyna]>=wymagany[i]  && (Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna] || obj.GetComponent<Szczur>()))
        {  
            if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
            {
                Menu.zloto[Menu.tura] -= obj.GetComponent<Jednostka>().cena;
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
            Guzikk.CenaMagic.text = jednostki[0].GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = jednostki[1].GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = jednostki[2].GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = jednostki[3].GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[4].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = jednostki[4].GetComponent<Jednostka>().cena.ToString();
          
            
            for(int i = 0 ; i < 5 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       

            for(int i = 0 ; i < 5 ; i++)
            {
                if(wymagany[i] > Menu.ratuszPoziom[druzyna])
                {
                    InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = loock;
                    switch(wymagany[i])
                    {
                        case 1:  teksty[i] = "Wymagany 2 poziom ratusza"; break;
                        case 2:  teksty[i] = "Wymagany 3 poziom ratusza"; break;
                    }
                }
            }
        }
    }
}
