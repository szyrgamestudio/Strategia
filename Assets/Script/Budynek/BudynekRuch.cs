using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BudynekRuch : MonoBehaviour
{
    public float targetZ = -1.0f; // Pożądana pozycja 'z'
    public bool wybudowany;
    public float targetX; // Przesuń te deklaracje zmiennych na poziom klasy, nie w funkcji Update
    public float targetY;
    public GameObject ObiektRuszany;
    public GameObject canvasRuch;
    public GameObject pole;

    private bool update;

    public static GameObject budowlaniec;

    public static bool pomoc = false;
    public bool prekoniec = false;

    //private bool polepomoc = true;

    void Start()
    {

    }
    public void startMultiMap()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("ZaktualizujWybudowany", RpcTarget.All);
    }
    public void startMulti()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("ZaktualizujWybudowany", RpcTarget.All);
        wybudowany = false;
    }
    public void dedMulti()
    {
        if(MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ded", RpcTarget.All);
        }
    }


    void Update()
    {
        if (wybudowany == false && (!MenuGlowne.multi || ObiektRuszany.GetComponent<Budynek>().druzyna == Ip.ip)) // Użyj '==' do porównywania, a nie '='
        {
            Debug.Log(ObiektRuszany.GetComponent<Budynek>().druzyna == Ip.ip);
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            targetX = Mathf.Round(mousePosition.x);
            targetY = Mathf.Round(mousePosition.y);
            transform.position = new Vector3(targetX, targetY, targetZ);
            if (Input.GetMouseButtonDown(0))
            {
                if (targetX < Menu.BoardSizeX - 1 && targetY < Menu.BoardSizeY - 1 && targetX > 0 && targetY > 0 && Walka.odleglosc(budowlaniec, ObiektRuszany) == 1)
                {
                    if (!Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().Zajete && !Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().ZajeteLot)
                    {
                        wybudowany = true;
                        update = true; // chyba
                        // Budowa.wybieranie = false;
                        pomoc = true;
                        Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().Zajete = true;
                        Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().postac = ObiektRuszany;
                        Pole.Clean2();
                        Menu.zloto[Menu.tura] -= ObiektRuszany.GetComponent<Budynek>().zloto;
                        Menu.drewno[Menu.tura] -= ObiektRuszany.GetComponent<Budynek>().drewno;
                    }
                }
            }
            if (targetX < Menu.BoardSizeX - 1 && targetY < Menu.BoardSizeY - 1 && targetX > 0 && targetY > 0 && Walka.odleglosc(budowlaniec, ObiektRuszany) == 1)
            {
                ObiektRuszany.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
            }
            else
            {
                ObiektRuszany.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
            }

            Pole.Clean2();
            if(Menu.preNext)
            {
                prekoniec = true;
            }
            if(prekoniec && !Menu.preNext)
            {
                if(!wybudowany)
                {
                    Apteka apteka2 = ObiektRuszany.GetComponent<Apteka>();
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("ded", RpcTarget.All);
                    }
                    if(apteka2 != null)
                        Apteka.apteka[budowlaniec.GetComponent<Jednostka>().druzyna] = false;
                    Destroy(ObiektRuszany);
                }
            }
        }
        else{
            if(update && MenuGlowne.multi)
            {
             
                update = false;
                Vector3 nowePołożenie = transform.position;
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("ZaktualizujPołożenieRPC", RpcTarget.All, nowePołożenie);
                photonView.RPC("ZaktualizujWybudowany", RpcTarget.All);
            }
        }
        if(ObiektRuszany == Jednostka.Select || wybudowany == false)
        {
            if (Input.GetKeyDown(KeyCode.Q)) // Sprawdza, czy klawisz "Q" został naciśnięty
            {
                if (ObiektRuszany.GetComponent<Budynek>().strzalka.transform.rotation.eulerAngles.z == 360.0f)
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 0.0f);
                else
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 90.0f);
            }
            if (Input.GetKeyDown(KeyCode.E)) // Sprawdza, czy klawisz "Q" został naciśnięty
            {
                if (ObiektRuszany.GetComponent<Budynek>().strzalka.transform.rotation.eulerAngles.z == 0)
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 270.0f);
                else
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, -90.0f);
            }
        }
        if (wybudowany && ObiektRuszany.transform.position.x != -10f && (!MenuGlowne.multi || ObiektRuszany.GetComponent<Budynek>().druzyna == Ip.ip))
        {
            switch (ObiektRuszany.GetComponent<Budynek>().strzalka.transform.rotation.eulerAngles.z)
            {
                case 90.0f: pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x + 1][(int)ObiektRuszany.transform.position.y]; break;
                case 270.0f: pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x - 1][(int)ObiektRuszany.transform.position.y]; break;
                case 0.0f: pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y - 1]; break;
                case 180.0f: pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y + 1]; break;
            }
            // polepomoc = false;
        }

    }
    
    [PunRPC]
    void ZaktualizujPołożenieRPC(Vector3 nowePołożenie)
    {
        transform.position = nowePołożenie;
    }

    [PunRPC]
    void ded()
    {
        Debug.Log("o chuj");
        Destroy(ObiektRuszany);
    }

    [PunRPC]
    void ZaktualizujWybudowany()
    {
        this.wybudowany = true;
        Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y].GetComponent<Pole>().Zajete = true;
        Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y].GetComponent<Pole>().postac = ObiektRuszany;
    }
}
