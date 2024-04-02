using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opcje : MonoBehaviour
{
    public void SetVolume(float volume)
    {
        Debug.Log(volume);
    }


    public void Quit()
    {
        Application.Quit();
    }
}
