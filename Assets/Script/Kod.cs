using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kod : MonoBehaviour
{
    private bool kodShow = true;
    public InputField inputField;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote)) // Tylda znajduje się na klawiszu BackQuote
        {
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
            default:
                break;
        }
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
