using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : GetUnitBase, IConsumable
{
    // 소환에서 등급에 따른 가중치 설정
    private Dictionary<HeroGradeType, int> gradeWeightMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 72 },
        { HeroGradeType.Elite, 24 },
        { HeroGradeType.Rare, 6 },
        { HeroGradeType.Legend, 1 }
    };

    // 소환 테스트
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) GetUnitHandle();
    }

    // 소환 구체화
    public override void GetUnitHandle()
    {
        // 재화 체크
        if(!ConsumeCurrency()) return;
        ++spawnCnt;

        // 최대 유닛 체크
        if(CurUnit >= maxUnit)
        {
            Debug.Log("최대 유닛 수!");
            return;
        }

        // 랜덤 유닛
        GameObject instantUnit = GetUnit(gradeWeightMap);

        // 스폰 위치
        GameObject unitPos = GetUnitPos(instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);

        // 스폰 위치 체크
        if(unitPos == null)
        {
            PoolManager.instance.ReturnPool(PoolManager.instance.unitPool.queMap, instantUnit, instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);
            return;
        }

        // 유닛 소환
        instantUnit.transform.SetParent(unitPos.transform);
        instantUnit.transform.localPosition = new Vector3(unitPos.transform.childCount == 3 ? 0.1f : 0.2f * (unitPos.transform.childCount - 1), unitPos.transform.childCount == 3 ? 0 : 0.2f, 0);
        ++CurUnit;
    }

    // 재화
    private int spawnCnt = 0;
    public int amount { get; set; }
    public bool ConsumeCurrency()
    {
        return CurrencyManager.instance.ConsumeCurrency(10 + spawnCnt / 2, true);
    }
}
