using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSparkle : MonoBehaviour
{
    [SerializeField] private float sparklingTime;
    private Image thisImage;
    private void Awake()
    {
        thisImage = GetComponent<Image>();
    }

    private void Update()
    {
        float alphaValue = Mathf.PingPong(Time.time / sparklingTime, 1f);
        Color currentColor = thisImage.color;
        currentColor.a = alphaValue;
        thisImage.color = currentColor;
    }
}
