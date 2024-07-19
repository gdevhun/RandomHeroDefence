using UnityEngine;

public class SellUnit : MonoBehaviour
{
    // 판매 테스트
    private void Update()
    {
        if(SelectUnit.instance.selectedPos != null && Input.GetKeyDown(KeyCode.Alpha4)) Sell();
    }

    // 유닛 판매
    public void Sell()
    {
        // 유닛이 있는지 체크
        if(SelectUnit.instance.selectedPos.transform.childCount < 1) return;

        // 판매 유닛 처리
        // 1.유닛이 한 개면 맵핑 삭제하기
        // 2.가장 마지막 자식 부모 해제하고 풀에 반환하기
        // 3.재화 처리 => 등급에 따라 골드 및 다이아 => 재화 매니저
        // 4.유닛 수 처리
        UnitType selectedUnitType = SelectUnit.instance.selectedPos.transform.GetChild(0).GetComponent<CharacterBase>().heroInfo.unitType;
        if(SelectUnit.instance.selectedPos.transform.childCount == 1) GetUnitBase.unitPosMap[selectedUnitType].Remove(SelectUnit.instance.selectedPos);
        GameObject selectedCharacter = SelectUnit.instance.selectedPos.transform.GetChild(SelectUnit.instance.selectedPos.transform.childCount - 1).gameObject;
        selectedCharacter.transform.SetParent(PoolManager.instance.poolSet.transform);
        PoolManager.instance.ReturnPool(PoolManager.instance.queUnitMap, selectedCharacter, selectedUnitType);
        GetUnitBase.curUnit -= 1;
    }
}
