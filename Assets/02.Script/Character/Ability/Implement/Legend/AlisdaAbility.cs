using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/전설/알리스다")]
public class AlisdaAbility : AsyncAbilityBase
{
    // 500% 데미지, 1.5초 스턴
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.moveSpeed = 0;
                enemyBase.TakeDamage(characterBase.heroInfo.attackDamage * 5);
            }
        }
        yield return oneSecond;
        yield return halfSecond;
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<EnemyBase>().moveSpeed = hit.GetComponent<EnemyBase>().originMoveSpeed;
            }
        }

        PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantAbilityEffect, abilityEffectType);
    }
}
