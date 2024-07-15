using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class WyburRas : MonoBehaviour
{
    static public bool[] aktywny = new bool[4];
    static public int[] rasa = new int[4];
    static public int[] heros = new int[4];
    static public int[] team = new int[4];


    public Sprite[] rasaArt = new Sprite[5];
    public Sprite[] herosArt = new Sprite[15];
    public Sprite[] teamArt = new Sprite[4];

    public int id;
    public int wybierany = 1;

    public Image loockArt;
    public Image unlockArt;

    public Image main;
    public Image lewy;
    public Image prawy;

    public Image strzalka1;
    public Image strzalka2;

    public GameObject updaterZiom;

    private PhotonView photonView;

    public void Start()
    {
        photonView = updaterZiom.GetComponent<PhotonView>();
        team[id] = id;
        lewy.sprite = teamArt[id];
        if (id >= 2 && !MenuGlowne.multi)
        {
            loockArt.enabled = true;
            aktywny[id] = false;
            unlockArt.enabled = false;
            main.enabled = false;
            lewy.enabled = false;
            prawy.enabled = false;
            strzalka1.enabled = false;
            strzalka2.enabled = false;
        }
        else
        {
            aktywny[id] = true;
            unlockArt.enabled = false;
            loockArt.enabled = false;
        }
        if(Ip.ip != 1 && MenuGlowne.multi)// && id == 0)
        {
            photonView.RPC("UpdateStart", RpcTarget.All);
        }
    }

    [PunRPC]
    public void UpdateStart()
    {
        if(Ip.ip == 1)
            photonView.RPC("SendStart", RpcTarget.All,  MapCheck.opis, MapLoad.nazwa, End.pvp, End.control, End.economy, End.boss, SimultanTurns.simultanTurns, PoleOdkryj.mgla,
            rasa, heros, team, Menu.BoardSizeX, Menu.BoardSizeY);
    }

    
    [PunRPC]
    public void SendStart(string opis, string nazwa, bool pvp, bool control, bool economy, bool boss, bool symulatn, bool mgla, int[] a, int[] b, int[] c, int BoardSizeX, int BoardSizeY)
    {
         MapCheck.opis = opis;
         MapLoad.nazwa = nazwa;
         End.pvp = pvp;
         End.control = control;
         End.economy = economy;
         End.boss = boss;
         SimultanTurns.simultanTurns = symulatn;
         PoleOdkryj.mgla = mgla;
        rasa = a;
        heros = b;
        team = c;
        main.sprite = rasaArt[rasa[id]];
        prawy.sprite = herosArt[heros[id]+rasa[id]*3];
        lewy.sprite = teamArt[team[id]];
        Menu.BoardSizeX = BoardSizeX;
        Menu.BoardSizeY = BoardSizeY;
    // main.sprite[] rasaArt = new Sprite[2];
    // herosArt = new Sprite[4];
    // teamArt

    }

    [PunRPC]
    public void UpdateSprite(Sprite sprite)
    {
        this.main.sprite = sprite;
    }

    [PunRPC]
    public void SynchronizeChoice(int selected, int value)
    {
        switch (selected)
        {
            case 1:
                rasa[id] = value;
                main.sprite = rasaArt[rasa[id]];
                break;
            case 3:
                heros[id] = value;
                prawy.sprite = herosArt[heros[id]+rasa[id]*3];
                break;
            case 2:
                team[id] = value;
                lewy.sprite = teamArt[team[id]];
                break;
        }
        prawy.sprite = herosArt[heros[id]+rasa[id]*3];
    }


    [PunRPC]
    public void SynchronizeMovement(Image A, Image B, Image C, int IP)
    {
        if(Ip.ip != IP)
            StartCoroutine(PrzeniesObiekty(A, B, C));
    }
    [PunRPC]
    public void SynchronizeTransform(Vector3 nowePołożenieA,Vector3 nowePołożenieB,Vector3 nowePołożenieC)
    {
        lewy.transform.position = nowePołożenieA;
        prawy.transform.position = nowePołożenieB;
        main.transform.position = nowePołożenieC;
    }
    

    public void prawo()
    {
        if(id+1 == Ip.ip || !MenuGlowne.multi)
        {
            switch (wybierany)
            {
                case 1:
                    rasa[id]++;
                    if (rasa[id] == 5)
                        rasa[id] = 0;
                    main.sprite = rasaArt[rasa[id]];
                    if(MenuGlowne.multi)
                        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, rasa[id]);
                    break;
                case 3:
                    heros[id]++;
                    if (heros[id] == 3)
                        heros[id] = 0;
                   // prawy.sprite = herosArt[heros[id]+rasa[id]*2];
                   
                    if(MenuGlowne.multi)
                        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, heros[id]);
                    break;
                case 2:
                    team[id]++;
                    if (team[id] == 4)
                        team[id] = 0;
                    lewy.sprite = teamArt[team[id]];
                    if(MenuGlowne.multi)
                        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, team[id]);
                    break;
            }
            prawy.sprite = herosArt[heros[id]+rasa[id]*3];
        }
    }
    public void lewo()
    {
        if(id+1 == Ip.ip || !MenuGlowne.multi)
        {
        switch (wybierany)
        {
            case 1:
                rasa[id]--;
                if (rasa[id] == -1)
                    rasa[id] = 4;
                main.sprite = rasaArt[rasa[id]];
                if(MenuGlowne.multi)
                        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, rasa[id]);
                break;
            case 3:
                heros[id]--;
                if (heros[id] == -1)
                    heros[id] = 2;
                if(MenuGlowne.multi)
                        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, heros[id]);
                break;
            case 2:
                team[id]--;
                if (team[id] == -1)
                    team[id] = 3;
                lewy.sprite = teamArt[team[id]];
                if(MenuGlowne.multi)
                        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, team[id]);
                break;
        }
            prawy.sprite = herosArt[heros[id]+rasa[id]*3];
        }

    }
    public void lewyGora()
    {
        if(id+1 == Ip.ip || !MenuGlowne.multi)
        switch (wybierany)
        {
            case 1:
                StartCoroutine(PrzeniesObiekty(prawy, main, lewy));
                wybierany = 2;
                break;
            case 3:
                StartCoroutine(PrzeniesObiekty(prawy, lewy, main));
                wybierany = 2;
                break;
        }
        if(MenuGlowne.multi)
            photonView.RPC("SynchronizeMovement", RpcTarget.All, prawy, main, lewy, Ip.ip);

    }
    public void prawyGora()
    {
        if(id+1 == Ip.ip || !MenuGlowne.multi)
        switch (wybierany)
        {
            case 2:
                StartCoroutine(PrzeniesObiekty(prawy, main, lewy));
                wybierany = 3;
                break;
            case 1:
                StartCoroutine(PrzeniesObiekty(prawy, lewy, main));
                wybierany = 3;
                break;
        }
        if(MenuGlowne.multi)
            photonView.RPC("SynchronizeMovement", RpcTarget.All, prawy, main, lewy, Ip.ip);
    }
    public void mainGora()
    {
        if(id+1 == Ip.ip || !MenuGlowne.multi)
        switch (wybierany)
        {
            case 2:
                StartCoroutine(PrzeniesObiekty(prawy, lewy, main));
                wybierany = 1;
                break;
            case 3:
                StartCoroutine(PrzeniesObiekty(prawy, main, lewy));
                wybierany = 1;
                break;
        }

        if(MenuGlowne.multi)
            photonView.RPC("SynchronizeMovement", RpcTarget.All, prawy, lewy, main, Ip.ip);
    }
    IEnumerator PrzeniesObiekty(Image A, Image B, Image C)
    {
        // Zdefiniuj nowe pozycje
        Vector3 nowaPozycjaMain = C.transform.position;
        Vector3 nowaPozycjaLewy = A.transform.position;
        Vector3 nowaPozycjaPrawy = B.transform.position;

        // Ustaw obiekty na pozycjach początkowych
        A.rectTransform.position = A.transform.position;
        B.rectTransform.position = B.transform.position;
        C.rectTransform.position = C.transform.position;

        // Przesuwaj stopniowo obiekty w ciągu jednej sekundy
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            A.rectTransform.position = Vector3.Lerp(A.rectTransform.position, nowaPozycjaMain, elapsedTime);
            B.rectTransform.position = Vector3.Lerp(B.rectTransform.position, nowaPozycjaLewy, elapsedTime);
            C.rectTransform.position = Vector3.Lerp(C.rectTransform.position, nowaPozycjaPrawy, elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ustaw obiekty na dokładnych nowych pozycjach
        A.rectTransform.position = nowaPozycjaMain;
        B.rectTransform.position = nowaPozycjaLewy;
        C.rectTransform.position = nowaPozycjaPrawy;
       // photonView.RPC("SynchronizeTransform", RpcTarget.All, lewy.transform.position, prawy.transform.position, main.transform.position);
    }
    public void loock()
    {
        aktywny[id] = true;
        loockArt.enabled = false;
        unlockArt.enabled = true;
        main.enabled = true;
        lewy.enabled = true;
        prawy.enabled = true;
        strzalka1.enabled = true;
        strzalka2.enabled = true;
    }
    public void unloock()
    {
        aktywny[id] = false;
        loockArt.enabled = true;
        unlockArt.enabled = false;
        main.enabled = false;
        lewy.enabled = false;
        prawy.enabled = false;
        strzalka1.enabled = false;
        strzalka2.enabled = false;
    }
    public void zacznij()
    {
        
        if (!MenuGlowne.multi)
        {
            Menu.IloscGraczy = 2;
            for (int i = 2; i < 4; i++)
            {
                if (aktywny[i] == true)
                {
                    Menu.IloscGraczy++;
                }
            }

            for(int i = 0; i < 4; i++)
            {
                if(rasa[i] == 4)
                    rasa[i] = UnityEngine.Random.Range(0, 4);
                if(heros[i] == 2)
                    heros[i] = UnityEngine.Random.Range(0, 2);
            }
            SceneManager.LoadScene(2);
        }
        else
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount >= 2 && Ip.ip==1) //zmienic na 2
            {
                for(int i = 0; i < 4; i++)
            {
                if(rasa[i] == 4)
                {
                    rasa[i] = UnityEngine.Random.Range(0, 4);
                    heros[i] = UnityEngine.Random.Range(0, 2);
                }
                if(heros[i] == 2)
                    heros[i] = UnityEngine.Random.Range(0, 2);
            }
                Menu.IloscGraczy = PhotonNetwork.CurrentRoom.PlayerCount;
                for(int i = 0 ; i<4;i++)
                    aktywny[i] = false;
                for(int i = 0 ; i<Menu.IloscGraczy;i++)
                    aktywny[i] = true;
                // Spróbuj pobrać komponent PhotonView
                PhotonView photonView = GetComponent<PhotonView>();

                // Jeśli nie istnieje, dodaj go dynamicznie
                if (photonView == null)
                {
                    photonView = gameObject.AddComponent<PhotonView>();
                }
                if(MenuGlowne.multi)
                    photonView.RPC("LoadSceneRPC", RpcTarget.All, End.tureKontroli, End.poziomRatusza, End.tureDoKonca);
            }
        }
    }

    [PunRPC]
    void LoadSceneRPC(int tureKontroli, int poziomRatusza, int tureDoKonca)
    {
        End.tureKontroli = tureKontroli;
        End.poziomRatusza = poziomRatusza;
        End.tureDoKonca = tureDoKonca;
        SceneManager.LoadScene(2);
    }


}
