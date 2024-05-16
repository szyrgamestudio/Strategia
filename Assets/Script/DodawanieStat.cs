using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodawanieStat : MonoBehaviour
{
    public static int zloto;
    public static int drewno;
    public static int diament;
    public static int magia;

    public Text zlotoText;
    public Text drewnoText;
    public Text diamentText;
    public Text magiaText;

    void Update()
    {
        if(!MenuGlowne.multi || Menu.tura == Ip.ip)
        {
            if(zloto != 0)
                StartCoroutine(zlotoLoad());
            if(drewno != 0)
                StartCoroutine(drewnoLoad());
            if(diament != 0)
                StartCoroutine(diamentLoad());
            if(magia != 0)
                StartCoroutine(magiaLoad());
        }
        else
        {
            zloto = 0;
            drewno = 0;
            magia = 0;
        }
        
    }

    IEnumerator zlotoLoad()
    {
        yield return new WaitForSeconds(0.2f);
        if(zloto!=0)
        {
            ShowDMG(zlotoText,zloto,new Color(0f, 0f, 0f, 1.0f), -565.5f);
            zloto = 0;
        }
    }
        IEnumerator drewnoLoad()
    {
        yield return new WaitForSeconds(0.2f);
        if(drewno!=0)
        {
            ShowDMG(drewnoText,drewno,new Color(0f, 0f, 0f, 1.0f), -265f);
            drewno = 0;
        }
    }
        IEnumerator diamentLoad()
    {
        yield return new WaitForSeconds(0.2f);
        if(diament!=0)
        {
            ShowDMG(diamentText,diament,new Color(0f, 0f, 0f, 1.0f), 235.91f);
            diament = 0;
        }
    }
        IEnumerator magiaLoad()
    {
        yield return new WaitForSeconds(0.2f);
        if(magia!=0)
        {
            ShowDMG(magiaText,magia,new Color(0f, 0f, 0f, 1.0f), 29.8f);
            magia = 0;
        }
    }

    public void ShowDMG(Text TextShow, float dmg, Color myColor, float fx)
    {
        TextShow.text = dmg.ToString();
        TextShow.color = myColor;
        RectTransform textTransform = TextShow.GetComponent<RectTransform>();
        float x = fx;
        textTransform.anchoredPosition = new Vector2(x, 404.0f);
        StartCoroutine(AnimacjaHP(TextShow, x));
    }

    IEnumerator AnimacjaHP(Text TextShow, float x)
    {
        RectTransform textTransform = TextShow.GetComponent<RectTransform>();
        Color textColor = TextShow.color;
         // Wartość alpha (przezroczystość) od 0 (pełna przezroczystość) do 1 (brak przezroczystości)
        
        for(int i=0;i<40;i++)
        {
            textTransform.anchoredPosition = new Vector2(x, 404.0f + 3.6f*(float)i);
            yield return new WaitForSeconds(0.04f);
            textColor.a = 1f - 0.025f*(float)i;
            TextShow.color = textColor;
        }
    }
}