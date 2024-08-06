using UnityEngine;

[CreateAssetMenu(menuName = "스킬/일반/솔져")]
public class SoldierAbility : SyncAbilityBase
{
    // 유닛 판매 추가 골드
    public override void CastAbility(CharacterBase characterBase) { SellUnit.instance.soldierCnt++; }
}
