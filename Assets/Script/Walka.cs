using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Walka : MonoBehaviour
{
    public static float odleglosc(GameObject A, GameObject B)
    {
        Vector3 pozycjaA = A.transform.position;
        Vector3 pozycjaB = B.transform.position;

        float dystansX = Mathf.Abs(pozycjaA.x - pozycjaB.x);
        float dystansY = Mathf.Abs(pozycjaA.y - pozycjaB.y);

        float dystans = dystansX + dystansY;
        return dystans;
    }
}
