using System.Collections.Generic;
using UnityEngine;

public class CombUnit : GetUnitBase
{
    private static GameObject selectedPos; // 선택된 유닛 스폰 위치
    [Header ("유닛 스폰 위치만 클릭되게")] public LayerMask posLayerMask;

    // 일반 합성, 고급 합성, 희귀 합성
    Dictionary<HeroGradeType, int> NormalCombMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Elite, 100 }
    };

    Dictionary<HeroGradeType, int> EliteCombMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 25 },
        { HeroGradeType.Rare, 75 }
    };

    Dictionary<HeroGradeType, int> RareCombMap = new Dictionary<HeroGradeType, int>
    {
        { HeroGradeType.Normal, 50 },
        { HeroGradeType.Legend, 50 }
    };

    // 합성 테스트
    private void Update()
    {
        // 마우스 왼쪽 클릭 또는 모바일 터치 시
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 마우스 클릭 또는 모바일 터치 위치 가져옴
            Vector3 inputPosition = Input.GetMouseButtonDown(0) ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            inputPosition.z = 0;

            // 히트된 스폰 할 위치 콜라이더 가져와서 선택된 스폰 위치로 설정
            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero, Mathf.Infinity, posLayerMask);

            // 히트된 콜라이더가 있는지 체크
            if(hit.collider == null) return;

            // 선택된 유닛 스폰 위치 저장
            selectedPos = hit.transform.gameObject;
        }

        if(selectedPos != null && Input.GetKeyDown(KeyCode.Alpha3)) GetUnitHandle();
    }

    // 합성 구체화
    public override void GetUnitHandle()
    {
        // 유닛이 3 개 인지 체크
        if(selectedPos.transform.childCount < 3) return;

        // 합성 할 유닛 처리
        // 1.등급 가져오기
        // 2.맵핑 삭제하기
        // 3.부모 해제하기
        // 4.풀에 반환하기
        CharacterBase selectedCharacterBase = selectedPos.transform.GetChild(0).GetComponent<CharacterBase>();
        HeroGradeType selectedGradeType = selectedCharacterBase.heroInfo.heroGradeType;
        UnitType selectedUnitType = selectedCharacterBase.heroInfo.unitType;
        unitPosMap[selectedUnitType].Remove(selectedPos);
        for(int i = 0; i < 3; i++)
        {
            GameObject selectedCharacter = selectedPos.transform.GetChild(0).gameObject;
            selectedCharacter.transform.SetParent(null);
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
        curUnit -= 2;
    }
}
