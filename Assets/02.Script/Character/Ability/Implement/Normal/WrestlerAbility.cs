using UnityEngine;

[CreateAssetMenu(menuName = "스킬/일반/레슬러")]
public class WrestlerAbility : SyncAbilityBase
{
    // 200% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
        MeleeWeapon meleeWeapon = instantAbilityEffect.GetComponent<MeleeWeapon>();
        meleeWeapon.weaponEffect = characterBase.weaponEffect;
        meleeWeapon.damageType = characterBase.heroInfo.damageType;
        meleeWeapon.attackDamage = characterBase.heroInfo.attackDamage * 2;
        meleeWeapon.characterBase = characterBase;
        meleeWeapon.isEnter = false;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;
    }
}
