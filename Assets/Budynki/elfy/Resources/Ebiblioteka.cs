using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ebiblioteka : MonoBehaviour
{
    public GameObject budynek;
    public GameObject pole;
    public int druzyna;

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
                if(Przycisk.budynek[0]==true)
                {
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(4+update1[druzyna]* 2 , 0 , 0, false);
                    Przycisk.budynek[0]=false;
                    if(Menu.zloto[Menu.tura]>=4+update1[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update1[druzyna])
                    {
                        
                        Menu.zloto[Menu.tura]-=4 + update1[druzyna];
                        update1[druzyna]+=1;
                        ulepszIstniejace("all",0);
                        OnMouseDown();
                    }
                }
                if(Przycisk.budynek[1]==true)
                {
                    Przycisk.budynek[1]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(4+update2[druzyna]* 2 , 0 , 0, false);
                    if(Menu.zloto[Menu.tura]>=4+update2[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update2[druzyna])
                    {
                        
                        Menu.zloto[Menu.tura]-=4 + update2[druzyna];
                        update2[druzyna]+=1;
                        ulepszIstniejace("all",1);
                        OnMouseDown();
                    }
                }
                if(Przycisk.budynek[2]==true)
                {
                    Przycisk.budynek[2]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(4+update3[druzyna]* 2 , 0 , 0, false);
                    if(Menu.zloto[Menu.tura]>=4 + update3[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update3[druzyna])
                    {
                        
                        Menu.zloto[Menu.tura]-=4 + update3[druzyna];
                        update3[druzyna]+=1;
                        ulepszIstniejace("all",7);
                        OnMouseDown();
                    }
                }
                if(Przycisk.budynek[3]==true)
                {
                    Przycisk.budynek[3]=false;
                    Interface.interfaceStatic.GetComponent<Interface>().Brak(4+update4[druzyna]* 2 , 0 , 0, false);
                    if(Menu.zloto[Menu.tura]>=4 + update4[druzyna]* 2 && Menu.ratuszPoziom[druzyna] >= update4[druzyna])
                    {
                        
                        Menu.zloto[Menu.tura]-=4 + update4[druzyna];
                        update4[druzyna]+=1;
                        ulepszIstniejace("Ent",5);
                        ulepszIstniejace("Drzewiec",5);
                        OnMouseDown();
                    }
                }
            }
    }
    public void ulepszIstniejace(string nazwa, int parametr)
    {
        int i = 0;
        while(Menu.jednostki[druzyna,i] != null)
        {
            if(Menu.jednostki[druzyna,i] != null && !Menu.jednostki[druzyna,i].GetComponent<Heros>() && (Menu.jednostki[druzyna,i].GetComponent<Jednostka>().nazwa == nazwa || nazwa == "all"))
                {
                    switch(parametr){
                        case 0: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().atak += 2; break;
                        case 1: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().obrona += 2; break;
                        case 2: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().szybkosc += 2;
                        Menu.jednostki[druzyna,i].GetComponent<Jednostka>().maxszybkosc += 2; break;
                        case 3: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().maxdmg += 1; break;
                        case 4:Menu.jednostki[druzyna,i].GetComponent<Jednostka>().atak += 1; 
                        Menu.jednostki[druzyna,i].GetComponent<Jednostka>().obrona += 1;break;
                        case 5: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().HP += 2;
                        Menu.jednostki[druzyna,i].GetComponent<Jednostka>().maxHP += 2; break;
                        case 6: Menu.jednostki[druzyna,i].GetComponent<Golem>().DMG += 1; break;
                        case 7: Menu.jednostki[druzyna,i].GetComponent<Jednostka>().szybkosc += 1;
                        Menu.jednostki[druzyna,i].GetComponent<Jednostka>().maxszybkosc += 1; break;
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
          
            
            for(int i = 0 ; i < 4 ; i++)
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
