using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/에키온")]
public class EkionAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 모든 몬스터 이동속도 감소 5
        EnemyBase.decreaseMoveSpeed += 0.05f;
    }
}
