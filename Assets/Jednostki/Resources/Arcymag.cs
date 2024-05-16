using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Arcymag : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool ignis;
    public bool fireBall;
    public int teleportacja;
    GameObject teleportowany1;
    GameObject teleportowany2;
    public bool koniec;

    public bool regenMany;

    public Texture2D customCursorBudowa;

    void Update()
    { 
        
        if(jednostka.GetComponent<Heros>().levelUp)
        {
            jednostka.GetComponent<Heros>().levelUp=false;
            levelUp(jednostka.GetComponent<Heros>().level);
        }
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=4)
                    {
                        jednostka.GetComponent<Ramki>().showRamka3();
                        Przycisk.jednostka[0]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        ignis = true;
                    }
                if(Przycisk.jednostka[1]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=7)
                    {
                        jednostka.GetComponent<Ramki>().showRamka3();
                        Przycisk.jednostka[1]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        fireBall = true;
                    }
                if(Przycisk.jednostka[2]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=7)
                    {
                        jednostka.GetComponent<Ramki>().showRamka3();
                        Przycisk.jednostka[2]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        teleportacja = 1;
                    }
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 4 && teleportacja == 1)
                {
                    teleportacja = 2;
                    teleportowany1 = Jednostka.Select2;
                    Menu.usunSelect2();
                    Jednostka.wybieranie = true;
                    Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                }          
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 4 && teleportacja == 2)
                {
                    teleportacja = 0;
                    Menu.magia[Menu.tura]-=7;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    teleportowany2 = Jednostka.Select2;
                    Vector3 tempPosition = teleportowany1.transform.position;

                    teleportowany1.transform.position = teleportowany2.transform.position;
                    teleportowany2.transform.position = tempPosition;

                    Menu.kafelki[(int)teleportowany1.transform.position.x][(int)teleportowany1.transform.position.y].GetComponent<Pole>().postac = teleportowany2;
                    Menu.kafelki[(int)teleportowany2.transform.position.x][(int)teleportowany2.transform.position.y].GetComponent<Pole>().postac = teleportowany1;

                    Menu.usunSelect2();
                }          
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && ignis)
                {
                    ignis = false;
                    Menu.magia[Menu.tura]-=4;
                                if(MenuGlowne.multi)
                                {
                                    PhotonView photonView = GetComponent<PhotonView>();
                                    photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, 4, Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                                }
                    Jednostka.Select2.GetComponent<Jednostka>().HP -= 4;
                    jednostka.GetComponent<Jednostka>().akcja = false;
                    Jednostka.Select2.GetComponent<Jednostka>().ShowDMG(4f,new Color(1.0f, 0.0f, 0.0f, 1.0f));
                    Menu.usunSelect2();
                }
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && fireBall)
                {
                    Menu.magia[Menu.tura]-=7;
                    fireBall = false;
                    for(int i = -1; i < 2 ; i++)
                        for(int j = -1; j < 2 ; j++)
                            if(Menu.kafelki[(int)Jednostka.Select2.transform.position.x + i][(int)Jednostka.Select2.transform.position.y + j].GetComponent<Pole>().postac != null)
                            {
                                GameObject postka = Menu.kafelki[(int)Jednostka.Select2.transform.position.x + i][(int)Jednostka.Select2.transform.position.y + j].GetComponent<Pole>().postac;
                                postka.GetComponent<Jednostka>().HP -= 3;
                                if(MenuGlowne.multi)
                                {
                                    PhotonView photonView = GetComponent<PhotonView>();
                                    photonView.RPC("dmg", RpcTarget.All,Ip.ip, postka.GetComponent<Jednostka>().nr_jednostki, 3,postka.GetComponent<Jednostka>().druzyna);
                                }
                                jednostka.GetComponent<Jednostka>().akcja = false;
                                postka.GetComponent<Jednostka>().ShowDMG(3f,new Color(1.0f, 0.0f, 0.0f, 1.0f));
                            }
                    Menu.usunSelect2();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    jednostka.GetComponent<Ramki>().Start();
                }
            }
            else
            {
                ignis = false;
                fireBall = false;
                teleportacja = 0;
                teleportowany1 = null;
                teleportowany2 = null;
            }
              if(Menu.Next)
            {
                koniec = true;
            }
            if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
            {
                koniec = false;
                if(regenMany)
                    Menu.magia[Menu.tura]++;
            }
    }
    [PunRPC]
    public void dmg(int ip, int id, int dmg, int team)
    {
        if(ip != Ip.ip)
        {
            GameObject Oponenet = Menu.jednostki[team,id];
            Debug.Log(Oponenet.name);
            Oponenet.GetComponent<Jednostka>().HP -= dmg;
        }
    }

    [PunRPC]
    public void teleportMulti(int ip, int id, int team,int id2, int team2)
    {
        if(ip != Ip.ip)
        {
                    teleportowany1 = Menu.jednostki[team,id];
                    teleportowany2 = Menu.jednostki[team2,id2];

                    Vector3 tempPosition = teleportowany1.transform.position;

                    teleportowany1.transform.position = teleportowany2.transform.position;
                    teleportowany2.transform.position = tempPosition;

                    Menu.kafelki[(int)teleportowany1.transform.position.x][(int)teleportowany1.transform.position.y].GetComponent<Pole>().postac = teleportowany2;
                    Menu.kafelki[(int)teleportowany2.transform.position.x][(int)teleportowany2.transform.position.y].GetComponent<Pole>().postac = teleportowany1;
        }
    }

     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "4"; 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "7"; 
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "7"; 
            
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
        staty.HP += 1;
        staty.maxHP += 1;   
        switch (level){
            case 2:
                staty.atak += 1;
                staty.obrona += 2;
                staty.HP += 1;
                staty.maxHP += 1;   
                break;
            case 3:
                staty.zdolnosci += 1;
                break;
            case 4:
                regenMany = true;
                break;
            case 5:
                staty.zdolnosci += 1;
                break;
        }
    }
}
