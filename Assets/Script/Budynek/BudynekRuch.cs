using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BudynekRuch : MonoBehaviour
{
   // public float targetZ = -1.0f; // Pożądana pozycja 'z'
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

    private bool polepomoc = false;

    void Start()
    {
        polepomoc = true;
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
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            targetX = Mathf.Round(mousePosition.x);
            targetY = Mathf.Round(mousePosition.y);
            transform.position = new Vector3(targetX, targetY, -3f);
            if (Input.GetMouseButtonDown(0))
            {
                if (Walka.odleglosc(budowlaniec, ObiektRuszany) <= 3 && Menu.istnieje((int)targetX,(int)targetY))//targetX < Menu.BoardSizeX - 1 && targetY < Menu.BoardSizeY - 1 && targetX > 0 && targetY > 0 && Walka.odleglosc(budowlaniec, ObiektRuszany) == 1)
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
                        ObiektRuszany.transform.position = new Vector3(targetX, targetY, -2f);
                    }
                }
            }
            if (Walka.odleglosc(budowlaniec, ObiektRuszany) <= 3 && Menu.istnieje((int)targetX,(int)targetY) && (!Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().Zajete && !Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().ZajeteLot))//targetX < Menu.BoardSizeX - 1 && targetY < Menu.BoardSizeY - 1 && targetX > 0 && targetY > 0 && Walka.odleglosc(budowlaniec, ObiektRuszany) == 1)
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
                polepomoc = true;
                if (ObiektRuszany.GetComponent<Budynek>().strzalka.transform.rotation.eulerAngles.z == 360.0f)
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 0.0f);
                else
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 90.0f);
            }
            if (Input.GetKeyDown(KeyCode.E)) // Sprawdza, czy klawisz "Q" został naciśnięty
            {
                polepomoc = true;
                if (ObiektRuszany.GetComponent<Budynek>().strzalka.transform.rotation.eulerAngles.z == 0)
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, 270.0f);
                else
                    ObiektRuszany.GetComponent<Budynek>().strzalka.transform.Rotate(0.0f, 0.0f, -90.0f);
            }
        }
        
        if (wybudowany && ObiektRuszany.transform.position.x != -10f && (!MenuGlowne.multi || ObiektRuszany.GetComponent<Budynek>().druzyna == Ip.ip) && polepomoc)
        {
            switch (Mathf.Round(ObiektRuszany.GetComponent<Budynek>().strzalka.transform.rotation.eulerAngles.z))
            {
                case 90.0f: if(Menu.istnieje((int)ObiektRuszany.transform.position.x+1,(int)ObiektRuszany.transform.position.y))pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x + 1][(int)ObiektRuszany.transform.position.y];  break;
                case 270.0f: if(Menu.istnieje((int)ObiektRuszany.transform.position.x-1,(int)ObiektRuszany.transform.position.y))pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x - 1][(int)ObiektRuszany.transform.position.y];  break;
                case 0.0f: if(Menu.istnieje((int)ObiektRuszany.transform.position.x,(int)ObiektRuszany.transform.position.y - 1))pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y - 1]; break;
                case 180.0f: if(Menu.istnieje((int)ObiektRuszany.transform.position.x,(int)ObiektRuszany.transform.position.y + 1)) pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y + 1];  break;
                
            }
            if(MenuGlowne.multi)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                photonView.RPC("ZaktualizujPole", RpcTarget.All, Ip.ip, ObiektRuszany.GetComponent<Budynek>().strzalka.transform.rotation.eulerAngles.z);
            }
            polepomoc = false;
        }

    }

    [PunRPC]
    void ZaktualizujPole(int ip, float stopnie)
    {
        if(Ip.ip != ip)
        {
            try{
            switch (Mathf.Round(stopnie))
            {
                case 90.0f: if(Menu.istnieje((int)ObiektRuszany.transform.position.x + 1,(int)ObiektRuszany.transform.position.y))pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x + 1][(int)ObiektRuszany.transform.position.y];  break;
                case 270.0f:if(Menu.istnieje((int)ObiektRuszany.transform.position.x - 1,(int)ObiektRuszany.transform.position.y )) pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x - 1][(int)ObiektRuszany.transform.position.y];  break;
                case 0.0f: if(Menu.istnieje((int)ObiektRuszany.transform.position.x,(int)ObiektRuszany.transform.position.y - 1))pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y - 1];  break;
                case 180.0f: if(Menu.istnieje((int)ObiektRuszany.transform.position.x,(int)ObiektRuszany.transform.position.y + 1))pole = Menu.kafelki[(int)ObiektRuszany.transform.position.x][(int)ObiektRuszany.transform.position.y + 1];  break;
            }
            }catch(Exception ex){Debug.Log(ex.ToString());}
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
