using TMPro;
using UnityEngine;

public class UiUnit : MonoBehaviour
{
    public static UiUnit instance;
    private void Awake() { instance = this; }

    [Header ("툴팁 패널")] public ToolTipUnit toolTipPanel;
    [Header ("유닛 핸들 패널")] public GameObject unitHandlePanel;
    [Header ("유닛 도박 패널")] public GameObject unitGamblePanel;
    [Header ("유닛 판매 / 합성 패널")] public GameObject unitSellCompPanel;
    [Header ("신화 합성 패널")] public GameObject mythicCombPanel;
    [Header ("유닛 판매 골드 이미지")] public GameObject unitSellGoldImage;
    [Header ("유닛 판매 다이아 이미지")] public GameObject unitSellDiaImage;
    [Header ("유닛 판매 골드 텍스트")] public TextMeshProUGUI unitSellGoldText;
    [Header ("유닛 판매 다이아 텍스트")] public TextMeshProUGUI unitSellDiaText;
    [Header ("유닛 소환")] public SpawnUnit unitSpawn;
    [Header ("물리 증가 텍스트")] public TextMeshProUGUI phyText;
    [Header ("마법 증가 텍스트")] public TextMeshProUGUI magText;
    [Header ("물리 방깎 텍스트")] public TextMeshProUGUI phyDecText;
    [Header ("마법 방깎 텍스트")] public TextMeshProUGUI magDecText;
    [Header ("이속 감소 텍스트")] public TextMeshProUGUI slowText;

    // 패널 열기
    public void OpenPanel(GameObject panel) { panel.SetActive(true); }

    // 패널 닫기
    public void ExitPanel(GameObject panel) { panel.SetActive(false); }

    // 클릭 사운드
    public void ClickSound() { SoundManager.instance.SFXPlay(SoundType.Click); }
}
