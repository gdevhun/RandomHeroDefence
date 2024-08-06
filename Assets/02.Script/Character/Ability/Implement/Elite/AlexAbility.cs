using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/알렉스")]
public class AlexAbility : AsyncAbilityBase
{
    // 4초 동안 100% 데미지 총알 2개 추가 발사
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        for(int i = 0; i < 4; i++)
        {
            if(!characterBase.isOnTarget) { yield return oneSecond; continue; }
            for(int j = 1; j < 3; j++)
            {
                instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
                instantAbilityEffect.GetComponent<RangeWeapon>().weaponEffect = characterBase.weaponEffect;
                instantAbilityEffect.GetComponent<RangeWeapon>().attackDamage = characterBase.heroInfo.attackDamage;
                characterBase.SetLastBulletPos(instantAbilityEffect, characterBase.enemyTrans);
                instantAbilityEffect.transform.position += new Vector3(1f * j, 0, 0);
            }
            yield return oneSecond;
        }
    }
}
