using UnityEngine;

[CreateAssetMenu(menuName = "스킬/전설/막더스")]
public class MakdusAbility : SyncAbilityBase
{
    // 500% 데미지, 물방 마방 50 감소
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(characterBase.enemyTrans.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 5, characterBase.heroInfo.damageType);
            }
        }
    }
}
