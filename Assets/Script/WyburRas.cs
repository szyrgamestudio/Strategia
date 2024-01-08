using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class WyburRas : MonoBehaviour
{
    static public bool[] aktywny = new bool[4];
    static public int[] rasa = new int[4];
    static public int[] heros = new int[4];
    static public int[] team = new int[4];

    public Sprite[] rasaArt = new Sprite[2];
    public Sprite[] herosArt = new Sprite[2];
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
        if (id >= 2)
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
    }

    [PunRPC]
    public void UpdateSprite(Sprite sprite)
    {
        this.main.sprite = sprite;
    }

    [PunRPC]
    public void SynchronizeChoice(int selected, int value)
    {
        Debug.Log($"SynchronizeChoice RPC called with selected: {selected}, value: {value}");
        switch (selected)
        {
            case 1:
                rasa[id] = value;
                main.sprite = rasaArt[rasa[id]];
                break;
            case 3:
                heros[id] = value;
                prawy.sprite = herosArt[heros[id]];
                break;
            case 2:
                team[id] = value;
                lewy.sprite = teamArt[team[id]];
                break;
        }
    }


    [PunRPC]
    public void SynchronizeMovement(Image A, Image B, Image C)
    {
        StartCoroutine(PrzeniesObiekty(A, B, C));
    }

    public void prawo()
    {
        switch (wybierany)
        {
            case 1:
                rasa[id]++;
                if (rasa[id] == 2)
                    rasa[id] = 0;
                main.sprite = rasaArt[rasa[id]];
                photonView.RPC("UpdateSprite", RpcTarget.All, rasaArt[rasa[id]]);
                break;
            case 3:
                heros[id]++;
                if (heros[id] == 2)
                    heros[id] = 0;
                prawy.sprite = herosArt[heros[id]];
                break;
            case 2:
                team[id]++;
                if (team[id] == 4)
                    team[id] = 0;
                lewy.sprite = teamArt[team[id]];
                break;

        }
        Debug.Log(rasa[id]);
        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, team[id]);
    }
    public void lewo()
    {
        switch (wybierany)
        {
            case 1:
                rasa[id]--;
                if (rasa[id] == -1)
                    rasa[id] = 1;
                main.sprite = rasaArt[rasa[id]];
                photonView.RPC("UpdateSprite", RpcTarget.All, rasaArt[rasa[id]]);
                break;
            case 3:
                heros[id]--;
                if (heros[id] == -1)
                    heros[id] = 1;
                prawy.sprite = herosArt[heros[id]];
                break;
            case 2:
                team[id]--;
                if (team[id] == -1)
                    team[id] = 3;
                lewy.sprite = teamArt[team[id]];
                break;
        }

        photonView.RPC("SynchronizeChoice", RpcTarget.All, wybierany, rasa[id]);
    }
    public void lewyGora()
    {
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
        photonView.RPC("SynchronizeMovement", RpcTarget.All, prawy, main, lewy);

    }
    public void prawyGora()
    {
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
        photonView.RPC("SynchronizeMovement", RpcTarget.All, prawy, main, lewy);
    }
    public void mainGora()
    {
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

        photonView.RPC("SynchronizeMovement", RpcTarget.All, prawy, lewy, main);
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
            SceneManager.LoadScene(2);
        }
        else
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount >= 1) //zmienic na 2
            {
                Menu.IloscGraczy = PhotonNetwork.CurrentRoom.PlayerCount;

                // Spróbuj pobrać komponent PhotonView
                PhotonView photonView = GetComponent<PhotonView>();

                // Jeśli nie istnieje, dodaj go dynamicznie
                if (photonView == null)
                {
                    photonView = gameObject.AddComponent<PhotonView>();
                }
                photonView.RPC("LoadSceneRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void LoadSceneRPC()
    {
        SceneManager.LoadScene(2);
    }


}
