using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int currentHp;
    [SerializeField] private int moveSpeed;
    
    void Start()
    {
        currentHp = maxHp;
    }
    
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Enemy took {damage} damage, remaining HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //비활성화후
        //풀에 다시 집어넣기
    }
    void Update()
    {
        
    }
}
