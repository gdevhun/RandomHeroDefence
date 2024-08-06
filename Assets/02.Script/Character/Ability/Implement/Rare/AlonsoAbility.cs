using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/알론소")]
public class AlonsoAbility : SyncAbilityBase
{
    // 모든 몬스터를 죽이고 얻는 골드량 10% 증가
    public override void CastAbility(CharacterBase characterBase) { EnemyBase.increaseEnemyGold += 10; }
}
