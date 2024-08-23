using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/벙커")]
public class BunkerAbility : SyncAbilityBase
{
    // 300% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
        RangeWeapon rangeWeapon = instantAbilityEffect.GetComponent<RangeWeapon>();
        rangeWeapon.weaponEffect = characterBase.weaponEffect;
        rangeWeapon.damageType = characterBase.heroInfo.damageType;
        rangeWeapon.attackDamage = characterBase.heroInfo.attackDamage * 3;
        rangeWeapon.characterBase = characterBase;
        characterBase.SetLastBulletPos(instantAbilityEffect, characterBase.enemyTrans, characterBase.gunPointTrans);
    }
}
