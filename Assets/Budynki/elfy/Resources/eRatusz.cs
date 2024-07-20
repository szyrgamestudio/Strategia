using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class eRatusz : MonoBehaviour
{
    public GameObject budynek;

    private bool dodaj=false;
    public int nr_jednostki;
    public int druzyna;
    public int poziom = 0;

    public GameObject pole;

    public GameObject zbieracz;
    public GameObject poszukiwacz;
    public GameObject budowlaniec;
    public GameObject adept;

    public Sprite[] budynki;
    public string[] teksty;

    public Sprite loock;

    public Image ramka;

    public bool ulepsza = true;


    void Start()
    {
        druzyna = budynek.GetComponent<Budynek>().druzyna;
        budynek.GetComponent<Budynek>().poZniszczeniu = 1;
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
        ramka.enabled = budynek.GetComponent<Budynek>().strzalka.enabled;
        if(budynek == Jednostka.Select)
        {
            pole = budynek.GetComponent<BudynekRuch>().pole;
            if(Przycisk.budynek[0]==true)
            {
                Przycisk.budynek[0]=false;
                Interface.interfaceStatic.GetComponent<Interface>().Brak(zbieracz.GetComponent<Jednostka>().cena , 0 , 0, true);
                if(Menu.zloto[Menu.tura]>=zbieracz.GetComponent<Jednostka>().cena && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= zbieracz.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = null;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti(zbieracz.name,ref nowyZbieracz);
                        }
                        else
                            nowyZbieracz = Instantiate(zbieracz, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                        nowyZbieracz.GetComponent<Jednostka>().obrona += Kuznia.update3[druzyna];
                        nowyZbieracz.GetComponent<Jednostka>().atak += Kuznia.update3[druzyna];
                    }
                }
            }
            if(Przycisk.budynek[1]==true)
            {
                Interface.interfaceStatic.GetComponent<Interface>().Brak(poszukiwacz.GetComponent<Jednostka>().cena , 0 , 0, true);
                Przycisk.budynek[1]=false;
                if(Menu.zloto[Menu.tura]>=poszukiwacz.GetComponent<Jednostka>().cena && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {
                if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= poszukiwacz.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = null;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti(poszukiwacz.name,ref nowyZbieracz);
                        }
                        else
                        nowyZbieracz = Instantiate(poszukiwacz, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
            }
            if(Przycisk.budynek[2]==true)
            {
                Interface.interfaceStatic.GetComponent<Interface>().Brak(budowlaniec.GetComponent<Jednostka>().cena , 0 , 0, true);
                Przycisk.budynek[2]=false;
                if(Menu.zloto[Menu.tura]>=budowlaniec.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                {    
                if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= budowlaniec.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = null;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti(budowlaniec.name,ref nowyZbieracz);
                        }
                        else
                        nowyZbieracz = Instantiate(budowlaniec, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
            }
            if(Przycisk.budynek[3]==true)
            {
                Interface.interfaceStatic.GetComponent<Interface>().Brak(adept.GetComponent<Jednostka>().cena , 0 , 0, true);
                Przycisk.budynek[3]=false;
                if( Menu.zloto[Menu.tura]>=adept.GetComponent<Jednostka>().cena  && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                { 
                if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                    {
                        Menu.zloto[Menu.tura] -= adept.GetComponent<Jednostka>().cena;
                        GameObject nowyZbieracz = null;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti(adept.name,ref nowyZbieracz);
                        }
                        else
                        nowyZbieracz = Instantiate(adept, pole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = nowyZbieracz.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = druzyna;
                        pole.GetComponent<Pole>().Zajete=true;
                        pole.GetComponent<Pole>().postac=nowyZbieracz;
                    }
                }
            }
            if(Przycisk.budynek[4]==true)
            {
                Interface.interfaceStatic.GetComponent<Interface>().Brak(5 , 0 , 0, false);
                Przycisk.budynek[4]=false;
                if(Menu.zloto[Menu.tura]>=5 && Budowlaniec.punktyBudowyBonus[druzyna] == 0)
                {
                   
                    Menu.zloto[Menu.tura] -= 5;
                    InterfaceBuild.przyciski[4].GetComponent<Image>().sprite = loock;
                    Budowlaniec.punktyBudowyBonus[druzyna]++;
                    OnMouseDown();
                }
            }
            if(Menu.ratuszPoziom[druzyna] < poziom && budynek.GetComponent<Budynek>().punktyBudowy >= budynek.GetComponent<Budynek>().punktyBudowyMax)
                Menu.ratuszPoziom[druzyna] = poziom;
            if(Przycisk.budynek[5]==true)
            {
                Przycisk.budynek[5]=false;
                Interface.interfaceStatic.GetComponent<Interface>().Brak(1 + poziom * 2 , 15 + poziom * 5 , 0, false);
                if(Menu.drewno[Menu.tura]>=15 + poziom * 5 && Menu.zloto[Menu.tura]>=1 + poziom * 2)
                    {
                        
                        Menu.drewno[Menu.tura] -= 15 + poziom * 5;
                        Menu.zloto[Menu.tura] -= 1 + poziom * 2;
                        budynek.GetComponent<Budynek>().punktyBudowy = 0;
                        budynek.GetComponent<Budynek>().punktyBudowyMax += 2;
                        if(MenuGlowne.multi)
                        {     
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("remont", RpcTarget.All, budynek.GetComponent<Budynek>().punktyBudowy,budynek.GetComponent<Budynek>().punktyBudowyMax, Ip.ip);
                        }
                        ulepsza = false;
                        budynek.GetComponent<Budynek>().zdolnosci = 5;
                        budynek.GetComponent<Budynek>().OnMouse();
                        OnMouseDown();
                    }
            }
        }

        if(budynek.GetComponent<Budynek>().punktyBudowy >= budynek.GetComponent<Budynek>().punktyBudowyMax && !ulepsza)
        {
            ulepsza = true;
            poziom++;
        }

        if(budynek.GetComponent<Budynek>().punktyBudowy >= budynek.GetComponent<Budynek>().punktyBudowyMax && !dodaj)
        {
            dodaj = true;
            Menu.maxludnosc[druzyna] += 7;
            poziom++;
            StartCoroutine(przyporzadkuj());
        }
        if(budynek.GetComponent<Budynek>().poZniszczeniu == 2)
        {
            Menu.kafelki[(int)budynek.transform.position.x][(int)budynek.transform.position.y].GetComponent<Pole>().Zajete = false;
            if(budynek.GetComponent<Budynek>().punktyBudowy >= budynek.GetComponent<Budynek>().punktyBudowyMax || budynek.GetComponent<Budynek>().punktyBudowyMax > 6)
            {
                while(Menu.bazy[druzyna,nr_jednostki+1] != null)
                {
                    Menu.bazy[druzyna,nr_jednostki] = Menu.bazy[druzyna,nr_jednostki+1];
                    Menu.bazy[druzyna,nr_jednostki].GetComponent<eRatusz>().nr_jednostki -= 1;
                    nr_jednostki++;
                }
                Menu.maxludnosc[druzyna] -= 7;
                Menu.bazy[druzyna,nr_jednostki] = null;
                Menu.bazyIlosc[druzyna]--;
            }
            Destroy(budynek);
        }
    }
    [PunRPC]
    public void przeniesNaStarcie()
    {
        if(nr_jednostki == 0 && Ip.ip == druzyna)
        {
            StartCoroutine(ruchPlynnyCamery(druzyna));
        }
    }

    [PunRPC]
    public void remont(int x, int y, int ip)
    {
        if(Ip.ip != ip)
        {
            budynek.GetComponent<Budynek>().punktyBudowy = x;
            budynek.GetComponent<Budynek>().punktyBudowyMax = y;
            poziom++;
        }
    }
    IEnumerator przyporzadkuj()
    {
        yield return new WaitForSeconds(0.2f);
        Menu.bazy[druzyna , Menu.bazyIlosc[druzyna]] = budynek;
        nr_jednostki = Menu.bazyIlosc[druzyna];
        Menu.bazyIlosc[druzyna]++;
        Debug.Log("w domu najlepiej");
        if(MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("przeniesNaStarcie", RpcTarget.All);
        }
    }


    static public IEnumerator ruchPlynnyCamery(int druzynaBaza)
        {
            float x = Menu.bazy[druzynaBaza,0].transform.position.x + 1f;
            float y = Menu.bazy[druzynaBaza,0].transform.position.y;
            if(y < Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f)
            {
                y = Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f;
            }        
            if(x < Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f)
            {
                x = Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f;
            }
            if(y > Menu.BoardSizeY - Menu.kamera.GetComponent<Camera>().orthographicSize * 1f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.2f - 0.5f)
            {
                y = Menu.BoardSizeY - Menu.kamera.GetComponent<Camera>().orthographicSize * 1f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.2f - 0.5f;
            }
            if(x > Menu.BoardSizeX - Menu.kamera.GetComponent<Camera>().orthographicSize * 1.75f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.6f - 0.5f)
            {
                x = Menu.BoardSizeX - Menu.kamera.GetComponent<Camera>().orthographicSize * 1.75f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.6f - 0.5f;
            }
            float a = Menu.kamera.transform.position.x;
            float b = Menu.kamera.transform.position.y;

            float x1 = (x-a)/60;
            float y1 = (y-b)/60;
           for(int i=0; i<60; i++)
            {
                a+=x1;
                b+=y1;
                Vector3 newPosition = new Vector3(a, b, -10f);
                Menu.kamera.transform.position = newPosition;
                yield return new WaitForSeconds(0.001f);
            } 
        }

 public void OnMouseDown()
    {
        if(budynek == Jednostka.Select)
        {
            if(!ulepsza)
            {
                budynek.GetComponent<Budynek>().zdolnosci = 5;
            }
            else
                budynek.GetComponent<Budynek>().zdolnosci = 6;
            InterfaceBuild.Czyszczenie(); 
            
            PrzyciskInter Guzikk = InterfaceBuild.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = zbieracz.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = poszukiwacz.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = budowlaniec.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = adept.GetComponent<Jednostka>().cena.ToString();
            


            Guzikk = InterfaceBuild.przyciski[5].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = (1+2*poziom).ToString(); Guzikk.CenaDrewno.text = (15+5*poziom).ToString(); 
          
            
            for(int i = 0 ; i < 6 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       
            Guzikk = InterfaceBuild.przyciski[4].GetComponent<PrzyciskInter>();
            if(Budowlaniec.punktyBudowyBonus[druzyna]==0)
            {
                
                Guzikk.CenaMagic.text = "5";
            }
            else
            {
                InterfaceBuild.przyciski[4].GetComponent<Image>().sprite = loock;
                Guzikk.IconMagic.enabled = false;
            }

            Guzikk = InterfaceBuild.przyciski[5].GetComponent<PrzyciskInter>();
            Guzikk.IconZloto.enabled = true;
            Guzikk.IconDrewno.enabled = true;
            Guzikk.IconMagic.enabled = false;
            
        }
    }
}
