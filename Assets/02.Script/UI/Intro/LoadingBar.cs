using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LoadingBar : MonoBehaviour
{
    private string[] loadingStrings = { "Loading", "Loading.", "Loading..", "Loading..." };
    private float timer = 0f;
    private float changeInterval = 0.5f;
    private int currentIndex = 0;
    [SerializeField] private TextMeshProUGUI loadingText;
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            timer = 0f;
            
            loadingText.text = loadingStrings[currentIndex];
            
            currentIndex = (currentIndex + 1) % loadingStrings.Length;
        }
    }
}
