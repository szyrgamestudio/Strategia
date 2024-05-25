using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class End : MonoBehaviour
{
    public static int wygrany;
    public Text napis;

    void Update()
    {
        napis.text = "Zwyciężył Gracz: " + wygrany.ToString();
    }
}
