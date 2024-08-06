using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/벙커")]
public class BunkerAbility : SyncAbilityBase
{
    // 최대체력 4% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
        instantAbilityEffect.GetComponent<RangeWeapon>().weaponEffect = characterBase.weaponEffect;
        instantAbilityEffect.GetComponent<RangeWeapon>().attackDamage = characterBase.enemyTrans.GetComponent<EnemyBase>().maxHp * 0.04f;
        characterBase.SetLastBulletPos(instantAbilityEffect, characterBase.enemyTrans, characterBase.gunPointTrans);
        instantAbilityEffect.transform.position += new Vector3(1f, 0, 0);
    }
}
