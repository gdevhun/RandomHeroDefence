using UnityEngine;

[CreateAssetMenu(menuName = "스킬/전설/알리스다")]
public class AlisdaAbility : SyncAbilityBase
{
    // 500% 데미지, 1.5초 스턴
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(characterBase.enemyTrans.transform.position, 2f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.SetStunTime += 2f;
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 5, characterBase.heroInfo.damageType);
            }
        }
    }
}
