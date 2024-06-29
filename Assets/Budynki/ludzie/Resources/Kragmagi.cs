using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Kragmagi : MonoBehaviour
{
    public float targetZ = -1.0f; // Pożądana pozycja 'z'
    public bool wybudowany;
    public float targetX; // Przesuń te deklaracje zmiennych na poziom klasy, nie w funkcji Update
    public float targetY;
    public GameObject ObiektRuszany;
    public Sprite zolte;
    public int zloto;
    public int drewno;

    public static GameObject budowlaniec;

    public static bool pomoc = false;

    void LateUpdate()
    {
        if (wybudowany == false  && (!MenuGlowne.multi || (Menu.tura == Ip.ip && (!SimultanTurns.simultanTurns || budowlaniec.GetComponent<Jednostka>().druzyna == Ip.ip)))) // Użyj '==' do porównywania, a nie '='
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            targetX = Mathf.Round(mousePosition.x);
            targetY = Mathf.Round(mousePosition.y);
            transform.position = new Vector3(targetX, targetY, targetZ);
            if (Input.GetMouseButtonDown(0))
                {
                if (Walka.odleglosc(budowlaniec,ObiektRuszany) == 1
                && Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().magia == 1) 
                    {
                        if (!Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().Zajete)
                        {
                            Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().magia = 2;
                            Menu.kafelki[(int)targetX][(int)targetY].GetComponent<SpriteRenderer>().sprite = zolte;
                            if(MenuGlowne.multi)
                            {
                                PhotonView photonView = GetComponent<PhotonView>();
                                photonView.RPC("multi", RpcTarget.All, Ip.ip, (int)targetX, (int)targetY);
                            }
                            Pole.Clean2();
                            Menu.zloto[Menu.tura]-=4;
                            Menu.drewno[Menu.tura]-=4;
                            Budowlaniec.wybieranie = false;
                            if(MenuGlowne.multi)
                            {
                                PhotonView photonView = GetComponent<PhotonView>();
                                photonView.RPC("ded", RpcTarget.All);
                            }
                            Destroy(ObiektRuszany);
                        }

                    }
                }
            if (Walka.odleglosc(budowlaniec,ObiektRuszany) == 1
            && Menu.kafelki[(int)targetX][(int)targetY].GetComponent<Pole>().magia == 1) 
            {
                ObiektRuszany.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
            }
            else
            {
                ObiektRuszany.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
            }
            Pole.Clean2();
        }
    }
        public void dedMulti()
    {
        if(MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("ded", RpcTarget.All);
        }
    }
    [PunRPC]
    void ded()
    {
        Destroy(ObiektRuszany);
    }
    [PunRPC]
    public void multi(int ip, int x, int y)
    {
        if(Ip.ip != ip)
        {
            Menu.kafelki[x][y].GetComponent<Pole>().magia = 2;
            Menu.kafelki[x][y].GetComponent<SpriteRenderer>().sprite = zolte;
            Destroy(ObiektRuszany);
        }
    }
}
