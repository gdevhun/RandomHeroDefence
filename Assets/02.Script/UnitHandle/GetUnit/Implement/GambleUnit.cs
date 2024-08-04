using System.Collections.Generic;
using UnityEngine;

public class GambleUnit : GetUnitBase, IConsumable
{
    // 도박에서 등급에 따른 가중치 설정
    private Dictionary<HeroGradeType, int> gradeWeightMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 50 },
        { HeroGradeType.Rare, 38 },
        { HeroGradeType.Legend, 12 }
    };

    private void Start() { amount = 2; }

    // 도박 구체화
    public override void GetUnitHandle()
    {
        // 재화 체크
        if(!ConsumeCurrency()) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 최대 유닛 체크
        if(CurUnit >= maxUnit) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 랜덤 유닛
        GameObject instantUnit = GetUnit(gradeWeightMap);

        // 스폰 위치
        GameObject unitPos = null;
        if(instantUnit.GetComponent<CharacterBase>().heroInfo.heroGradeType != HeroGradeType.Normal) unitPos = GetUnitPos(instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);

        // 스폰 위치 체크, 노말 == 실패 체크
        if(unitPos == null)
        {
            PoolManager.instance.ReturnPool(PoolManager.instance.unitPool.queMap, instantUnit, instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);
            SoundManager.instance.SFXPlay(SoundType.NotEnough);
            return;
        }

        // 유닛 소환
        instantUnit.transform.SetParent(unitPos.transform);
        instantUnit.transform.localPosition = new Vector3(unitPos.transform.childCount == 3 ? 0.1f : 0.2f * (unitPos.transform.childCount - 1), unitPos.transform.childCount == 3 ? 0 : 0.2f, -0.1f * (unitPos.transform.childCount - 1));
        ++CurUnit;
    }

    // 재화
    public int amount { get; set; }
    public bool ConsumeCurrency() { return CurrencyManager.instance.ConsumeCurrency(amount, false); }
}
