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
    public bool poczatek;

    public Slider healthGracza;
    public Gradient gradient;
    public Image obramowka;
    public Image wybrane;
    public Text TextShowDMG;
    public Texture2D customCursor;

    public GameObject canvasAnimacji;
    public Animator animator;
    public GameObject pocisk;

    //private bool wybrany;
    public AudioSource src;

    public int drewno;

    public bool elf;
    public bool boss = false;

    public void Start()
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
        if(!GetComponent<Wieza>())
            StartCoroutine(przyporzadkuj());
        odkryj(4);
    }

    IEnumerator przyporzadkuj()
    {
        yield return new WaitForSeconds(0.2f);
        
        Menu.jednostki[druzyna , Menu.ludnosc[druzyna]] = jednostka;
        nr_jednostki = Menu.ludnosc[druzyna];
        Menu.ludnosc[druzyna]++;
        
    }

    public void walkSound()
    {
        src.clip = Sound.sound.GetComponent<Sound>().walk;
        src.Play();
    }

    public void rozlozenie()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("rozlozenieMulti", RpcTarget.All);
    }
    [PunRPC]
    public void poprawkaMulti(float x, float y)
    {
        if(jednostka.transform.position.x != x || jednostka.transform.position.y != y)
        {
            if(lata)
            {
                Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().ZajeteLot = false;
                Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().ZajeteLot = true;
            }
            else
            {
                Menu.kafelki[(int)(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().Zajete = false;
                Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().Zajete = true;
            }
            if(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().postac == jednostka)
            {
                Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().postac = null;
            }
            Menu.kafelki[(int)x][(int)y].GetComponent<Pole>().postac = jednostka;
            Vector3 wekto = new Vector3(x, y, jednostka.transform.position.z);
            jednostka.transform.position = wekto;

        }
    }
    [PunRPC]
    public void rozlozenieMulti()
    {
        Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().Zajete = true;
        Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().postac = jednostka;
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
                if(!(MenuGlowne.multi && Menu.tura==0))
                {
                    Interface.przeniesDoSelect();
                }
            }

            if(Select != null && CzyJednostka && Select.GetComponent<Jednostka>().druzyna == Menu.tura && sojusz != Select.GetComponent<Jednostka>().sojusz && 
            (Select.GetComponent<Jednostka>().zasieg>=Walka.odleglosc(jednostka, Select) && Select.GetComponent<Jednostka>().akcja && (Select.GetComponent<Jednostka>().zasieg > 1 || Select.GetComponent<Jednostka>().lata || !jednostka.GetComponent<Jednostka>().lata)))
            {
                
                
                zaatakowanie(Jednostka.Select);
                

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

    public Jednostka tarczownik()
    {
        int x = (int)jednostka.transform.position.x;
        int y = (int)jednostka.transform.position.y;
        if(tarczownik2(x+1,y) != null)
            return tarczownik2(x+1,y);
        if(tarczownik2(x-1,y) != null)
            return tarczownik2(x-1,y);
        if(tarczownik2(x,y+1) != null)
            return tarczownik2(x,y+1);
        if(tarczownik2(x,y-1) != null)
            return tarczownik2(x,y-1);
        return null;
        
    }
    private Jednostka tarczownik2(int x, int y)
    {
        if(Menu.istnieje(x,y) && Menu.kafelki[x][y].GetComponent<Pole>().postac != null && Menu.kafelki[x][y].GetComponent<Pole>().postac.GetComponent<Tarczownik>() && 
        Menu.kafelki[x][y].GetComponent<Pole>().postac.GetComponent<Jednostka>().sojusz == sojusz)
        {
            return Menu.kafelki[x][y].GetComponent<Pole>().postac.GetComponent<Jednostka>();
        }
        return null;
    }

    
    [PunRPC]
    public void zaatakowanieMulti(int ip, float hp)
    {
        if(ip != Ip.ip)
        {
            src.clip = Sound.sound.GetComponent<Sound>().damage;
            if(Menu.kafelki[(int)transform.position.x][(int)transform.position.y].GetComponent<PoleOdkryj>().dark == null)
                src.Play();
            HP -= hp;
        }
    }

    public void zaatakowanie(GameObject atakujacy)
    {
        if(tarczownik() != null && !jednostka.GetComponent<Tarczownik>())
            tarczownik().zaatakowanie(Jednostka.Select);
        else
        {
        if(atakujacy.GetComponent<Jednostka>().nazwa == "Wilk")
            atakujacy.GetComponent<Jednostka>().atak += atakujacy.GetComponent<Wilk>().dodatkowyAtak();
        if(End.boss && boss)
        {
            Ending.wygrany = atakujacy.GetComponent<Jednostka>().sojusz;
        }
        Jednostka Atakujacy = atakujacy.GetComponent<Jednostka>();
        if(Atakujacy.zasieg>=Walka.odleglosc(jednostka, atakujacy) && Atakujacy.akcja && (Atakujacy.zasieg > 1 || Atakujacy.lata || !jednostka.GetComponent<Jednostka>().lata))
         {
            if(druzyna != 0 && Atakujacy.druzyna != 0)
            {
                Muzyka.muzyka.progresja[Atakujacy.druzyna - 1] = 2;
                Muzyka.muzyka.progresja[druzyna - 1] = 2;
                Muzyka.muzyka.tury2[Atakujacy.druzyna - 1] = Menu.nrTury;
                Muzyka.muzyka.tury2[druzyna - 1] = Menu.nrTury;
                if(MenuGlowne.multi)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("updateMusic", RpcTarget.All, Atakujacy.druzyna, druzyna);
                }
            }
            if(jednostka.GetComponent<Golem>())
                jednostka.GetComponent<Golem>().zadaj(atakujacy);
            if(Atakujacy.GetComponent<Ent>())
            {
                szybkosc = 0;
                jednostka.GetComponent<Buff>().buffZ(0,0,-maxszybkosc,0,0,0);
                maxszybkosc = 0;
            }
            float result;
            if(atakujacy.GetComponent<Jednostka>().lata || jednostka.GetComponent<Jednostka>().lata)
                result = UnityEngine.Random.Range(Atakujacy.mindmg, Atakujacy.maxdmg) * (float)(1 + 0.1 * (Atakujacy.atak - obrona));
            else
            {
            result = UnityEngine.Random.Range((float)Atakujacy.mindmg, (float)Atakujacy.maxdmg) * (1f + 0.1f * ((float)Atakujacy.atak + (Menu.kafelki[(int)atakujacy.transform.position.x][(int)atakujacy.transform.position.y].GetComponent<Pole>().poziom -Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().poziom) - (float)obrona));
            }

            float roundedResult = (float)Math.Round(result, 1);

            if(atakujacy.GetComponent<Jednostka>().nazwa == "Wilk")
                atakujacy.GetComponent<Jednostka>().atak -= atakujacy.GetComponent<Wilk>().dodatkowyAtak();
            if(Walka.odleglosc(jednostka, atakujacy)==1)
                Atakujacy.animacjaMiecz(jednostka);
            else
                Atakujacy.animacjaPocisk(jednostka);
            if(roundedResult<1)
            {
                roundedResult = 1;
            }
                if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("zaatakowanieMulti", RpcTarget.All,Ip.ip, roundedResult);
                    }
            src.clip = Sound.sound.GetComponent<Sound>().damage;
            if(Menu.kafelki[(int)transform.position.x][(int)transform.position.y].GetComponent<PoleOdkryj>().dark == null)
                src.Play();
            if(atakujacy.GetComponent<Wampir>())
            {
                atakujacy.GetComponent<Jednostka>().HP += atakujacy.GetComponent<Wampir>().lifeSteal * roundedResult;
                Jednostka.Select.GetComponent<Jednostka>().ShowDMG((atakujacy.GetComponent<Wampir>().lifeSteal * roundedResult),new Color(0.0f, 1.0f, 0.0f, 1.0f));
                if(MenuGlowne.multi)
                    {
                        PhotonView photonView = atakujacy.GetComponent<PhotonView>();
                        photonView.RPC("zaatakowanieMulti", RpcTarget.All, Ip.ip, -atakujacy.GetComponent<Wampir>().lifeSteal * roundedResult);
                    }
            }

            if(HP - roundedResult < 0.1)
            {
                if(druzyna == 0 || Atakujacy.druzyna == 0)
                {
                    if(druzyna == 0)
                    {
                        Muzyka.muzyka.progresja[Atakujacy.druzyna - 1] = 1;
                        Muzyka.muzyka.tury1[Atakujacy.druzyna - 1] = Menu.nrTury;

                    }
                    else
                    {
                        Muzyka.muzyka.progresja[druzyna - 1] = 1;
                        Muzyka.muzyka.tury1[druzyna - 1] = Menu.nrTury;
                    }
                }
            }
            HP -= roundedResult;

            ShowDMG(roundedResult, new Color(1.0f, 0.0f, 0.0f, 1.0f));

            if(atakujacy.GetComponent<Kamikaze>())
            {
                Atakujacy.HP = 0;
                Atakujacy.Aktualizuj();
            }
            Atakujacy.akcja = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        }
    }

    [PunRPC]
    public void updateMusic(int a, int b)
    {
        Muzyka.muzyka.progresja[a - 1] = 2;
        Muzyka.muzyka.progresja[b - 1] = 2;
        Muzyka.muzyka.tury2[a - 1] = Menu.nrTury;
        Muzyka.muzyka.tury2[b - 1] = Menu.nrTury;
    }

    public void odkryj(int zasieg)
    {
        if(MenuGlowne.multi && druzyna == Ip.ip)
        for(int p = -zasieg; p<=zasieg;p++)
            for(int p2 = -zasieg; p2<=zasieg;p2++)
            {
                if(Mathf.Abs(p) + Mathf.Abs(p2) <= zasieg &&Menu.istnieje(p+(int)transform.position.x,p2+ (int)transform.position.y) && Menu.kafelki[p+(int)transform.position.x][p2+(int)transform.position.y].GetComponent<PoleOdkryj>().dark != null)
                    Menu.kafelki[p+(int)transform.position.x][p2+(int)transform.position.y].GetComponent<PoleOdkryj>().remove();
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
        src.clip = Sound.sound.GetComponent<Sound>().sword;
        if(Menu.kafelki[(int)transform.position.x][(int)transform.position.y].GetComponent<PoleOdkryj>().dark == null)
            src.Play();
    }
    public void animacjaPocisk(GameObject cel)
    {
        GameObject strzal = Instantiate(pocisk, jednostka.transform.position, Quaternion.identity); 
        strzal.GetComponent<Pocisk>().cel = cel;
        src.clip = Sound.sound.GetComponent<Sound>().magic;
        if(Menu.kafelki[(int)transform.position.x][(int)transform.position.y].GetComponent<PoleOdkryj>().dark == null)
            src.Play();

    }

    public void Aktualizuj()
    {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, druzyna, sojusz, this.HP, maxHP, atak, obrona, zasieg, maxszybkosc, szybkosc,
                            mindmg, maxdmg, zdolnosci, zbieracz, lata, cena, nazwa, akcja, nr_jednostki, koniec, Ip.ip);

            //photonView.RPC("ZaktualizujPołożenieRPC", RpcTarget.All, aktualnePołożenie);
    }
    public void AktualizujPol()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("ZaktualizujPołożenieRPC", RpcTarget.All, jednostka.transform.position, Ip.ip);
    }

    [PunRPC]
    void ZaktualizujPołożenieRPC(Vector3 nowePołożenie, int ip)
    {
        // RPC wywołane na wszystkich klientach - zaktualizuj położenie jednostki
        if(ip != Ip.ip)
        {
            transform.position = nowePołożenie;
            if(!lata)
                Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().Zajete = true;
            else
                Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().ZajeteLot = true;
            Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().postac = jednostka;
        }
    }

    [PunRPC]
    void ZaktualizujStatystykiRPC(int druzyna, int sojusz, float HP, float maxHP, float atak, float obrona, int zasieg, int maxszybkosc,
                                   int szybkosc, float mindmg, float maxdmg, int zdolnosci, bool zbieracz, bool lata, int cena,
                                   string nazwa, bool akcja, int nr_jednostki, bool koniec, int ip)
    {
        if(ip != Ip.ip)
        {
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
        this.nr_jednostki = nr_jednostki;
        this.koniec = koniec;
        poczatek = true;
            switch(druzyna)
            {
                case 0: obramowka.color = new Color(0.0f, 0.0f, 0.0f); break;
                case 1: obramowka.color = new Color(1.0f, 0.0f, 0.0f); break;
                case 2: obramowka.color = new Color(0.0f, 1.0f, 0.0f); break;
                case 3: obramowka.color = new Color(0.0f, 0.0f, 1.0f); break;
                case 4: obramowka.color = new Color(1.0f, 1.0f, 0.0f); break;
            }
        }
    }



    
    void Update()
    {
        // if(jednostka == Select)
        //     wybrany = true;
        // else
        //     wybrany = false;
        if(jednostka==Select)
            wybrane.enabled = true;
        else    
            wybrane.enabled = false;
        UpdateHealthBarColor();
        healthGracza.value = HP;
        healthGracza.maxValue = maxHP;
        if(HP > maxHP)
            HP = maxHP;
        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next )//&& (druzyna+1)%(Menu.IloscGraczy+1) == Menu.tura)
        {
            koniec = false;
            if(MenuGlowne.multi && !poczatek)
            {
                poczatek = true;
                Aktualizuj();
                if(jednostka == Menu.heros[druzyna])
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("heros", RpcTarget.All);
                }
            }
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

    [PunRPC]
    public void heros()
    {
        Menu.heros[druzyna] = jednostka;
    }

    public virtual void umieranie()
    {
            if(End.boss && boss)
            {
                End.bossPokonany = true;
            }
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

    [PunRPC]
    public void zdradaMulti(int ip, int poziom)
    {
        if(Ip.ip != ip)
            zdrada(poziom);
    }

    public void zdrada(int poziom)
    {
        if(MenuGlowne.multi && poziom == 0)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("zdradaMulti", RpcTarget.All, Ip.ip, 1);
        }
        nr_jednostki = Menu.NPC.Count;
        Menu.NPC.Add(jednostka);
        spanie = true;
        druzyna = 0;
        obramowka.color = new Color(0.0f, 0.0f, 0.0f);
    }

    private void koniecTury()
    {
        if(druzyna != 0 && WyburRas.aktywny[druzyna-1] == false)
        {
            zdrada(0);
        }
        szybkosc = maxszybkosc;
        akcja = true;
        if((druzyna == (Menu.tura+Menu.IloscGraczy)%(Menu.IloscGraczy+1)) || (SimultanTurns.simultanTurns && SimultanTurns.ready))
        {
            switch(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().magia)
            {
                case 1: HP+=1 ;if(HP>maxHP) HP=maxHP; break;
                case 2: HP+=2 ;if(HP>maxHP) HP=maxHP; break;
                case 3: HP+=4 ;if(HP>maxHP) HP=maxHP; if(!lata) rozdajPunktyKontroli(false); break; 
            }
            if(Menu.kafelki[(int)jednostka.transform.position.x][(int)jednostka.transform.position.y].GetComponent<Pole>().las && elf)
            {
                HP+=1 ;if(HP>maxHP) HP=maxHP;
            }
        }
        if(MenuGlowne.multi && (druzyna == Ip.ip || druzyna == 0))
        {
            Aktualizuj();
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("poprawkaMulti", RpcTarget.All, jednostka.transform.position.x, jednostka.transform.position.y);
        }

    }

    void rozdajPunktyKontroli(bool powtorz)
    {
        if(MenuGlowne.multi && !powtorz  && (!SimultanTurns.simultanTurns))
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("rozdajMulti", RpcTarget.All, Ip.ip);
        }
        if(End.control )
        {
            if(End.druzynaKontroli == sojusz)
            {
                End.punktyKontroli++;
            }
            else
            {
                if(End.punktyKontroli == 0)
                {
                    End.druzynaKontroli = sojusz;
                    End.punktyKontroli++;
                }
                else
                    End.punktyKontroli--;
            }
        }
    }
    [PunRPC]
    void rozdajMulti(int ip)
    {
        if(ip != Ip.ip)
        {
            rozdajPunktyKontroli(true);
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
        if(druzyna!=Menu.tura && Select!=null && CzyJednostka && Select.GetComponent<Jednostka>().zasieg >= Walka.odleglosc(Select, jednostka) && (Select.GetComponent<Jednostka>().akcja) && !Jednostka.wybieranie && Select.GetComponent<Jednostka>().druzyna != druzyna && (!MenuGlowne.multi || (Select.GetComponent<Jednostka>().druzyna == Ip.ip)))
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }    
    }

    void OnMouseExit()
    {
        if(!Jednostka.wybieranie)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
