using UnityEngine;

[CreateAssetMenu(menuName = "스킬/전설/배니스")]
public class BaniesAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 모든 물리 영웅 공격력 10% 증가
        UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] += 10;
    }
}
