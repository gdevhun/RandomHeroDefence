using UnityEngine;

[CreateAssetMenu(menuName = "스킬/전설/배니스")]
public class BaniesAbility : SyncAbilityBase
{
    // 모든 물리 영웅 공격력 10% 증가
    public override void CastAbility(CharacterBase characterBase)
    {
        UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] += 10;
        UiUnit.instance.phyText.text = UpgradeUnit.instance.damageUpgradeMap[DamageType.물리].ToString() + " %";
    }
}
