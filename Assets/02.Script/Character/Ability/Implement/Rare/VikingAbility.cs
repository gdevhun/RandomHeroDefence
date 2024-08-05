using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/바이킹")]
public class VikingAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 모든 몬스터 물방 10 감소(6번 제한)
        if(EnemyBase.decreasePhyDef >= 60f) return;
        EnemyBase.decreasePhyDef += 10f;
    }
}
