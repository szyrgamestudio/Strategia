using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class MenuGlowne : MonoBehaviour
{
    public static bool wczytka;
    public static bool multi;
    
    public static bool nieCelanMulti;

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
}
