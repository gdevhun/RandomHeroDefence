using UnityEngine;

[CreateAssetMenu(menuName = "스킬/전설/아아솔")]
public class AasoleAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 모든 마법 영웅 공격력 10% 증가
        UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] += 10;
    }
}
