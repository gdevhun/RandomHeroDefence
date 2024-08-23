using UnityEngine;

public class SellUnit : MonoBehaviour
{
    public static SellUnit instance;
    private void Awake() { instance = this; }
    [HideInInspector] public int soldierCnt;

    // 유닛 판매
    public void Sell()
    {
        // 유닛이 있는지 체크
        if(SelectUnit.instance.selectedPos.transform.childCount < 1) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 신화 등급인지 체크
        CharacterBase selectedUnit = SelectUnit.instance.selectedPos.transform.GetChild(0).GetComponent<CharacterBase>();
        HeroGradeType selectedGradeType = selectedUnit.heroInfo.heroGradeType;
        if(selectedGradeType == HeroGradeType.신화) { SoundManager.instance.SFXPlay(SoundType.NotEnough); return; }

        // 판매 유닛 처리
        // 1.유닛이 한 개면 맵핑 삭제하기, 아니면 자식 수 감소하기
        // 2.가장 마지막 자식 부모 해제하고 풀에 반환하기
        // 3.재화 처리
        // 4.유닛 수 처리
        // 5.사운드
        // 6.패널
        // 7.신화 조합 가능 수 체크
        UnitType selectedUnitType = selectedUnit.heroInfo.unitType;
        if(SelectUnit.instance.selectedPos.transform.childCount == 1) GetUnitBase.unitPosMap[selectedUnitType].Remove(SelectUnit.instance.selectedPos);
        else --GetUnitBase.unitPosMap[selectedUnitType][SelectUnit.instance.selectedPos];
        GameObject selectedCharacter = SelectUnit.instance.selectedPos.transform.GetChild(SelectUnit.instance.selectedPos.transform.childCount - 1).gameObject;
        selectedCharacter.transform.SetParent(PoolManager.instance.poolSet.transform);
        PoolManager.instance.ReturnPool(PoolManager.instance.unitPool.queMap, selectedCharacter, selectedUnitType);
        if(selectedGradeType == HeroGradeType.일반 || selectedGradeType == HeroGradeType.고급) CurrencyManager.instance.AcquireCurrency(soldierCnt > 0 ? 2 * (20 + 20 * (int)selectedGradeType) : 20 + 20 * (int)selectedGradeType, true);
        else if(selectedGradeType == HeroGradeType.희귀 || selectedGradeType == HeroGradeType.전설) CurrencyManager.instance.AcquireCurrency((int)selectedGradeType, false);
        GetUnitBase.CurUnit -= 1;
        SoundManager.instance.SFXPlay(SoundType.Sell);
        if(SelectUnit.instance.selectedPos.transform.childCount == 0)
        {
            UiUnit.instance.ExitPanel(UiUnit.instance.unitSellCompPanel);
            UiUnit.instance.ExitPanel(UiUnit.instance.toolTipPanel.gameObject);  
        }
        MissionManager.instance.heroSaleCnt++;

        // 신화 조합 가능 개수 표시
        UiUnit.instance.mythicCombPanel.SetActive(true);
        MythicUnit.instance.mythicCombCheckCnt.text = MythicUnit.instance.CheckMythicComb().ToString();
        UiUnit.instance.ExitPanel(UiUnit.instance.mythicCombPanel);
    }
}
