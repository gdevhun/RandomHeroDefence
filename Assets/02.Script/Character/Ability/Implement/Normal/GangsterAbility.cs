using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/일반/갱스터")]
public class GangsterAbility : AsyncAbilityBase
{
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.transform.position = characterBase.enemyTrans.transform.position;

        // 150% 데미지, 0.5초 스턴
        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.moveSpeed = 0;
                enemyBase.TakeDamage(characterBase.heroInfo.attackDamage * 1.5f);
            }
        }

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
