using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;



public class Menu : MonoBehaviour
{
    public static int xd;
    public GameObject kafelek;
    public static int BoardSizeX = 25;
    public static int BoardSizeY = 25;
    public static GameObject[][] kafelki;
    public GameObject obiekt1;
    public static bool NIERUSZAC;

    public static int[] zloto = new int[5];
    public static int[] drewno = new int[5];
    public static int[] diament = new int[5];
    public static int[] magia = new int[5];
    public static int[] ludnosc = new int[5];
    public static int[] maxludnosc = new int[5];
    public static int[] ratuszPoziom = new int[5];
    public static GameObject[] heros = new GameObject[5];

    public static GameObject kamera;
    public GameObject camerapriv;


    public static GameObject[,] jednostki = new GameObject[5, 50];
    public static List<GameObject> NPC = new List<GameObject>();


    public static GameObject[,] bazy = new GameObject[5, 10];
    public static int[] bazyIlosc = new int[5];

    public static int IloscGraczy;
    public static int tura;
    public static int nrTury = 1;
    public GameObject PrivPanelUnity;
    public static GameObject PanelUnit;
    public GameObject PrivPanelBuild;
    public static GameObject PanelBuild;
    public static bool Next;

    public Image KolejnaTuraButton;
    public GameObject InfoKolejnaTura;
    private int NaPewnoKoniec;
    private int NumerTypa;
    public Text[] infoText;
    public string[] infoString;
    public static int idInfo;

    public Slider turaNPC;
    public Text nrTuryText;
    public Image kolejnaTuraImage;

    public static Menu menu;


