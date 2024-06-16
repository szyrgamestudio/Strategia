using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Apteka : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;
    public static bool[] apteka = new bool[5];

    public GameObject medyk;

    public Sprite[] budynki;
    public string[] teksty;
    public Sprite loock;

    public Image wykrzyknik;

    private bool dodaj=false;

    public bool koniec;

    public int regeneracja = 0;

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
        if(budynek == Jednostka.Select)
            {
                pole = budynek.GetComponent<BudynekRuch>().pole;
                if(Przycisk.budynek[0]==true)
                {
                    Przycisk.budynek[0]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(medyk.GetComponent<Jednostka>().cena , 0 , 0, true);

                    if(Menu.zloto[Menu.tura]>=medyk.GetComponent<Jednostka>().cena && Menu.maxludnosc[druzyna] > Menu.ludnosc[druzyna])
                    {
                        
                        if(!pole.GetComponent<Pole>().Zajete && !pole.GetComponent<Pole>().ZajeteLot)
                        {
                            Menu.zloto[Menu.tura] -= medyk.GetComponent<Jednostka>().cena;
                            GameObject nowyLucznik = null;
                            if(MenuGlowne.multi)
                            {
                                jednostkaMulti("medyk",ref nowyLucznik);
                            }
                            else
                                nowyLucznik = Instantiate(medyk, pole.transform.position, Quaternion.identity); 
                            Vector3 newPosition = nowyLucznik.transform.position;
                            newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                            nowyLucznik.transform.position = newPosition;
                            nowyLucznik.GetComponent<Jednostka>().druzyna = druzyna;
                            pole.GetComponent<Pole>().Zajete=true;
                            pole.GetComponent<Pole>().postac=nowyLucznik;

                            
                        }
                    }
                }
                if(Przycisk.budynek[1]==true && Menu.zloto[Menu.tura]>= Menu.heros[druzyna].GetComponent<Heros>().level && regeneracja < 5)
                {
                    Menu.zloto[Menu.tura]-=Menu.heros[druzyna].GetComponent<Heros>().level;
                    Przycisk.budynek[1]=false;
                    regeneracja++;
                }
                if(InterfaceBuild.zdrowieHerosa.gameObject.activeSelf)
                {
                    InterfaceBuild.zdrowieHerosa.value = regeneracja;
                }
            }
            
            if(Menu.Next)
            {
                koniec = true;
            }
            if(koniec && !Menu.Next && budynek.GetComponent<Budynek>().druzyna == Menu.tura)
            {
                koniec = false;
                if(!Menu.heros[druzyna].activeSelf && regeneracja < 5)
                {
                    regeneracja++;
                }
            }
            if(regeneracja >= 5)
                wykrzyknik.enabled = true;
            else
                wykrzyknik.enabled = false;
        if(!Menu.heros[druzyna].activeSelf && regeneracja >= 5 && !budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete
        && Menu.ludnosc[druzyna] < Menu.maxludnosc[druzyna])
        {
            if(MenuGlowne.multi)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("wskrzeszenieMulti", RpcTarget.All, (int)budynek.GetComponent<BudynekRuch>().pole.transform.position.x ,  (int)budynek.GetComponent<BudynekRuch>().pole.transform.position.y);
            }
            else
            {
            Jednostka staty = Menu.heros[druzyna].GetComponent<Jednostka>();
            regeneracja = 0;
            Menu.ludnosc[druzyna]++;
            staty.HP = staty.maxHP;
            budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().Zajete = true;
            budynek.GetComponent<BudynekRuch>().pole.GetComponent<Pole>().postac = Menu.heros[druzyna];
            Vector3 newPosition = budynek.GetComponent<BudynekRuch>().pole.transform.position;
            newPosition.z = -2f;
            Menu.heros[druzyna].transform.position = newPosition;
            Menu.heros[druzyna].SetActive(true);
            staty.ShowDMG(staty.maxHP, Color.green);
            OnMouseDown();
            }
        }
        if(!dodaj)
        {
            dodaj = true;
            apteka[druzyna] = true;
        }
        if(budynek.GetComponent<Budynek>().poZniszczeniu == 2)
        {
            apteka[druzyna] = false;
            Destroy(budynek);
        }
    }
    [PunRPC]
    public void wskrzeszenieMulti(int x, int y)
    {
        Debug.Log("zaczynamy");
        Jednostka staty = Menu.heros[druzyna].GetComponent<Jednostka>();
            regeneracja = 0;
            Menu.ludnosc[druzyna]++;
            staty.HP = staty.maxHP;
            Menu.kafelki[x][y].GetComponent<Pole>().Zajete = true;
            Menu.kafelki[x][y].GetComponent<Pole>().postac = Menu.heros[druzyna];
            Vector3 newPosition = Menu.kafelki[x][y].transform.position;
            newPosition.z = -2f;
            Menu.heros[druzyna].transform.position = newPosition;
            Menu.heros[druzyna].SetActive(true);
            staty.ShowDMG(staty.maxHP, Color.green);
            Debug.Log("kończymy");
    }
    public void OnMouseDown()
    {
        if(budynek == Jednostka.Select)
        {
            if(Menu.heros[druzyna].activeSelf)
            {
                InterfaceBuild.twarzHerosa.enabled = false;
                InterfaceBuild.zdrowieHerosa.gameObject.SetActive(false);
                budynek.GetComponent<Budynek>().zdolnosci = 1;
                budynek.GetComponent<Budynek>().OnMouseDown();
            }
            else
            {
                InterfaceBuild.twarzHerosa.enabled = true;
                InterfaceBuild.zdrowieHerosa.gameObject.SetActive(true);
                InterfaceBuild.twarzHerosa.sprite = Menu.heros[druzyna].GetComponent<SpriteRenderer>().sprite;
                InterfaceBuild.zdrowieHerosa.maxValue = 5;
                budynek.GetComponent<Budynek>().zdolnosci = 2;
                budynek.GetComponent<Budynek>().OnMouseDown();
            }

            InterfaceBuild.Czyszczenie(); 
            
            PrzyciskInter Guzikk = InterfaceBuild.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = medyk.GetComponent<Jednostka>().cena.ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = Menu.heros[druzyna].GetComponent<Heros>().level.ToString();

           
            for(int i = 0 ; i < 2 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.IconMagic.sprite = InterfaceBuild.zlotoArt;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }
}
