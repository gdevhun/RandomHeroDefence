using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossNotifyTool : MonoBehaviour
{
    public List<TextMeshProUGUI> pingPongTxt;
    public List<Image> pingPongImg;

    private void Update()
    {
        // 0.75초에 걸쳐서 알파값이 100~255 사이에서 핑퐁
        float alphaValue = Mathf.PingPong(Time.time / 0.75f * 155f, 155f) + 100f;

        // Text알파값 조정
        foreach (var txt in pingPongTxt)
        {
            Color color = txt.color;
            color.a = alphaValue / 255f; // 알파값은 0~1 사이의 값이어야 하므로 255로 나눔
            txt.color = color;
        }

        // Image 알파값 조정
        foreach (var img in pingPongImg)
        {
            Color color = img.color;
            color.a = alphaValue / 255f;
            img.color = color;
        }
    }
}
