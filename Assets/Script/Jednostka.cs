using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Jednostka : MonoBehaviour
{
    static public GameObject Select;
    static public GameObject Select2;
    static public bool wybieranie = false;
    static public bool CzyJednostka;
    static public bool CzyJednostka2;
    public GameObject jednostka;

    public int druzyna;
    public int sojusz;

    public float HP;
    public float maxHP;
    public float atak;
    public float obrona;
    public int zasieg;
    public int maxszybkosc;
    public int szybkosc;
    public float mindmg;
    public float maxdmg;
    public int zdolnosci;
    public bool zbieracz;
    public bool lata;
    public int cena;
    public String nazwa;

    public bool akcja;
    public bool spanie;

    public int nr_jednostki;
    public bool koniec;

    public Slider healthGracza;
    public Gradient gradient;
    public Image obramowka;
    public Image wybrane;
    public Text TextShowDMG;
    public Texture2D customCursor;

    public GameObject canvasAnimacji;
    public Animator animator;
    public GameObject pocisk;
    
    private Vector3 aktualnePołożenie;  
    void Start()
    {
        switch(druzyna)
        {
            case 0: obramowka.color = new Color(0.0f, 0.0f, 0.0f); break;
            case 1: obramowka.color = new Color(1.0f, 0.0f, 0.0f); break;
            case 2: obramowka.color = new Color(0.0f, 1.0f, 0.0f); break;
            case 3: obramowka.color = new Color(0.0f, 0.0f, 1.0f); break;
            case 4: obramowka.color = new Color(1.0f, 1.0f, 0.0f); break;
        }
        if(druzyna != 0)
            sojusz = WyburRas.team[druzyna-1] + 1;
        else
            sojusz = 0;
        healthGracza.maxValue = maxHP;
        healthGracza.value = HP;
        StartCoroutine(przyporzadkuj());
    }

    IEnumerator przyporzadkuj()
    {
        yield return new WaitForSeconds(0.2f);
        Menu.jednostki[druzyna , Menu.ludnosc[druzyna]] = jednostka;
        nr_jednostki = Menu.ludnosc[druzyna];
        Menu.ludnosc[druzyna]++;
    }

        public void ShowDMG(float dmg, Color myColor)
    {
        TextShowDMG.text = dmg.ToString();
        TextShowDMG.color = myColor;
        RectTransform textTransform = TextShowDMG.GetComponent<RectTransform>();
        float x = UnityEngine.Random.Range(-2f,2f);
        textTransform.anchoredPosition = new Vector2(x, 10.4f);
        StartCoroutine(AnimacjaHP(x));
    }
    IEnumerator AnimacjaHP(float x)
    {
        RectTransform textTransform = TextShowDMG.GetComponent<RectTransform>();
        Color textColor = TextShowDMG.color;
        
        for(int i=0;i<40;i++)
        {
            textTransform.anchoredPosition = new Vector2(x, 10.4f + 0.15f*(float)i);
            yield return new WaitForSeconds(0.02f);
            textColor.a = 1f - 0.025f*(float)i;
            TextShowDMG.color = textColor;
        }
    }
    public void OnMouseDown()
    {
        if(!Menu.NIERUSZAC)
            OnMouse();
    }
    public void OnMouse()
    {
        Menu.przyciskiClear();
        if(wybieranie)
            {
                Select2 = jednostka;
                CzyJednostka2 = true;
            }
        else
        {
            if(jednostka == Select)
            {
                Interface.przeniesDoSelect();
            }

            if(Select != null && CzyJednostka && Select.GetComponent<Jednostka>().druzyna == Menu.tura && sojusz != Select.GetComponent<Jednostka>().sojusz && 
            (Select.GetComponent<Jednostka>().zasieg>=Walka.odleglosc(jednostka, Select) && Select.GetComponent<Jednostka>().akcja && (Select.GetComponent<Jednostka>().zasieg > 1 || Select.GetComponent<Jednostka>().lata || !jednostka.GetComponent<Jednostka>().lata)))
            {
                zaatakowanie();
            }
            else
            {
                CzyJednostka = true;
                Menu.PanelUnit.SetActive(true);
                Menu.PanelBuild.SetActive(false);
                Select = jednostka;
                Pole.Clean2();
                for(int i=0;i<16;i++)
                {
                    InterfaceUnit.przyciski[i].SetActive(false);
                    if(zdolnosci>i)
                        InterfaceUnit.przyciski[i].SetActive(true);
                }
            }
            
        }
    }

    public void zaatakowanie()
    {
        if(Select.GetComponent<Jednostka>().nazwa == "Wilk")
            Select.GetComponent<Jednostka>().atak += Select.GetComponent<Wilk>().dodatkowyAtak();
        Jednostka Atakujacy = Select.GetComponent<Jednostka>();
        if(Atakujacy.zasieg>=Walka.odleglosc(jednostka, Select) && Atakujacy.akcja && (Atakujacy.zasieg > 1 || Atakujacy.lata || !jednostka.GetComponent<Jednostka>().lata))
         {
            float result;
            if(Select.GetComponent<Jednostka>().lata || jednostka.GetComponent<Jednostka>().lata)
                result = UnityEngine.Random.Range(Atakujacy.mindmg, Atakujacy.maxdmg) * (float)(1 + 0.1 * (Atakujacy.atak - obrona));
            else
            {
            result = UnityEngine.Random.Range((float)Atakujacy.mindmg, (float)Atakujacy.maxdmg) * (1f + 0.1f * ((float)Atakujacy.atak + (Menu.kafelki[(int)Select.transform.position.x][(int)Select.transform.position.y].GetComponent<Pole>().poziom -Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().poziom) - (float)obrona));
            }

            float roundedResult = (float)Math.Round(result, 1);

            if(Select.GetComponent<Jednostka>().nazwa == "Wilk")
                Select.GetComponent<Jednostka>().atak -= Select.GetComponent<Wilk>().dodatkowyAtak();
            if(Walka.odleglosc(jednostka, Select)==1)
                Atakujacy.animacjaMiecz(jednostka);
            else
                Atakujacy.animacjaPocisk(jednostka);
            if(roundedResult<1)
            {
                roundedResult = 1;
            }
            HP -= roundedResult;

            ShowDMG(roundedResult, new Color(1.0f, 0.0f, 0.0f, 1.0f));
            Atakujacy.akcja = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
    public void animacjaMiecz(GameObject atakowany)
    {
        if (atakowany.transform.position.x > jednostka.transform.position.x)
            canvasAnimacji.transform.rotation = Quaternion.Euler(0, 0, 0f);
        else if (atakowany.transform.position.y > jednostka.transform.position.y)
            canvasAnimacji.transform.rotation = Quaternion.Euler(0, 0, 90f);
        else if (atakowany.transform.position.x < jednostka.transform.position.x)
            canvasAnimacji.transform.rotation = Quaternion.Euler(0, 0, 180f);
        else if (atakowany.transform.position.y < jednostka.transform.position.y)
            canvasAnimacji.transform.rotation = Quaternion.Euler(0, 0, 270f);

        animator.SetBool("MeleeAttack", true);
    }
    public void animacjaPocisk(GameObject cel)
    {
        GameObject strzal = Instantiate(pocisk, jednostka.transform.position, Quaternion.identity); 
        // Vector3 newPosition = strzal.transform.position;
        // strzal.z = -3f; // Zmiana pozycji w trzecim wymiarze (Z)
        // strzal.transform.position = newPosition;
        strzal.GetComponent<Pocisk>().cel = cel;
    }
    IEnumerator AktualizujPołożenie()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 nowePołożenie = transform.position;
        Debug.Log(nowePołożenie + "   " + aktualnePołożenie);
        // Sprawdź, czy pozycja się zmieniła od ostatniego razu
        if (nowePołożenie != aktualnePołożenie)
        {
            Debug.Log("pizda");
            // Zaktualizuj położenie i statystyki, a następnie wywołaj RPC, aby poinformować innych graczy
            PhotonView photonView = GetComponent<PhotonView>();
            aktualnePołożenie = nowePołożenie;

            // Aktualizuj statystyki
            photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, druzyna, sojusz, HP, maxHP, atak, obrona, zasieg, maxszybkosc, szybkosc,
                            mindmg, maxdmg, zdolnosci, zbieracz, lata, cena, nazwa, akcja, spanie, nr_jednostki, koniec);

            // Aktualizuj położenie
            photonView.RPC("ZaktualizujPołożenieRPC", RpcTarget.All, aktualnePołożenie);
        }
    }

    [PunRPC]
    void ZaktualizujPołożenieRPC(Vector3 nowePołożenie)
    {
        // RPC wywołane na wszystkich klientach - zaktualizuj położenie jednostki
        transform.position = nowePołożenie;
    }

    [PunRPC]
    void ZaktualizujStatystykiRPC(int druzyna, int sojusz, float HP, float maxHP, float atak, float obrona, int zasieg, int maxszybkosc,
                                   int szybkosc, float mindmg, float maxdmg, int zdolnosci, bool zbieracz, bool lata, int cena,
                                   string nazwa, bool akcja, bool spanie, int nr_jednostki, bool koniec)
    {
        // RPC wywołane na wszystkich klientach - zaktualizuj statystyki jednostki
        this.druzyna = druzyna;
        this.sojusz = sojusz;
        this.HP = HP;
        this.maxHP = maxHP;
        this.atak = atak;
        this.obrona = obrona;
        this.zasieg = zasieg;
        this.maxszybkosc = maxszybkosc;
        this.szybkosc = szybkosc;
        this.mindmg = mindmg;
        this.maxdmg = maxdmg;
        this.zdolnosci = zdolnosci;
        this.zbieracz = zbieracz;
        this.lata = lata;
        this.cena = cena;
        this.nazwa = nazwa;
        this.akcja = akcja;
        this.spanie = spanie;
        this.nr_jednostki = nr_jednostki;
        this.koniec = koniec;
    }



    
    void Update()
    {
        if(MenuGlowne.multi)
        {
            aktualnePołożenie = transform.position;
            StartCoroutine(AktualizujPołożenie());
        }
        if(jednostka==Select)
            wybrane.enabled = true;
        else    
            wybrane.enabled = false;
        UpdateHealthBarColor();
        healthGracza.value = HP;
        healthGracza.maxValue = maxHP;

        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next && druzyna == Menu.tura)
        {
            koniec = false;
            koniecTury();
        }
        if (Input.GetKeyDown(KeyCode.Z) && druzyna == Menu.tura && jednostka==Select)
        {
            spanie = !spanie;
        }
        Wieza wieza = jednostka.GetComponent<Wieza>();
        if(HP<0.1 && (wieza==null || wieza.wybudowany))
        {
            umieranie();
        }
    }

    public void umieranie()
    {
            Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().Zajete = false;
            while(Menu.jednostki[druzyna,nr_jednostki+1] != null)
            {
                Menu.jednostki[druzyna,nr_jednostki] = Menu.jednostki[druzyna,nr_jednostki+1];
                Menu.jednostki[druzyna,nr_jednostki].GetComponent<Jednostka>().nr_jednostki -= 1;
                nr_jednostki++;
            }
            rozdajeExp();
            Menu.jednostki[druzyna,nr_jednostki] = null;
            Menu.ludnosc[druzyna]--;
            Heros heros = jednostka.GetComponent<Heros>(); if(heros == null)
            Destroy(jednostka);
            else
                jednostka.SetActive(false);
    }

    public void rozdajeExp()
    {
        for(int p = -4; p<=4;p++)
                for(int p2 = -4; p2<=4;p2++)
                {
                    if(Mathf.Abs(p2) + Mathf.Abs(p) < 5 && (int)(transform.position.x + p) >= 0 && (int)transform.position.x + p <= Menu.BoardSizeX-1 && (int)(transform.position.y + p2) >= 0 && (int)transform.position.y + p2 <= Menu.BoardSizeY-1)
                    {
                        if(Menu.kafelki[(int)transform.position.x + p][ (int)transform.position.y + p2].GetComponent<Pole>().postac != null)
                        {
                            GameObject heros = Menu.kafelki[(int)transform.position.x + p][ (int)transform.position.y + p2].GetComponent<Pole>().postac;
                            Heros herosSkrypt = heros.GetComponent<Heros>();
                            if(herosSkrypt != null && heros.GetComponent<Jednostka>().sojusz != sojusz)
                            {
                                herosSkrypt.exp += cena;
                            }
                        }
                    }
                }
    }

    private void koniecTury()
    {
        szybkosc = maxszybkosc;
        akcja = true;
        switch(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().magia)
        {
            case 1: HP+=1 ;if(HP>maxHP) HP=maxHP; break;
            case 2: HP+=2 ;if(HP>maxHP) HP=maxHP; break;
            case 3: HP+=4 ;if(HP>maxHP) HP=maxHP; break; 
        }
    }


        void UpdateHealthBarColor()
    {
        float normalizedHP = HP / maxHP;
        Color healthColor = gradient.Evaluate(normalizedHP); // Pobierz kolor z Gradientu

        // Ustaw kolor paska zdrowia
        Image fillImage = healthGracza.fillRect.GetComponent<Image>();
        fillImage.color = healthColor;
    }
        void OnMouseEnter()
    {
        if(druzyna!=Menu.tura && Select!=null && CzyJednostka && Select.GetComponent<Jednostka>().zasieg >= Walka.odleglosc(Select, jednostka) && Select.GetComponent<Jednostka>().akcja && !Jednostka.wybieranie && Select.GetComponent<Jednostka>().druzyna != druzyna)
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        if(!Jednostka.wybieranie)
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
