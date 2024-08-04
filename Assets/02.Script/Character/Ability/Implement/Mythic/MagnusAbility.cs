
using UnityEngine;

public class MagnusAbility : AbilityBase, IHiddenAbility
{
    protected override void CastAbility()
    {
        
    }

    // 히든 스킬
    [SerializeField] private AbilityInfo hiddenAbilityInfo;
    public AbilityInfo HiddenAbilityInfo
    {
        get { return hiddenAbilityInfo; }
        set { hiddenAbilityInfo = value; }
    }
    public bool IsHidden()
    {
        return true;
    }
}
