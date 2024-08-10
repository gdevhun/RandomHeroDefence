using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeleeWeapon: MonoBehaviour
{
    [HideInInspector] public WeaponEffect weaponEffect;
    [HideInInspector] public DamageType damageType;
    [Header ("유지 시간")] public float activeTime;
    [SerializeField] private WaitForSeconds thisWaitForSeconds;
    [HideInInspector] public float attackDamage;

    // 초기화
    void Awake() { thisWaitForSeconds = new WaitForSeconds(activeTime); }

    // 유지 시간 지나면 반환
    private void OnEnable() { StartCoroutine(ActiveTime()); }

    // 유지 시간 지나면 반환
    private IEnumerator ActiveTime()
    {
        yield return thisWaitForSeconds;
        PoolManager.instance.ReturnPool(PoolManager.instance.weaponEffectPool.queMap, gameObject, weaponEffect);
    }

    // 몬스터 타격
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent(out EnemyBase enemyBase))
            {
                enemyBase.TakeDamage(attackDamage, damageType); 
            }
        }
    }
}
