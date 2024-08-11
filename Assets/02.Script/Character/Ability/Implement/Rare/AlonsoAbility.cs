using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/알론소")]
public class AlonsoAbility : AsyncAbilityBase
{
    // 200% 데미지, 이속 감소 10(3초 유지)
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 2, characterBase.heroInfo.damageType);
            }
        }
        
        EnemyBase.DecreaseMoveSpeed += 0.1f;
        yield return oneSecond;
        yield return oneSecond;
        yield return oneSecond;
        EnemyBase.DecreaseMoveSpeed -= 0.1f;
    }
}
