using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Sound : MonoBehaviour
{
    public static GameObject sound;

    public AudioClip walk;
    public AudioClip sword;
    public AudioClip magic;
    public AudioClip damage;
    public AudioClip build;
    public AudioClip dead;

    void Start()
    {
        sound = this.gameObject;

    }
}
