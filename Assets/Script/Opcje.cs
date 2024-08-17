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

    public static String first = "null";
    public static int nr;


    void Start()
    {
        if(first == "null")
        {
            first = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
        }
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        if(first != "null")
        {
            string[] parts = first.Split(' '); // Używamy Split zamiast split

            if (parts.Length >= 3) // Sprawdzamy, czy tablica ma wystarczającą liczbę elementów
            {
                string width = parts[0];
                string height = parts[2];

                // Zmiana formatu rozdzielczości i dodanie do listy opcji
                first = width + "x" + height;
                options.Add(first);
            }
            else
            {
                options.Add(first);
            }
        }
        for(int i = 0;i<resolutions.Length;i++)
        {
            string option  = resolutions[i].width + "x" + resolutions[i].height;
            if(i != nr)
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
            int width = Screen.width;
            int height = Screen.height;
            first = width + "x" + height;
            opcjeOn();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        if(resolutionIndex == 0)
            resolutionIndex = nr;
        else
            nr = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                // Update dropdown value
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        first = resolution.ToString();
        
        Debug.Log(first);
    }
        public void CofnijMenu(int resolutionIndex)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene(0);
        for(int i = 0; i < 4; i++)
        {
            WyburRas.aktywny[i] = false;
            WyburRas.rasa[i] = 0;
            WyburRas.heros[i] = 0;
            WyburRas.team[i] = 0;
            SimultanTurns.simultanTurns = false;
            PoleOdkryj.mgla = true;
        }
        MenuGlowne.multi = false;
        Ip.ip = 0;
        
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

