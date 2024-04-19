using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kuznia : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;

    public GameObject lucznik;
    public GameObject piechur;
    public GameObject rycerz;
    public GameObject kusznik;
    public GameObject kawalerzysta;
    public GameObject jaskolka;
    public GameObject wilk;
    public GameObject gryf;
    public GameObject magOgnia;
    public GameObject magDruid;
    public GameObject magKaplan;

    public static int[] update1 = new int[5];
    public static int[] update2 = new int[5];
    public static int[] update3 = new int[5];
    public static int[] update4 = new int[5];
    public static int[] update5 = new int[5];

    public Sprite[] budynki;
    public string[] teksty;
    public Sprite loock;

    void Start()
    {
        druzyna = budynek.GetComponent<Budynek>().druzyna;
    }

    void Update()
    {
        if(budynek == Jednostka.Select)
            {
                pole = budynek.GetComponent<BudynekRuch>().pole;
                if(Przycisk.budynek[0]==true && Menu.zloto[Menu.tura]>=4+update1[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update1[druzyna])
                {
                    Przycisk.budynek[0]=false;
                    Menu.zloto[Menu.tura]-=4 + update1[druzyna];
                    update1[druzyna]+=1;
                    ulepszIstniejace("Piechur",1);
                    ulepszIstniejace("Rycerz",1);
                    ulepszIstniejace("Kawalerzysta",1);
                    OnMouseDown();
                }
                if(Przycisk.budynek[1]==true && Menu.zloto[Menu.tura]>=4+update2[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update2[druzyna])
                {
                    Przycisk.budynek[1]=false;
                    Menu.zloto[Menu.tura]-=4 + update2[druzyna];
                    update2[druzyna]+=1;
                    ulepszIstniejace("Piechur",0);
                    ulepszIstniejace("Rycerz",0);
                    ulepszIstniejace("Wilk",0);
                    ulepszIstniejace("Kawalerzysta",0);
                    OnMouseDown();

                }
                if(Przycisk.budynek[2]==true && Menu.zloto[Menu.tura]>=4 + update3[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update3[druzyna])
                {
                    Przycisk.budynek[2]=false;
                    Menu.zloto[Menu.tura]-=4 + update3[druzyna];
                    update3[druzyna]+=1;
                    ulepszIstniejace("Jaskółka",2);
                    ulepszIstniejace("Kawalerzysta",2);
                    ulepszIstniejace("Gryf",2);
                    ulepszIstniejace("Wilk",2);
                    OnMouseDown();
                }
                if(Przycisk.budynek[3]==true && Menu.zloto[Menu.tura]>=4 + update4[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update4[druzyna])
                {
                    Przycisk.budynek[3]=false;
                    Menu.zloto[Menu.tura]-=4 + update4[druzyna];
                    update4[druzyna]+=1;
                    ulepszIstniejace("Łucznik",0);
                    ulepszIstniejace("Kusznik",0);
                    ulepszIstniejace("Wilk",0);
                    ulepszIstniejace("Gryf",0);
                    OnMouseDown();
                }
                if(Przycisk.budynek[4]==true && Menu.zloto[Menu.tura]>=4 + update5[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update5[druzyna])
                {
                    Przycisk.budynek[4]=false;
                    Menu.zloto[Menu.tura]-=4 + update5[druzyna];
                    update5[druzyna]+=1;
                    ulepszIstniejace("Piroman",1);
                    ulepszIstniejace("Druid",1);
                    ulepszIstniejace("Kapłan",1);
                    ulepszIstniejace("Gryf",1);
                    OnMouseDown();
                }
            }
    }
    public void ulepszIstniejace(string nazwa, int parametr)
    {
        int i = 0;
        while(Menu.jednostki[druzyna,i] != null)
        {
            if(Menu.jednostki[druzyna,i] != null && Menu.jednostki[druzyna,i].GetComponent<Jednostka>().nazwa == nazwa)
                {
                    Debug.Log("Jednostka o nazwie WYBRANA:" + Menu.jednostki[druzyna,i].GetComponent<Jednostka>().nazwa);
                    switch(parametr){
                        case 0: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().atak += 2; break;
                        case 1: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().obrona += 2; break;
                        case 2: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().szybkosc += 2;
                        Menu.jednostki[druzyna,i].GetComponent<Jednostka>().maxszybkosc += 2; break;
                    }
                }
                i++;
        }
    }
    public void OnMouseDown()
    {
        if(budynek == Jednostka.Select)
        {
            InterfaceBuild.Czyszczenie(); 
            
            PrzyciskInter Guzikk = InterfaceBuild.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = (4 + update1[druzyna] * 2).ToString();
            Guzikk = InterfaceBuild.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = (4 + update2[druzyna] * 2).ToString();
            Guzikk = InterfaceBuild.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = (4 + update3[druzyna]* 2).ToString();
            Guzikk = InterfaceBuild.przyciski[3].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = (4 + update4[druzyna]* 2).ToString();
            Guzikk = InterfaceBuild.przyciski[4].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = (4 + update5[druzyna]* 2).ToString();
          
            
            for(int i = 0 ; i < 5 ; i++)
            {
                InterfaceBuild.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceBuild.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            } 
            if(Menu.ratuszPoziom[druzyna] < update1[druzyna])  
            {
                InterfaceBuild.przyciski[0].GetComponent<Image>().sprite = loock;
            }  
            if(Menu.ratuszPoziom[druzyna] < update2[druzyna])  
            {
                InterfaceBuild.przyciski[1].GetComponent<Image>().sprite = loock;
            }  
            if(Menu.ratuszPoziom[druzyna] < update3[druzyna])  
            {
                InterfaceBuild.przyciski[2].GetComponent<Image>().sprite = loock;
            }  
            if(Menu.ratuszPoziom[druzyna] < update4[druzyna])  
            {
                InterfaceBuild.przyciski[3].GetComponent<Image>().sprite = loock;
            }  
            if(Menu.ratuszPoziom[druzyna] < update5[druzyna])  
            {
                InterfaceBuild.przyciski[4].GetComponent<Image>().sprite = loock;
            }  
        }
    }
}
