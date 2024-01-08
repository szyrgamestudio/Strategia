using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public GameObject jednostka;

    public float[] HP;
    public float[] maxHP;
    public float[] atak;
    public float[] obrona;
    public int[] zasieg;
    public int[] maxszybkosc;
    public int[] szybkosc;
    public float[] mindmg;
    public float[] maxdmg;

    public bool koniec;

    void Start()
    {
    HP= new float[10];
    maxHP= new float[10];
    atak= new float[10];
    obrona= new float[10];
    zasieg= new int[10];
    maxszybkosc= new int[10];
    szybkosc= new int[10];
    mindmg= new float[10];
    maxdmg= new float[10];
    }    

    public void buffP(int tura, float nHP, float nmaxHP, float natak, float nobrona)
    {
        HP[tura] += nHP;
        maxHP[tura] += nmaxHP;
        atak[tura] += natak;
        obrona[tura] += nobrona;
    }
    public void buffZ(int tura, int nzasieg, int nmaxszybkosc,int nszybkosc, float nmindmg, float nmaxdmg)
    {
        zasieg[tura] = nzasieg;
        maxszybkosc[tura] += nmaxszybkosc;
        szybkosc[tura] += nszybkosc;
        mindmg[tura] += nmindmg;
        maxdmg[tura] += nmaxdmg;
    }

    void Update()
    {
        if(Menu.Next)
        {
            koniec = true;
        }
        if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
        {
            koniec = false;
            KoniecTury();
        }
    }

    private void KoniecTury()
    {
        Jednostka Ziomal = jednostka.GetComponent<Jednostka>();
        Ziomal.HP -= HP[0];
        Ziomal.maxHP -= maxHP[0];
        Ziomal.atak -= atak[0];
        Ziomal.obrona -= obrona[0];
        Ziomal.zasieg -= zasieg[0];
        Ziomal.szybkosc -= szybkosc[0];
        Ziomal.maxszybkosc -= maxszybkosc[0];
        Ziomal.mindmg -= mindmg[0];
        Ziomal.maxdmg -= maxdmg[0];
        for(int i=0; i<9; i++)
        {
            HP[i] = HP[i+1];
            maxHP[i] = maxHP[i+1];
            atak[i] = atak[i+1];
            obrona[i] = obrona[i+1];
            zasieg[i] = zasieg[i+1];
            szybkosc[i] = szybkosc[i+1];
            maxszybkosc[i] = maxszybkosc[i+1];
            mindmg[i] = mindmg[i+1];
            maxdmg[i] = maxdmg[i+1];
        }
        HP[9] = 0;
        maxHP[9] = 0;
        atak[9] = 0;
        obrona[9] = 0;
        zasieg[9] = 0;
        szybkosc[9] = 0;
        maxszybkosc[9] = 0;
        mindmg[9]= 0;
        maxdmg[9] = 0;
    }
}
