using UnityEngine;

[CreateAssetMenu(menuName = "스킬/전설/루이지")]
public class LouizyAbility : SyncAbilityBase
{
    public override void CastAbility(CharacterBase characterBase)
    {
        // 모든 캐릭 스태미너 1 감소(제한 1번)
        AbilityManage.louizyCnt++;
    }
}
