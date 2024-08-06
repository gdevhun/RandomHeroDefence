using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/일반/헌터")]
public class HunterAbility : AsyncAbilityBase
{
    // 5초 동안 100% 데미지 총알 1개 추가 발사
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        for(int i = 0; i < 5; i++)
        {
            if(!characterBase.isOnTarget) { yield return oneSecond; continue; }
            instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
            instantAbilityEffect.GetComponent<RangeWeapon>().weaponEffect = characterBase.weaponEffect;
            instantAbilityEffect.GetComponent<RangeWeapon>().attackDamage = characterBase.heroInfo.attackDamage;
            characterBase.SetLastBulletPos(instantAbilityEffect, characterBase.enemyTrans);
            instantAbilityEffect.transform.position += new Vector3(1f, 0, 0);
            yield return oneSecond;
        }
    }
}
