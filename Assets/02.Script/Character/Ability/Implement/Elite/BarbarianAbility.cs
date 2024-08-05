using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/바바리안")]
public class BarbarianAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;

        // 150% 데미지, 현재체력 6% 데미지
        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(characterBase.heroInfo.attackDamage * 1.5f + enemyBase.currentHp * 0.06f);
            }
        }
        
        PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantAbilityEffect, abilityEffectType);
    }
}
