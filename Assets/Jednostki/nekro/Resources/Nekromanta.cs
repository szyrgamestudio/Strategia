using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Nekromanta : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public GameObject szczur;
    public List<GameObject> szkielety = new List<GameObject>();
    public List<GameObject> mumie = new List<GameObject>();


    void Update()
    { 
        if(jednostka.GetComponent<Heros>().levelUp)
        {
            jednostka.GetComponent<Heros>().levelUp=false;
            levelUp(jednostka.GetComponent<Heros>().level);
        }
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true)
                    {
                        Przycisk.jednostka[0]=false;
                        Interface.interfaceStatic.GetComponent<Interface>().Brak(0 , 0 , 3, false);
                        if(Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] >= 3)
                            spawn(0);
                    }
                if(Przycisk.jednostka[1]==true)
                    {
                        Przycisk.jednostka[1]=false;
                        Interface.interfaceStatic.GetComponent<Interface>().Brak(0 , 0 , 5, true);
                        if(Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] >= 5 && (Menu.maxludnosc[jednostka.GetComponent<Jednostka>().druzyna] > Menu.ludnosc[jednostka.GetComponent<Jednostka>().druzyna]))
                            spawn(1);
                    }
                if(Przycisk.jednostka[2]==true)
                    {
                        Przycisk.jednostka[2]=false;
                        Interface.interfaceStatic.GetComponent<Interface>().Brak(0 , 0 , 8, true);
                        if(Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] >= 8 && (Menu.maxludnosc[jednostka.GetComponent<Jednostka>().druzyna] > Menu.ludnosc[jednostka.GetComponent<Jednostka>().druzyna]))
                            spawn(2);
                    }
            }

    }

    public void jednostkaMulti(string nazwa, ref GameObject nowyZbieracz, Vector3 vektor)
    {
        nowyZbieracz = PhotonNetwork.Instantiate(nazwa, new Vector3(0, 0, 1), Quaternion.identity);
        nowyZbieracz.transform.position = vektor;
        nowyZbieracz.GetComponent<Jednostka>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
        nowyZbieracz.GetComponent<Jednostka>().sojusz = jednostka.GetComponent<Jednostka>().sojusz;
        nowyZbieracz.transform.position = new Vector3(nowyZbieracz.transform.position.x, nowyZbieracz.transform.position.y, -2f);
        nowyZbieracz.GetComponent<Jednostka>().Aktualizuj();
        nowyZbieracz.GetComponent<Jednostka>().AktualizujPol();    
    }

    private void spawn(int skill)
    {
        List<GameObject> pola = new List<GameObject>();

        int x = (int)transform.position.x; 
        int y = (int)transform.position.y; 
        
        if (Menu.istnieje(x - 1, y) && Menu.kafelki[x - 1][y].GetComponent<Pole>().postac == null)
        {
            pola.Add(Menu.kafelki[x - 1][y]);
        }
        if (Menu.istnieje(x + 1, y) && Menu.kafelki[x + 1][y].GetComponent<Pole>().postac == null)
        {
            pola.Add(Menu.kafelki[x + 1][y]);
        }
        if (Menu.istnieje(x, y - 1) && Menu.kafelki[x][y - 1].GetComponent<Pole>().postac == null)
        {
            pola.Add(Menu.kafelki[x][y - 1]);
        }
        if (Menu.istnieje(x, y + 1) && Menu.kafelki[x][y + 1].GetComponent<Pole>().postac == null)
        {
            pola.Add(Menu.kafelki[x][y + 1]);
        }

        if (pola.Count > 0)
        {
            int randomIndex = Random.Range(0, pola.Count);
            GameObject wylosowanePole = pola[randomIndex];
            GameObject nowa = null;
            switch(skill)
            {
                case 0: nowa = szczur; break;
                case 1: nowa = szkielety[Random.Range(0, szkielety.Count)]; break;
                case 2: nowa = mumie[Random.Range(0, mumie.Count)]; break;
            }
            /////
                        GameObject nowyZbieracz = nowa;
                        if(MenuGlowne.multi)
                        {
                            jednostkaMulti(nowa.name,ref nowyZbieracz, wylosowanePole.transform.position);
                        }
                        else
                        nowyZbieracz = Instantiate(nowa, wylosowanePole.transform.position, Quaternion.identity); 
                        Vector3 newPosition = wylosowanePole.transform.position;
                        newPosition.z = -2f; // Zmiana pozycji w trzecim wymiarze (Z)
                        nowyZbieracz.transform.position = newPosition;
                        nowyZbieracz.GetComponent<Jednostka>().druzyna = jednostka.GetComponent<Jednostka>().druzyna;
                        wylosowanePole.GetComponent<Pole>().Zajete=true;
                        wylosowanePole.GetComponent<Pole>().postac=nowyZbieracz;
                        /////
            switch(skill)
            {
                case 0: Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] -= 2; break;
                case 1: Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] -= 3; break;
                case 2: Menu.magia[jednostka.GetComponent<Jednostka>().druzyna] -= 4; break;
            }
        }
    }

     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "3"; 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "5"; 
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "8"; 
            
            
            for(int i = 0 ; i < jednostka.GetComponent<Jednostka>().zdolnosci  ; i++)
            {
                InterfaceUnit.przyciski[i].GetComponent<Image>().sprite = budynki[i];
                PrzyciskInter Guzik = InterfaceUnit.przyciski[i].GetComponent<PrzyciskInter>();
                Guzik.IconZloto.enabled = false;
                Guzik.IconDrewno.enabled = false;
                Guzik.IconMagic.enabled = true;
                Guzik.Opis.text = teksty[i];  
            }       
        }
    }

    private void levelUp(int level)
    {
        Jednostka staty = jednostka.GetComponent<Jednostka>();
        staty.HP += 2;
        staty.maxHP += 2;   
        switch (level){
            case 2:
                staty.zdolnosci += 1;
                break;
            case 3:
                staty.atak += 1;
                staty.obrona += 1;
                staty.mindmg += 1;
                staty.maxdmg += 1;
                staty.maxszybkosc += 2;
                break;
            case 4:
                staty.zdolnosci += 1;
                break;
            case 5:
                staty.obrona += 4;
                break;
        }
    }
}
