// 히든 액티브를 가지는 캐릭터 상속
public interface IHiddenAbility
{
    AbilityInfo HiddenAbilityInfo { get; set; }
    bool IsHidden();
}
