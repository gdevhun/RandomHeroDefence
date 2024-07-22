using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [SerializeField] private WaitForSeconds thisWaitForSeconds;
    private float moveSpeed;
    private float moveDirection;
    private int attackDamage;
    void Start()
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
