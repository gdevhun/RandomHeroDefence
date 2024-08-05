using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeleeWeapon: MonoBehaviour
{
    public WeaponEffect weaponEffect;
    public float activeTime;
    [SerializeField] private WaitForSeconds thisWaitForSeconds;
    public float attackDamage;
    void Start()
    {
        
        thisWaitForSeconds = new WaitForSeconds(activeTime);
        StartCoroutine(ActiveTime());
    }

    private IEnumerator ActiveTime()
    {
        yield return thisWaitForSeconds;
        PoolManager.instance.ReturnPool(PoolManager.instance.weaponEffectPool.queMap, gameObject, weaponEffect);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent(out EnemyBase enemyBase))
            {
                enemyBase.TakeDamage(attackDamage); 
            }
        }
    }

    private void ReturnInActiveTime()
    {
       
    }
}
