using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Photon.Pun;
using System;


public class Opcje : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public GameObject opcje;
    public GameObject tworcy;

    public bool pierwszyRaz;

    public Slider glos;
    public Slider glosSFX;


    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0;i<resolutions.Length;i++)
        {
            string option  = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);

        float volume;
        audioMixer.GetFloat("music", out volume);
        glos.value = (float)-Math.Log(-volume, 2);

        audioMixer.GetFloat("SFX", out volume);
        glosSFX.value = (float)-Math.Log(-volume, 2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            opcjeOn();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                // Update dropdown value
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
        public void CofnijMenu(int resolutionIndex)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene(0);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music", (float)(-Math.Pow(2,-volume)+1));
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", (float)(-Math.Pow(2,-volume)+1));;
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void opcjeOn()
    {
        if(opcje.activeSelf)
            opcje.SetActive(false);
        else
            opcje.SetActive(true);
    }

    public void tworcyOn()
    {
        if(tworcy.activeSelf)
            tworcy.SetActive(false);
        else
            tworcy.SetActive(true);
    }

    public string youtubeURL = "https://youtu.be/VfR1Mfr6JBM"; 

    public void OpenLink()
    {
            System.Diagnostics.Process.Start(youtubeURL);
    }
}

