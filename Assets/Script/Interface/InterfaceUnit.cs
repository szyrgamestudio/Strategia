using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InterfaceUnit : MonoBehaviour
{
    public Text TAtak;
    public Text TObrona;
    public Text Tdmg;
    public Text TSzybkosc;
    public Text TZasieg;
    public Text TNazwa;
    public Text level;

    public Slider healthGracza;
    public Gradient gradient;
    public TextMeshProUGUI THP;

    public Slider ExpGracza;
    public TextMeshProUGUI TExp;

    public Image akcja;
    public Sprite urzyta;
    public Sprite dostepna;
    public Image spanieImage;

    public static GameObject[] przyciski;
    public GameObject  obiekt1;

    public GameObject strzalkaPrawo;
    public GameObject strzalkaLewo;

    void Start()
    {
        przyciski = new GameObject[16];
        if (obiekt1 != null)
        {
            for (int i = 0; i < 16; i++)
            {
                    przyciski[i] = obiekt1.transform.GetChild(i).gameObject;
            }
        }
    }

    void Update()
    {
        if(Jednostka.Select!=null && Jednostka.CzyJednostka && !(MenuGlowne.multi && Menu.tura == 0))
        {
            Jednostka Wybrany = Jednostka.Select.GetComponent<Jednostka>();
            try{
            healthGracza.maxValue = Wybrany.maxHP;
            healthGracza.value = Wybrany.HP;
            }catch(Exception ex)
            {
                Debug.Log(ex.ToString());
            }
            
            Heros heros = Jednostka.Select.GetComponent<Heros>();
            if(heros != null)
            {
                ExpGracza.gameObject.SetActive(true);
                ExpGracza.maxValue = heros.expToNext;
                ExpGracza.value = heros.exp;
                TExp.text = heros.exp.ToString() + "/" + heros.expToNext.ToString();

            }
            else
            {
                ExpGracza.gameObject.SetActive(false);
            }

            UpdateHealthBarColor();
            TAtak.text = Wybrany.atak.ToString();
            TObrona.text = Wybrany.obrona.ToString();
            Tdmg.text = Wybrany.mindmg.ToString() + "-" + Jednostka.Select.GetComponent<Jednostka>().maxdmg.ToString();
            TSzybkosc.text = ((float)(Wybrany.szybkosc) / 2).ToString() + "/" + ((float)(Jednostka.Select.GetComponent<Jednostka>().maxszybkosc) / 2).ToString();
            THP.text = Math.Round(Wybrany.HP, 1).ToString("N1") + "/" + Jednostka.Select.GetComponent<Jednostka>().maxHP.ToString("N1");
            TZasieg.text = Wybrany.zasieg.ToString();
            TNazwa.text = Wybrany.nazwa;
            if(heros != null)
                level.text = "Lv " + heros.level.ToString();
            else
                level.text =  " ";
            if(Wybrany.akcja)
                akcja.sprite = dostepna;
            else   
                akcja.sprite = urzyta;
            if(Wybrany.druzyna == Menu.tura)
            {
                strzalkaLewo.SetActive(true);
                strzalkaPrawo.SetActive(true);
            }
            else
            {
                strzalkaLewo.SetActive(false);
                strzalkaPrawo.SetActive(false);
            }
            if(Wybrany.spanie)
                spanieImage.enabled = true;
            else
                spanieImage.enabled = false;
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
            }
    }

    public void UstawSpanie()
    {
        if(!Menu.NIERUSZAC)
            Jednostka.Select.GetComponent<Jednostka>().spanie = true;
    }

    public void skokprawo()
    {
        if(!Menu.NIERUSZAC)
        {
            int i = Jednostka.Select.GetComponent<Jednostka>().nr_jednostki;
            if(Menu.jednostki[Menu.tura,i+1]!=null)
            {
                Jednostka.Select = Menu.jednostki[Menu.tura,i+1];
            }
            else
            {
                Jednostka.Select = Menu.jednostki[Menu.tura,0];
            }
            Interface.przeniesDoSelect();
        }
    }
    public void skoklewo()
    {
        if(!Menu.NIERUSZAC)
        {
            int i = Jednostka.Select.GetComponent<Jednostka>().nr_jednostki;
            if(i!=0)
            {
                Jednostka.Select = Menu.jednostki[Menu.tura,i-1];
            }
            else
            {
                Jednostka.Select = Menu.jednostki[Menu.tura, Menu.ludnosc[Menu.tura] - 1];
            }
            Interface.przeniesDoSelect();
        }
    }

        void UpdateHealthBarColor()
    {
        float normalizedHP = Jednostka.Select.GetComponent<Jednostka>().HP / Jednostka.Select.GetComponent<Jednostka>().maxHP;
        Color healthColor = gradient.Evaluate(normalizedHP); // Pobierz kolor z Gradientu
        Image fillImage = healthGracza.fillRect.GetComponent<Image>();
        fillImage.color = healthColor;
    }

}
