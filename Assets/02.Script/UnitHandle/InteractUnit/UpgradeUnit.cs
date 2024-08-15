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
        { HeroGradeType.일반, 0 },
        { HeroGradeType.고급, 0 },
        { HeroGradeType.희귀, 0 },
        { HeroGradeType.전설, 0 },
        { HeroGradeType.신화, 0 },
    };
    public Dictionary<DamageType, int> damageUpgradeMap = new Dictionary<DamageType, int>
    {
        { DamageType.물리, 0 },
        { DamageType.마법, 0 }
    };

    [Header ("일반 레벨 텍스트")] [SerializeField] private TextMeshProUGUI normalLvText;
    [Header ("전설 레벨 텍스트")] [SerializeField] private TextMeshProUGUI legendLvText;
    [Header ("골드 텍스트")] [SerializeField] private TextMeshProUGUI goldText;
    [Header ("다이아 텍스트")] [SerializeField] private TextMeshProUGUI diaText;
    private int normalUpgradeCnt = 0, legendUpgradeCnt = 0;

    // 유닛 업그레이드
    private void Upgrade(HeroGradeType heroGradeType)
    {
        curGradeType = heroGradeType;

        // 최대 업그레이드 체크
        int upgradeCnt = curGradeType == HeroGradeType.일반 ? normalUpgradeCnt : legendUpgradeCnt;
        if(upgradeCnt >= 20) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 재화 체크
        amount = curGradeType == HeroGradeType.일반 ? 100 + 25 * upgradeCnt : 2 + upgradeCnt;
        if(!ConsumeCurrency()) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 업그레이드
        int e = curGradeType == HeroGradeType.일반 ? 3 : 2;

        HeroGradeType standardGradeType = heroGradeType;
        for(int i = 0; i < e; i++) gradeUpgradeMap[standardGradeType++] += 20 + 20 * (upgradeCnt / 5);

        if(curGradeType == HeroGradeType.일반) normalUpgradeCnt++;
        else legendUpgradeCnt++;

        // UI 갱신
        UpdateUpgradeUI(heroGradeType);

        // 사운드
        SoundManager.instance.SFXPlay(SoundType.Upgrade);
    }

    public void NormalUpgrade() { Upgrade(HeroGradeType.일반); }
    public void LegendUpgrade() { Upgrade(HeroGradeType.전설); }

    // 재화
    private HeroGradeType curGradeType;
    public int amount { get; set; }
    public bool ConsumeCurrency()
    {
        if(curGradeType == HeroGradeType.일반) return CurrencyManager.instance.ConsumeCurrency(amount, true);
        return CurrencyManager.instance.ConsumeCurrency(amount, false);
    }

    // 업그레이드 UI 갱신
    private void UpdateUpgradeUI(HeroGradeType heroGradeType)
    {
        if(heroGradeType == HeroGradeType.일반)
        {
            normalLvText.text = "Lv." + normalUpgradeCnt.ToString();
            goldText.text = (100 + 25 * normalUpgradeCnt).ToString();
            return;
        }

        legendLvText.text = "Lv." + legendUpgradeCnt.ToString();
        diaText.text = (2 + legendUpgradeCnt).ToString();
    }
}
