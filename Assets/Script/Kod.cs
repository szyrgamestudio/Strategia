using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Kod : MonoBehaviour
{
    private bool kodShow = true;
    public InputField inputField;
    public static bool sancher;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote)) // Tylda znajduje się na klawiszu BackQuote
        {
            Debug.Log(Menu.IloscGraczy + " " + SimultanTurns.readyCount);
            if(kodShow)
            {
                RectTransform rectTransform = inputField.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = new Vector2(2000, 0);
                }
                
            }
            else
            {
                RectTransform rectTransform = inputField.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = new Vector2(0, 75f);
                }
            }
            kodShow =!kodShow;
        }
    }

    public void activeKod()
    {
        switch(inputField.text)
        {
            case "kasa":
                kasaKod();
                Debug.Log("essa");
                break;
            case "skip":
                SimultanTurns.kod();
                Debug.Log("essa");
                break;
            case "sancher":
                if(MenuGlowne.multi)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("sancherr", RpcTarget.All, true);
                }
                else
                    sancher = true;
                Debug.Log("kod");
                break;
            case "skos":
                if(MenuGlowne.multi)
                {
                    PhotonView photonView = GetComponent<PhotonView>();
                    photonView.RPC("sancherr", RpcTarget.All, false);
                }
                else
                    sancher = false;
                Debug.Log("kod");
                break;
            case "kick1":
                kick(1);
            break;
            case "kick2":
                kick(2);
            break;
            case "kick3":
                kick(3);
            break;
            case "kick4":
                kick(4);
            break;
            default:
                break;
        }
    }
    [PunRPC]
    public void sancherr(bool skos)
    {
        sancher = skos;
    }

    private void kick(int nr)
    {
        
        if(MenuGlowne.multi)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("updateMulti", RpcTarget.All, nr);
        }
        else
        {
            WyburRas.aktywny[nr-1] = false;
            Menu.IloscGraczy--;
        }
    }

    [PunRPC]
    public void updateMulti(int nr)
    {
        WyburRas.aktywny[nr-1] = false;
        Menu.IloscGraczy--;
    }

    
    private void kasaKod()
    {
        for(int i = 1 ; i <=4 ; i++ )
        {
            Menu.zloto[i] = 140;
            Menu.drewno[i] = 140;
            Menu.maxludnosc[i] = 140;
            Menu.magia[i] = 140;
        }
        
    }
}
