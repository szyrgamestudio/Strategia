using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleOdkryj : MonoBehaviour
{
    public GameObject kafelek;
    public GameObject dark;
    // Start is called before the first frame update
    void Start()
    {
        if(!MenuGlowne.multi)
            Destroy(dark);
    }

    public void remove()
    {
        Destroy(dark);
    }
}
