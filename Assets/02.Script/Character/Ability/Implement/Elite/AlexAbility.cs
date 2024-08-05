using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/고급/알렉스")]
public class AlexAbility : AsyncAbilityBase
{
    public override IEnumerator CastAbility(CharacterBase characterBase)
    {
        // 4초 동안 100% 데미지 총알 2개 추가 발사
        for(int i = 0; i < 4; i++)
        {
            // 타겟이 있는지 체크
            if(!characterBase.isOnTarget) { yield return oneSecond; continue; }

            // 총알 발사
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
