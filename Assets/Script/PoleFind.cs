using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PoleFind : MonoBehaviour
{
    public GameObject poleObj;
    public SpriteRenderer image;
    public Sprite[] sprite; //= new Sprite[8];
    public Pole pole;

    public int rodzaj;

    public void Start()
    {
        if(rodzaj != 0)
        {
            image.enabled = true;
            image.sprite = sprite[rodzaj-1];
        }
        else
            image.enabled = false;
    }

    [PunRPC]
    public void updateMulti(int rodzaj)
    {
        Debug.Log("essa");
        this.rodzaj = rodzaj;
        Start();
    }
    public void updateMultiWywolaj(int rodzaj)
    {
        if(MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("updateMulti", RpcTarget.All, rodzaj);
        }
    }
    void Update()
    {
        if(rodzaj != 0)
        {
            if(pole.postac != null)
            {
                Jednostka jednostka = pole.postac.GetComponent<Jednostka>();
                if(jednostka != null && !jednostka.lata && jednostka.druzyna != 0)
                {
                    switch(rodzaj)
                    {
                        case 1: Menu.zloto[jednostka.druzyna] += 3; jednostka.ShowDMG(3, new Color(1.0f, 1.0f, 0.0f, 1.0f)); break;
                        case 2: Menu.zloto[jednostka.druzyna] += 5; jednostka.ShowDMG(5, new Color(1.0f, 1.0f, 0.0f, 1.0f));break;
                        case 3: Menu.zloto[jednostka.druzyna ] += 8; jednostka.ShowDMG(8, new Color(1.0f, 1.0f, 0.0f, 1.0f)); break; 
                        case 4: Menu.drewno[jednostka.druzyna] += 3; jednostka.ShowDMG(3, new Color(0.6f, 0.4f, 0.2f, 1.0f)); break;
                        case 5: Menu.drewno[jednostka.druzyna] += 5; jednostka.ShowDMG(5, new Color(0.6f, 0.4f, 0.2f, 1.0f)); break;
                        case 6: Menu.drewno[jednostka.druzyna] += 8; jednostka.ShowDMG(8, new Color(0.6f, 0.4f, 0.2f, 1.0f)); break;
                        case 7: Menu.magia[jednostka.druzyna] += 3; jednostka.ShowDMG(3, new Color(1.0f, 0.0f, 1.0f, 1.0f)); break;
                        case 8: Menu.magia[jednostka.druzyna] += 8; jednostka.ShowDMG(8, new Color(1.0f, 0.0f, 1.0f, 1.0f)); break;
                    }
                    rodzaj = 0;
                    Start();
                    if(MenuGlowne.multi)
                    {
                        PhotonView photonView = GetComponent<PhotonView>();
                        photonView.RPC("updateMulti", RpcTarget.All, rodzaj);
                    }
                }
            }
        }
    }
}
