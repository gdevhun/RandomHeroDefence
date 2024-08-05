using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/일반/헌터")]
public class HunterAbility : AsyncAbilityBase
{
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        // 5초 동안 100% 데미지 총알 1개씩 발사
        for(int i = 0; i < 5; i++)
        {
            // 타겟이 있는지 체크
            if(!characterBase.isOnTarget) { yield return oneSecond; continue; }

            // 총알 발사
            instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
            instantAbilityEffect.GetComponent<RangeWeapon>().weaponEffect = characterBase.weaponEffect;
            instantAbilityEffect.GetComponent<RangeWeapon>().attackDamage = characterBase.heroInfo.attackDamage;
            characterBase.SetLastBulletPos(instantAbilityEffect, characterBase.enemyTrans);
            instantAbilityEffect.transform.position += new Vector3(1f, 0, 0);
            yield return oneSecond;
            PoolManager.instance.ReturnPool(PoolManager.instance.weaponEffectPool.queMap, instantAbilityEffect, characterBase.weaponEffect);
        }
    }
}
