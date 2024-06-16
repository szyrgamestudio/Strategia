using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MultiOptions : MonoBehaviour
{
    public Toggle mgla;
    public Toggle tury;

    private bool isUpdating = false;
    public GameObject Settings;

    void Start()
    {
        mgla.onValueChanged.AddListener(delegate { OnMglaValueChanged(); });
        tury.onValueChanged.AddListener(delegate { OnTuryValueChanged(); });
        if(MenuGlowne.multi == false)
        {
            Settings.SetActive(false);
        }
    }

    void Update()
    {
        if (!isUpdating)
        {
            isUpdating = true;
            mgla.isOn = PoleOdkryj.mgla;
            tury.isOn = SimultanTurns.simultanTurns;
            isUpdating = false;
        }
    }

    public void OnMglaValueChanged()
    {
        if (!isUpdating)
        {
            if (Ip.ip == 1)
            {
                PoleOdkryj.mgla = mgla.isOn;
            }
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("updatet", RpcTarget.All, PoleOdkryj.mgla, SimultanTurns.simultanTurns);
        }
    }

    public void OnTuryValueChanged()
    {
        if (!isUpdating)
        {
            if (Ip.ip == 1)
            {
                SimultanTurns.simultanTurns = tury.isOn;
            }
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("updatet", RpcTarget.All, PoleOdkryj.mgla, SimultanTurns.simultanTurns);
        }
    }

    [PunRPC]
    public void updatet(bool jeden, bool dwa)
    {
        isUpdating = true;
        PoleOdkryj.mgla = jeden;
        SimultanTurns.simultanTurns = dwa;
        mgla.isOn = jeden;
        tury.isOn = dwa;
        isUpdating = false;
    }
}
