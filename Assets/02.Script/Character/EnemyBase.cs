using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int maxHp; //초기설정 Hp
    private int currentHp;
    [SerializeField] private int moveSpeed; //몬스터이동속도

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        animator.SetTrigger("isDead");
        //풀에 다시 집어넣기
    }

    public void InactiveObj()
    {
        gameObject.SetActive(false); //에니메이션 데드이벤트 끝쪽에서 호출
    }
    void Update()
    {
        
    }
}
