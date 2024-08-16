using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/바바리안")]
public class BarbarianAbility : SyncAbilityBase
{
    // 150% 데미지, 현재체력 6% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
        MeleeWeapon meleeWeapon = instantAbilityEffect.GetComponent<MeleeWeapon>();
        meleeWeapon.weaponEffect = characterBase.weaponEffect;
        meleeWeapon.damageType = characterBase.heroInfo.damageType;
        meleeWeapon.attackDamage = characterBase.heroInfo.attackDamage * 1.5f;
        meleeWeapon.characterBase = characterBase;
        meleeWeapon.isEnter = false;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(instantAbilityEffect.transform.position, 1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = hit.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(enemyBase.CurrentHp * 0.06f, characterBase.heroInfo.damageType);
            }
        }
    }
}
