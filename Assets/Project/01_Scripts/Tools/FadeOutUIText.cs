using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutUIText : MonoBehaviour
{


    public bool isFading;

    private Color textColor;
    private Color fadedColor;
    
    void Start()
    {
        textColor = GetComponent<Text>().color;
       textColor = new Color(textColor.r, textColor.g,textColor.b, 1f);
        fadedColor = textColor;
        fadedColor.a = 0;
        textColor.a = 0;

        GetComponent<Text>().color = textColor;
    }

   
    void Update()
    {


        if (isFading)
        {
            textColor = Color.Lerp(textColor, fadedColor, Time.deltaTime / 2);
            GetComponent<Text>().color = textColor;
        }

        if (textColor.Equals(fadedColor))
        {
            isFading = false;
        }
    }


    public void startFading()
    {
        isFading = true;
        textColor = new Color(textColor.r, textColor.g,textColor.b, 1f);
        GetComponent<Text>().color = textColor;
    }
}
