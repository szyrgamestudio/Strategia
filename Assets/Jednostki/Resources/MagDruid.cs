using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MagDruid : MonoBehaviour
{
    public GameObject jednostka;
    public bool niedzwiedz;
    public Sprite niedzwiedzArt;
    public Sprite druidArt;

    public Sprite[] budynki;
    public string[] teksty;

    public bool leczenie;

    public Texture2D customCursorBudowa;

    void Update()
    { 
        if(jednostka == Jednostka.Select)
            {
                if(Przycisk.jednostka[0]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=5)
                    {
                        Przycisk.jednostka[0]=false;
                        Menu.magia[Menu.tura]-=5;
                        if(niedzwiedz)
                        {
                            niedzwiedz = false;
                            Jednostka staty = jednostka.GetComponent<Jednostka>();
                            staty.atak = 2;
                            staty.obrona = 1 + Kuznia.update5[Menu.tura];
                            staty.mindmg = 1;
                            staty.maxdmg = 2;
                            staty.zasieg = 3;
                            staty.zdolnosci = 2;
                            jednostka.GetComponent<SpriteRenderer>().sprite = druidArt;
                            budynki[0] = niedzwiedzArt;
                            teksty[0] = "Udeżenie piorunek wywołuje łańcuch błyskawic raniących dwie jednostki w pobliżu";
                            staty.OnMouseDown();
                            OnMouseDown();
                        }
                        else
                        {
                            niedzwiedz = true;
                            Jednostka staty = jednostka.GetComponent<Jednostka>();
                            staty.atak = 4;
                            staty.obrona = 4;
                            staty.mindmg = 3;
                            staty.maxdmg = 4;
                            staty.zasieg = 1;
                            staty.zdolnosci = 1;
                            jednostka.GetComponent<SpriteRenderer>().sprite = niedzwiedzArt;
                            budynki[0] = druidArt;
                            teksty[0] = "Przemień się z powrotem w postać druida";
                            staty.OnMouseDown();
                            OnMouseDown();
                        }

                    }
                if(Przycisk.jednostka[1]==true && jednostka.GetComponent<Jednostka>().akcja && Menu.magia[Menu.tura]>=5)
                    {
                        Przycisk.jednostka[1]=false;
                        Jednostka.wybieranie = true;
                        Cursor.SetCursor(customCursorBudowa, Vector2.zero, CursorMode.Auto);
                        leczenie = true;
                    }

            
                if (Jednostka.Select2 != null && Jednostka.CzyJednostka2 && Walka.odleglosc(jednostka, Jednostka.Select2) <= 3 && leczenie)
                {
                    Menu.magia[Menu.tura]-=5;
                    leczenie = false;  
                    Jednostka.Select2.GetComponent<Jednostka>().HP -= 3;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("dmg", RpcTarget.All,Ip.ip, Jednostka.Select2.GetComponent<Jednostka>().nr_jednostki, 3,Jednostka.Select2.GetComponent<Jednostka>().druzyna);
                    }
                    Jednostka.Select2.GetComponent<Jednostka>().ShowDMG(3f,new Color(1.0f, 0.0f, 0.0f, 1.0f));
                    piorun(2,Jednostka.Select2);
                    Menu.usunSelect2();
                }
            }
            else
            {
                leczenie = false;
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

    private void piorun(int x, GameObject y)
    {
    if(x>0)
    {
        for(int i = -1; i < 2 ; i++)
            for(int j = -1; j < 2 ; j++)
                if(!(i==0 && j == 0))
                {
                    if(Menu.kafelki[(int)y.transform.position.x + i][(int)y.transform.position.y + j].GetComponent<Pole>().postac != null)
                    {
                        GameObject postka = Menu.kafelki[(int)y.transform.position.x + i][(int)y.transform.position.y + j].GetComponent<Pole>().postac;
                        postka.GetComponent<Jednostka>().HP -= 3;
                        if(MenuGlowne.multi)
                        {
                            PhotonView photonView = GetComponent<PhotonView>();
                            photonView.RPC("dmg", RpcTarget.All,Ip.ip, postka.GetComponent<Jednostka>().nr_jednostki, 3,postka.GetComponent<Jednostka>().druzyna);
                        }
                        postka.GetComponent<Jednostka>().ShowDMG(3f,new Color(1.0f, 0.0f, 0.0f, 1.0f));
                        piorun(x-1,postka);
                        i = 3;
                        j = 3;
                    }
                }
    }
    }
     void OnMouseDown()
    {
        if(jednostka == Jednostka.Select)
        {
            InterfaceUnit.Czyszczenie(); 
            PrzyciskInter Guzikk = InterfaceUnit.przyciski[0].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "5"; 
            Guzikk = InterfaceUnit.przyciski[1].GetComponent<PrzyciskInter>();
            Guzikk.CenaMagic.text = "5"; 

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
}
