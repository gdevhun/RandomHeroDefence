using UnityEngine;

[CreateAssetMenu(menuName = "스킬/신화/마그너스")]
public class MagnusAbility : SyncAbilityBase, IHiddenAbility
{
    public override void CastAbility(CharacterBase characterBase)
    {
        
    }

    // 히든 스킬
    [Header ("히든 스킬 UI 정보")] [SerializeField] private AbilityUiInfo hiddenAbilityUiInfo;
    public AbilityUiInfo HiddenAbilityUiInfo
    {
        get { return hiddenAbilityUiInfo; }
        set { hiddenAbilityUiInfo = value; }
    }
    public bool IsHidden()
    {
        return true;
    }
}
