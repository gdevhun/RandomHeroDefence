using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class UiUnit : MonoBehaviour
{
    public static UiUnit instance;
    private void Awake() { instance = this; }

    [Header ("툴팁 패널")] public ToolTipUnit toolTipPanel;
    [Header ("유닛 판매 / 합성 패널")] public GameObject unitSellCompPanel;
    [Header ("유닛 판매 골드 이미지")] public GameObject unitSellGoldImage;
    [Header ("유닛 판매 다이아 이미지")] public GameObject unitSellDiaImage;
    [Header ("유닛 판매 골드 텍스트")] public TextMeshProUGUI unitSellGoldText;
    [Header ("유닛 판매 다이아 텍스트")] public TextMeshProUGUI unitSellDiaText;

    // 패널 열기
    public void OpenPanel(GameObject panel)
    {
        SoundManager.instance.SFXPlay(SoundType.Click);
        panel.SetActive(true);
    }

    // 패널 닫기
    public void ExitPanel(GameObject panel) { panel.SetActive(false); }
}
