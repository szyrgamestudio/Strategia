using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMenu : MonoBehaviour
{
    public GameObject przycisk;
    private float scaleSpeed = 8f; // Prędkość zmiany rozmiaru
    private Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f); // Docelowy rozmiar po powiększeniu
    private Vector3 initialScale;

    private bool isHovered = false;
    private bool isScaling = false;

    void Start()
    {
        if (przycisk == null)
        {
            przycisk = this.gameObject;
        }
        initialScale = przycisk.transform.localScale;
    }

    void OnMouseEnter()
    {
        isHovered = true;
        if (!isScaling)
        {
            StartCoroutine(ScaleObject(przycisk, targetScale));
        }
    }

    void OnMouseExit()
    {
        isHovered = false;
        if (!isScaling)
        {
            StartCoroutine(ScaleObject(przycisk, initialScale));
        }
    }

    IEnumerator ScaleObject(GameObject obj, Vector3 target)
    {
        //isScaling = true;
        while (isHovered && obj.transform.localScale != target)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, target, scaleSpeed * Time.deltaTime);
            yield return null;
        }
        while (!isHovered && obj.transform.localScale != initialScale)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, initialScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }
        isScaling = false;
    }
}
