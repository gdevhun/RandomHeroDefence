using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUnit : MonoBehaviour, IConsumable
{
    public static UpgradeUnit instance;
    private void Awake() { instance = this; }

    // 업그레이드 수치
    public Dictionary<HeroGradeType, int> gradeUpgradeMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 0 },
        { HeroGradeType.Elite, 0 },
        { HeroGradeType.Rare, 0 },
        { HeroGradeType.Legend, 0 },
        { HeroGradeType.Myth, 0 },
    };

    [Header ("일반 레벨 텍스트")] [SerializeField] private TextMeshProUGUI normalLvText;
    [Header ("전설 레벨 텍스트")] [SerializeField] private TextMeshProUGUI legendLvText;
    [Header ("골드 텍스트")] [SerializeField] private TextMeshProUGUI goldText;
    [Header ("다이아 텍스트")] [SerializeField] private TextMeshProUGUI diaText;

    // 유닛 업그레이드
    private void Upgrade(HeroGradeType heroGradeType)
    {
        // 재화 체크
        curGradeType = heroGradeType;
        amount = curGradeType == HeroGradeType.Normal ? 30 + 10 * gradeUpgradeMap[curGradeType] : 2 + gradeUpgradeMap[curGradeType];
        if(!ConsumeCurrency()) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 업그레이드
        int e = 3;
        if(heroGradeType == HeroGradeType.Legend) --e;
        for(int i = 0; i < e; i++) gradeUpgradeMap[curGradeType++]++;

        // UI 갱신
        UpdateUpgradeUI(heroGradeType);

        // 사운드
        SoundManager.instance.SFXPlay(SoundType.Upgrade);
    }

    public void NormalUpgrade() { Upgrade(HeroGradeType.Normal); }
    public void LegendUpgrade() { Upgrade(HeroGradeType.Legend); }

    // 재화
    private HeroGradeType curGradeType;
    public int amount { get; set; }
    public bool ConsumeCurrency()
    {
        if(curGradeType == HeroGradeType.Normal) return CurrencyManager.instance.ConsumeCurrency(amount, true);
        return CurrencyManager.instance.ConsumeCurrency(amount, false);
    }

    // 업그레이드 UI 갱신
    private void UpdateUpgradeUI(HeroGradeType heroGradeType)
    {
        if(heroGradeType == HeroGradeType.Normal)
        {
            normalLvText.text = "Lv." + gradeUpgradeMap[heroGradeType].ToString();
            goldText.text = (30 + 10 * gradeUpgradeMap[heroGradeType]).ToString();

            return;
        }

        legendLvText.text = "Lv." + gradeUpgradeMap[heroGradeType].ToString();
        diaText.text = (2 + gradeUpgradeMap[heroGradeType]).ToString();
    }
}
