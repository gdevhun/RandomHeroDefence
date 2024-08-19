using UnityEngine;

public class ThiefAbility : SyncAbilityBase
{
    // 150% 데미지, 골드 10 획득
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.transform.position + new Vector3(0f,0.5f,0f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(characterBase.enemyTrans.transform.position, 0.5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 1.5f, characterBase.heroInfo.damageType);
            }
        }
        CurrencyManager.instance.AcquireCurrency(10, true);
    }
}
