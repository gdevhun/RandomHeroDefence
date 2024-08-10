using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/배트맨")]
public class BatmanAbility : SyncAbilityBase, IHiddenAbility
{
    // 모든 몬스터에 200% 데미지 총알 1개 발사
    public override void CastAbility(CharacterBase characterBase)
    {
        for(int i = 0; i < StageManager.instance.instantEnemyList.gameObjectList.Count; i++)
        {
            instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
            instantAbilityEffect.GetComponent<RangeWeapon>().weaponEffect = characterBase.weaponEffect;
            instantAbilityEffect.GetComponent<RangeWeapon>().attackDamage = characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 2;
            characterBase.SetLastBulletPos(instantAbilityEffect, StageManager.instance.instantEnemyList.gameObjectList[i].transform, characterBase.gunPointTrans);
        }

        CastHiddenAbility(characterBase);
    }

    // 히든 스킬
    // 배트가 존재하면 배트에서도 1발 발사
    [Header ("히든 스킬 UI 정보")] [SerializeField] private AbilityUiInfo hiddenAbilityUiInfo;
    public AbilityUiInfo HiddenAbilityUiInfo
    {
        get { return hiddenAbilityUiInfo; }
        set { hiddenAbilityUiInfo = value; }
    }
    public void CastHiddenAbility(CharacterBase characterBase)
    {
        for(int i = 0; i < GetUnitBase.unitPosMap[UnitType.뱃].Count; i++)
        {
            for(int j = 0; j < GetUnitBase.unitPosMap[UnitType.뱃].ElementAt(i).Key.transform.childCount; j++)
            {
                for(int k = 0; k < StageManager.instance.instantEnemyList.gameObjectList.Count; k++)
                {
                    CharacterBase batCharacterBase = GetUnitBase.unitPosMap[UnitType.뱃].ElementAt(i).Key.transform.GetChild(j).GetComponent<CharacterBase>();
                    instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, characterBase.weaponEffect);
                    instantAbilityEffect.GetComponent<RangeWeapon>().weaponEffect = characterBase.weaponEffect;
                    instantAbilityEffect.GetComponent<RangeWeapon>().attackDamage = characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage) * 2;
                    characterBase.SetLastBulletPos(instantAbilityEffect, StageManager.instance.instantEnemyList.gameObjectList[k].transform, batCharacterBase.transform);
                }
            }
        }
    }
}
