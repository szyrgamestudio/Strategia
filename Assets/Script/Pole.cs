using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Pole : MonoBehaviour
{
    public GameObject postac;
    public GameObject kafelek;
    public static GameObject[] lista1;
    public static GameObject[] lista2;
    public static GameObject[] droga;
    public static GameObject LastDroga;
    public static bool idzie;

    public bool Zajete;
    public bool ZajeteLot;

    public bool woda;
    public bool las;

    public int poziom;
    public int zloto;
    public int magia;
    public int trudnosc = 2;

    public int Nin;
    public int CzasDrogi;
    public int Nout;
    public int spriteName;
    public string nazwa;

    private bool koniec;

    public static int[] pomocnicza;

    void Start()
    {
        lista1 = new GameObject[4000];
        lista2 = new GameObject[4000];
        droga = new GameObject[2500];
        pomocnicza = new int[3];
        if(Ip.ip == 1)
            AktualizujPołożenie();
        
        Menu.kafelki[(int)transform.position.x][(int)transform.position.y] = kafelek;
    }
    void Update()
    {
        if(Menu.Next)
        {
            koniec = true;
        }
        if(!Menu.Next && koniec)
        {
            koniec = false;
            if(postac == null)
            {
                if(!las)
                    Zajete = false;
                ZajeteLot = false;
            }
            Clean2();
        }
    }

    IEnumerator Aktualizuj(bool Zajete)
    {
        yield return new WaitForSeconds(0.1f);

        if (Zajete != this.Zajete)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, Zajete, ZajeteLot, woda, las, poziom, zloto, magia,   CzasDrogi, kafelek.name, spriteName);
        }
    }

    public void AktualizujPołożenie()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, Zajete, ZajeteLot, woda, las, poziom, zloto, magia, CzasDrogi, kafelek.name, spriteName);
    }

    [PunRPC]
    void ZaktualizujStatystykiRPC(bool zajete, bool zajeteLot, bool woda, bool las, int level, int gold, int magic, int czasDrogi, string nazwa, int spriteName)
    {
        Zajete = zajete;
        ZajeteLot = zajeteLot;
        this.woda = woda;
        this.las = las;
        poziom = level;
        zloto = gold;
        magia = magic;
        CzasDrogi = czasDrogi;
        this.nazwa = nazwa;
        this.spriteName = spriteName;
        aktualizujDane();
    }
    void aktualizujDane()
    {
        kafelek.GetComponent<SpriteRenderer>().sprite = MapLoad.obrazStatic[spriteName];
        kafelek.name = nazwa;
    }

    [PunRPC]
    void ZaktualizujRuch(int idJednostki, int ip)
    {
        if(Ip.ip != ip )
            OnMouse(Menu.jednostki[Menu.tura , idJednostki], 0);
    }

    public void OnMouseDown()
    {
        if(!Menu.NIERUSZAC)
            OnMouse(Jednostka.Select, 1);
    }

    public void OnMouse(GameObject poruszany, int dostane, bool dwa)
    {
        OnMouse(poruszany,dostane);
        OnMouse(poruszany,dostane);
    }

    public void OnMouse(GameObject poruszany, int dostane)
    {
        if(!idzie && poruszany != null)
        {
            if(MenuGlowne.multi && dostane == 1 && !SimultanTurns.simultanTurns)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                Jednostka jednostkaComponent = poruszany.GetComponent<Jednostka>();
                
                if(poruszany != null  && jednostkaComponent != null && poruszany.GetComponent<Jednostka>().druzyna == Menu.tura)
                    photonView.RPC("ZaktualizujRuch", RpcTarget.All, poruszany.GetComponent<Jednostka>().nr_jednostki, Ip.ip);
            }

            if(droga[0]==null)
            {
                if((poruszany!=null && !Zajete && !ZajeteLot && ( Jednostka.CzyJednostka && poruszany.GetComponent<Jednostka>().druzyna == Menu.tura) || dostane == 0))
                {
                    int x = (int)poruszany.transform.position.x;
                    int y = (int)poruszany.transform.position.y;
                    pomocnicza[0]=1;
                    pomocnicza[1]=1;
                    pomocnicza[2] = poruszany.GetComponent<Jednostka>().szybkosc;
                    lista1[0] = Menu.kafelki[x][y];
                    lista2[0] = Menu.kafelki[(int)kafelek.transform.position.x][(int)kafelek.transform.position.y];
                    int n=0;
                    while(spr()==0 && n<25)
                    {
                        if(poruszany.GetComponent<Jednostka>().lata)
                            wybordrogiLot(n);
                        else
                                wybordrogi(n);
                        n++;
                        if(n==25)
                        {
                            Clean();
                         }        
                    }
                }
            }
            else
            {
                if(kafelek==LastDroga)
                {
                    Menu.kafelki[(int)poruszany.transform.position.x][(int)poruszany.transform.position.y].GetComponent<Pole>().Zajete = false;
                    Menu.kafelki[(int)poruszany.transform.position.x][(int)poruszany.transform.position.y].GetComponent<Pole>().ZajeteLot = false;
                    Menu.kafelki[(int)poruszany.transform.position.x][(int)poruszany.transform.position.y].GetComponent<Pole>().postac = null;
                    
                    idzie = true;
                if(poruszany.GetComponent<Jednostka>().lata)
                    StartCoroutine(LotPowoli(poruszany));
                else
                    StartCoroutine(RuchPowoli(poruszany));
                }
                else
                {
                    Clean2();
                }
            }
        }
    }
    public int spr()
    {
        bool czy_gotowe = false;
        for(int i =0;i<25;i++)
        {
            for(int j =0;j<25;j++)
            {
                if(lista2[j]==null)
                {
                    break;
                }
                if(lista1[i]==lista2[j])
                {     
                    czy_gotowe = true;
                    if(droga[0]==null)
                    {
                        droga[0]=lista1[i];
                    }
                    else
                    {
                        if(lista1[i].GetComponent<Pole>().CzasDrogi < droga[0].GetComponent<Pole>().CzasDrogi)
                        {
                            droga[0] = lista1[i];
                        }
                    }
                }
            }  
                if(lista1[i]==null)
                {
                    break;
                } 
        }
        if(czy_gotowe)
        {
                int x = (int)droga[0].transform.position.x;
                    int y = (int)droga[0].transform.position.y; 
                    int n=0;
                    while(droga[0]!=lista1[0])
                    {
                        for(int k=0;k<=n;k++)
                        {
                            droga[n-k+1]=droga[n-k];
                        }
                        switch(droga[1].GetComponent<Pole>().Nin)
                        {
                            case 1: x++; y++; break;
                            case 2: x++; break;
                            case 3: x++; y--; break;
                            case 4: y--; break;
                            case 5: x--; y--; break;
                            case 6: x--; break;
                            case 7: x--; y++; break;
                            case 8: y++; break;
                        }
                        droga[0] = Menu.kafelki[x][y];
                        n++;
                    }
                    if(droga[n] != null && n-1>0 && droga[n].GetComponent<Pole>().Nin != 0 && droga[n].GetComponent<Pole>().Nout != 0 && droga[n-1].GetComponent<Pole>().Nin != 0
                    && droga[n-1].GetComponent<Pole>().Nout != 0)
                    {
                        Debug.Log(droga[n-1].name);
                        Debug.Log(droga[n].name);
                        dzinaPoprawkoa(n-1,n);
                    }
                    x = (int)droga[n].transform.position.x;
                    y = (int)droga[n].transform.position.y; 
                    while(droga[n]!=lista2[0])
                    {
                        switch(droga[n].GetComponent<Pole>().Nout)
                        {
                            case 1: x++; y++; break;
                            case 2: x++; break;
                            case 3: x++; y--; break;
                            case 4: y--; break;
                            case 5: x--; y--; break;
                            case 6: x--; break;
                            case 7: x--; y++; break;
                            case 8: y++; break;
                        }
                        droga[n+1] = Menu.kafelki[x][y];
                        n++;
                    }
                    
                    n=0;
                    while(droga[n].GetComponent<Pole>().Nout==0)
                    {
                        if(droga[n+1]==null)
                        {
                            break;
                        }
                        droga[n].GetComponent<Pole>().Nout = (droga[n+1].GetComponent<Pole>().Nin + 3 )% 8 + 1;
                        n++;
                       // Debug.Log("zgery" + droga[n].name);
                        
                    }
                    //n++;
                    while(droga[n]!=null)
                    {
                        droga[n].GetComponent<Pole>().Nin = (droga[n-1].GetComponent<Pole>().Nout + 3 )% 8 + 1;
                        droga[n].GetComponent<Pole>().CzasDrogi = droga[n-1].GetComponent<Pole>().CzasDrogi + droga[n].GetComponent<Pole>().trudnosc + (droga[n].GetComponent<Pole>().Nin % 2);
                       // Debug.Log("pala" + droga[n].name);
                        n++;
                    }
                    droga[0].GetComponent<Pole>().Nin=0;
                    droga[n-1].GetComponent<Pole>().Nout=0;
                    LastDroga=droga[n-1];

                   Clean();

                    return 1;
        }
        return 0;
    }
    private void dzinaPoprawkoa(int jeden, int dwa)
    {
        Vector3 roznica = droga[dwa].transform.position - droga[jeden].transform.position;
        if(roznica.x == 1 && roznica.y == 0)
            {droga[jeden].GetComponent<Pole>().Nout = 2; droga[jeden].GetComponent<Pole>().Nin = 6;}
        if(roznica.x == -1 && roznica.y == 0)
            {droga[jeden].GetComponent<Pole>().Nout = 6; droga[jeden].GetComponent<Pole>().Nin = 2;}
        if(roznica.x == 0 && roznica.y == -1)
            {droga[jeden].GetComponent<Pole>().Nout = 4; droga[jeden].GetComponent<Pole>().Nin = 8;}
        if(roznica.x == 0 && roznica.y == 1)
            {droga[jeden].GetComponent<Pole>().Nout = 8; droga[jeden].GetComponent<Pole>().Nin = 4;}
        
    }
   public void Clean()
    {
        int i = 0;

        while(lista1[i]!=null)
        {
            if(!IsInDroga(lista1[i]))
            {
                lista1[i].GetComponent<Pole>().Nin=0;
                lista1[i].GetComponent<Pole>().Nout=0;
                lista1[i].GetComponent<Pole>().CzasDrogi=0;
            }
            lista1[i]=null;
            i++;
        }
        i=0;
        while(lista2[i]!=null )
        {
            if(!IsInDroga(lista2[i]))
            {
                lista2[i].GetComponent<Pole>().Nin=0;
                lista2[i].GetComponent<Pole>().Nout=0;
                lista2[i].GetComponent<Pole>().CzasDrogi=0;
            }
            lista2[i]=null;
            i++;
        }
    }
   public static void Clean2(int spr)
    {
        int i = 0;
        while(droga[i]!=null)
        {
            droga[i].GetComponent<Pole>().Nin=0;
            droga[i].GetComponent<Pole>().Nout=0;
            droga[i].GetComponent<Pole>().CzasDrogi=0;
            droga[i]=null;
            i++;
        }
    }
   public static void Clean2()
    {
        int i = 0;
        while(droga[i]!=null)
        {
            droga[i].GetComponent<Pole>().Nin=0;
            droga[i].GetComponent<Pole>().Nout=0;
            droga[i].GetComponent<Pole>().CzasDrogi=0;
            droga[i]=null;
            i++;
        }
    if(MenuGlowne.multi)
        Menu.kafelki[0][0].GetComponent<Pole>().wywolajCealn();
    }
    public void wywolajCealn()
    {
        // PhotonView photonView = GetComponent<PhotonView>();
        // photonView.RPC("CleanMulti", RpcTarget.All, Ip.ip);
    }

    [PunRPC]
    public void CleanMulti(int ip)
    {
    int i = 0;
    if(ip!=Ip.ip && !MenuGlowne.nieCelanMulti)
    {
        while(droga[i]!=null)
        {
            droga[i].GetComponent<Pole>().Nin=0;
            droga[i].GetComponent<Pole>().Nout=0;
            droga[i].GetComponent<Pole>().CzasDrogi=0;
            droga[i]=null;
            i++;
        }
    }
    }

