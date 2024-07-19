using System.Collections.Generic;
using UnityEngine;

public class GambleUnit : GetUnitBase
{
    // 도박에서 등급에 따른 가중치 설정
    private Dictionary<HeroGradeType, int> gradeWeightMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 50 },
        { HeroGradeType.Rare, 38 },
        { HeroGradeType.Legend, 12 }
    };

    // 도박 테스트
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2)) GetUnitHandle();
    }

    // 도박 구체화
    public override void GetUnitHandle()
    {
        // 최대 유닛 체크
        if(curUnit >= maxUnit)
        {
            Debug.Log("최대 유닛 수!");
            return;
        }

        // 랜덤 유닛
        GameObject instantUnit = GetUnit(gradeWeightMap);

        // 스폰 위치
        GameObject unitPos = GetUnitPos(instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);

        // 스폰 위치 체크, 노말 == 실패 체크
        if(unitPos == null || instantUnit.GetComponent<CharacterBase>().heroInfo.heroGradeType == HeroGradeType.Normal)
        {
            PoolManager.instance.ReturnPool(PoolManager.instance.queUnitMap, instantUnit, instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);
            return;
        }

        // 유닛 소환
        instantUnit.transform.SetParent(unitPos.transform);
        instantUnit.transform.localPosition = new Vector3(0.2f * (unitPos.transform.childCount - 1), 0, 0);
        ++curUnit;
    }
}
