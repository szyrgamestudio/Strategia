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

    //public GameObject camera;

    void Update()
    {
        tura.text = "KOLENJA TURA\nTura gracza " + Menu.tura.ToString();
        //if (Jednostka.Select != null)
        //    twarz.sprite = Jednostka.Select.GetComponent<SpriteRenderer>().sprite;
        //else
        //    twarz.sprite = puste;

        twarz.sprite = (Jednostka.Select != null) ? Jednostka.Select.GetComponent<SpriteRenderer>().sprite : puste;

        zlotoText.text = Menu.zloto[Menu.tura].ToString();
        drewnoText.text = Menu.drewno[Menu.tura].ToString();
        diamentText.text = Menu.diament[Menu.tura].ToString();
        magiaText.text = Menu.magia[Menu.tura].ToString();
        ludnoscText.text = Menu.ludnosc[Menu.tura].ToString() + "/" + Menu.maxludnosc[Menu.tura].ToString();
        if (ruch)
        {
            ruch = false;
            StartCoroutine(ruchPlynnyCamery());
        }
    }
    public void usun()
    {
        if(!Menu.NIERUSZAC)
        {
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
