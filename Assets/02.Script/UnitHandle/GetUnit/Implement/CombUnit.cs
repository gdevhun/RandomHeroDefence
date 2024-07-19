using System.Collections.Generic;
using UnityEngine;

public class CombUnit : GetUnitBase
{
    // 일반 합성, 고급 합성, 희귀 합성
    private Dictionary<HeroGradeType, int> NormalCombMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Elite, 100 }
    };

    private Dictionary<HeroGradeType, int> EliteCombMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 25 },
        { HeroGradeType.Rare, 75 }
    };

    private Dictionary<HeroGradeType, int> RareCombMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 50 },
        { HeroGradeType.Legend, 50 }
    };

    // 합성 테스트
    private void Update()
    {
        if(SelectUnit.instance.selectedPos != null && Input.GetKeyDown(KeyCode.Alpha3)) GetUnitHandle();
    }

    // 합성 구체화
    public override void GetUnitHandle()
    {
        // 유닛이 3 개 인지 체크
        if(SelectUnit.instance.selectedPos.transform.childCount < 3) return;

        // 합성 할 유닛 처리
        // 1.등급 가져오기
        // 2.맵핑 삭제하기
        // 3.부모 해제하기
        // 4.풀에 반환하기
        CharacterBase selectedCharacterBase = SelectUnit.instance.selectedPos.transform.GetChild(0).GetComponent<CharacterBase>();
        HeroGradeType selectedGradeType = selectedCharacterBase.heroInfo.heroGradeType;
        if(selectedGradeType == HeroGradeType.Legend || selectedGradeType == HeroGradeType.Myth) return; // 전설 / 신화 체크
        UnitType selectedUnitType = selectedCharacterBase.heroInfo.unitType;
        unitPosMap[selectedUnitType].Remove(SelectUnit.instance.selectedPos);
        for(int i = 0; i < 3; i++)
        {
            GameObject selectedCharacter = SelectUnit.instance.selectedPos.transform.GetChild(0).gameObject;
            selectedCharacter.transform.SetParent(PoolManager.instance.poolSet.transform);
            PoolManager.instance.ReturnPool(PoolManager.instance.queUnitMap, selectedCharacter, selectedUnitType);
        }

        // 합성 할 유닛의 등급에 따라 유닛 합성
        GameObject instantUnit = null;
        switch(selectedGradeType)
        {
            case HeroGradeType.Normal : instantUnit = GetUnit(NormalCombMap); break;
            case HeroGradeType.Elite : instantUnit = GetUnit(EliteCombMap); break;
            case HeroGradeType.Rare : instantUnit = GetUnit(RareCombMap); break;
        }

        // 스폰 위치
        GameObject unitPos = null;
        if(instantUnit.GetComponent<CharacterBase>().heroInfo.heroGradeType != HeroGradeType.Normal) unitPos = GetUnitPos(instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);

        // 스폰 위치 체크, 노말 == 실패 체크
        if(unitPos == null)
        {
            PoolManager.instance.ReturnPool(PoolManager.instance.queUnitMap, instantUnit, instantUnit.GetComponent<CharacterBase>().heroInfo.unitType);
            curUnit -= 3;
            return;
        }

        // 유닛 소환
        instantUnit.transform.SetParent(unitPos.transform);
        instantUnit.transform.localPosition = new Vector3(unitPos.transform.childCount == 3 ? 0.1f : 0.2f * (unitPos.transform.childCount - 1), unitPos.transform.childCount == 3 ? -0.2f : 0, 0);
        curUnit -= 2;
    }
}
