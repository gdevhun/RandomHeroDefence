using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/배트")]
public class BatAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 모든 몬스터 마방 10 감소(6번 제한)
        if(EnemyBase.decreaseMagDef >= 60f) return;
        EnemyBase.decreaseMagDef += 10f;
    }
}
