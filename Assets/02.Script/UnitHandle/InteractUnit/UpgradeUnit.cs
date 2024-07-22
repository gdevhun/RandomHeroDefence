using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeUnit : MonoBehaviour, IConsumable
{
    public static UpgradeUnit instance;
    private void Awake() { instance = this; }

    // 희귀 / 전설 / 신화 업그레이드 수치
    public Dictionary<HeroGradeType, int> gradeUpgradeMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Rare, 0 },
        { HeroGradeType.Legend, 0 },
        { HeroGradeType.Myth, 0 },
    };

    // 업그레이드 테스트
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5)) Upgrade(HeroGradeType.Rare);
        if(Input.GetKeyDown(KeyCode.Alpha6)) Upgrade(HeroGradeType.Legend);
        if(Input.GetKeyDown(KeyCode.Alpha7)) Upgrade(HeroGradeType.Myth);
    }

    // 유닛 업그레이드
    public void Upgrade(HeroGradeType heroGradeType)
    {
        // 재화 체크
        curGradeType = heroGradeType;
        if(!ConsumeCurrency()) return;

        gradeUpgradeMap[curGradeType = heroGradeType]++;

        // 사운드
        SoundManager.instance.SFXPlay(SoundType.Upgrade);

        for(int i = 0; i < gradeUpgradeMap.Count; i++)
        {
            Debug.Log($"{gradeUpgradeMap.ElementAt(i).Key} 업그레이드 수치: {gradeUpgradeMap.ElementAt(i).Value}");
        }
    }

    // 재화
    private HeroGradeType curGradeType;
    public int amount { get; set; }
    public bool ConsumeCurrency()
    {
        return CurrencyManager.instance.ConsumeCurrency(2 + gradeUpgradeMap[curGradeType], false);
    }
}
