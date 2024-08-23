using UnityEngine;

[CreateAssetMenu(menuName = "스킬/희귀/통키")]
public class TonkeyAbility : SyncAbilityBase
{
    // 다이아 1개 추가
    public override void CastAbility(CharacterBase characterBase)
    {
        instantAbilityEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, abilityEffectType);
        instantAbilityEffect.GetComponent<DeActiveAbility>().abilityEffectType = abilityEffectType;
        instantAbilityEffect.transform.position = characterBase.transform.position + new Vector3(0f,0.5f,0f);
        
        CurrencyManager.instance.AcquireCurrency(1, false);
    }
}
