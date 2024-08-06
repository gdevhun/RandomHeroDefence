using System.Collections;
using UnityEngine;

public class DeActiveAbility : MonoBehaviour
{
    [HideInInspector] public AbilityEffectType abilityEffectType;

    public float activeTime;
    private WaitForSeconds thisWaitForSeconds;
    public float moveSpeed;

    private void Awake() { thisWaitForSeconds = new WaitForSeconds(activeTime); }

    private void OnEnable() { StartCoroutine(ActiveTime()); }

    private void Update() { transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); }

    private IEnumerator ActiveTime()
    {
        yield return thisWaitForSeconds;
        PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, gameObject, abilityEffectType);
    }
}