    private void Start()
    {
        menu = GetComponent<Menu>();
        if(!MenuGlowne.wczytka)
        {
            kamera = camerapriv;

            zloto[1] = 14;
            drewno[1] = 14;
            zloto[2] = 15;
            drewno[2] = 15;
            zloto[3] = 16;
            drewno[3] = 16;
            zloto[4] = 17;
            drewno[4] = 17;
        }

        kafelki = new GameObject[BoardSizeX][];
        for (int i = 0; i < BoardSizeX; i++)
        {
            kafelki[i] = new GameObject[BoardSizeY];
        }
         if(!MenuGlowne.multi) 
         {
             GetComponent<MapLoad>().LoadMapData();
         }

        // if(Ip.ip == 1)
        //     Aktualizuj(tura, IloscGraczy);
        // if(IloscGraczy==0)
        //     IloscGraczy=2;
        PanelUnit = PrivPanelUnity;
        PanelBuild = PrivPanelBuild;
        PanelBuild.SetActive(false);
        PanelUnit.SetActive(false);
        tura = 1;
        turaNPC.gameObject.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote)) // Tylda znajduje się na klawiszu BackQuote
        {
            Debug.Log(PoleOdkryj.mgla);
            Debug.Log(SimultanTurns.simultanTurns);
        }
        if (Input.GetMouseButtonDown(1))
        {
            usunSelect2();
        }
        if(MenuGlowne.multi && !SimultanTurns.simultanTurns)
            if((Ip.ip == tura))
                NIERUSZAC = false;
            else
            {
                NIERUSZAC = true;
            }
        if(tura > IloscGraczy && Ip.ip == 1)
        {
            tura=0;
            turaNPC.value = 0;
            StartCoroutine(NPCtura(0));
        }
        // if(MenuGlowne.multi)
        // {
        //     StartCoroutine(Aktualizuj(tura, IloscGraczy));
        // }
        switch (tura)
        {
            case 0: KolejnaTuraButton.color = new Color(0.0f, 0.0f, 0.0f); break;
            case 1: KolejnaTuraButton.color = new Color(1.0f, 0.0f, 0.0f); break;
            case 2: KolejnaTuraButton.color = new Color(0.0f, 1.0f, 0.0f); break;
            case 3: KolejnaTuraButton.color = new Color(0.0f, 0.0f, 1.0f); break;
            case 4: KolejnaTuraButton.color = new Color(1.0f, 1.0f, 0.0f); break;
        }
    }

    IEnumerator Aktualizuj(int tura, int IloscGraczy)
    {
        yield return new WaitForSeconds(0.2f);

        if (tura != Menu.tura || IloscGraczy != Menu.IloscGraczy)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, tura, BoardSizeX, BoardSizeY, IloscGraczy, nrTury);
        }
    }


    [PunRPC]
    void ZaktualizujStatystykiRPC(int tura, int BoardSizeX, int BoardSizeY, int IloscGraczy, int nrTury)
    {
        Menu.tura = tura;
        Menu.BoardSizeX = BoardSizeX;
        Menu.BoardSizeY = BoardSizeY;
        Menu.IloscGraczy = IloscGraczy;
        Menu.nrTury = nrTury;
    }

    [PunRPC]
    public void aktualizacjaId(int[] bazy)
    {
        bazyIlosc = bazy;
    }

    [PunRPC]
    public void koniecMulti(int wygrany)
    {
        End.wygrany = wygrany;
        SceneManager.LoadScene(3); 
    }

    public static void usunSelect2()
    {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Jednostka.Select2 = null;
            Jednostka.wybieranie = false;
    }

    [PunRPC]
     IEnumerator nowaTuraPrzypomnienie(int tura)
    {
        if(tura+1 == Ip.ip)
        {

            // Przenieś obraz na początkową pozycję
            kolejnaTuraImage.transform.localPosition = new Vector3(0, kolejnaTuraImage.transform.localPosition.y, kolejnaTuraImage.transform.localPosition.z);
            
            // Stopniowo zwiększ przezroczystość
            Color color = kolejnaTuraImage.color;
            float alpha = color.a;
            float duration = 1f;
            color.a = 0f;

            // Fade in
            for (float t = 0.01f; t < duration; t += 0.01f)
            {
                color.a += 0.01f / duration;
                kolejnaTuraImage.color = color;
                yield return 0.01f;
            }
            for (float t = 0.01f; t < duration; t += 0.01f)
            {
                color.a -= 0.01f / duration;
                kolejnaTuraImage.color = color;
                yield return 0.01f;
            }
            color.a = 1;
            kolejnaTuraImage.color = color;

            // Zmień pozycję
            kolejnaTuraImage.transform.localPosition = new Vector3(1300, kolejnaTuraImage.transform.localPosition.y, kolejnaTuraImage.transform.localPosition.z);

        }
    }

    public void NextTurn()
    {
        if((!NIERUSZAC || (SimultanTurns.ready && SimultanTurns.readyCount == 0)) && (!MenuGlowne.multi || (Ip.ip == tura || tura == 0)))
        {
            NaPewnoKoniec++;
            StartCoroutine(PrzedKoniec());
            for (int i = 0; i < 50; i++)
            {
                if (jednostki[tura, i] == null)
                {
                    i--;
                    break;
                }
                if (jednostki[tura, i].GetComponent<Jednostka>().szybkosc == jednostki[tura, i].GetComponent<Jednostka>().maxszybkosc &&
                jednostki[tura, i].activeSelf && jednostki[tura, i].GetComponent<Jednostka>().akcja && !jednostki[tura, i].GetComponent<Jednostka>().spanie && NaPewnoKoniec != 2)
                {
                    NumerTypa = i;
                    idInfo = 0;
                    for(int k = 0; k<=2;k++)
                        infoText[k].text = infoString[k];
                    InfoKolejnaTura.SetActive(true);
                    NaPewnoKoniec = 0;
                }
            }
            if (NaPewnoKoniec > 0)
            {
                if(!SimultanTurns.simultanTurns || SimultanTurns.ready)
                {
                    if(SimultanTurns.simultanTurns)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("przelocznikMulti", RpcTarget.All);
                    }
                    else
                        StartCoroutine(Przelocznik());
                }
                NaPewnoKoniec = 0;

                if (MenuGlowne.multi && tura == Ip.ip)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("aktualizacjaId", RpcTarget.All, bazyIlosc);

                }

                if(SimultanTurns.simultanTurns)
                {
                    NIERUSZAC = true;
                    SimultanTurns.EndTurn();
                }
                else
                {
                    if (MenuGlowne.multi)
                    {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("nowaTuraPrzypomnienie", RpcTarget.All, tura);
                  //  StartCoroutine(nowaTuraPrzypomnienie(tura));
                    }
                    tura++;
                }

                int zostalo = 0;
                int wygrany = 0;

                for(int i = 1; i<5;i++)
                {
                    if(bazyIlosc[i]==0)
                    {
                        
                        WyburRas.aktywny[i-1] = false;
                        zostalo++;
                    }
                    else
                        wygrany = i;
                }
                IloscGraczy = 4 - zostalo;
                if(MenuGlowne.multi)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("IloscUpdate", RpcTarget.All, IloscGraczy);
                }
                if(zostalo == 3)
                {
                    End.wygrany = wygrany;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("koniecMulti", RpcTarget.All, wygrany);
                    }
                    SceneManager.LoadScene(3);   //KONIEC GRY
                }
                
                if(!SimultanTurns.simultanTurns)
                {
                    for(int xd = 1; xd<5;xd++)
                        if (tura == xd && !WyburRas.aktywny[xd-1])
                            tura++;
                    if (tura > IloscGraczy)
                        tura = 0;
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("ZaktualizujStatystykiRPC", RpcTarget.All, tura, BoardSizeX, BoardSizeY, IloscGraczy, nrTury);
                    }
                }
                Jednostka.Select = null;
                PanelUnit.SetActive(false);
                PanelBuild.SetActive(false);
                przyciskiClear();

                usunSelect2();
                if(!MenuGlowne.multi)
                    if(tura != 0)
                        StartCoroutine(Ratusz.ruchPlynnyCamery(tura));
                //StartCoroutine(Przelocznik());

                if(tura == 0)
                {
                    if(MenuGlowne.multi)
                    {
                        try{
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("ZaktualizujNPC", RpcTarget.All);
                        } catch(Exception ex){Debug.Log(ex.ToString());}
                        
                    }
                    else
                    {
                        if(Menu.NPC.Count != 0)
                        {
                            NIERUSZAC = true;
                            turaNPC.value = 0;
                            StartCoroutine(NPCtura(0));
                        }
                        else
                        {
                            nrTury++;
                            nrTuryText.text = nrTury.ToString();
                            tura++;
                        }
                    }
                }
            }
        }
    }
    [PunRPC]
    void IloscUpdate(int x)
    {
        IloscGraczy = x;
    }
    [PunRPC]
    void ZaktualizujNPC()
    {
        nrTuryText.text = (nrTury + 1).ToString();
        if(Ip.ip == 1)
        {
            if(Menu.NPC.Count != 0)
                {
                    NIERUSZAC = true;
                    turaNPC.value = 0;
                    StartCoroutine(NPCtura(0));
                }
                else
                {
                if(!SimultanTurns.simultanTurns)
                    nrTury++;
                
                tura++;
            }
        }
        
    }
    public static bool istnieje(int x, int y)
    {
        if(x >= 0 && x <= (Menu.BoardSizeX -1) && y >= 0 && y <= (Menu.BoardSizeY -1))
            return true;
        return false;
    }

    public IEnumerator NPCtura(int id)
    {
        Pole.Clean2();
        turaNPC.gameObject.SetActive(true);
        // if(NPC[id] == null)
        // {
        //     NPC.RemoveAt(id);
        //     if(Menu.NPC.Count - 1 > id)
        //     {
        //         turaNPC.value = id;
        //         turaNPC.maxValue = Menu.NPC.Count - 2;
        //         yield return new WaitForSeconds(0.15f);
        //         StartCoroutine(NPCtura(id + 1));
        //     }
        //      yield break;
        // }
        //Debug.Log(NPC[id].name + " " + id + " " + NPC[id].GetComponent<Jednostka>().nr_jednostki);
        if(NPC[id]== null)
            NPC.RemoveAt(id);
        GameObject postacGracza = null;
        try
        {
            postacGracza = przeszukanie(1, NPC[id]);
        }
        catch (Exception e)
        {
            Debug.Log("Exception caught: " + e.Message);
        }
        yield return new WaitForSeconds(0.15f);
        if(postacGracza != null)
            {
                GameObject pole = null;
                int close = 99;
                    yield return new WaitForSeconds(0.2f);
                    Jednostka.Select = postacGracza;//NPC[id];
                    if(!(MenuGlowne.multi && Menu.tura==0))
                        Interface.przeniesDoSelect();
                    yield return new WaitForSeconds(0.2f);
                    Jednostka.CzyJednostka = true;

                if(Walka.odleglosc(postacGracza, NPC[id]) > NPC[id].GetComponent<Jednostka>().zasieg)
                {
                    if(NPC[id].GetComponent<Jednostka>().zasieg == 3)
                    {
                        Debug.Log("zdetu");
                        int x = (int)postacGracza.transform.position.x; int y = (int)postacGracza.transform.position.y;
                        Debug.Log(x + " " + y + " " + NPC[id].transform.position.x + " " + NPC[id].transform.position.y);
                        if(y < NPC[id].transform.position.y)
                            if(istnieje(x,y-1) &&  !kafelki[x][y-1].GetComponent<Pole>().Zajete && !kafelki[x][y-1].GetComponent<Pole>().ZajeteLot)
                                pole = kafelki[(int)NPC[id].transform.position.x][(int)NPC[id].transform.position.y-1];
                        else
                            if(istnieje(x,y+1) &&  !kafelki[x][y+1].GetComponent<Pole>().Zajete && !kafelki[x][y+1].GetComponent<Pole>().ZajeteLot)
                                pole = kafelki[(int)NPC[id].transform.position.x][(int)NPC[id].transform.position.y+1]; 
                        if(pole == null)
                        {
                        if(x < NPC[id].transform.position.x)
                            if(istnieje(x-1,y) &&  !kafelki[x-1][y].GetComponent<Pole>().Zajete && !kafelki[x-1][y].GetComponent<Pole>().ZajeteLot)
                                pole = kafelki[(int)NPC[id].transform.position.x-1][(int)NPC[id].transform.position.y];
                        else
                            if(istnieje(x+1,y) &&  !kafelki[x+1][y].GetComponent<Pole>().Zajete && !kafelki[x+1][y].GetComponent<Pole>().ZajeteLot)
                                pole = kafelki[(int)NPC[id].transform.position.x+1][(int)NPC[id].transform.position.y];
                        }
                        if(pole != null)
                            Debug.Log(pole.name);
                    }
                    else
                    {
                        MenuGlowne.nieCelanMulti = true;
                        int x = (int)postacGracza.transform.position.x + 1; int y = (int)postacGracza.transform.position.y;
                        if(istnieje(x,y) && !kafelki[x][y].GetComponent<Pole>().Zajete && !kafelki[x][y].GetComponent<Pole>().ZajeteLot)
                        {
                            Pole.Clean2();
                            yield return new WaitForSeconds(0.209f);
                            kafelki[x][y].GetComponent<Pole>().OnMouse(NPC[id],0);
                            //yield return new WaitForSeconds(0.09f);

                            if(close > kafelki[x][y].GetComponent<Pole>().CzasDrogi && kafelki[x][y].GetComponent<Pole>().CzasDrogi!=0) {
                                close = kafelki[x][y].GetComponent<Pole>().CzasDrogi;
                                pole = kafelki[x][y];}
                        }
                        x = (int)postacGracza.transform.position.x - 1; y = (int)postacGracza.transform.position.y;
                        if(istnieje(x,y) && !kafelki[x][y].GetComponent<Pole>().Zajete && !kafelki[x][y].GetComponent<Pole>().ZajeteLot)
                        {
                            Pole.Clean2();
                            yield return new WaitForSeconds(0.209f);
                            kafelki[x][y].GetComponent<Pole>().OnMouse(NPC[id],0);
                            //yield return new WaitForSeconds(0.09f);
                            if(close > kafelki[x][y].GetComponent<Pole>().CzasDrogi && kafelki[x][y].GetComponent<Pole>().CzasDrogi!=0) {
                                close = kafelki[x][y].GetComponent<Pole>().CzasDrogi;
                                pole = kafelki[x][y];}
                        }
                        x = (int)postacGracza.transform.position.x; y = (int)postacGracza.transform.position.y + 1;
                        if(istnieje(x,y) && !kafelki[x][y].GetComponent<Pole>().Zajete && !kafelki[x][y].GetComponent<Pole>().ZajeteLot)
                        {
                            Pole.Clean2();
                            yield return new WaitForSeconds(0.209f);
                            kafelki[x][y].GetComponent<Pole>().OnMouse(NPC[id],0);
                            //yield return new WaitForSeconds(0.09f);
                            if(close > kafelki[x][y].GetComponent<Pole>().CzasDrogi && kafelki[x][y].GetComponent<Pole>().CzasDrogi!=0) {
                                close = kafelki[x][y].GetComponent<Pole>().CzasDrogi;
                                pole = kafelki[x][y];}
                        }
                        x = (int)postacGracza.transform.position.x; y = (int)postacGracza.transform.position.y - 1;
                        if(istnieje(x,y) && !kafelki[x][y].GetComponent<Pole>().Zajete && !kafelki[x][y].GetComponent<Pole>().ZajeteLot)
                        {
                            Pole.Clean2();
                            yield return new WaitForSeconds(0.22f);
                            kafelki[x][y].GetComponent<Pole>().OnMouse(NPC[id],0);
                            //yield return new WaitForSeconds(0.2f);
                            if(close > kafelki[x][y].GetComponent<Pole>().CzasDrogi && kafelki[x][y].GetComponent<Pole>().CzasDrogi!=0) {
                                close = kafelki[x][y].GetComponent<Pole>().CzasDrogi;
                                pole = kafelki[x][y];}
                        }
                    }
                        Pole.Clean2();
                        if(pole != null) {
                            pole.GetComponent<Pole>().OnMouse(NPC[id],1,true);
                            // yield return new WaitForSeconds(0.15f);
                            // pole.GetComponent<Pole>().OnMouse(NPC[id],1);
                            if(close > 50)
                                close = 2;
                            yield return new WaitForSeconds(0.3f * close);
                        }
                        usunSelect2();
                    
                    
                } 
                else
                {
                    int x = (int)postacGracza.transform.position.x; int y = (int)postacGracza.transform.position.y;
                    pole = kafelki[x][y];
                    close = 1;
                    yield return new WaitForSeconds(0.15f);
                }

                MenuGlowne.nieCelanMulti = false;

                if(pole != null)
                {
                    postacGracza.GetComponent<Jednostka>().zaatakowanie(NPC[id]);
                }
                
                
            }
            postacGracza = null;
        if(Menu.NPC.Count - 1 > id)
        {
            turaNPC.value = id;
            turaNPC.maxValue = Menu.NPC.Count - 2;
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(NPCtura(id + 1));
            
        }
        else{
            turaNPC.gameObject.SetActive(false);
            NIERUSZAC = false;
            if(!SimultanTurns.simultanTurns)
                nrTury++;
            nrTuryText.text = nrTury.ToString();
            Jednostka.CzyJednostka = false;
            if(SimultanTurns.simultanTurns)
                SimultanTurns.playerTurn();
            else
                NextTurn();
            
        }
    }

    private GameObject przeszukanie(int odleglosc, GameObject NPC)
    {
        GameObject zwrot = null;
        GameObject help;
        for(int j = -odleglosc; j<=odleglosc;j++)
            for(int i = -odleglosc; i<=odleglosc;i++)
            {
                if(Mathf.Abs(i) + Mathf.Abs(j) < odleglosc && (int)(NPC.transform.position.x + i) >= 0 && (int)(NPC.transform.position.x + i) <= (Menu.BoardSizeX -1) 
                && (int)(NPC.transform.position.y + j) >= 0 && (int)(NPC.transform.position.y + j) <= (Menu.BoardSizeY -1))
                {
                    if(kafelki[(int)NPC.transform.position.x + i][(int)NPC.transform.position.y + j].GetComponent<Pole>().postac != null)
                    {
                        help = kafelki[(int)NPC.transform.position.x + i][(int)NPC.transform.position.y + j].GetComponent<Pole>().postac;
                        Jednostka czek = help.GetComponent<Jednostka>();
                        if(czek!=null && help.GetComponent<Jednostka>().druzyna != 0 && czek.HP>0.1 && !czek.lata)
                        {
                            zwrot = help;
                        }
                    }
                }
            }
        if(zwrot == null && odleglosc <= 4)
        {
            zwrot = przeszukanie(odleglosc + 1,NPC);
        }

        return zwrot;
    }

    public static bool preNext;

    public IEnumerator PrzedKoniec()
    {
        preNext = true;
        yield return new WaitForSeconds(0.15f);
        preNext = false;
    }

    public void cofnijInfo()
    {
            InfoKolejnaTura.SetActive(false);
            Jednostka.Select = jednostki[tura, NumerTypa];
            Jednostka.Select.GetComponent<Jednostka>().OnMouseDown();
            Interface.przeniesDoSelect();
            PanelUnit.SetActive(true);
            PanelBuild.SetActive(false);
            for(int i=0;i<16;i++)
                InterfaceUnit.przyciski[i].SetActive(false);
    }
    public void cofnij()
    {
        Pole.Clean2();
        switch(idInfo){
            case 0: cofnijInfo(); break;
            case 1: PanelBuild.GetComponent<InterfaceBuild>().cofnij(); break;
        }
    }
    public static void przyciskiClear()
    {
            for(int i=0;i<16;i++)
            {
                Przycisk.jednostka[i]=false;
                Przycisk.budynek[i]=false;
            }
    }
    public void dalej()
    {
        Pole.Clean2();
        switch(idInfo){
            case 0: dalejInfo(); break;
            case 1: PanelBuild.GetComponent<InterfaceBuild>().dalej(); break;
        }
    }
    public void dalejInfo()
    {
            Pole.Clean2();
            NaPewnoKoniec = 1;
            InfoKolejnaTura.SetActive(false);
            NextTurn();
    }

    // private void PushBoard()
    // {
    //     for (int x = 0; x < BoardSizeX; x++)
    //         for (int y = 0; y < BoardSizeY; y++)
    //         {
    //             Vector3 TilePosition = new Vector3(x, y, 3);
    //             GameObject newUnit = Instantiate(kafelek, TilePosition, Quaternion.identity);
    //             newUnit.transform.SetParent(transform);
    //             newUnit.name = x.ToString() + " " + y.ToString();
    //             kafelki[x][y] = kafelek;
    //         }
    // }
    [PunRPC]
    void przelocznikMulti()
    {
        StartCoroutine(Przelocznik());
    }

    IEnumerator Przelocznik()
    {
        Next = true;
        yield return new WaitForSeconds(0.15f);
        nrTury++;
        Next = false;
    }
}
