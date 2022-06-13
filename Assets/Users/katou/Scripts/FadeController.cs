using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するため

public class FadeController : MonoBehaviour
{
    float fadeSpeed = 1f;
    float red, green, blue, alfa;

    public bool isFadeOut = false;
    public bool isFadeIn = false;

    Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
        
        



    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            StartFadeIn();
        }
        else if (isFadeOut)
        {
            StartFadeOut();
        }
    }

    void StartFadeIn()
    {
        alfa -= Time.deltaTime / fadeSpeed;
        SetAlpha();
        if(alfa <= 0)
        {
            isFadeIn = false;
            fadeImage.enabled = false;
        }
    }
    void StartFadeOut()
    {
        fadeImage.enabled = true;
        alfa += Time.deltaTime / fadeSpeed;
        SetAlpha();
        if(alfa >= 1)
        {
            isFadeOut = false;
        }
    }

    void SetAlpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }
}

