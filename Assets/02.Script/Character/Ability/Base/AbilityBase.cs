using UnityEngine;

// 스킬 베이스
public class AbilityBase : ScriptableObject
{
    [Header ("스킬 UI 정보")] public AbilityUiInfo abilityUiInfo;
    [Header ("스킬 이펙트 타입")] [SerializeField] protected AbilityEffectType abilityEffectType;
    [Header ("스킬 사운드 타입")] public SoundType abilitySoundType;
    [HideInInspector] public GameObject instantAbilityEffect; // 생성된 스킬 이펙트
}