bool IsInDroga(GameObject lista)
{
int i=0;
   while(null != droga[i])
   {
    if(droga[i]==lista)
    {
        return true;
    }
    i++;
   }
   return false;
}


    public void wybordrogi(int n)
    {
                
            for(int j =0;j<pomocnicza[0];j++)
            {
               if(lista1[j].GetComponent<Pole>().CzasDrogi == n)
               {
            int x = (int)lista1[j].transform.position.x;
            int y = (int)lista1[j].transform.position.y;
            if (x + 1 >= 0 && x + 1 < Menu.BoardSizeX && y >= 0 && y < Menu.BoardSizeY &&
                Menu.kafelki[x + 1][y].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x + 1][y].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x + 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x + 1][y];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 6;
                pomocnicza[0]++;
            }

            if (x - 1 >= 0 && x - 1 < Menu.BoardSizeX && y >= 0 && y < Menu.BoardSizeY &&
                Menu.kafelki[x - 1][y].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x - 1][y].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x - 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x - 1][y];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 2;
                pomocnicza[0]++;
            }

            if (x >= 0 && x < Menu.BoardSizeX && y + 1 >= 0 && y + 1 < Menu.BoardSizeY &&
                Menu.kafelki[x][y + 1].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x][y + 1].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x][y + 1];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 4;
                pomocnicza[0]++;
            }

            if (x >= 0 && x < Menu.BoardSizeX && y - 1 >= 0 && y - 1 < Menu.BoardSizeY &&
                Menu.kafelki[x][y - 1].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x][y - 1].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x][y - 1];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 8;
                pomocnicza[0]++;
            }

            if (x + 1 >= 0 && x + 1 < Menu.BoardSizeX && y + 1 >= 0 && y + 1 < Menu.BoardSizeY &&
                Menu.kafelki[x + 1][y + 1].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x + 1][y + 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x][y + 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x + 1][y].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x + 1][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x + 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x + 1][y + 1];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc + 1;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 5;
                pomocnicza[0]++;
            }

            if (x + 1 >= 0 && x + 1 < Menu.BoardSizeX && y - 1 >= 0 && y - 1 < Menu.BoardSizeY &&
                Menu.kafelki[x + 1][y - 1].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x + 1][y - 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x][y - 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x + 1][y].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x + 1][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x + 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x + 1][y - 1];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc + 1;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 7;
                pomocnicza[0]++;
            }

            if (x - 1 >= 0 && x - 1 < Menu.BoardSizeX && y + 1 >= 0 && y + 1 < Menu.BoardSizeY &&
                Menu.kafelki[x - 1][y + 1].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x - 1][y + 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x][y + 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x - 1][y].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x - 1][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x - 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x - 1][y + 1];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc + 1;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 3;
                pomocnicza[0]++;
            }

            if (x - 1 >= 0 && x - 1 < Menu.BoardSizeX && y - 1 >= 0 && y - 1 < Menu.BoardSizeY &&
                Menu.kafelki[x - 1][y - 1].GetComponent<Pole>().Nin == 0 &&
                Menu.kafelki[x - 1][y - 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x][y - 1].GetComponent<Pole>().Zajete == false &&
                Menu.kafelki[x - 1][y].GetComponent<Pole>().Zajete == false &&
                Math.Abs(Menu.kafelki[x - 1][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x - 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                Math.Abs(Menu.kafelki[x][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1)
            {
                lista1[pomocnicza[0]] = Menu.kafelki[x - 1][y - 1];
                lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n + lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc + 1;
                lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 1;
                pomocnicza[0]++;
            }

               }
            }
            for(int j =0;j<pomocnicza[1];j++)
            {
               if(lista2[j].GetComponent<Pole>().CzasDrogi == n)
               {
            int x = (int)lista2[j].transform.position.x;
            int y = (int)lista2[j].transform.position.y;
                  if (x + 1 >= 0 && x + 1 < Menu.BoardSizeX && y >= 0 && y < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x + 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x + 1][y].GetComponent<Pole>().Nout == 0 && Menu.kafelki[x + 1][y].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x + 1][y];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 6;
                    pomocnicza[1]++;
                }

                if (x - 1 >= 0 && x - 1 < Menu.BoardSizeX && y >= 0 && y < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x - 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x - 1][y].GetComponent<Pole>().Nout == 0 && Menu.kafelki[x - 1][y].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x - 1][y];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 2;
                    pomocnicza[1]++;
                }

                if (x >= 0 && x < Menu.BoardSizeX && y + 1 >= 0 && y + 1 < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x][y + 1].GetComponent<Pole>().Nout == 0 && Menu.kafelki[x][y + 1].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x][y + 1];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 4;
                    pomocnicza[1]++;
                }

                if (x >= 0 && x < Menu.BoardSizeX && y - 1 >= 0 && y - 1 < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x][y - 1].GetComponent<Pole>().Nout == 0 && Menu.kafelki[x][y - 1].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x][y - 1];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 8;
                    pomocnicza[1]++;
                }

                if (x + 1 >= 0 && x + 1 < Menu.BoardSizeX && y + 1 >= 0 && y + 1 < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x + 1][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x + 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x + 1][y + 1].GetComponent<Pole>().Nout == 0 &&
                    Menu.kafelki[x + 1][y + 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x][y + 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x + 1][y].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x + 1][y + 1];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc + 1;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 5;
                    pomocnicza[1]++;
                }

                if (x + 1 >= 0 && x + 1 < Menu.BoardSizeX && y - 1 >= 0 && y - 1 < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x + 1][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x + 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x + 1][y - 1].GetComponent<Pole>().Nout == 0 &&
                    Menu.kafelki[x + 1][y - 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x][y - 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x + 1][y].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x + 1][y - 1];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc + 1;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 7;
                    pomocnicza[1]++;
                }

                if (x - 1 >= 0 && x - 1 < Menu.BoardSizeX && y + 1 >= 0 && y + 1 < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x - 1][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x - 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x][y + 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x - 1][y + 1].GetComponent<Pole>().Nout == 0 &&
                    Menu.kafelki[x - 1][y + 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x][y + 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x - 1][y].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x - 1][y + 1];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc + 1;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 3;
                    pomocnicza[1]++;
                }

                if (x - 1 >= 0 && x - 1 < Menu.BoardSizeX && y - 1 >= 0 && y - 1 < Menu.BoardSizeY &&
                    Math.Abs(Menu.kafelki[x - 1][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x - 1][y].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Math.Abs(Menu.kafelki[x][y - 1].GetComponent<Pole>().poziom - Menu.kafelki[x][y].GetComponent<Pole>().poziom) <= 1 &&
                    Menu.kafelki[x - 1][y - 1].GetComponent<Pole>().Nout == 0 &&
                    Menu.kafelki[x - 1][y - 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x][y - 1].GetComponent<Pole>().Zajete == false &&
                    Menu.kafelki[x - 1][y].GetComponent<Pole>().Zajete == false)
                {
                    lista2[pomocnicza[1]] = Menu.kafelki[x - 1][y - 1];
                    lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n + lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc + 1;
                    lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 1;
                    pomocnicza[1]++;
                }

               }
            }     
    }
    public void wybordrogiLot(int n)
    {
                
            for(int j =0;j<pomocnicza[0];j++)
            {
               if(lista1[j].GetComponent<Pole>().CzasDrogi == n)
               {
            int x = (int)lista1[j].transform.position.x;
            int y = (int)lista1[j].transform.position.y;
                    if(x+1>=0 && x+1 < Menu.BoardSizeX && y>=0 && y < Menu.BoardSizeY) if(Menu.kafelki[x+1][y].GetComponent<Pole>().Nin == 0)
                    {lista1[pomocnicza[0]] = Menu.kafelki[x+1][y];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 6; pomocnicza[0]++; }
                    if(x-1>=0 && x-1 < Menu.BoardSizeX && y>=0 && y < Menu.BoardSizeY)if(Menu.kafelki[x-1][y].GetComponent<Pole>().Nin == 0)
                    { lista1[pomocnicza[0]] = Menu.kafelki[x-1][y];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 2; pomocnicza[0]++;}
                    if(x>=0 && x < Menu.BoardSizeX && y+1 >=0 && y+1 < Menu.BoardSizeY) if(Menu.kafelki[x][y+1].GetComponent<Pole>().Nin == 0) 
                    {lista1[pomocnicza[0]] = Menu.kafelki[x][y+1];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 4; pomocnicza[0]++;}
                    if(x>=0 && x < Menu.BoardSizeX && y-1 >=0 && y-1 < Menu.BoardSizeY) if(Menu.kafelki[x][y-1].GetComponent<Pole>().Nin == 0) 
                    {lista1[pomocnicza[0]] = Menu.kafelki[x][y-1];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 8; pomocnicza[0]++;}
                    if(x+1>=0 && x+1 < Menu.BoardSizeX && y+1>=0 && y+1 < Menu.BoardSizeY)if(Menu.kafelki[x+1][y+1].GetComponent<Pole>().Nin == 0) 
                    {lista1[pomocnicza[0]] = Menu.kafelki[x+1][y+1];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc+1;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 5; pomocnicza[0]++;}
                    if(x+1>=0 && x+1 < Menu.BoardSizeX && y-1>=0 && y-1 < Menu.BoardSizeY)if(Menu.kafelki[x+1][y-1].GetComponent<Pole>().Nin == 0) 
                    {lista1[pomocnicza[0]] = Menu.kafelki[x+1][y-1];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc+1;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 7; pomocnicza[0]++;}
                    if(x-1>=0 && x-1 < Menu.BoardSizeX && y+1>=0 && y+1 < Menu.BoardSizeY)if(Menu.kafelki[x-1][y+1].GetComponent<Pole>().Nin == 0) 
                    {lista1[pomocnicza[0]] = Menu.kafelki[x-1][y+1];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc+1;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 3; pomocnicza[0]++;}
                    if(x-1>=0 && x-1 < Menu.BoardSizeX && y-1>=0 && y-1 < Menu.BoardSizeY)if(Menu.kafelki[x-1][y-1].GetComponent<Pole>().Nin == 0) 
                    {lista1[pomocnicza[0]] = Menu.kafelki[x-1][y-1];lista1[pomocnicza[0]].GetComponent<Pole>().CzasDrogi = n+lista1[pomocnicza[0]].GetComponent<Pole>().trudnosc+1;lista1[pomocnicza[0]].GetComponent<Pole>().Nin = 1; pomocnicza[0]++;}
               }
            }
            for(int j =0;j<pomocnicza[1];j++)
            {
               if(lista2[j].GetComponent<Pole>().CzasDrogi == n)
               {
            int x = (int)lista2[j].transform.position.x;
            int y = (int)lista2[j].transform.position.y;
                    if(x+1>=0 && x+1 < Menu.BoardSizeX && y>=0 && y < Menu.BoardSizeY)if(Menu.kafelki[x+1][y].GetComponent<Pole>().Nout == 0)
                    {lista2[pomocnicza[1]] = Menu.kafelki[x+1][y];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 6; pomocnicza[1]++;}
                    if(x-1>=0 && x-1 < Menu.BoardSizeX && y>=0 && y < Menu.BoardSizeY)if(Menu.kafelki[x-1][y].GetComponent<Pole>().Nout == 0 )
                    { lista2[pomocnicza[1]] = Menu.kafelki[x-1][y];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 2; pomocnicza[1]++;}
                    if(x>=0 && x < Menu.BoardSizeX && y+1 >=0 && y+1 < Menu.BoardSizeY) if(Menu.kafelki[x][y+1].GetComponent<Pole>().Nout == 0) 
                    {lista2[pomocnicza[1]] = Menu.kafelki[x][y+1];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 4; pomocnicza[1]++;}
                    if(x>=0 && x < Menu.BoardSizeX && y-1 >=0 && y-1 < Menu.BoardSizeY) if(Menu.kafelki[x][y-1].GetComponent<Pole>().Nout == 0) 
                    {lista2[pomocnicza[1]] = Menu.kafelki[x][y-1];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 8; pomocnicza[1]++;}
                    if(x+1>=0 && x+1 < Menu.BoardSizeX && y+1>=0 && y+1 < Menu.BoardSizeY)if(Menu.kafelki[x+1][y+1].GetComponent<Pole>().Nout == 0 ) 
                    {lista2[pomocnicza[1]] = Menu.kafelki[x+1][y+1];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc+1;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 5; pomocnicza[1]++;}
                    if(x+1>=0 && x+1 < Menu.BoardSizeX && y-1>=0 && y-1 < Menu.BoardSizeY)if(Menu.kafelki[x+1][y-1].GetComponent<Pole>().Nout == 0) 
                    {lista2[pomocnicza[1]] = Menu.kafelki[x+1][y-1];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc+1;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 7; pomocnicza[1]++;}
                    if(x-1>=0 && x-1 < Menu.BoardSizeX && y+1>=0 && y+1 < Menu.BoardSizeY)if(Menu.kafelki[x-1][y+1].GetComponent<Pole>().Nout == 0) 
                    {lista2[pomocnicza[1]] = Menu.kafelki[x-1][y+1];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc+1;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 3; pomocnicza[1]++;}
                    if(x-1>=0 && x-1 < Menu.BoardSizeX && y-1>=0 && y-1 < Menu.BoardSizeY)if(Menu.kafelki[x-1][y-1].GetComponent<Pole>().Nout == 0) 
                    {lista2[pomocnicza[1]] = Menu.kafelki[x-1][y-1];lista2[pomocnicza[1]].GetComponent<Pole>().CzasDrogi = n+lista2[pomocnicza[1]].GetComponent<Pole>().trudnosc+1;lista2[pomocnicza[1]].GetComponent<Pole>().Nout = 1; pomocnicza[1]++;}
               }
            }     
    }

     IEnumerator RuchPowoli(GameObject ziomek)
     {
         int ko=0;
        GameObject blok = null;
        while(droga[ko + 1]!=null)
        {
            ko++;
            if(droga[ko].GetComponent<Pole>().CzasDrogi > ziomek.GetComponent<Jednostka>().szybkosc - 2)
            {
                while(droga[ko].GetComponent<Pole>().ZajeteLot && ko > 0)
                {
                    blok = droga[ko];
                    ko--;
                }
                break;
            }     
        }
        int k = 0;
        while(droga[k+1]!=null && droga[k + 1] != blok)
        {
            
            if(ziomek.GetComponent<Jednostka>().szybkosc >= droga[k].GetComponent<Pole>().trudnosc + (droga[k].GetComponent<Pole>().Nout % 2))
            {
                if(kafelek.GetComponent<PoleOdkryj>().dark == null)
                    ziomek.GetComponent<Jednostka>().walkSound();
                ziomek.GetComponent<Jednostka>().szybkosc -= droga[k].GetComponent<Pole>().trudnosc + (droga[k].GetComponent<Pole>().Nout % 2);
                Vector3 start = droga[k].transform.position;
                Vector3 cel = droga[k+1].transform.position;
                Menu.kafelki[(int)start.x][(int)start.y].GetComponent<Pole>().postac = null;
                for(int i=0;i<20;i++)
                {
                    ziomek.transform.position += (cel - start)/20;
                    yield return new WaitForSeconds(0.015f);
                }
                ziomek.transform.position = new Vector3(Mathf.Round(ziomek.transform.position.x), Mathf.Round(ziomek.transform.position.y), -2);
                Menu.kafelki[(int)cel.x][(int)cel.y].GetComponent<Pole>().postac = ziomek;
                k++;
                yield return new WaitForSeconds(0.015f);
            }
            else{
                break;
            }
            ziomek.GetComponent<Jednostka>().odkryj(4);
            if(SimultanTurns.simultanTurns && MenuGlowne.multi)
                {
                PhotonView photonView = ziomek.GetComponent<PhotonView>();
                photonView.RPC("poprawkaMulti", RpcTarget.All, ziomek.transform.position.x, ziomek.transform.position.y);
                }
            
        }

        Clean2(0);
        Menu.kafelki[(int)ziomek.transform.position.x][(int)ziomek.transform.position.y].GetComponent<Pole>().Zajete = true;
        Menu.kafelki[(int)ziomek.transform.position.x][(int)ziomek.transform.position.y].GetComponent<Pole>().postac = ziomek; 
        idzie = false;
    }
    IEnumerator LotPowoli(GameObject ziomek)
    {
         int ko=0;
        GameObject blok = null;
        while(droga[ko + 1]!=null)
        {
            ko++;
            if(droga[ko].GetComponent<Pole>().CzasDrogi > ziomek.GetComponent<Jednostka>().szybkosc - 2)
            {
                while(droga[ko].GetComponent<Pole>().Zajete && ko > 0)
                {
                    blok = droga[ko];
                    ko--;
                }
                break;
            }     

        }
        int k = 0;
        while(droga[k+1]!=null && droga[k + 1] != blok)
        {
            if(ziomek.GetComponent<Jednostka>().szybkosc >= trudnosc + (droga[k].GetComponent<Pole>().Nout % 2))
            {
                ziomek.GetComponent<Jednostka>().szybkosc -= trudnosc + (droga[k].GetComponent<Pole>().Nout % 2);
                Vector3 start = droga[k].transform.position;
                Vector3 cel = droga[k+1].transform.position;
                for(int i=0;i<20;i++)
                {
                    ziomek.transform.position += (cel - start)/20;
                    yield return new WaitForSeconds(0.015f);
                }
                ziomek.transform.position = new Vector3(Mathf.Round(ziomek.transform.position.x), Mathf.Round(ziomek.transform.position.y), -2);
                k++;
                yield return new WaitForSeconds(0.015f);
            }
            else{
                break;
            }
            ziomek.GetComponent<Jednostka>().odkryj(4);
            if(SimultanTurns.simultanTurns && MenuGlowne.multi)
                {
                PhotonView photonView = ziomek.GetComponent<PhotonView>();
                photonView.RPC("poprawkaMulti", RpcTarget.All, ziomek.transform.position.x, ziomek.transform.position.y);
                }
        }
        Clean2(0);
        Menu.kafelki[(int)ziomek.transform.position.x][(int)ziomek.transform.position.y].GetComponent<Pole>().ZajeteLot = true;
        postac = ziomek;
        idzie = false;
    }
        
    
}
