// 히든 스킬 보유한 유닛 상속
public interface IHiddenAbility
{
    AbilityUiInfo HiddenAbilityUiInfo { get; set; }
    void CastHiddenAbility(CharacterBase characterBase);
}
