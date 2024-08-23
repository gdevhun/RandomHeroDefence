using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/바이킹")]
public class VikingAbility : SyncAbilityBase
{
    // 모든 몬스터 물방 20 감소
    public override void CastAbility(CharacterBase characterBase) { EnemyBase.DecreasePhyDef += 20f; }
}
