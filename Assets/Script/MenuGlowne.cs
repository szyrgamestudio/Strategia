using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class MenuGlowne : MonoBehaviour
{
    public Image tlo;
    public float speed = -3f;
    public static bool wczytka;
    public static bool multi;

    void Start()
    {
        // Dodaj kod inicjalizacyjny, jeśli jest potrzebny
    }
    public void local()
    {
        SceneManager.LoadScene(1);
    }
    public void multiplsyer()
    {
        multi = true;
        SceneManager.LoadScene("Loading");
    }

    // Update is called once per frame
    void Update()
    {
        if(tlo != null)
        {
        if (tlo.rectTransform.anchoredPosition.y < -420f)
        {
            speed = Mathf.Abs(speed); // Ustawiamy prędkość na dodatnią
        }
        else if (tlo.rectTransform.anchoredPosition.y > 420f)
        {
            speed = -Mathf.Abs(speed); // Ustawiamy prędkość na ujemną
        }

        tlo.rectTransform.anchoredPosition += new Vector2(0, 1f) * speed * Time.deltaTime;
        }
    }
}
