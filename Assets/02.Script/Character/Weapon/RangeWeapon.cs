using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class RangeWeapon : MonoBehaviour
{
    public float activeTime;
    [SerializeField] private WaitForSeconds thisWaitForSeconds;
    public float moveSpeed;
    private float moveDirection;
    private int attackDamage;
    void Start()
    {
        thisWaitForSeconds = new WaitForSeconds(activeTime);
        StartCoroutine(ActiveTime());
    }

    private void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private IEnumerator ActiveTime()
    {
        yield return thisWaitForSeconds;
        gameObject.SetActive(false);
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
