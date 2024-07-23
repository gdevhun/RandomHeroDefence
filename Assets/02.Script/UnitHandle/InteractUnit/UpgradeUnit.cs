using System.Collections.Generic;
using System.Linq;
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

    // 업그레이드 테스트
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5)) Upgrade(HeroGradeType.Normal);
        if(Input.GetKeyDown(KeyCode.Alpha6)) Upgrade(HeroGradeType.Legend);
    }

    // 유닛 업그레이드
    private void Upgrade(HeroGradeType heroGradeType)
    {
        // 재화 체크
        curGradeType = heroGradeType;
        if(!ConsumeCurrency()) return;

        // 업그레이드
        int e = 3;
        if(heroGradeType == HeroGradeType.Legend) --e;
        for(int i = 0; i < e; i++) gradeUpgradeMap[curGradeType++]++;

        // UI 갱신
        UpdateUpgradeUI(heroGradeType);

        // 사운드
        SoundManager.instance.SFXPlay(SoundType.Upgrade);

        for(int i = 0; i < gradeUpgradeMap.Count; i++)
        {
            Debug.Log($"{gradeUpgradeMap.ElementAt(i).Key} 업그레이드 수치: {gradeUpgradeMap.ElementAt(i).Value}");
        }
    }

    public void NormalUpgrade()
    {
        Upgrade(HeroGradeType.Normal);
    }

    public void LegendUpgrade()
    {
        Upgrade(HeroGradeType.Legend);
    }

    // 재화
    private HeroGradeType curGradeType;
    public int amount { get; set; }
    public bool ConsumeCurrency()
    {
        if(curGradeType == HeroGradeType.Normal) return CurrencyManager.instance.ConsumeCurrency(30 + 10 * gradeUpgradeMap[curGradeType], true);
        return CurrencyManager.instance.ConsumeCurrency(2 + gradeUpgradeMap[curGradeType], false);
    }

    // 업그레이드 UI 갱신
    public void UpdateUpgradeUI(HeroGradeType heroGradeType)
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
