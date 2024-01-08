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

       void Update()
    {
        healthGracza.value = exp;
        healthGracza.maxValue = expToNext;
        if(exp>=expToNext)
        {
            exp = 0;
            level++;
            expToNext = level * 20;
            levelUp = true;
        }
    }
}
