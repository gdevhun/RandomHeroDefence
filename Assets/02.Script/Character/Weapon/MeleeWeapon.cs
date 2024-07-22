using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon: MonoBehaviour
{
    [SerializeField] private WaitForSeconds thisWaitForSeconds;
    private int attackDamage;
    void Start()
    {
        StartCoroutine(ActiveTime());
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
