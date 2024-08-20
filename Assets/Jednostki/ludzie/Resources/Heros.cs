using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heros : MonoBehaviour
{
    public GameObject jednostka;
    public int level = 1;
    public int exp = 0;
    public int expToNext = 20;
    public Slider healthGracza;

    public bool levelUp;

    void Start()
    {
        Menu.heros[jednostka.GetComponent<Jednostka>().druzyna] = jednostka;
    }


       void Update()
    {
        healthGracza.value = exp;
        healthGracza.maxValue = expToNext;
        if(exp>=expToNext)
        {
            exp -= expToNext;
            level++;
            jednostka.GetComponent<Jednostka>().cena += 4;
            expToNext = level * 20;
            levelUp = true;
        }
    }
}
