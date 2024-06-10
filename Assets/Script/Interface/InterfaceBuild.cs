using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InterfaceBuild : MonoBehaviour
{
    public Text TObrona;
    public Text CzasBudowy;
    public Text nazwa;
    public Slider healthGracza;
    public Gradient gradient;

    public static GameObject[] przyciski;
    public GameObject  obiekt1;

    public static GameObject[] obrazkikopalnia;
    public GameObject[] obrazkikopalniapriv;
    public static Slider[] paskikopalnia;
    public Slider[] paskikopalniapriv;

    public GameObject przyciskischowaj;
    public GameObject kopalniaschowaj;
    public GameObject medykschowaj;

    public static Image twarzHerosa;
    public static Slider zdrowieHerosa;
    public Image privtwarzHerosa;
    public Slider privzdrowieHerosa;

    public Sprite drewnoArtPriv;
    public Sprite zlotoArtPriv;
    public static Sprite drewnoArt;
    public static Sprite zlotoArt;

    public GameObject menu;
    public string[] usuwanieString;

    private GameObject budynek;

    void Start()
    {
        drewnoArt = drewnoArtPriv;
        zlotoArt = zlotoArtPriv;
        twarzHerosa = privtwarzHerosa;
        zdrowieHerosa = privzdrowieHerosa;
        obrazkikopalnia = new GameObject[7];
        paskikopalnia = new Slider[7];
        przyciski = new GameObject[16];
        if (obiekt1 != null)
        {
            for (int i = 0; i < 16; i++)
            {
                przyciski[i] = obiekt1.transform.GetChild(i).gameObject;
            }
        }
        for (int i = 0; i < 7; i++)
        {
            obrazkikopalnia[i] = obrazkikopalniapriv[i];
            paskikopalnia[i] = paskikopalniapriv[i];
        }
    }

    void Update()
    {
        if(Jednostka.Select!=null && !Jednostka.CzyJednostka)
        {
            Budynek Wybrany = Jednostka.Select.GetComponent<Budynek>();

            switch(Wybrany.rodzaj)
            {
                case(0): przyciskischowaj.SetActive(true); kopalniaschowaj.SetActive(false); medykschowaj.SetActive(false); break;
                case(1): przyciskischowaj.SetActive(false); kopalniaschowaj.SetActive(true); medykschowaj.SetActive(false);break;
                case(2): przyciskischowaj.SetActive(true); kopalniaschowaj.SetActive(false); 
                if(Wybrany.punktyBudowy>=Wybrany.punktyBudowyMax) 
                {
                    medykschowaj.SetActive(true);
                }
                else
                {
                    medykschowaj.SetActive(false);
                }
                break;
                case(3): przyciskischowaj.SetActive(true); kopalniaschowaj.SetActive(true); medykschowaj.SetActive(false); break;
            }

            UpdateHealthBarColor();
            if(Wybrany.punktyBudowy<Wybrany.punktyBudowyMax)
            {
                healthGracza.value = Wybrany.punktyBudowy;
                healthGracza.maxValue = Wybrany.punktyBudowyMax;
            }
            else{
                healthGracza.value = Wybrany.HP;
                healthGracza.maxValue = Wybrany.maxHP;
            }
            
            if(Wybrany.punktyBudowy < Wybrany.punktyBudowyMax && ((!Jednostka.Select.GetComponent<Ratusz>() &&  !Jednostka.Select.GetComponent<nRatusz>()) || (Wybrany.punktyBudowyMax < 7) ) )
            {
                for(int i=0;i<=Wybrany.zdolnosci;i++)
                    {
                        InterfaceBuild.przyciski[i].SetActive(false);
                    }
            } 
            if(Wybrany.punktyBudowy >= Wybrany.punktyBudowyMax)
            {  
                CzasBudowy.text = Wybrany.HP.ToString() + "/" + Wybrany.maxHP.ToString();
                TObrona.text = Wybrany.obrona.ToString();
            }
            else
                CzasBudowy.text = Wybrany.punktyBudowy.ToString() + "/" + Wybrany.punktyBudowyMax.ToString();
            nazwa.text = Wybrany.nazwa;
        }
    }

        public static void Czyszczenie()
    {
        for (int i = 0; i < 16; i++)
            {
                PrzyciskInter Guzik = przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.CenaZloto.text = "";
                Guzik.CenaDrewno.text = "";
                Guzik.CenaMagic.text = "";
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.IconMagic.enabled = false;
                Guzik.IconZloto.sprite = zlotoArt;
                Guzik.IconDrewno.sprite = drewnoArt;
                Guzik.IconMagic.sprite = zlotoArt;
            }
    }


            void UpdateHealthBarColor()
    {
        float normalizedHP;
        if(Jednostka.Select.GetComponent<Budynek>().punktyBudowy<Jednostka.Select.GetComponent<Budynek>().punktyBudowyMax)
            normalizedHP = Jednostka.Select.GetComponent<Budynek>().punktyBudowy / Jednostka.Select.GetComponent<Budynek>().punktyBudowyMax;
        else
            normalizedHP = Jednostka.Select.GetComponent<Budynek>().HP / Jednostka.Select.GetComponent<Budynek>().maxHP;
        Color healthColor = gradient.Evaluate(normalizedHP); // Pobierz kolor z Gradientu

        // Ustaw kolor paska zdrowia
        Image fillImage = healthGracza.fillRect.GetComponent<Image>();
        fillImage.color = healthColor;
    }

    public void usuwanie()
    {
        budynek = Jednostka.Select;
        if(budynek.GetComponent<BudynekRuch>().wybudowany)
        {
        Budynek staty = budynek.GetComponent<Budynek>();
        if(staty.druzyna == Menu.tura)
        {
            Menu.idInfo = 1;
            usuwanieString[0] = "Czy na pewno chcesz usunąć ten obiekt? Otrzymasz w zamian " + (int)(staty.zloto * 0.5) + " zlota oraz " + (int)(staty.drewno*0.5) + " drewna.";
            usuwanieString[1] = "Cofnij";
            usuwanieString[2] = "Usuń";
            for(int i =0;i<=2;i++)
                menu.GetComponent<Menu>().infoText[i].text = usuwanieString[i];
            menu.GetComponent<Menu>().InfoKolejnaTura.SetActive(true);
        }
        }
    }
    public void cofnij()
    {
        menu.GetComponent<Menu>().InfoKolejnaTura.SetActive(false);
    }
    public void dalej()
    {
        menu.GetComponent<Menu>().InfoKolejnaTura.SetActive(false);
        Budynek staty = budynek.GetComponent<Budynek>();
        //budynek.GetComponent<Budynek>().poZniszczeniu = 0;
        GameObject kafelek = Menu.kafelki[(int)budynek.transform.position.x][(int)budynek.transform.position.y];
        kafelek.GetComponent<Pole>().Zajete = false;
        kafelek.GetComponent<Pole>().postac = null;
        if(MenuGlowne.multi)
        {
            kafelek.GetComponent<Pole>().AktualizujPołożenie();
            PhotonView photonView = budynek.GetComponent<PhotonView>();
            photonView.RPC("deadMulti", RpcTarget.All, Ip.ip);
        }
            
        Menu.zloto[Menu.tura] += (int)(staty.zloto * 0.5);
        Menu.drewno[Menu.tura] += (int)(staty.drewno*0.5);
        Jednostka.Select = null;
        Menu.PanelUnit.SetActive(false);
        Menu.PanelBuild.SetActive(false);
        Pole.Clean2();
        Debug.Log(budynek.name);
        Debug.Log(staty.poZniszczeniu); 
        if(staty.poZniszczeniu==0)
            Destroy(budynek);
        else{staty.poZniszczeniu=2;}
    }
}
