using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartNotifyTool : MonoBehaviour
{
    private float endSec = 10;
    private float startSec = 0;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI infoSecond;
    private void Update()
    {
        startSec += Time.deltaTime;

        // 카운트다운 타이머
        float remainingTime = Mathf.Clamp(endSec - startSec, 0f, endSec);
        infoSecond.text = remainingTime.ToString("F0");  

        // 텍스트 알파값
        float alpha = Mathf.PingPong(Time.time, 1.5f) / 1.5f; // 1.5초에 걸쳐 알파값 0,255반복
        SetAlpha(infoText, alpha);
        
        if (startSec >= endSec)
        {
            gameObject.SetActive(false);
        }
    }

    private void SetAlpha(TextMeshProUGUI textMeshPro, float alpha)
    {
        Color color = textMeshPro.color;
        color.a = alpha;
        textMeshPro.color = color;
    }
}
