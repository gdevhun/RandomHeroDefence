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
                RangeWeapon rangeWeapon = instantAbilityEffect.GetComponent<RangeWeapon>();
                rangeWeapon.weaponEffect = characterBase.weaponEffect;
                rangeWeapon.damageType = characterBase.heroInfo.damageType;
                rangeWeapon.attackDamage = characterBase.heroInfo.attackDamage;
                rangeWeapon.characterBase = characterBase;
                characterBase.SetLastBulletPos(instantAbilityEffect, characterBase.enemyTrans, characterBase.gunPointTrans);
                instantAbilityEffect.transform.position += new Vector3(0.1f * j, 0, 0);
            }
            if(i > 0) SoundManager.instance.SFXPlay(abilitySoundType);
            yield return oneSecond;
        }
    }
}
