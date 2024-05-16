using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ramki : MonoBehaviour
{
    public Image ramka2;
    public Image ramka3;
    public Image ramka4;

    private bool pokazane = true;

    public GameObject jednostka;

    public void Start()
    {
        ramka2.enabled = false;
        ramka3.enabled = false;
        ramka4.enabled = false;
    }
    public void showRamka2()
    {
        ramka2.enabled = true;
        pokazane = true;
    }
    public void showRamka3()
    {
        ramka3.enabled = true;
        pokazane = true;
    }
    public void showRamka4()
    {
        ramka4.enabled = true;
        pokazane = true;
    }

    void Update()
    {
        if(jednostka != Jednostka.Select && pokazane)
        {
            pokazane = false;
            Start();
        }
    }

}
