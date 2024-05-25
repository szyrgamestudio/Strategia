using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Palladyn : MonoBehaviour
{
    public GameObject jednostka;

    public Sprite[] budynki;
    public string[] teksty;

    public bool leczenie;
    public bool buff;
    public int cooldown;
    public bool koniec;
    public int cooldown2;

    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(cooldown2>0)
        {
            jednostka.GetComponent<Jednostka>().HP = jednostka.GetComponent<Jednostka>().maxHP;
        }
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
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        leczenie = true;
                    }
                if(Przycisk.jednostka[1]==true && cooldown == 0)
                    {
                        Przycisk.jednostka[1]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        buff = true;
                    }
                if(Przycisk.jednostka[2]==true && Menu.magia[Menu.tura]>=10 && cooldown == 0)
                    {
                        Przycisk.jednostka[2]=false;
                        Menu.magia[Menu.tura]-=10;
                        cooldown2 = 2;
                        if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("cooldownUpdate", RpcTarget.All,Ip.ip, 2);
                        }
                    }
            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) == 1 && leczenie && 
                Jednostka.Select2.GetComponent<Jednostka>().sojusz == jednostka.GetComponent<Jednostka>().sojusz)
                {
                    leczenie = false;
                    float rozniaca = Jednostka.Select2.GetComponent<Jednostka>().maxHP - Jednostka.Select2.GetComponent<Jednostka>().HP;
                    if(rozniaca < Menu.magia[Menu.tura])
                    {
                        Jednostka.Select2.GetComponent<Jednostka>().HP += rozniaca;
                        if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, -rozniaca,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                        }
                        Jednostka.Select2.GetComponent<Jednostka>().ShowDMG(rozniaca,new Color(0.0f, 1.0f, 0.0f, 1.0f));
                        Menu.magia[Menu.tura] -= (int)rozniaca;
                    }
                    else
                    {
                        Jednostka.Select2.GetComponent<Jednostka>().HP += (float)Menu.magia[Menu.tura];
                        if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, -(float)Menu.magia[Menu.tura],Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                        }
                        Jednostka.Select2.GetComponent<Jednostka>().ShowDMG((float)Menu.magia[Menu.tura],new Color(0.0f, 1.0f, 0.0f, 1.0f));
                        Menu.magia[Menu.tura] = 0;
                    }

                    Menu.usunSelect2();
                }
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) == 1 && buff && 
                Jednostka.Select2.GetComponent<Jednostka>().sojusz == jednostka.GetComponent<Jednostka>().sojusz)
                {
                    buff = false;
                    Jednostka.Select2.GetComponent<Jednostka>().atak += 1;
                    Jednostka.Select2.GetComponent<Jednostka>().obrona += 1;
                    cooldown = 2;
                    Jednostka.Select2.GetComponent<Buff>().buffP(2,0,0,1,1);
                    Menu.usunSelect2();
                    OnMouseDown();
                }
            }
            else
            {
                leczenie = false;
            }
            if(Menu.Next)
            {
                koniec = true;
            }
            if(koniec && !Menu.Next && jednostka.GetComponent<Jednostka>().druzyna == Menu.tura)
            {
                koniec = false;
                if(cooldown2 != 0)
                    cooldown2--;
                if(cooldown != 0)
                    cooldown--;
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
    public void cooldownUpdate(int ip, int cooldown3)
    {
        if(ip != Ip.ip)
        {
            cooldown2 = cooldown3;
        }
    }

     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.IconMagic.enabled = true;
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();

            if(cooldown>0)
            {
                Guzikk.CenaMagic.text = cooldown.ToString(); 
                Guzikk.IconMagic.enabled = true;
            }
            else
            {
               Guzikk.CenaMagic.text = "";
               Guzikk.IconMagic.enabled = false;
            }
            Guzikk = InterfaceUnit.przyciski[2].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "10"; 
            
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
                staty.atak += 1;
                staty.obrona += 1;
                staty.mindmg += 1;
                staty.maxdmg += 1;
                break;
            case 3:
                staty.zdolnosci += 1;
                break;
            case 4:
                staty.atak += 1;
                staty.obrona += 1;
                staty.mindmg += 1;
                staty.maxdmg += 1;
                break;
            case 5:
                staty.zdolnosci += 1;
                break;
        }
    }
}
