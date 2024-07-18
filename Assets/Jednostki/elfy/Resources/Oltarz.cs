using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Oltarz : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;
 
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
                if(Przycisk.budynek[0]==true && !DuchLasu.duch)
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
        Interface.interfaceStatic.GetComponent<Interface>().Brak(DuchLasu.cenaD , DuchLasu.drewnoD , 0, true);
        if(Menu.zloto[Menu.tura]>=DuchLasu.cenaD && Menu.drewno[Menu.tura]>=DuchLasu.drewnoD && Menu.ratuszPoziom[druzyna]>=wymagany[i]  && (Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna] || obj.GetComponent<Szczur>()))
        {  
            if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
            {
                Menu.zloto[Menu.tura] -= DuchLasu.cenaD;
                Menu.drewno[Menu.tura] -= DuchLasu.drewnoD;
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
                DuchLasu.duch = true;
                OnMouseDown();
                /////////////ZWIĘKSZANIE STATYSTYK////////////////////////////////
            }
        }
    }

    public void OnMouseDown()
    {
        if(budynek == Jednostka.Select)
        {
            InterfaceBuild.Czyszczenie(); 
            
            for(int i = 0 ; i < 1 ; i++)
            {
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                if(jednostki[i].GetComponent<Jednostka>().drewno == 0)
                {
                    Guzik.CenaMagic.text = DuchLasu.cenaD.ToString();
                    Guzik.IconMagic.enabled = true;
                }
                else
                {
                    Guzik.CenaZloto.text = DuchLasu.cenaD.ToString();
                    Guzik.CenaDrewno.text = DuchLasu.drewnoD.ToString();
                    Guzik.IconZloto.enabled = true;
                    Guzik.IconDrewno.enabled = true;
                }
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                
                
                Guzik.Opis.text = teksty[i];  
            }
            

            for(int i = 0 ; i < 1 ; i++)
            {
                if(DuchLasu.duch)
                {
                    InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = loock;
                    PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                    Guzik.CenaZloto.text = "";
                    Guzik.CenaDrewno.text = "";
                    Guzik.IconZloto.enabled = false;
                    Guzik.IconDrewno.enabled = false;
                    teksty[i] = "Wymagany 2 poziom ratusza"; 
                }
            }
        }
    }
}
