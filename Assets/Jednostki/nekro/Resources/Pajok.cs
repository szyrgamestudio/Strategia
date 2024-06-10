using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pajok : MonoBehaviour
{
    public GameObject jednostka;
    void Start()
    {
        Menu.zloto[jednostka.GetComponent<Jednostka>().druzyna] -= 4;
        if(Menu.zloto[jednostka.GetComponent<Jednostka>().druzyna] < 0)
            Menu.zloto[jednostka.GetComponent<Jednostka>().druzyna] = 0;
    }

}
