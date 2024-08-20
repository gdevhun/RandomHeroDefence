using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/알론소")]
public class AlonsoAbility : SyncAbilityBase
{
    // 200% 데미지, 스턴 1.5초
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 2f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.SetStunTime += 1.5f;
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 2, characterBase.heroInfo.damageType);
            }
        }
    }
}
