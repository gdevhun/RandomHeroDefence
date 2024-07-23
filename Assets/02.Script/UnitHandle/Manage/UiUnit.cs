using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUnit : MonoBehaviour
{
    public static UiUnit instance;
    private void Awake() { instance = this; }

    // 패널 열기
    public void OpenPanel(GameObject panel)
    {
        SoundManager.instance.SFXPlay(SoundType.Click);
        panel.SetActive(true);
    }

    // 패널 닫기
    public void ExitPanel(GameObject panel)
    {
        SoundManager.instance.SFXPlay(SoundType.Click);
        panel.SetActive(false);
    }
}
