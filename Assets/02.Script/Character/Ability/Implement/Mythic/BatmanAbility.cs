using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/배트맨")]
public class BatmanAbility : SyncAbilityBase, IHiddenAbility
{
    // 모든 몬스터에 200% 데미지 총알 1개 발사
    public override void CastAbility(CharacterBase characterBase)
    {
        // 배트맨 바디
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.transform.position + Vector3.up * 0.5f;

        // 배트맨 총알 발사
        float cur = 0f;
        for (int i = 0; i < 20; i++)
        {
            // 배트맨 총알 풀링
            RangeWeapon rangeWeapon = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, WeaponEffect.BatmanAbilityBullet).GetComponent<RangeWeapon>();
            rangeWeapon.weaponEffect = WeaponEffect.BatmanAbilityBullet;
            rangeWeapon.damageType = characterBase.heroInfo.damageType;
            rangeWeapon.attackDamage = characterBase.heroInfo.attackDamage;
            rangeWeapon.characterBase = characterBase;

            // 발사 방향 셋
            float radian = cur * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            rangeWeapon.transform.SetPositionAndRotation(characterBase.gunPointTrans.position, Quaternion.LookRotation(Vector3.forward, direction));

            // 다음 발사
            cur += 18;
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
                float cur = 0f;
                for (int k = 0; k < 20; k++)
                {
                    // 배트맨 총알 풀링
                    CharacterBase batCharacterBase = GetUnitBase.unitPosMap[UnitType.뱃].ElementAt(i).Key.transform.GetChild(j).GetComponent<CharacterBase>();
                    RangeWeapon rangeWeapon = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, WeaponEffect.BatmanAbilityBullet).GetComponent<RangeWeapon>();
                    rangeWeapon.weaponEffect = WeaponEffect.BatmanAbilityBullet;
                    rangeWeapon.damageType = characterBase.heroInfo.damageType;
                    rangeWeapon.attackDamage = characterBase.heroInfo.attackDamage * 2;
                    rangeWeapon.characterBase = characterBase;

                    // 발사 방향 셋
                    float radian = cur * Mathf.Deg2Rad;
                    Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
                    rangeWeapon.transform.SetPositionAndRotation(batCharacterBase.gunPointTrans.position, Quaternion.LookRotation(Vector3.forward, direction));

                    // 다음 발사
                    cur += 18;
                }
            }
        }
    }
}
