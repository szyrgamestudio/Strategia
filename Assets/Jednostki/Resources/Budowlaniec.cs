using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Budowlaniec : MonoBehaviour
{
    public GameObject jednostka;

    public GameObject ratusz;
    public GameObject kopalnia;
    public GameObject tartak;
    public GameObject chatka;
    public GameObject koszary;
    public GameObject plac;
    public GameObject gildia;
    public GameObject kragmagi;
    public GameObject medyk;
    public GameObject wierza;
    public Sprite droga;
    public GameObject portal;
    public GameObject sciana;
    public GameObject kuznia;
    public static bool wybieranie;
    public GameObject BudowanyObiekt; // Dodaj pole do przechowywania referencji
    public Texture2D customCursorBudowa;
    public Sprite loock;

    public Sprite[] budynki;
    public string[] teksty;

    public bool budowanie;
    public bool prekoniec;

    public int punktyBudowy = 2;
    public static int[] punktyBudowyBonus = new int[5];

    void Start()
    {
        wybieranie = false;
        budowanie = false;
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (BudowanyObiekt != null) 
            {
                BudynekRuch ruch = BudowanyObiekt.GetComponent<BudynekRuch>();
                if(ruch != null && !ruch.wybudowany)
                {
                    Apteka apteka2 = BudowanyObiekt.GetComponent<Apteka>();
                    if(apteka2 != null)
                        Apteka.apteka[jednostka.GetComponent<Jednostka>().druzyna] = false;
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
                Wieza wiezaSkrypt = BudowanyObiekt.GetComponent<Wieza>();
                if(wiezaSkrypt != null)
                {
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
                Kragmagi krag = BudowanyObiekt.GetComponent<Kragmagi>();
                if(krag != null)
                {
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
            }
            

            
            if(budowanie)
            {
                budowanie=false;
                Jednostka.wybieranie = false;

                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            Jednostka.Select2 = null;
        }

        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true)
                    {
                        Przycisk.jednostka[0]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=6 && Menu.drewno[Menu.tura]>=15)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("ratusz");
                            }
                            else
                            {
                                BudowanyObiekt = Instantiate(ratusz, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                                BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            }
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[1]==true)
                    {
                        Przycisk.jednostka[1]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=5 && Menu.drewno[Menu.tura]>=5)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("kopalnia");
                            }
                            else
                                BudowanyObiekt = Instantiate(kopalnia, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[2]==true)
                    {
                        Przycisk.jednostka[2]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=6 && Menu.drewno[Menu.tura]>=14)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("tartak");
                            }
                            else
                                BudowanyObiekt = Instantiate(tartak, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[3]==true)
                    {
                        Przycisk.jednostka[3]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=2 && Menu.drewno[Menu.tura]>=5)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("chatka");
                            }
                            else
                                BudowanyObiekt = Instantiate(chatka, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[4]==true)
                    {
                        Przycisk.jednostka[4]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=4 && Menu.drewno[Menu.tura]>=10)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("koszary");
                            }
                            else
                            BudowanyObiekt = Instantiate(koszary, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[5]==true)
                    {
                        Przycisk.jednostka[5]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=4 && Menu.drewno[Menu.tura]>=10)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("plac");
                            }
                            else
                            BudowanyObiekt = Instantiate(plac, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[6]==true)
                    {
                        Przycisk.jednostka[6]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=4 && Menu.drewno[Menu.tura]>=10)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("gildia");
                            }
                            else
                            BudowanyObiekt = Instantiate(gildia, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[7]==true)
                    {
                        Przycisk.jednostka[7]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=5 && Menu.drewno[Menu.tura]>=5)
                            {
                            Kragmagi.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("kragmagi");
                            }
                            else
                            BudowanyObiekt = Instantiate(kragmagi, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[8]==true)
                    {
                        Przycisk.jednostka[8]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=5 && Menu.drewno[Menu.tura]>=9 && !Apteka.apteka[jednostka.GetComponent<Jednostka>().druzyna])
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("medyk");
                            }
                            else
                            BudowanyObiekt = Instantiate(medyk, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[9]==true)
                    {
                        Przycisk.jednostka[9]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=3 && Menu.drewno[Menu.tura]>=7)
                            {
                            Wieza.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("wierza");
                            }
                            else
                            BudowanyObiekt = Instantiate(wierza, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Wieza>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[10]==true)
                    {
                        Przycisk.jednostka[10]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=0 && Menu.drewno[Menu.tura]>=2)
                            {
                            if(Menu.kafelki[(int)Jednostka.Select.transform.position.x][(int)Jednostka.Select.transform.position.y].GetComponent<Droga>().droga == false && 
                            Menu.kafelki[(int)Jednostka.Select.transform.position.x][(int)Jednostka.Select.transform.position.y].GetComponent<Pole>().magia == 0 &&
                            Menu.kafelki[(int)Jednostka.Select.transform.position.x][(int)Jednostka.Select.transform.position.y].GetComponent<Pole>().las == false)
                            {
                                Debug.Log("jeden");
                                if(MenuGlowne.multi)
                                {
                                    PhotonView photonView = GetComponent<PhotonView>();
                                    photonView.RPC("drogaMulti", RpcTarget.All, Ip.ip,(int)Jednostka.Select.transform.position.x,(int)Jednostka.Select.transform.position.y);
                                }
                                Menu.kafelki[(int)Jednostka.Select.transform.position.x][(int)Jednostka.Select.transform.position.y].GetComponent<Pole>().trudnosc -= 1;
                                Menu.kafelki[(int)Jednostka.Select.transform.position.x][(int)Jednostka.Select.transform.position.y].GetComponent<Droga>().updateDroga(1);
                                Menu.zloto[Menu.tura]-=0;
                                Menu.drewno[Menu.tura]-=2;
                                
                            }

                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[11]==true)
                    {
                        Przycisk.jednostka[11]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=8 && Menu.drewno[Menu.tura]>=8 && Menu.ratuszPoziom[jednostka.GetComponent<Jednostka>().druzyna] != 0)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("portal");
                            }
                            else
                            BudowanyObiekt = Instantiate(portal, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[12]==true)
                    {
                        Przycisk.jednostka[12]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=0 && Menu.drewno[Menu.tura]>=3)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("sciana");
                            }
                            else
                            BudowanyObiekt = Instantiate(sciana, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[13]==true)
                    {
                        Przycisk.jednostka[13]=false;
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=3 && Menu.drewno[Menu.tura]>=12)
                            {
                            BudynekRuch.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti("kuznia");
                            }
                            else
                            BudowanyObiekt = Instantiate(kuznia, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                // if(Przycisk.jednostka[14]==true && jednostka.GetComponent<Jednostka>().akcja)
                //     {
                //         Przycisk.jednostka[14]=false;
                //         Jednostka.wybieranie = true;
                //         budowanie = true;
                //         Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                //     }
            }
        if(BudynekRuch.pomoc)
        {
            BudynekRuch.pomoc=false;
            wybieranie = false;
        }
        // if(Jednostka.Select2 != null && budowanie&& Walka.odleglosc(Jednostka.Select2,jednostka) <= jednostka.GetComponent<Jednostka>().zasieg)
        // {
        //     Budynek budynekSkrypt = Jednostka.Select2.GetComponent<Budynek>();
        //     Wieza wiezaSkrypt = Jednostka.Select2.GetComponent<Wieza>();

        //     if(budynekSkrypt != null && Jednostka.Select2.GetComponent<Budynek>().punktyBudowy < Jednostka.Select2.GetComponent<Budynek>().punktyBudowyMax)
        //     {
        //         budynekSkrypt.punktyBudowy += punktyBudowy + punktyBudowyBonus[Menu.tura];
        //         if(budynekSkrypt.nazwa != "Droga")
        //             jednostka.GetComponent<Jednostka>().akcja = false;
        //         budynekSkrypt.ShowDMG(punktyBudowy + punktyBudowyBonus[Menu.tura], new Color(255 / 255.0f, 165 / 255.0f, 0 / 255.0f, 0.0f));
        //         budowanieVoid();
        //     }
        //     if(wiezaSkrypt != null && wiezaSkrypt.punktyBudowy < wiezaSkrypt.punktyBudowyMax)
        //     {
        //         wiezaSkrypt.punktyBudowy += punktyBudowy + punktyBudowyBonus[Menu.tura];
        //         jednostka.GetComponent<Jednostka>().akcja = false;
        //         wiezaSkrypt.ShowDMG(punktyBudowy + punktyBudowyBonus[Menu.tura], new Color(255 / 255.0f, 165 / 255.0f, 0 / 255.0f, 0.0f));    
        //         budowanieVoid();
        //     }        
        // }
        if(Menu.preNext)
        {
            prekoniec = true;
        }
        if(prekoniec && !Menu.preNext)
        {
            prekoniec = false;
            wybieranie = false;
            if (BudowanyObiekt != null) 
            {
                Wieza wiezaSkrypt = BudowanyObiekt.GetComponent<Wieza>();
                if(wiezaSkrypt != null)
                {
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
                Kragmagi krag = BudowanyObiekt.GetComponent<Kragmagi>();
                if(krag != null)
                {
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
            }
        }
    }
    [PunRPC]
    public void drogaMulti(int ip, int x, int y)
    {
        if(Ip.ip != ip)
        {
            Menu.kafelki[x][y].GetComponent<Pole>().trudnosc -= 1;
            Menu.kafelki[x][y].GetComponent<Droga>().updateDroga(1);
        }
    }
    public void budowanieMulti(string nazwa)
    {
        BudowanyObiekt = PhotonNetwork.Instantiate(nazwa, new Vector3(0, 0, 1), Quaternion.identity);
        BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
        BudowanyObiekt.GetComponent<Budynek>().sojusz = jednostka.GetComponent<Jednostka>().sojusz;
        BudowanyObiekt.GetComponent<Budynek>().Aktualizuj();
    }

     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "6"; Guzikk.CenaDrewno.text = "15"; 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "5"; Guzikk.CenaDrewno.text = "5"; 
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "6"; Guzikk.CenaDrewno.text = "14"; 
            Guzikk = InterfaceUnit.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "2"; Guzikk.CenaDrewno.text = "5"; 
            Guzikk = InterfaceUnit.przyciski[4].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "4"; Guzikk.CenaDrewno.text = "10"; 
            Guzikk = InterfaceUnit.przyciski[5].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "4"; Guzikk.CenaDrewno.text = "10"; 
            Guzikk = InterfaceUnit.przyciski[6].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "4"; Guzikk.CenaDrewno.text = "10"; 
            Guzikk = InterfaceUnit.przyciski[7].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "5"; Guzikk.CenaDrewno.text = "5"; 
            Guzikk = InterfaceUnit.przyciski[8].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "5"; Guzikk.CenaDrewno.text = "9"; 
            Guzikk = InterfaceUnit.przyciski[9].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "3"; Guzikk.CenaDrewno.text = "7"; 
            Guzikk = InterfaceUnit.przyciski[10].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "0"; Guzikk.CenaDrewno.text = "2"; 
            Guzikk = InterfaceUnit.przyciski[11].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "6"; Guzikk.CenaDrewno.text = "6";
            Guzikk = InterfaceUnit.przyciski[12].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "0"; Guzikk.CenaDrewno.text = "3"; 
            Guzikk = InterfaceUnit.przyciski[13].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "3"; Guzikk.CenaDrewno.text = "12"; 
               
            for(int i = 0 ; i < jednostka.GetComponent<Jednostka>().zdolnosci  ; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = true;
                Guzik.IconDrewno.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       
            Guzikk = InterfaceUnit.przyciski[14].GetComponent<PrzyciskInter>();
            Guzikk.IconZloto.enabled = false;
            Guzikk.IconDrewno.enabled = false;
            if(Apteka.apteka[jednostka.GetComponent<Jednostka>().druzyna] == true)
            {
                InterfaceUnit.przyciski[8].GetComponent<Image>().sprite = loock;
                PrzyciskInter Guzik = InterfaceUnit.przyciski[8].GetComponent<PrzyciskInter>();
                Guzik.CenaZloto.text = ""; Guzik.CenaDrewno.text = ""; 
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
            }

            if(Menu.ratuszPoziom[jednostka.GetComponent<Jednostka>().druzyna] == 0)
            {
                InterfaceUnit.przyciski[11].GetComponent<Image>().sprite = loock;
                PrzyciskInter Guzik = InterfaceUnit.przyciski[11].GetComponent<PrzyciskInter>();
                Guzik.CenaZloto.text = ""; Guzik.CenaDrewno.text = ""; 
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
            }
        }
    }
}
