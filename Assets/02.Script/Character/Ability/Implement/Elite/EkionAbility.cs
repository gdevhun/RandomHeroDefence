using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/에키온")]
public class EkionAbility : SyncAbilityBase
{
    // 모든 몬스터 이동속도 감소 5
    public override void CastAbility(CharacterBase characterBase) { EnemyBase.DecreaseMoveSpeed += 0.05f; }
}
