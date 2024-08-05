using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/벙커")]
public class BunkerAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;

        // 최대체력 4% 데미지
        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(enemyBase.maxHp * 0.04f);
            }
        }
        
        PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantAbilityEffect, abilityEffectType);
    }
}
