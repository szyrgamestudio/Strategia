using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatka : MonoBehaviour
{
    public GameObject budynek;

    private bool dodaj=false;

    void Start()
    {
        budynek.GetComponent<Budynek>().poZniszczeniu = 1;
    }

    void Update()
    {
        if(budynek.GetComponent<Budynek>().punktyBudowy >= budynek.GetComponent<Budynek>().punktyBudowyMax && !dodaj)
        {
            dodaj = true;
            Menu.maxludnosc[budynek.GetComponent<Budynek>().druzyna] += 3;
        }
        if(budynek.GetComponent<Budynek>().poZniszczeniu == 2)
        {
            if(budynek.GetComponent<Budynek>().punktyBudowy >= budynek.GetComponent<Budynek>().punktyBudowyMax)
                Menu.maxludnosc[budynek.GetComponent<Budynek>().druzyna] -= 3;
            Destroy(budynek);
        }
    }
}
