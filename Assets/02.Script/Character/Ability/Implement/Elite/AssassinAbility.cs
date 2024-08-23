using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/어쌔신")]
public class AssassinAbility : SyncAbilityBase
{
    // 300% 데미지
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
        MeleeWeapon meleeWeapon = instantAbilityEffect.GetComponent<MeleeWeapon>();
        meleeWeapon.weaponEffect = characterBase.weaponEffect;
        meleeWeapon.damageType = characterBase.heroInfo.damageType;
        meleeWeapon.attackDamage = characterBase.heroInfo.attackDamage * 3;
        meleeWeapon.characterBase = characterBase;
        meleeWeapon.isEnter = false;
        instantAbilityEffect.transform.position = characterBase.enemyTrans.position;
    }
}
