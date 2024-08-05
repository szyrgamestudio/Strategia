using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public void CreateRoom()
    {
        if(createInput.text != "")
        {
            Ip.ip = 1;
            PhotonNetwork.CreateRoom(createInput.text);
            SimultanTurns.simultanTurns = true;
        }
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
