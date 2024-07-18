using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : GetUnitBase
{
    // 소환에서 등급에 따른 가중치 설정
    Dictionary<HeroGradeType, int> gradeWeightMap = new Dictionary<HeroGradeType, int>
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
        GameObject instantUnit = GetUnit(gradeWeightMap);
        instantUnit.transform.position = Vector3.zero;
    }
}
