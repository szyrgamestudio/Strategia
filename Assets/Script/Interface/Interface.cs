using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // You need to add this for Text component

public class Interface : MonoBehaviour
{
    public Text tura;
    public Image twarz;
    public Sprite puste;

    public Text zlotoText;
    public Text drewnoText;
    public Text diamentText;
    public Text magiaText;
    public Text ludnoscText;

    public static bool ruch;

    public static GameObject interfaceStatic;

    public GameObject controlTur;
    public Text controlText;

    //public GameObject camera;

    void Start()
    {
        interfaceStatic = this.gameObject;
        if(!End.control)
            controlTur.SetActive(false);

    }

    void Update()
    {
        tura.text = "KOLEJNA TURA";//\nTura gracza " + Menu.tura.ToString();
        //if (Jednostka.Select != null)
        //    twarz.sprite = Jednostka.Select.GetComponent<SpriteRenderer>().sprite;
        //else
        //    twarz.sprite = puste;

        twarz.sprite = (Jednostka.Select != null) ? Jednostka.Select.GetComponent<SpriteRenderer>().sprite : puste;

        int wyswietlanaWartosc = Menu.tura;
        if(MenuGlowne.multi)
            wyswietlanaWartosc = Ip.ip;
        zlotoText.text = Menu.zloto[wyswietlanaWartosc].ToString();
        drewnoText.text = Menu.drewno[wyswietlanaWartosc].ToString();
        diamentText.text = Menu.diament[wyswietlanaWartosc].ToString();
        magiaText.text = Menu.magia[wyswietlanaWartosc].ToString();
        ludnoscText.text = Menu.ludnosc[wyswietlanaWartosc].ToString() + "/" + Menu.maxludnosc[wyswietlanaWartosc].ToString();
        if(End.control)
        {
            controlText.text = End.punktyKontroli.ToString() + "/" + End.tureKontroli;
            switch(End.druzynaKontroli)
            {
            case 0: controlText.color = new Color(0.0f, 0.0f, 0.0f); break;
            case 1: controlText.color = new Color(1.0f, 0.0f, 0.0f); break;
            case 2: controlText.color = new Color(0.0f, 1.0f, 0.0f); break;
            case 3: controlText.color = new Color(0.0f, 0.0f, 1.0f); break;
            case 4: controlText.color = new Color(1.0f, 1.0f, 0.0f); break;
            }
        }

        if (ruch)
        {
            ruch = false;
            StartCoroutine(ruchPlynnyCamery());
        }
    }

    public void Brak(int zloro, int drewno,  int magia, bool jednostka)
    {
        if(zloro > Menu.zloto[Menu.tura])
        {
            zlotoText.color = Color.red;
            StartCoroutine(PowiekszTekst(0));
            OdtworzDzwiekAnulowania();
        }
        if(drewno > Menu.drewno[Menu.tura])
        {
            drewnoText.color = Color.red;
            StartCoroutine(PowiekszTekst(1));
            OdtworzDzwiekAnulowania();
        }
        if(magia > Menu.magia[Menu.tura])
        {
            magiaText.color = Color.red;
            StartCoroutine(PowiekszTekst(2));
            OdtworzDzwiekAnulowania();
        }
        if(Menu.ludnosc[Menu.tura] >= Menu.maxludnosc[Menu.tura] && jednostka)
        {
            ludnoscText.color = Color.red;
            StartCoroutine(PowiekszTekst(3));
            OdtworzDzwiekAnulowania();
        }
    }


    IEnumerator PowiekszTekst(int i)
    {
        Text tekst = null;
        switch(i)
        {
            case 0: tekst = zlotoText; break;
            case 1: tekst = drewnoText; break;
            case 2: tekst = magiaText; break;
            case 3: tekst = ludnoscText; break;
        }
        
        tekst.transform.localScale *= 1.3f; // Powiększenie tekstu o 10%
        yield return new WaitForSeconds(0.3f); // Poczekaj 0.5 sekundy
        tekst.transform.localScale /= 1.3f; // Przywróć pierwotny rozmiar tekstu
        tekst.color = Color.black;
    }

    public AudioSource src;
    public AudioClip anulowanie;

    void OdtworzDzwiekAnulowania()
    {
        src.clip = anulowanie;
        src.Play();
    }

    public void usun()
    {
        Debug.Log("jeden");
        if(!Menu.NIERUSZAC)
        {
            Debug.Log("dwa");
            Jednostka.Select = null;
            Menu.PanelUnit.SetActive(false);
            Menu.PanelBuild.SetActive(false);
            Pole.Clean2();
        }   
    }

    public static void przeniesDoSelect()
    {
        ruch = true;
        //Menu.kamera.transform.position = newPosition;


        Pole.Clean2();
    }

    IEnumerator ruchPlynnyCamery()
    {

        float x = Jednostka.Select.transform.position.x + 0.2f * Menu.kamera.GetComponent<Camera>().orthographicSize;
        float y = Jednostka.Select.transform.position.y;
        if (y < Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f)
        {
            y = Menu.kamera.GetComponent<Camera>().orthographicSize * 1.0f - 0.5f;
        }
        if (x < Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f)
        {
            x = Menu.kamera.GetComponent<Camera>().orthographicSize * 1.8f - 0.6f;
        }
        if (y > Menu.BoardSizeY - Menu.kamera.GetComponent<Camera>().orthographicSize * 1f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.2f - 0.5f)
        {
            y = Menu.BoardSizeY - Menu.kamera.GetComponent<Camera>().orthographicSize * 1f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.2f - 0.5f;
        }
        if (x > Menu.BoardSizeX - Menu.kamera.GetComponent<Camera>().orthographicSize * 1.75f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.6f - 0.5f)
        {
            x = Menu.BoardSizeX - Menu.kamera.GetComponent<Camera>().orthographicSize * 1.75f + Menu.kamera.GetComponent<Camera>().orthographicSize * 0.6f - 0.5f;
        }
        float a = Menu.kamera.transform.position.x;
        float b = Menu.kamera.transform.position.y;

        float x1 = (x - a) / 60;
        float y1 = (y - b) / 60;
        for (int i = 0; i < 60; i++)
        {
            a += x1;
            b += y1;
            Vector3 newPosition = new Vector3(a, b, -10f);
            Menu.kamera.transform.position = newPosition;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
