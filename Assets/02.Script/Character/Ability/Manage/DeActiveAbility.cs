using System.Collections;
using UnityEngine;

public class DeActiveAbility : MonoBehaviour
{
    [HideInInspector] public AbilityEffectType abilityEffectType;

    public float activeTime;
    private WaitForSeconds thisWaitForSeconds;

    private void Awake() { thisWaitForSeconds = new WaitForSeconds(activeTime); }

    private void OnEnable() { StartCoroutine(ActiveTime()); }

    private IEnumerator ActiveTime()
    {
        yield return thisWaitForSeconds;
        PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, gameObject, abilityEffectType);
    }
}
