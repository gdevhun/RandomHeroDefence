using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeWeaponHost
{
    Gangster, Thief, Wrestler,  //일반
    Warrior, Assassin, Barbarian,  //고급
    Alonso, Viking,  //영웅
    Alisda, Makdus,  //전설
    Magnus, Yumie  //신화
}
public class MeleeWeapon: MonoBehaviour
{
    public MeleeWeaponHost meleeWeaponHost;
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
