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
                    BudowanyObiekt.GetComponent<BudynekRuch>().dedMulti();
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
                Wieza wiezaSkrypt = BudowanyObiekt.GetComponent<Wieza>();
                if(wiezaSkrypt != null)
                {
                    BudowanyObiekt.GetComponent<Wieza>().dedMulti();
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
                Kragmagi krag = BudowanyObiekt.GetComponent<Kragmagi>();
                if(krag != null)
                {
                    BudowanyObiekt.GetComponent<Kragmagi>().dedMulti();
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
                        wybudowanie(ratusz, 0);
                    }
                if(Przycisk.jednostka[1]==true)
                    {
                        wybudowanie(kopalnia, 1);
                    }
                if(Przycisk.jednostka[2]==true)
                    {
                        wybudowanie(tartak, 2);
                    }
                if(Przycisk.jednostka[3]==true)
                    {
                        wybudowanie(chatka, 3);
                    }
                if(Przycisk.jednostka[4]==true)
                    {
                        wybudowanie(koszary, 4);
                    }
                if(Przycisk.jednostka[5]==true)
                    {
                        wybudowanie(plac, 5);
                    }
                if(Przycisk.jednostka[6]==true)
                    {
                        wybudowanie(gildia, 6);
                    }
                if(Przycisk.jednostka[7]==true)
                    {
                        Przycisk.jednostka[7]=false;
                        Kragmagi staty = kuznia.GetComponent<Kragmagi>();
                        Interface.interfaceStatic.GetComponent<Interface>().Brak(4 , 4 , 0, false);
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=5 && Menu.drewno[Menu.tura]>=5)
                            {
                            Kragmagi.budowlaniec = jednostka; 
                            if (MenuGlowne.multi)
                            {
                                budowanieMulti(kragmagi.name);
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
                        if(!Apteka.apteka[jednostka.GetComponent<Jednostka>().druzyna])
                            wybudowanie(medyk, 8);  
                    }
                if(Przycisk.jednostka[9]==true)
                    {
                        Przycisk.jednostka[9]=false;
                        Wieza staty = wierza.GetComponent<Wieza>();
                        Interface.interfaceStatic.GetComponent<Interface>().Brak(3 , 7 , 0, false);
                        if(wybieranie == false && Menu.zloto[Menu.tura]>=3 && Menu.drewno[Menu.tura]>=7)
                            {
                            Wieza.budowlaniec = jednostka; 
                            // Menu.ludnosc[jednostka.GetComponent<Jednostka>().druzyna]--;
                            // Debug.Log(jednostka.GetComponent<Jednostka>().druzyna);
                            // Debug.Log(Menu.ludnosc[jednostka.GetComponent<Jednostka>().druzyna]);v
                            if (MenuGlowne.multi)
                            {
                                BudowanyObiekt = PhotonNetwork.Instantiate(wierza.name, new Vector3(-10f, 0, 1), Quaternion.identity);
                                BudowanyObiekt.GetComponent<Jednostka>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                                BudowanyObiekt.GetComponent<Jednostka>().sojusz = jednostka.GetComponent<Jednostka>().sojusz;
                                BudowanyObiekt.GetComponent<Jednostka>().Aktualizuj();
                            }
                            else
                            {
                                BudowanyObiekt = Instantiate(wierza, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
                            }
                            BudowanyObiekt.GetComponent<Wieza>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            BudowanyObiekt.GetComponent<Jednostka>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                            wybieranie = true; // Zakończ tryb "przenoszenia"
                            Pole.Clean2();
                            }
                    }
                if(Przycisk.jednostka[10]==true)
                    {
                        Przycisk.jednostka[10]=false;
                        Interface.interfaceStatic.GetComponent<Interface>().Brak(0 , 2 , 0, false);
                        if(Menu.zloto[Menu.tura]>=0 && Menu.drewno[Menu.tura]>=2)
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
                        if(Menu.ratuszPoziom[jednostka.GetComponent<Jednostka>().druzyna] != 0)
                            wybudowanie(portal, 11);
                    }
                if(Przycisk.jednostka[12]==true)
                    {
                        wybudowanie(sciana, 12);
                    }
                if(Przycisk.jednostka[13]==true)
                    {
                        wybudowanie(kuznia, 13);
                    }

            }
        if(BudynekRuch.pomoc)
        {
            BudynekRuch.pomoc=false;
            wybieranie = false;
        }

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
               BudynekRuch ruch = BudowanyObiekt.GetComponent<BudynekRuch>();
                if(ruch != null && !ruch.wybudowany)
                {
                    Apteka apteka2 = BudowanyObiekt.GetComponent<Apteka>();
                    if(apteka2 != null)
                        Apteka.apteka[jednostka.GetComponent<Jednostka>().druzyna] = false;
                    BudowanyObiekt.GetComponent<BudynekRuch>().dedMulti();
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
                Wieza wiezaSkrypt = BudowanyObiekt.GetComponent<Wieza>();
                if(wiezaSkrypt != null)
                {
                    BudowanyObiekt.GetComponent<Wieza>().dedMulti();
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
                Kragmagi krag = BudowanyObiekt.GetComponent<Kragmagi>();
                if(krag != null)
                {
                    BudowanyObiekt.GetComponent<Kragmagi>().dedMulti();
                    Destroy(BudowanyObiekt);
                    wybieranie = false;
                }
            }
        }
    }

    public void wybudowanie(GameObject obj, int nr)
    {
        Przycisk.jednostka[nr]=false;
        Budynek staty = obj.GetComponent<Budynek>();
        Interface.interfaceStatic.GetComponent<Interface>().Brak(staty.zloto , staty.drewno , 0, false);
        if(wybieranie == false && Menu.zloto[Menu.tura]>=staty.zloto && Menu.drewno[Menu.tura]>=staty.drewno)
        {
            BudynekRuch.budowlaniec = jednostka; 
            if (MenuGlowne.multi)
            {
                budowanieMulti(obj.name);
            }
            else
                BudowanyObiekt = Instantiate(obj, new Vector3(0, 0, -1), Quaternion.identity); // Przechowaj referencję do obiektu
            BudowanyObiekt.GetComponent<Budynek>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
            wybieranie = true; // Zakończ tryb "przenoszenia"
            Pole.Clean2();
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
        BudowanyObiekt = PhotonNetwork.Instantiate(nazwa, new Vector3(-10f, 0, -3), Quaternion.identity);
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
            Guzikk.CenaZloto.text = ratusz.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = ratusz.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = kopalnia.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = kopalnia.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = tartak.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = tartak.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = chatka.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = chatka.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[4].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = koszary.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = koszary.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[5].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = plac.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = plac.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[6].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = gildia.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = gildia.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[7].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = kragmagi.GetComponent<Kragmagi>().zloto.ToString(); Guzikk.CenaDrewno.text = kragmagi.GetComponent<Kragmagi>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[8].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = medyk.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = medyk.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[9].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = wierza.GetComponent<Wieza>().zloto.ToString(); Guzikk.CenaDrewno.text = wierza.GetComponent<Wieza>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[10].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = "0"; Guzikk.CenaDrewno.text = "2"; 
            Guzikk = InterfaceUnit.przyciski[11].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = portal.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = portal.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[12].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = sciana.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = sciana.GetComponent<Budynek>().drewno.ToString(); 
            Guzikk = InterfaceUnit.przyciski[13].GetComponent<PrzyciskInter>();
            Guzikk.CenaZloto.text = kuznia.GetComponent<Budynek>().zloto.ToString(); Guzikk.CenaDrewno.text = kuznia.GetComponent<Budynek>().drewno.ToString(); 
               
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
