using UnityEngine;

public class ThiefAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;

        // 150% 데미지, 골드 10 획득
        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(characterBase.heroInfo.attackDamage * 1.5f);
            }
        }
        CurrencyManager.instance.AcquireCurrency(10, true);

        PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantAbilityEffect, abilityEffectType);
    }
}
