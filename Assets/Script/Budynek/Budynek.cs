using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Budynek : MonoBehaviour
{
    public GameObject budynek;
    public int druzyna;
    public int sojusz;
    public float HP;
    public float maxHP;
    public int obrona;
    public int zdolnosci;
    public int zloto;
    public int drewno;
    public string nazwa;

    public int punktyBudowy;
    public int punktyBudowyMax;

    public Slider healthGracza;
    public Gradient gradient;
    public Image obramowka;
    public Image wybrane;
    public Text TextShowDMG;
    public Texture2D customCursor;
    public Texture2D buildCursor;
    public int rodzaj; //0 przyciski 1 kopalnia
    public Image strzalka;
    private bool moznaBudowac;

    //[HideInInspector]
    public int poZniszczeniu;
    
    public Sprite budowaArt;
    public Sprite budynekArt;

    public bool update;

    public float damage; //informacja o dmg dla innych plikow pomocnicza

    public void Start()
    {
        switch(druzyna)
        {
            case 1: obramowka.color = new Color(1.0f, 0.0f, 0.0f); break;
            case 2: obramowka.color = new Color(0.0f, 1.0f, 0.0f); break;
            case 3: obramowka.color = new Color(0.0f, 0.0f, 1.0f); break;
            case 4: obramowka.color = new Color(1.0f, 1.0f, 0.0f); break;
        }
        healthGracza.maxValue = punktyBudowyMax;
        healthGracza.value = punktyBudowy;
        sojusz = WyburRas.team[druzyna-1] + 1;
         
    }
    public void OnMouseDown()
    {
        if(!Menu.NIERUSZAC)
            OnMouse();
    }
    public void OnMouse()
    {
        if (moznaBudowac)
        {
            punktyBudowy += Jednostka.Select.GetComponent<Budowlaniec>().punktyBudowy + Budowlaniec.punktyBudowyBonus[Menu.tura];
            if(MenuGlowne.multi)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("budowanieMulti", RpcTarget.All, Ip.ip, Jednostka.Select.GetComponent<Budowlaniec>().punktyBudowy + Budowlaniec.punktyBudowyBonus[Menu.tura]);
            }
            Jednostka.Select.GetComponent<Budowlaniec>().GetComponent<Jednostka>().akcja = false;
            ShowDMG(Jednostka.Select.GetComponent<Budowlaniec>().punktyBudowy + Budowlaniec.punktyBudowyBonus[Menu.tura], new Color(255 / 255.0f, 165 / 255.0f, 0 / 255.0f, 0.0f));
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            moznaBudowac = false;
        }
        else
        {
        Menu.przyciskiClear();
        if(Jednostka.wybieranie)
            {
                Jednostka.Select2 = budynek;
                Jednostka.CzyJednostka2 = false;
            }
        else
        {
            if(budynek == Jednostka.Select)
            {
                Interface.przeniesDoSelect();
            }
            if(druzyna==Menu.tura)
            {
                if(druzyna==Menu.tura)
                {
                    Menu.PanelBuild.SetActive(true);
                    Menu.PanelUnit.SetActive(false);
                    Jednostka.CzyJednostka = false;
                    Jednostka.Select = budynek;
                    Pole.Clean2();
                    if(rodzaj==0 || rodzaj==2 || rodzaj==3)
                    {
                        for(int i=0;i<16;i++)
                        {
                            InterfaceBuild.przyciski[i].SetActive(false);
                            if(zdolnosci>i)
                                InterfaceBuild.przyciski[i].SetActive(true);
                        }
                    }
                    if(rodzaj==1)
                    {
                        for(int i=1;i<7;i++)
                        {
                            InterfaceBuild.obrazkikopalnia[i].SetActive(false);
                            if(zdolnosci>=i)
                                InterfaceBuild.obrazkikopalnia[i].SetActive(true);
                        }
                    }

                }
            }
            else
            {
                zaatakowanie();
            }
        }
        }
    }

    [PunRPC]
    public void zaatakowanieMulti(int ip, float hp)
    {
        if(ip != Ip.ip)
            HP -= hp;
    }
    [PunRPC]
    public void budowanieMulti(int ip, int hp)
    {
        if(ip != Ip.ip)
            punktyBudowy += hp;
    }

    public void zaatakowanie()
    {
        Jednostka Atakujacy = Jednostka.Select.GetComponent<Jednostka>();
                if(Atakujacy != null && Atakujacy.zasieg>=Walka.odleglosc(budynek, Jednostka.Select) && Atakujacy.akcja && Atakujacy.druzyna == Menu.tura && Atakujacy.sojusz != sojusz)
                    {
                        if(punktyBudowy >= punktyBudowyMax)
                        {
                            float result = UnityEngine.Random.Range(Atakujacy.mindmg, Atakujacy.maxdmg) * (float)(1 + 0.1 * (Atakujacy.atak - obrona));
                            float roundedResult = (float)Math.Round(result, 1);

                            if(roundedResult<1)
                            {
                                roundedResult = 1;
                            }
                                if(MenuGlowne.multi)
                            {
                                PhotonView photonView = GetComponent<PhotonView>();
                                photonView.RPC("zaatakowanieMulti", RpcTarget.All, Ip.ip, roundedResult);
                            }
                            HP -= roundedResult;
                            damage = roundedResult;
                            if(HP<=0)
                            {
                                if(MenuGlowne.multi)
                                {
                                    PhotonView photonView = GetComponent<PhotonView>();
                                    photonView.RPC("deadMulti", RpcTarget.All, Ip.ip);
                                }
                                Menu.kafelki[(int)budynek.transform.position.x][(int)budynek.transform.position.y].GetComponent<Pole>().Zajete = false;
                                if(poZniszczeniu==0)
                                    Destroy(budynek);
                                else{poZniszczeniu=2;}
                            }
                            ShowDMG(roundedResult, new Color(1.0f, 0.0f, 0.0f, 1.0f));
                            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        }
                        else
                        {
                            if(punktyBudowy == 0)
                            {
                                if(MenuGlowne.multi)
                                {
                                    PhotonView photonView = GetComponent<PhotonView>();
                                    photonView.RPC("deadMulti", RpcTarget.All, Ip.ip);
                                }
                                Menu.kafelki[(int)budynek.transform.position.x][(int)budynek.transform.position.y].GetComponent<Pole>().Zajete = false;
                                if(poZniszczeniu==0)
                                    Destroy(budynek);
                                else{poZniszczeniu=2;}
                            }
                            else
                            {
                                ShowDMG(punktyBudowy, new Color(1.0f, 0.0f, 0.0f, 1.0f));
                                if(MenuGlowne.multi)
                                {
                                    PhotonView photonView = GetComponent<PhotonView>();
                                    photonView.RPC("budowanieMulti", RpcTarget.All, Ip.ip, -punktyBudowy);
                                }
                                punktyBudowy = 0;
                            }
                        }
                    Atakujacy.akcja = false;
                    }
    }

    void Update()
    {
    //      if(MenuGlowne.multi && !update)
    //     {
    //         StartCoroutine(AktualizujPołożenie(HP,punktyBudowy));
    //     }
        if(punktyBudowy<punktyBudowyMax)
         //   budynek.GetComponent<SpriteRenderer>().sprite = budowaArt;
            budynek.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        else
          //  budynek.GetComponent<SpriteRenderer>().sprite = budynekArt;
            budynek.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);

        if(budynek==Jednostka.Select)
        {
            wybrane.enabled = true;
            strzalka.enabled = true;
        }
        else 
        {
            wybrane.enabled = false;
            if(budynek.GetComponent<BudynekRuch>().wybudowany)
                strzalka.enabled = false;
        }   

        UpdateHealthBarColor();
        if(punktyBudowy<punktyBudowyMax)
        {
            healthGracza.value = punktyBudowy;
            healthGracza.maxValue = punktyBudowyMax;
        }
        else{
            healthGracza.value = HP;
            healthGracza.maxValue = maxHP;
        }   
        if(update)
            StartCoroutine(DisableAfterDelay());
    }

    [PunRPC]
    public void deadMulti(int ip)
    {
        if(ip != Ip.ip)
        {
            Menu.kafelki[(int)budynek.transform.position.x][(int)budynek.transform.position.y].GetComponent<Pole>().Zajete = false;
            if(poZniszczeniu==0)
                Destroy(budynek);
            else{poZniszczeniu=2;}
        }
    }

        IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(0.3f); // Oczekiwanie przez 1 sekundę
        update = false;
    }
        IEnumerator AktualizujPołożenie(float HP, int punktyBudowy)
    {
        yield return new WaitForSeconds(0.1f);
        if (punktyBudowy != this.punktyBudowy || HP != this.HP && !update)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, druzyna, sojusz, HP,  maxHP, obrona, zdolnosci, zloto, drewno, punktyBudowy, punktyBudowyMax, poZniszczeniu);
        }
    }
    public void Aktualizuj()
    {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, druzyna, sojusz, HP,  maxHP, obrona, zdolnosci, zloto, drewno, punktyBudowy, punktyBudowyMax, poZniszczeniu);
    }

    [PunRPC]
    void ZaktualizujStatystykiRPC(int druzyna, int sojusz, float HP, float maxHP, int obrona, int zdolnosci, int zloto, int drewno, int punktyBudowy, int punktyBudowyMax, int poZniszczeniu)
    {
        this.druzyna = druzyna;
        this.sojusz = sojusz;
        this.HP = HP;
        this.maxHP = maxHP;
        this.obrona = obrona;
        this.zdolnosci = zdolnosci;
        this.zloto = zloto;
        this.drewno = drewno;
        this.punktyBudowy = punktyBudowy;
        this.punktyBudowyMax = punktyBudowyMax;

        this.poZniszczeniu = poZniszczeniu;
        this.update = true;
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
         // Wartość alpha (przezroczystość) od 0 (pełna przezroczystość) do 1 (brak przezroczystości)
        
        for(int i=0;i<40;i++)
        {
            textTransform.anchoredPosition = new Vector2(x, 10.4f + 0.16f*(float)i);
            yield return new WaitForSeconds(0.04f);
            textColor.a = 1f - 0.025f*(float)i;
            TextShowDMG.color = textColor;
        }
    }

        void UpdateHealthBarColor()
    {
        float normalizedHP;
        if(punktyBudowy<punktyBudowyMax)
            normalizedHP = punktyBudowy / punktyBudowyMax;
        else
            normalizedHP = HP / maxHP;
        Color healthColor = gradient.Evaluate(normalizedHP); // Pobierz kolor z Gradientu

        // Ustaw kolor paska zdrowia
        Image fillImage = healthGracza.fillRect.GetComponent<Image>();
        fillImage.color = healthColor;
    }

        void OnMouseEnter()
    {
        if(sojusz!=Menu.tura && Jednostka.Select!=null && Jednostka.CzyJednostka && Jednostka.Select.GetComponent<Jednostka>().zasieg >= Walka.odleglosc(Jednostka.Select, budynek) && Jednostka.Select.GetComponent<Jednostka>().akcja && !Jednostka.wybieranie && Jednostka.Select.GetComponent<Jednostka>().sojusz != sojusz)
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        if(Jednostka.Select != null && Jednostka.CzyJednostka && budynek.GetComponent<Budynek>().sojusz == Jednostka.Select.GetComponent<Jednostka>().sojusz  && Jednostka.Select.GetComponent<Budowlaniec>() != null  && Walka.odleglosc(Jednostka.Select,budynek)==1) 
        if((punktyBudowy < punktyBudowyMax) && Jednostka.Select.GetComponent<Jednostka>().akcja && budynek.GetComponent<BudynekRuch>().wybudowany)
        {
            Cursor.SetCursor(buildCursor, Vector2.zero, CursorMode.Auto);
            moznaBudowac = true;
        }
            
    }

    void OnMouseExit()
    {
        if(!Jednostka.wybieranie)
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        moznaBudowac = false;
    }
}
