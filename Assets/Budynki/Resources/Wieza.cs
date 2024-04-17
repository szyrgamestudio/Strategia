using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Wieza : MonoBehaviour
{
    public float targetZ = -1.0f; // Pożądana pozycja 'z'
    public bool wybudowany;
    public float targetX; // Przesuń te deklaracje zmiennych na poziom klasy, nie w funkcji Update
    public float targetY;
    public GameObject ObiektRuszany;

    public static GameObject budowlaniec;
    public int druzyna;
    public GameObject budynek;
    public Text TextShowDMG;

    public static bool pomoc = false;
    public Slider healthGracza;
    public Gradient gradient;

    private bool update;

    public int punktyBudowy;
    public int punktyBudowyMax;

    private bool dodajHP = false;
     public Texture2D buildCursor;

    

    void Start()
    {
        healthGracza.maxValue = punktyBudowyMax;
        healthGracza.value = punktyBudowy;
        // Menu.ludnosc[druzyna]--;
        moznaBudowac = false;
    }

    void Update()
    {
        if(Jednostka.Select2!=null)
            Debug.Log(Jednostka.Select2.name);
        if(punktyBudowy<punktyBudowyMax)
        {
            budynek.GetComponent<Jednostka>().akcja = false;
            healthGracza.maxValue = punktyBudowyMax;
            healthGracza.value = punktyBudowy;
            if(budynek.GetComponent<Jednostka>().HP>0.2f)
            {
                budynek.GetComponent<Jednostka>().HP = punktyBudowy;
            }
            budynek.GetComponent<Jednostka>().maxHP = punktyBudowyMax;
        }
        else
        {
            healthGracza.maxValue = budynek.GetComponent<Jednostka>().maxHP;
            healthGracza.value = budynek.GetComponent<Jednostka>().HP;  
        }
        if(Jednostka.Select==budynek)
        {
            Pole.Clean2();
        }
        if(punktyBudowy<punktyBudowyMax)
            budynek.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        else
        {
            budynek.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
            if(!dodajHP)
            {
                budynek.GetComponent<Jednostka>().HP = 7;
                budynek.GetComponent<Jednostka>().maxHP = 7;
                dodajHP = true;
            }
        }
          //  budynek.GetComponent<SpriteRenderer>().sprite = budynekArt;
        UpdateHealthBarColor();
    }
    public void dedMulti()
    {
        if(MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ded", RpcTarget.All);
        }
    }
    void LateUpdate()
    {
        if (wybudowany == false && (!MenuGlowne.multi || Menu.tura == Ip.ip)) // Użyj '==' do porównywania, a nie '='
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            targetX = Mathf.Round(mousePosition.x);
            targetY = Mathf.Round(mousePosition.y);
            transform.position = new Vector3(targetX, targetY, targetZ);
            if (Input.GetMouseButtonDown(0))
            {
                if (targetX < Menu.BoardSizeX - 1 && targetY < Menu.BoardSizeY - 1 && targetX > 0 && targetY > 0 && Walka.odleglosc(budowlaniec,ObiektRuszany) == 1 && punktyBudowy < punktyBudowyMax) 
                    {
                        if (!Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().Zajete)
                        {
                            update = true;
                            wybudowany = true;
                            Budowlaniec.wybieranie = false;
                            pomoc = true;
                            budowlaniec.GetComponent<Budowlaniec>().BudowanyObiekt = null;
                            Menu.zloto[Menu.tura] -= 3;
                            Menu.drewno[Menu.tura] -= 7;
                            Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().Zajete=true;
                            Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().postac=ObiektRuszany;
                            Pole.Clean2();
                        }
                    }
            }
            if (targetX < Menu.BoardSizeX - 1 && targetY < Menu.BoardSizeY - 1 && targetX > 0 && targetY > 0 && Walka.odleglosc(budowlaniec,ObiektRuszany) == 1) 
            {
                ObiektRuszany.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
            }
            else
            {
                ObiektRuszany.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
            }

            Pole.Clean2();
        }  
        else{
            if(update && MenuGlowne.multi)
            {
                update = false;
                Vector3 nowePołożenie = transform.position;
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("ZaktualizujPołożenieRPC", RpcTarget.All, nowePołożenie);
                photonView.RPC("ZaktualizujWybudowany", RpcTarget.All, druzyna);
            }
        }
    }
    [PunRPC]
    void buduj(int ip, int buduj)
    {
        if(ip!=Ip.ip)
        {
            punktyBudowy += buduj;
        }
    }
    void OnMouseDown()
    {
        if (moznaBudowac)
        {
            Debug.Log("jeden");
            punktyBudowy += budujacy.GetComponent<Budowlaniec>().punktyBudowy + Budowlaniec.punktyBudowyBonus[Menu.tura];
            Debug.Log("dwa");
            if(MenuGlowne.multi)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("buduj", RpcTarget.All, Ip.ip,(budujacy.GetComponent<Budowlaniec>().punktyBudowy + Budowlaniec.punktyBudowyBonus[Menu.tura]));
            }
            budujacy.GetComponent<Jednostka>().akcja = false;
            ShowDMG(budujacy.GetComponent<Budowlaniec>().punktyBudowy + Budowlaniec.punktyBudowyBonus[Menu.tura], new Color(255 / 255.0f, 165 / 255.0f, 0 / 255.0f, 0.0f));
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            moznaBudowac = false;
        }
        else
        {
            if(druzyna==Menu.tura)
            {

            }
            else
            {
                Jednostka Atakujacy = Jednostka.Select.GetComponent<Jednostka>();
                if(Atakujacy.zasieg>=Walka.odleglosc(budynek, Jednostka.Select) && Atakujacy.akcja && Jednostka.Select.GetComponent<Jednostka>().druzyna == Menu.tura)
                {
                    if(punktyBudowy >= punktyBudowyMax)
                    {
                        if(punktyBudowy == 0)
                        {
                            if(MenuGlowne.multi)
                            {
                                PhotonView photonView = GetComponent<PhotonView>();
                                photonView.RPC("ded", RpcTarget.All);
                            }
                            Menu.kafelki[(int)budynek.transform.position.x][(int)budynek.transform.position.y].GetComponent<Pole>().Zajete = false;
                            Destroy(budynek);
                        }
                        else
                        {
                            ShowDMG(punktyBudowy, new Color(1.0f, 0.0f, 0.0f, 1.0f));
                            punktyBudowy = 0;
                        }
                    }
                    Atakujacy.akcja = false;
                }
            }
        }   
    }
    private bool moznaBudowac;
    private GameObject budujacy;
    void OnMouseEnter()
    {
        if(Jednostka.Select != null && Jednostka.CzyJednostka && budynek.GetComponent<Jednostka>().sojusz == Jednostka.Select.GetComponent<Jednostka>().sojusz  && Jednostka.Select.GetComponent<Budowlaniec>() != null  && Walka.odleglosc(Jednostka.Select,budynek)==1) 
        if((punktyBudowy < punktyBudowyMax) && Jednostka.Select.GetComponent<Jednostka>().akcja && wybudowany)
        {
            Cursor.SetCursor(buildCursor, Vector2.zero, CursorMode.Auto);
            moznaBudowac = true;
            budujacy = Jednostka.Select;
        }
            
    }
    void OnMouseExit()
    {
        if(!Jednostka.wybieranie)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        moznaBudowac = false;
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
        void UpdateHealthBarColor()
    {
        Jednostka jednostka = budynek.GetComponent<Jednostka>();
        float normalizedHP;
        if(punktyBudowy<punktyBudowyMax)
            normalizedHP = punktyBudowy / punktyBudowyMax;
        else
            normalizedHP = jednostka.HP / jednostka.maxHP;
        Color healthColor = gradient.Evaluate(normalizedHP); // Pobierz kolor z Gradientu

        // Ustaw kolor paska zdrowia
        Image fillImage = healthGracza.fillRect.GetComponent<Image>();
        fillImage.color = healthColor;
    }
    [PunRPC]
    void ZaktualizujPołożenieRPC(Vector3 nowePołożenie)
    {
        transform.position = nowePołożenie;
    }
    [PunRPC]
    void ded()
    {
        Destroy(ObiektRuszany);
    }
    [PunRPC]
    void ZaktualizujWybudowany(int druzyna)
    {
        this.wybudowany = true;
        Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y].GetComponent<Pole>().Zajete = true;
        Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y].GetComponent<Pole>().postac = ObiektRuszany;
        this.druzyna = druzyna;
    }
}
