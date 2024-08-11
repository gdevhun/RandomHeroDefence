using UnityEngine;

[CreateAssetMenu(menuName = "스킬/일반/레슬러")]
public class WrestlerAbility : SyncAbilityBase
{
    // 200% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
        instantAbilityEffect.GetComponent<MeleeWeapon>().weaponEffect = characterBase.weaponEffect;
        instantAbilityEffect.GetComponent<MeleeWeapon>().damageType = characterBase.heroInfo.damageType;
        instantAbilityEffect.GetComponent<MeleeWeapon>().isEnter = false;
        instantAbilityEffect.GetComponent<MeleeWeapon>().attackDamage = characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage * 2);
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;
    }
}
