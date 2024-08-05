using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class RangeWeapon : MonoBehaviour
{
    public WeaponEffect weaponEffect;
    public float activeTime;
    [SerializeField] private WaitForSeconds thisWaitForSeconds;
    public float moveSpeed;
    private float moveDirection;
    [HideInInspector] public int attackDamage;
    void Awake()
    {
        thisWaitForSeconds = new WaitForSeconds(activeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(ActiveTime());
    }

    private void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
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
}
