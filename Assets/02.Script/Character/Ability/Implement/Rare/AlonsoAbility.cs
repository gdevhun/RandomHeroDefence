using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/알론소")]
public class AlonsoAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 모든 몬스터를 죽이고 얻는 골드량 10% 증가(3번 제한)
        if(EnemyBase.increaseEnemyGold >= 30) return;
        EnemyBase.increaseEnemyGold += 10;
    }
}
